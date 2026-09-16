using Speed.Server;
using RocketLeague.HUD;
using Controller.Hider;
using Controller.InputDetection;
using Controller.GhostController;


Hider hider = new Hider();
InputDetector inputDetector = new InputDetector();
GhostController ghostController = new GhostController();

HUD hud = new HUD(hider, inputDetector, ghostController);



// Conversion factor from km/h to MPH
const double KmhToMphFactor = 0.621371;

void Main()
{
    Hider hider = new Hider();
    hider.Hide();
    //Task.Run(UpdateSpeed);
    hud.MakeHUD();
}

async void UpdateSpeed()
{
    SpeedExtractor speedExtractor = new SpeedExtractor();

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

Main();




/*
Steps to get working:
1. Go to hid hide, disable device hiding, un plug re plug
2. Close hid hide, start this program, un plug re plug
3. Stop this program
4. Open rocket league
6. Go to free play
7. Try move, you should not be able to move
8. Start the program
9. Try move
*/
