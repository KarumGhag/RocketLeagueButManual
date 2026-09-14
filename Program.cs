using Speed.Extractor;

SpeedExtractor speedExtractor = new SpeedExtractor();

// Conversion factor from km/h to MPH
const double KmhToMphFactor = 0.621371;

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

    Console.WriteLine($"Speed: {mphSpeed:F1} MPH ({kmhSpeed:F1} km/h)");
}
