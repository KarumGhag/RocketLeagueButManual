using Raylib_cs;
using Controller.Hider;
using Controller.InputDetection;
using Controller.GhostController;
using SharpDX.XInput;
using XInputController = SharpDX.XInput.Controller;

namespace RocketLeague.HUD;

public class HUD
{
    readonly Hider? hider;
    readonly InputDetector? inputDetector;

    double currentSpeed;

    readonly GhostController ghostController;
    ControllerState controllerState;

    public int change;

    public HUD(Hider hider, InputDetector inputDetector, GhostController ghostController)
    {
        this.hider = hider;
        this.inputDetector = inputDetector;
        this.ghostController = ghostController;
    }

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
            ConfigFlags.AlwaysRunWindow
        );

        // Compact 200x50 window setup
        Raylib.InitWindow(200, 400, "Speed HUD");
        Raylib.SetTargetFPS(60);
        Raylib.SetWindowPosition(0, 0);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Blank);

            // Semi-transparent dark background card
            // for the y add 25 for each line of text at font 24
            Raylib.DrawRectangle(0, 0, 200, 150, new Color(0, 0, 0, 180));

            // Render live speed value
            Raylib.DrawText($"{currentSpeed:F1} MPH", 15, 12, 24, Color.Lime);

            if (hider.controlService.IsActive)
            {
                Raylib.DrawText("Hidden", 15, 34, 24, Color.Red);
            }
            else
            {
                Raylib.DrawText("Showing", 15, 34, 24, Color.Lime);
            }

            if (Raylib.IsKeyReleased(KeyboardKey.Space))
            {
                hider.controlService.IsActive = !hider.controlService.IsActive;
            }



            Raylib.DrawText($"Clutch: {controllerState.clutch}", 15, 56, 24, Color.Lime);
            Raylib.DrawText($"Accel: {controllerState.accel}", 15, 80, 24, Color.Lime);
            Raylib.DrawText($"Buttons: {controllerState.buttons}", 15, 104, 24, Color.Lime);
            Raylib.DrawText($"Change: {change}", 15, 128, 24 , Color.Lime);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
