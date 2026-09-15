using Raylib_cs;
using Controller.Hider;

namespace RocketLeague.HUD;

public class HUD
{
    public Hider? hider;

    private double currentSpeed;

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
        Raylib.InitWindow(200, 200, "Speed HUD");
        Raylib.SetTargetFPS(60);
        Raylib.SetWindowPosition(0, 0);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Blank);

            // Semi-transparent dark background card
            Raylib.DrawRectangle(0, 0, 200, 60, new Color(0, 0, 0, 180));

            // Render live speed value
            Raylib.DrawText($"{currentSpeed:F1} MPH", 15, 12, 24, Color.Lime);
            if (hider.controlService.IsActive)
            {
                Raylib.DrawText("Hidden", 15, 34, 24, Color.Red);
            } else {
                Raylib.DrawText("Showing", 15, 34, 24, Color.Lime);
            }


            if (Raylib.IsKeyReleased(KeyboardKey.Space))
            {
                hider.controlService.IsActive = !hider.controlService.IsActive;
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
