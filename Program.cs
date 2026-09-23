using Speed.Server;
using RocketLeague.HUD;
using Controller.Hider;
using Controller.InputDetection;
using Controller.GhostController;
using SharpDX.XInput;
using XInputController = SharpDX.XInput.Controller;
using GearSystem;

// Conversion factor from km/h to MPH
const double KmhToMphFactor = 0.621371;

Hider hider = new Hider();

// find the slot of the real controller for the input detector
UserIndex physicalSlot = FindPhysicalControllerSlot();

GhostController ghostController = new GhostController();
InputDetector inputDetector = new InputDetector(physicalSlot);

HUD hud = new HUD();

Gear currentGear;
float currentSpeed = 0;

Main();

void Main()
{
    hider.Hide();

    Thread hudThread = new Thread(new ThreadStart(hud.MakeHUD));
    hudThread.Start();

    Thread controllerThread = new Thread(new ThreadStart(Update));
    controllerThread.Start();

    Task.Run(UpdateSpeed);
    controllerThread.Join();
    hudThread.Join();
}


/*
TO DO:
Add clutch control
*/

void Update()
{
    byte lastFrameClutch = inputDetector.DetectInputs().clutch;
    byte thisFrameClutch = lastFrameClutch;

    ControllerState lastFrameState = inputDetector.DetectInputs();

    currentGear = GearSwitcher.gears[(int)GearSwitcher.GearNameIDS.Neutral];
    float maxSpeed = currentGear.maxSpeed;

    while (true)
    {
        lastFrameClutch = thisFrameClutch;

        ControllerState controllerState = inputDetector.DetectInputs();
        hud.UpdateControllerState(controllerState);

        thisFrameClutch = controllerState.clutch;

        int change = thisFrameClutch - lastFrameClutch;
        // if change > 0 then its you pressing down, dont care about that
        if (change > 0) change = 0;
        // after discarding you pressing down make coming off of it positive so its easier to understand
        change = Math.Abs(change);

        hud.change = change;


        if (controllerState.buttons.HasFlag(GamepadButtonFlags.RightShoulder) && !lastFrameState.buttons.HasFlag(GamepadButtonFlags.RightShoulder) && thisFrameClutch == 255)
        {
            GearSwitcher.GearUp(currentSpeed, ref currentGear);
        }

        if (controllerState.buttons.HasFlag(GamepadButtonFlags.LeftShoulder) && !lastFrameState.buttons.HasFlag(GamepadButtonFlags.LeftShoulder))
        {
            GearSwitcher.GeadDown(currentSpeed, ref currentGear);
        }


        float clutchAsPercent = thisFrameClutch / 255f;
        float shakeAsPercent = 65535 * clutchAsPercent;
        inputDetector.Shake((ushort)shakeAsPercent, (ushort)shakeAsPercent);



        if (currentSpeed > currentGear.maxSpeed)
        {
            GearSwitcher.Stall(ref currentGear, ref controllerState);
            hud.DisplayStalled();
        }

        hud.gear = currentGear.gearName;
        hud.maxSpeed = currentGear.maxSpeed;

        ghostController.Update(controllerState);



        lastFrameState = inputDetector.DetectInputs();
        Thread.Sleep(50);
    }
}



async void UpdateSpeed()
{
    SpeedExtractor speedExtractor = new SpeedExtractor();
    if (speedExtractor.Connect() == -1)
    {
        Console.WriteLine("network connection failed");
        return;
    }

    while (true)
    {
        // Speed received directly from the API in km/h
        double kmhSpeed = await speedExtractor.GetSpeed();

        if (kmhSpeed == -1)
        {
            continue;
        }

        // Convert km/h to MPH
        double mphSpeed = kmhSpeed * KmhToMphFactor;
        hud.UpdateSpeedValue(kmhSpeed);
        currentSpeed = (float)kmhSpeed;
        await Task.Delay(16);
    }
}

// Scans all XInput slots and returns the first connected one.
// Must be called BEFORE the ViGEm ghost controller is created, otherwise
// the ghost's virtual pad can occupy this slot instead of the real one.
UserIndex FindPhysicalControllerSlot()
{
    // when a controller gets connected windows assigns the controller a user index, we dont know what this index it
    // we make a new xinputcontroller at each index and check if a controller is connected to that slot
    // if true then it returns that index
    for (int i = 0; i < 4; i++)
    {
        UserIndex index = (UserIndex)i;
        if (new XInputController(index).IsConnected)
        {
            return index;
        }
    }

    throw new InvalidOperationException(
        "No controller connected, connect one and restart the program!"
    );
}


/*
Steps to get working:
1. Go to hid hide, enable device hiding, un plug re plug
2. Open rocket league, go to free play, try to move, you shouldnt be able to move
3. Run this program and try move
*/
