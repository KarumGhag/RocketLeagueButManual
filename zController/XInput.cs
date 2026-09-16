using SharpDX.XInput;
using XInputController = SharpDX.XInput.Controller;

namespace Controller.InputDetection;

public class InputDetector
{

    readonly XInputController controller;

    public byte clutchValue;
    public byte accelValue;
    public GamepadButtonFlags buttons;

    public InputDetector()
    {
        // Check all 4 player slots to find where the controller lives
        controller = new XInputController(UserIndex.One);
    }
    public void DetectInputs()
    {

        if (!controller.IsConnected)
        {
            Console.WriteLine("Controller not found");
            return;
        }

        State state = controller.GetState();
        Gamepad gamepad = state.Gamepad;


        buttons = state.Gamepad.Buttons;
        clutchValue = gamepad.LeftTrigger;
        accelValue = gamepad.RightTrigger;
    }
}
