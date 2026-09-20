using Speed.Server;
using RocketLeague.HUD;
using Controller.Hider;
using Controller.InputDetection;
using Controller.GhostController;
using SharpDX.XInput;
using XInputController = SharpDX.XInput.Controller;

// Conversion factor from km/h to MPH
const double KmhToMphFactor = 0.621371;

Hider hider = new Hider();

// find the slot of the real controller for the input detector
UserIndex physicalSlot = FindPhysicalControllerSlot();

GhostController ghostController = new GhostController();
InputDetector inputDetector = new InputDetector(physicalSlot);

HUD hud = new HUD(hider, inputDetector, ghostController);

Main();

void Main()
{
    hider.Hide();

    Thread hudThread = new Thread(new ThreadStart(hud.MakeHUD));
    hudThread.Start();

    Thread controllerThread = new Thread(new ThreadStart(UpdateController));
    controllerThread.Start();

    Task.Run(UpdateSpeed);
    controllerThread.Join();
    hudThread.Join();
}

void UpdateController()
{
    while (true)
    {
        ControllerState controllerState = inputDetector.DetectInputs();
        ghostController.Update(controllerState);
        hud.UpdateControllerState(controllerState);
    }
}

async void UpdateSpeed()
{
    SpeedExtractor speedExtractor = new SpeedExtractor();
    if (speedExtractor.Connect() == -1)
    {
        Console.WriteLine("connection failed!");
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
