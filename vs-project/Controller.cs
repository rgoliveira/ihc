using System.Runtime.InteropServices;
using ihc.Types;
using System;

namespace ihc
{
    public static class Controller
    {
        // "secret" function from xinput API that also returns the guide button state
        [DllImport("xinput1_3.dll", EntryPoint = "#100")]
        private static extern int _get_gamepad_state(int user_index, out XINPUT_GAMEPAD_STATE struc);
        
        private static XINPUT_GAMEPAD_STATE _lastState = default(XINPUT_GAMEPAD_STATE);

        public static XINPUT_GAMEPAD_STATE get_gamepad_state(int user_index)
        {
            _get_gamepad_state(user_index, out _lastState);
            // Console.WriteLine(Convert.ToString(_lastState.wButtons, 2).PadLeft(16, '0'));
            return _lastState;
        }

        public static bool is_button_down(ButtonFlags button)
        {                        
            return (_lastState.wButtons & (ushort)button) != 0;
        }
    }
}
