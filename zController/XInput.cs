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

    public ControllerState DetectInputs()
    {

        if (!controller.IsConnected)
        {
            Console.WriteLine("Controller not found");
            return new ControllerState(0, 0, buttons);
        }

        State state = controller.GetState();
        Gamepad gamepad = state.Gamepad;


        buttons = state.Gamepad.Buttons;
        clutchValue = gamepad.LeftTrigger;
        accelValue = gamepad.RightTrigger;

        return new ControllerState(clutchValue, accelValue, buttons);
    }
}

public struct ControllerState
{
    public byte clutch;
    public byte accel;
    public GamepadButtonFlags buttons;

    public ControllerState(byte clutch, byte accel, GamepadButtonFlags buttons)
    {
        this.clutch = clutch;
        this.accel = accel;
        this.buttons = buttons;
    }
}
