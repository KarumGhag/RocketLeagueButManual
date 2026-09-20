using Controller.InputDetection;
using Speed.Server;

namespace GearSystem;

// readonly bc once made you shouldnt change
public readonly struct Gear
{
    public readonly string gearName;
    public readonly float maxSpeed;
    public readonly float entrySpeed;

    public Gear(string gearName, float maxSpeed, float entrySpeed)
    {
        this.gearName = gearName;
        this.maxSpeed = maxSpeed;
        this.entrySpeed = entrySpeed;
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


    static GearNameIDS currentGearEnum = GearNameIDS.Neutral;

    public static void PopulateGears()
    {
        gears[0] = new Gear("reverse", 10, 0);
        gears[1] = new Gear("neutral", 5, -1); // -1 means any entry speed
        gears[2] = new Gear("first", 15, -1);
        gears[3] = new Gear("second", 24, 4);
        gears[4] = new Gear("third", 35, 17);
        gears[5] = new Gear("fourth", 45, 30);
        gears[6] = new Gear("fifth", 55, 40);
        gears[7] = new Gear("boost", 90, 50);
    }

    static GearSwitcher()
    {
        PopulateGears();
    }

    public static Gear GearUp(float currentSpeed)
    {
        Console.WriteLine(currentGearEnum);
        int currentGearIndex = (int)currentGearEnum;

        if (currentGearIndex + 1 >= gears.Length) return gears[(int)GearNameIDS.Boost];

        if (currentSpeed > gears[currentGearIndex + 1].entrySpeed)
        {
            currentGearEnum++;
        }
        else
        {
            currentGearEnum = GearNameIDS.Neutral;
            return gears[(int)GearNameIDS.Neutral]; // stall
        }

        return gears[(int)currentGearEnum];
    }

    public static Gear GeadDown(float currentSpeed)
    {
        int currentGearIndex = (int)currentGearEnum;
        // gear down in neutral puts you in reverse
        if (currentGearEnum == GearNameIDS.Neutral) return gears[(int)GearNameIDS.Reverse];
        if (currentGearIndex - 1 < 0) return gears[(int)GearNameIDS.Neutral];

        if (currentSpeed < gears[currentGearIndex - 1].maxSpeed)
        {
            currentGearEnum--;
        }
        else
        {
            return gears[(int)GearNameIDS.Neutral]; // stall
        }

        return gears[(int)currentGearEnum];
    }

    public static void Stall(ref Gear currentGear, ref ControllerState controllerState)
    {
        Console.WriteLine("stalled");
        currentGear = gears[(int)GearNameIDS.Neutral];
        currentGearEnum = GearNameIDS.Neutral;
        controllerState.accel = 0;
    }

}
