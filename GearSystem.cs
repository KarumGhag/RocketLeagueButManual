using Controller.InputDetection;
using Speed.Server;

namespace GearSystem;

// readonly bc once made you shouldnt change
public readonly struct Gear
{
    public readonly string gearName;
    public readonly float maxSpeed;
    public readonly float minSpeed;

    public Gear(string gearName, float maxSpeed, float entrySpeed)
    {
        this.gearName = gearName;
        this.maxSpeed = maxSpeed;
        this.minSpeed = entrySpeed;
    }
}

public static class GearSwitcher
{
    public static readonly Gear[] gears = new Gear[8];

    public enum GearNameIDS : int
    {
        Reverse = 0,
        Neutral = 1,
        First   = 2,
        Second  = 3,
        Third   = 4,
        Fourth  = 5,
        Fifth   = 6,
        Boost   = 7
    }

    public static void PopulateGears()
    {
        gears[0] = new Gear("Reverse", 10, 0);
        gears[1] = new Gear("Neutral", 5, -1); // -1 means any entry speed
        gears[2] = new Gear("First", 15, -1);
        gears[3] = new Gear("Second", 24, 4);
        gears[4] = new Gear("Third", 35, 17);
        gears[5] = new Gear("Fourth", 45, 30);
        gears[6] = new Gear("Fifth", 55, 40);
        gears[7] = new Gear("Boost", 90, 50);
    }

    static GearSwitcher()
    {
        PopulateGears();
    }

    public static void GearUp(float currentSpeed, ref Gear currentGear)
    {
        int currentGearIndex = 0;
        if (Enum.TryParse(currentGear.gearName, out GearNameIDS gearNameID)) currentGearIndex = (int)gearNameID;

        if (currentGearIndex + 1 >= gears.Length) { currentGear = gears[(int)GearNameIDS.Neutral]; return; }

        if (currentSpeed > gears[currentGearIndex + 1].minSpeed)
        {
            currentGear = gears[currentGearIndex + 1];
        }
        else
        {
            // stall
            currentGear = gears[(int)GearNameIDS.Neutral];
            return;
        }
    }

    public static void GeadDown(float currentSpeed, ref Gear currentGear)
    {
        int currentGearIndex = 0;
        if (Enum.TryParse(currentGear.gearName, out GearNameIDS gearNameID)) currentGearIndex = (int)gearNameID;
        // gear down in neutral puts you in reverse
        if (gearNameID == GearNameIDS.Neutral) { currentGear = gears[(int)GearNameIDS.Neutral]; return; }

        if (currentGearIndex - 1 < 0) { currentGear = gears[(int)GearNameIDS.Neutral]; return; }

        if (currentSpeed < gears[currentGearIndex - 1].maxSpeed)
        {
            currentGear = gears[currentGearIndex - 1];
        }
        else
        {
             // stall
            currentGear = gears[(int)GearNameIDS.Neutral];
            return;
        }
    }

    // currently doesnt car about coming off the clutch speed
    public static bool CheckForStall(Gear gear, float currentSpeed, int clutchChange)
    {
        return (currentSpeed > gear.maxSpeed) || (currentSpeed < gear.minSpeed);
    }

    public static void Stall(ref Gear currentGear, ref ControllerState controllerState)
    {
        Console.WriteLine("stalled");
        currentGear = gears[(int)GearNameIDS.Neutral];
        controllerState.accel = 0;
    }

}
