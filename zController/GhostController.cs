using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

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

    public void Update(byte accel)
    {
        ghostController.SetSliderValue(Xbox360Slider.RightTrigger, accel);
    }
}
