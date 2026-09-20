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
    int currentGearIndex = 1;
    Gear currentGear;

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

    int findIndex(string name)
    {
        for (int i = 0; i < gears.Length; i++)
        {
            if (gears[i].gearName == name) return i;
        }

        return (int)GearNameIDS.Neutral; // neutral
    }

    public Gear GearUp(float currentSpeed)
    {
        currentGearIndex = findIndex(currentGear.gearName);

        if (currentGearIndex + 1 >= gears.Length) return gears[(int)GearNameIDS.Boost];
        if (currentGearIndex - 1 < 0) return gears[(int)GearNameIDS.Neutral];


        return currentGear;
    }
}
