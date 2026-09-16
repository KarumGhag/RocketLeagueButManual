using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using Controller.InputDetection;

namespace Controller.GhostController;

public class GhostController
{
    ViGEmClient client;
    IXbox360Controller ghostController;

    public GhostController()
    {
        client = new ViGEmClient();
        ghostController = client.CreateXbox360Controller();
        ghostController.Connect();
    }

    public void Update(ControllerState controllerState)
    {
        ghostController.SetSliderValue(Xbox360Slider.RightTrigger, controllerState.accel);
    }
}
