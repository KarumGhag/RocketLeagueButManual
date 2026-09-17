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
    byte clutch;
    byte accel;
    GamepadButtonFlags buttons;

    readonly GhostController ghostController;
    ControllerState controllerState;

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
            Raylib.DrawRectangle(0, 0, 200, 120, new Color(0, 0, 0, 180));

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



            Raylib.DrawText($"Clutch: {clutch}", 15, 56, 24, Color.Lime);
            Raylib.DrawText($"Accel: {accel}", 15, 80, 24, Color.Lime);
            Raylib.DrawText($"Buttons: {buttons}", 15, 104, 24, Color.Lime);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    public void UpdateHudValues(double speed, byte clutch, byte accel, GamepadButtonFlags buttons)
    {
        currentSpeed = speed;
        this.clutch = clutch;
        this.accel = accel;
        this.buttons = buttons;
    }
}
