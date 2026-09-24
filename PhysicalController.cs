using static SDL2.SDL;
using Controller.InputDetection;  // using this for the controller struct, if this works will move the struct here;

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
            if (type == SDL_GameControllerType.SDL_CONTROLLER_TYPE_XBOX360)
                continue;

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
    }

    public NewControllerState GetInputs()
    {
        SDL_GameControllerUpdate();
        byte leftTrigger = (byte)SDL_GameControllerGetAxis(controllerPointer, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERLEFT);
        byte rightTrigger = (byte)SDL_GameControllerGetAxis(controllerPointer, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERRIGHT);

        bool A = ByteToBool(SDL_GameControllerGetButton(controllerPointer, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_A));
        bool B = ByteToBool(SDL_GameControllerGetButton(controllerPointer, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_B));
        bool Y = ByteToBool(SDL_GameControllerGetButton(controllerPointer, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_Y));
        bool X = ByteToBool(SDL_GameControllerGetButton(controllerPointer, SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_X));

        short leftX = SDL_GameControllerGetAxis(controllerPointer, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTX);
        short leftY = InvertShort(SDL_GameControllerGetAxis(controllerPointer, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTY));
        short rightX = SDL_GameControllerGetAxis(controllerPointer, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTX);
        short rightY = InvertShort(SDL_GameControllerGetAxis(controllerPointer, SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTY));


        return new NewControllerState(leftTrigger, rightTrigger, A, B, Y, X, leftX, leftY, rightX, rightY);
    }


    // need to invert the y axis because its inverted for some reason
    // cant just * -1 because short wraps around and is always negative
    // convert to converted int and then back to short but clamp around min and max
    short InvertShort(short toInvert)
    {
        int inverted = toInvert * -1;
        return (short)Math.Clamp(inverted, short.MinValue, short.MaxValue);
    }


    static bool ByteToBool(byte Byte)
    {
        if (Byte == 1) return true;
        return false;
    }
}

public struct NewControllerState
{
    public byte clutch;
    public byte accel;
    public bool A;
    public bool B;
    public bool Y;
    public bool X;
    public short leftX;
    public short leftY;
    public short rightX;
    public short rightY;

    public NewControllerState(byte leftTrigger, byte rightTrigger, bool A, bool B, bool Y, bool X, short leftX, short leftY, short rightX, short rightY)
    {
        clutch = leftTrigger;
        accel = rightTrigger;

        this.A = A;
        this.B = B;
        this.Y = Y;
        this.X = X;

        this.leftX = leftX;
        this.leftY = leftY;
        this.rightX = rightX;
        this.rightY = rightY;
    }
}
