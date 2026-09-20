using Controller.InputDetection;
using Speed.Server;

namespace GearSystem;

// readonly bc once made you shouldnt change
public readonly struct Gear
{
    public readonly string gearName;
    public readonly float maxSpeed;
    public readonly float entrySpeed;

    public Gear(string gear, float maxSpeed, float entrySpeed)
    {
        this.gearName = gear;
        this.maxSpeed = maxSpeed;
        this.entrySpeed = entrySpeed;
    }
}

public class GearSwitcher
{
    readonly Gear[] gears = new Gear[8];
    enum GearNameIDS : int
    {
        Neutral = 0,
        Reverse = 1,
        First   = 2,
        Second  = 3,
        Third   = 4,
        Fourth  = 5,
        Fifth   = 6,
        Boost   = 7
    }
    Gear currentGear;
    GearNameIDS currentGearEnum = GearNameIDS.Neutral;

    void PopulateGears()
    {
        gears[0] = new Gear("neutral", 2, -1); // -1 means any entry speed
        gears[1] = new Gear("reverse", 10, 0);
        gears[2] = new Gear("first", 10, 0);
        gears[3] = new Gear("second", 24, 6);
        gears[4] = new Gear("third", 35, 17);
        gears[5] = new Gear("fourth", 45, 30);
        gears[6] = new Gear("fifth", 55, 40);
        gears[7] = new Gear("boost", 90, 50);

        currentGear = gears[1];
    }

    public GearSwitcher()
    {
        PopulateGears();
    }

    public Gear GearUp(float currentSpeed)
    {
        int currentGearIndex = (int)currentGearEnum;

        if (currentGearIndex + 1 >= gears.Length) return gears[(int)GearNameIDS.Boost];

        if (currentSpeed > gears[currentGearIndex + 1].entrySpeed)
        {
            currentGearEnum++;
        }
        else
        {
            return gears[(int)GearNameIDS.Neutral]; // stall
        }

        return gears[(int)currentGearEnum];
    }

    public Gear GeadDown(float currentSpeed)
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
}
