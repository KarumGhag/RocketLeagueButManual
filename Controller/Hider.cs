using Nefarius.Drivers.HidHide;

namespace Controller.Hider;

public class Hider
{
    HidHideControlService controlService;
    string rocketLeague = @"D:\Unreal Engine\rocketleague\Binaries\Win64\RocketLeague.exe";

    public Hider()
    {
        controlService = new HidHideControlService();
    }
}
