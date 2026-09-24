using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using Controller.InputDetection;
using SharpDX.XInput;
using Controller.SDL2Input;

namespace Controller.GhostController;

public class GhostController
{
    ViGEmClient client;
    IXbox360Controller ghostController;

    public GhostController()
    {
        client = new ViGEmClient();
        ghostController = client.CreateXbox360Controller();
        ghostController.AutoSubmitReport = false;
        ghostController.Connect();

        Thread.Sleep(1000);
        Console.WriteLine($"Virtual XInput UserIndex: {ghostController.UserIndex}");
    }



    public void Update(NewControllerState controllerState)
    {
        ghostController.SetSliderValue(
            Xbox360Slider.RightTrigger,
            controllerState.accel
        );

        ghostController.SetAxisValue(Xbox360Axis.LeftThumbX, controllerState.leftX);
        ghostController.SetAxisValue(Xbox360Axis.LeftThumbY, controllerState.leftY);
        ghostController.SetAxisValue(Xbox360Axis.RightThumbX, controllerState.rightX);
        ghostController.SetAxisValue(Xbox360Axis.RightThumbY, controllerState.rightY);

        ghostController.SetButtonState(Xbox360Button.A, controllerState.A);
        ghostController.SetButtonState(Xbox360Button.B, controllerState.B);
        ghostController.SetButtonState(Xbox360Button.X, controllerState.X);
        ghostController.SetButtonState(Xbox360Button.Y, controllerState.Y);

        try
        {
            ghostController.SubmitReport();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ghost controller error: {ex}");
        }
    }
}
