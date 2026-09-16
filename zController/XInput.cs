using System.ComponentModel;
using Raylib_cs;
using SharpDX.XInput;
using XInputController = SharpDX.XInput.Controller;

namespace Controller.InputDetection;

public class InputDetector
{

    readonly XInputController controller;

    public byte clutchValue;
    public byte accelValue;

    public short leftX;
    public short leftY;

    public short rightX;
    public short rightY;

    public GamepadButtonFlags buttons;

    public InputDetector()
    {
        // Check all 4 player slots to find where the controller lives
        controller = new XInputController(UserIndex.One);
        Console.WriteLine($"Physical XInput UserIndex: {controller.UserIndex}");
    }

    public ControllerState DetectInputs()
    {

        if (!controller.IsConnected)
        {
            Console.WriteLine("Controller not found");
            return new ControllerState(0, 0, buttons, 0, 0, 0, 0);
        }

        State state = controller.GetState();
        Gamepad gamepad = state.Gamepad;


        buttons = state.Gamepad.Buttons;

        leftX = gamepad.LeftThumbX;
        leftY = gamepad.LeftThumbY;

        rightX = gamepad.RightThumbX;
        rightY = gamepad.RightThumbY;

        clutchValue = gamepad.LeftTrigger;
        accelValue = gamepad.RightTrigger;

        return new ControllerState(clutchValue, accelValue, buttons, leftX, leftY, rightX, rightY);
    }
}

public struct ControllerState
{
    public byte clutch;
    public byte accel;
    public short leftX;
    public short leftY;
    public short rightX;
    public short rightY;
    public GamepadButtonFlags buttons;

    public ControllerState(byte clutch, byte accel, GamepadButtonFlags buttons, short leftX, short leftY, short rightX, short rightY)
    {
        this.clutch = clutch;
        this.accel = accel;
        this.buttons = buttons;

        this.leftX = leftX;
        this.leftY = leftY;
        this.rightX = rightX;
        this.rightY = rightY;
    }
}
