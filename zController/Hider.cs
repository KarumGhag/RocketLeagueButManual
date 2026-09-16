using Nefarius.Drivers.HidHide;
using Nefarius.Utilities.DeviceManagement.PnP;
using Nefarius.Utilities.DeviceManagement.Extensions;


namespace Controller.Hider;

public class Hider
{
    public HidHideControlService controlService;
    string rocketLeague = @"D:\Unreal Engine\rocketleague\Binaries\Win64\RocketLeague.exe";
    string controllerID = @"USB\VID_045E&PID_0B12\3039373130313130393937333238";

    public Hider()
    {
        controlService = new HidHideControlService();
    }

    public void Hide()
    {
        controlService.AddBlockedInstanceId(controllerID);

        controlService.IsActive = true;

        Console.WriteLine($"HidHide active: {controlService.IsActive}");

        Console.WriteLine("Blocked devices:");

        foreach (string id in controlService.BlockedInstanceIds)
        {
            Console.WriteLine(id);
        }
    }
}
