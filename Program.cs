using Speed.Server;
using RocketLeague.HUD;
using Controller.Hider;

HUD hud = new HUD();
Hider hider = new Hider();


// Conversion factor from km/h to MPH
const double KmhToMphFactor = 0.621371;

void Main()
{
    Hider hider = new Hider();
    hider.Hide();
    Task.Run(UpdateSpeed);
    hud.hider = hider;
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
