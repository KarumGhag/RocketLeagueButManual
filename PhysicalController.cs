using static SDL2.SDL;
using Controller.InputDetection;
using SDL2;  // using this for the controller struct, if this works will move the struct here;

namespace Controller.SDL2Input;

public class PhysicalController
{
    static IntPtr controllerPointer;

    int FindPhysicalControllerIndex()
    {
        for (int i = 0; i < SDL_NumJoysticks(); i++)
        {
            if (SDL_IsGameController(i) != SDL_bool.SDL_TRUE) continue;

            var type = SDL_GameControllerTypeForIndex(i);

            // Skip anything that looks like the ViGEm ghost
            if (type == SDL_GameControllerType.SDL_CONTROLLER_TYPE_XBOX360) continue;

            return i;
        }

        throw new Exception("No physical controller found!");
    }

    public void Connect()
    {
        SDL_SetHint(SDL_HINT_XINPUT_ENABLED, "1");
        SDL_SetHint(SDL_HINT_JOYSTICK_RAWINPUT, "0");

        SDL_Init(SDL_INIT_GAMECONTROLLER);

        controllerPointer = SDL_GameControllerOpen(FindPhysicalControllerIndex());
        if (controllerPointer == IntPtr.Zero) // has no value
        {
            throw new Exception("Failed to connect to controller!");
        }
        else
        {
            Console.WriteLine("Controller connected");
        }

        Console.WriteLine(SDL_GameControllerGetType(controllerPointer));
    }

    public NewControllerState GetInputs()
    {
        SDL_GameControllerUpdate();
        byte leftTrigger = ShortToByte(SDL_GameControllerGetAxis(controllerPointer, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERLEFT));
        byte rightTrigger = ShortToByte(SDL_GameControllerGetAxis(controllerPointer, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERRIGHT));

        bool A  = ByteToBool(SDL_GameControllerGetButton(controllerPointer, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_A));
        bool B  = ByteToBool(SDL_GameControllerGetButton(controllerPointer, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_B));
        bool Y  = ByteToBool(SDL_GameControllerGetButton(controllerPointer, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_Y));
        bool X  = ByteToBool(SDL_GameControllerGetButton(controllerPointer, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_X));
        bool RB = ByteToBool(SDL_GameControllerGetButton(controllerPointer, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_RIGHTSHOULDER));
        bool LB = ByteToBool(SDL_GameControllerGetButton(controllerPointer, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_LEFTSHOULDER));

        Buttons buttons = new Buttons(A, B, Y, X, RB, LB);

        short leftX = SDL_GameControllerGetAxis(controllerPointer, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTX);
        short leftY = InvertShort(SDL_GameControllerGetAxis(controllerPointer, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTY));
        short rightX = SDL_GameControllerGetAxis(controllerPointer, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTX);
        short rightY = InvertShort(SDL_GameControllerGetAxis(controllerPointer, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTY));


        return new NewControllerState(leftTrigger, rightTrigger, leftX, leftY, rightX, rightY, buttons);
    }


    // need to invert the y axis because its inverted for some reason
    // cant just * -1 because short wraps around and is always negative
    // convert to converted int and then back to short but clamp around min and max
    static short InvertShort(short toInvert)
    {
        int inverted = toInvert * -1;
        return (short)Math.Clamp(inverted, short.MinValue, short.MaxValue);
    }

    static byte ShortToByte(short Short)
    {
        float percentThroughShort = (float)Short / short.MaxValue;
        float percentThroughByte = percentThroughShort * 255f;

        return (byte)percentThroughByte;
    }

    static bool ByteToBool(byte Byte)
    {
        if (Byte == 1) return true;
        return false;
    }

    public void Vibrate(float percent)
    {
        SDL_GameControllerUpdate();

        percent /= 100f;

        float max = ushort.MaxValue;
        ushort amount = (ushort)Math.Floor((percent * max));
        SDL_GameControllerRumble(controllerPointer, amount, amount, 1);
    }
}

public struct NewControllerState
{
    public byte clutch;
    public byte accel;

    public Buttons buttons;

    public short leftX;
    public short leftY;
    public short rightX;
    public short rightY;

    public NewControllerState(byte leftTrigger, byte rightTrigger, short leftX, short leftY, short rightX, short rightY, Buttons buttons)
    {
        clutch = leftTrigger;
        accel = rightTrigger;

        this.buttons = buttons;

        this.leftX = leftX;
        this.leftY = leftY;
        this.rightX = rightX;
        this.rightY = rightY;
    }
}

public struct Buttons
{
    public bool A;
    public bool B;
    public bool Y;
    public bool X;
    public bool RB;
    public bool LB;

    public Buttons(bool A, bool B, bool Y, bool X, bool RB, bool LB)
    {
        this.A = A;
        this.B = B;
        this.Y = Y;
        this.X = X;
        this.RB = RB;
        this.LB = LB;
    }
}
