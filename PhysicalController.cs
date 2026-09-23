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
        Console.WriteLine(leftTrigger);
        return new NewControllerState(leftTrigger);
    }
}

public struct NewControllerState
{
    public byte clutch;

    public NewControllerState(byte leftTrigger)
    {
        clutch = leftTrigger;
    }
}
