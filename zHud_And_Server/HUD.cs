using Raylib_cs;
using Controller.Hider;
using Controller.InputDetection;
using Controller.GhostController;
using SharpDX.XInput;
using XInputController = SharpDX.XInput.Controller;

namespace RocketLeague.HUD;

public class HUD
{
    double currentSpeed;

    ControllerState controllerState;

    public int change;
    public string? gear;
    public float maxSpeed;

    bool stalled = false;
    int framesSinceStalled = 0;
    int stalledTextTime = 30; //how many frames to display that youve stalled


    public void UpdateSpeedValue(double speed)
    {
        currentSpeed = speed;
    }

    public void UpdateControllerState(ControllerState controllerState)
    {
        this.controllerState = controllerState;
    }


    public void MakeHUD()
    {
        // Pure Raylib window flag configuration
        Raylib.SetConfigFlags(
            ConfigFlags.UndecoratedWindow |
            ConfigFlags.TransparentWindow |
            ConfigFlags.TopmostWindow |
            ConfigFlags.AlwaysRunWindow |
            ConfigFlags.MousePassthroughWindow
        );

        Raylib.InitWindow(1920, 1700, "Speed HUD");
        Raylib.SetTargetFPS(60);
        Raylib.SetWindowPosition(0, 0);



        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Blank);

            // Semi-transparent dark background card
            // for the y add 25 for each line of text at font 24
            Raylib.DrawRectangle(0, 0, 200, 175, new Color(0, 0, 0, 180));

            // Render live speed value
            Raylib.DrawText($"{currentSpeed:F1} MPH", 15, 12, 24, Color.Lime);

            Raylib.DrawText($"Clutch: {controllerState.clutch}", 15, 34, 24, Color.Lime);
            Raylib.DrawText($"Accel: {controllerState.accel}", 15, 56, 24, Color.Lime);
            Raylib.DrawText($"Buttons: {controllerState.buttons}", 15, 80, 24, Color.Lime);
            Raylib.DrawText($"Change: {change}", 15, 104, 24, Color.Lime);
            Raylib.DrawText($"Gear: {gear}", 15, 128, 24, Color.Lime);
            Raylib.DrawText($"MaxSpeed: {maxSpeed}", 15, 152, 24, Color.Lime);

            if (stalled)
            {
                int textSize = Raylib.MeasureText("Stalled!", 35);
                Raylib.DrawRectangle(960 - (textSize / 2) - 20, 85, 40 + textSize, 49, new Color(0, 0, 0, 180));
                Raylib.DrawText("Stalled!", 960 - (textSize / 2), 92, 35, Color.Red);
                framesSinceStalled++;
                if (framesSinceStalled > stalledTextTime)
                {
                    stalled = false;
                }
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    public void DisplayStalled()
    {
        stalled = true;
        framesSinceStalled = 0;
    }
}
