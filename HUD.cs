using Raylib_cs;

namespace RocketLeague.HUD;

public class HUD
{
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
        Raylib.InitWindow(200, 50, "Speed HUD");
        Raylib.SetTargetFPS(60);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Blank);

            // Semi-transparent dark background card
            Raylib.DrawRectangle(0, 0, 200, 50, new Color(0, 0, 0, 180));

            // Render live speed value
            Raylib.DrawText($"{currentSpeed:F1} MPH", 15, 12, 24, Color.Lime);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
