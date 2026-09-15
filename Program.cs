using Speed.Server;
using RocketLeague.HUD;

SpeedExtractor speedExtractor = new SpeedExtractor();
HUD hud = new HUD();

// Conversion factor from km/h to MPH
const double KmhToMphFactor = 0.621371;

void Main()
{
    Task.Run(UpdateSpeed);
    hud.MakeHUD();
}

async void UpdateSpeed()
{
    while (true)
    {
        // Speed received directly from the API in km/h
        double kmhSpeed = await speedExtractor.GetSpeed();

        if (kmhSpeed == -1)
        {
            Console.WriteLine("failed");
            continue;
        }

        // Convert km/h to MPH
        double mphSpeed = kmhSpeed * KmhToMphFactor;
        hud.UpdateSpeedValue(kmhSpeed);
        await Task.Delay(16);
    }
}

Main();
