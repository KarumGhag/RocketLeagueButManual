using SharpDX.XInput;
using XInputController = SharpDX.XInput.Controller;

namespace Controller.InputDetection;

public class InputDetection
{
    public void GetInput()
    {
        XInputController controller = new XInputController();
        State state = controller.GetState();
        Console.WriteLine(state.ToString());
    }
}
