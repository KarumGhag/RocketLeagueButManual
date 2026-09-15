using System;
using SharpDX.XInput;
using XInputController = SharpDX.XInput.Controller;

namespace RocketLeague.Controller.InputDetection;

public class InputDetection
{
    public void GetInput()
    {
        XInputController controller = new XInputController();
    }
}
