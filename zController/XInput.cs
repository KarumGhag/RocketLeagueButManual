using SharpDX.XInput;
using XInputController = SharpDX.XInput.Controller;

namespace Controller.InputDetection;

public class InputDetector
{

    XInputController controller;

    public InputDetector()
    {
        // Check all 4 player slots to find where the controller lives
        controller = new XInputController(UserIndex.One);
    }
    public void GetInput()
    {

        if (!controller.IsConnected)
        {
            Console.WriteLine("Controller not found");
            return;
        }

        State state = controller.GetState();
        Console.WriteLine(state.ToString());
    }
}
