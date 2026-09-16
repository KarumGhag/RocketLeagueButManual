using Speed.Server;
using RocketLeague.HUD;
using Controller.Hider;
using Controller.InputDetection;



Hider hider = new Hider();
InputDetector inputDetector = new InputDetector();

HUD hud = new HUD(hider, inputDetector);



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
