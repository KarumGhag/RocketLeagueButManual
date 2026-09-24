using Speed.Server;
using RocketLeague.HUD;
using Controller.Hider;
using Controller.GhostController;
using GearSystem;
using Controller.SDL2Input;

// Conversion factor from km/h to MPH
const double KmhToMphFactor = 0.621371;

Hider hider = new Hider();

GhostController ghostController = new GhostController();

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

Swap to sdl2, will need to change how inputs are detected and maybe re write FindPhysicalControllerSlot()
*/

// void Update()
// {
//     byte lastFrameClutch = inputDetector.DetectInputs().clutch;
//     byte thisFrameClutch = lastFrameClutch;

//     ControllerState lastFrameState = inputDetector.DetectInputs();

//     currentGear = GearSwitcher.gears[(int)GearSwitcher.GearNameIDS.Neutral];
//     float maxSpeed = currentGear.maxSpeed;

//     while (true)
//     {
//         lastFrameClutch = thisFrameClutch;

//         ControllerState controllerState = inputDetector.DetectInputs();
//         hud.UpdateControllerState(controllerState);

//         thisFrameClutch = controllerState.clutch;

//         int change = thisFrameClutch - lastFrameClutch;
//         // if change > 0 then its you pressing down, dont care about that
//         if (change > 0) change = 0;
//         // after discarding you pressing down make coming off of it positive so its easier to understand
//         change = Math.Abs(change);

//         hud.change = change;


//         if (controllerState.buttons.HasFlag(GamepadButtonFlags.RightShoulder) && !lastFrameState.buttons.HasFlag(GamepadButtonFlags.RightShoulder) && thisFrameClutch == 255)
//         {
//             GearSwitcher.GearUp(currentSpeed, ref currentGear);
//         }

//         if (controllerState.buttons.HasFlag(GamepadButtonFlags.LeftShoulder) && !lastFrameState.buttons.HasFlag(GamepadButtonFlags.LeftShoulder))
//         {
//             GearSwitcher.GeadDown(currentSpeed, ref currentGear);
//         }


//         float clutchAsPercent = thisFrameClutch / 255f;
//         float shakeAsPercent = 65535 * clutchAsPercent;
//         inputDetector.Shake((ushort)shakeAsPercent, (ushort)shakeAsPercent);



//         if (currentSpeed > currentGear.maxSpeed)
//         {
//             GearSwitcher.Stall(ref currentGear, ref controllerState);
//             hud.DisplayStalled();
//         }

//         hud.gear = currentGear.gearName;
//         hud.maxSpeed = currentGear.maxSpeed;

//         ghostController.Update(controllerState);



//         lastFrameState = inputDetector.DetectInputs();
//         Thread.Sleep(50);
//     }
// }

void Update()
{
    PhysicalController physicalController = new PhysicalController();
    physicalController.Connect();
    NewControllerState controllerState = physicalController.GetInputs();
    NewControllerState lastControllerState;

    byte thisFrameClutch = 0;
    byte lastFrameClutch = 0;

    int clutchChange;

    currentGear = GearSwitcher.gears[(int)GearSwitcher.GearNameIDS.Neutral];

    while (true)
    {
        lastFrameClutch = thisFrameClutch;
        lastControllerState = controllerState;

        controllerState = physicalController.GetInputs();
        thisFrameClutch = controllerState.clutch;

        // get how much clutch has changed
        // if its positive then its because your going on it
        // dont care about that so set to 0, otherwise make it positive so its easier to work out
        clutchChange = thisFrameClutch - lastFrameClutch;

        if (controllerState.buttons.RB && !lastControllerState.buttons.RB)
        {
            GearSwitcher.GearUp(currentSpeed, ref currentGear);
        }

        if (controllerState.buttons.LB && !lastControllerState.buttons.LB)
        {
            GearSwitcher.GearDown(currentSpeed, ref currentGear);
        }

        if (GearSwitcher.CheckForStall(currentGear, currentSpeed, clutchChange)) { GearSwitcher.Stall(ref currentGear, ref controllerState); hud.DisplayStalled(); }

        hud.gear = currentGear.gearName;
        hud.maxSpeed = currentGear.maxSpeed;
        hud.change = clutchChange;
        hud.UpdateControllerState(controllerState);
        ghostController.Update(controllerState);

        float vibratePercent = GearSwitcher.PercentThroughGear(currentGear, currentSpeed);
        physicalController.Vibrate(vibratePercent);

        lastFrameClutch = thisFrameClutch;
        lastControllerState = controllerState;

        Console.WriteLine();

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

/*
Steps to get working:
1. Go to hid hide, enable device hiding, un plug re plug
2. Open rocket league, go to free play, try to move, you shouldnt be able to move
3. Run this program and try move
*/
