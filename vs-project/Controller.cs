using System.Runtime.InteropServices;
using ihc.Types;
using System;

namespace ihc
{
    public static class Controller
    {
        //[DllImport(@"guide.dll", CallingConvention = CallingConvention.Cdecl)]
        //private static extern double is_guide_button_down(double which_controller);

        // "secret" function from xinput API that also returns the guide button state
        [DllImport("xinput1_3.dll", EntryPoint = "#100")]
        private static extern int get_gamepad_state(int user_index, out XINPUT_GAMEPAD_STATE struc);

        //private XINPUT_GAMEPAD_STATE _lastState = default(XINPUT_GAMEPAD_STATE);

        public static bool is_button_down(UserIndex user_index, ButtonFlags button)
        {
            ihc.Types.XINPUT_GAMEPAD_STATE s;
            get_gamepad_state(0/*(int)user_index*/, out s);
            // Console.WriteLine(Convert.ToString(s.wButtons, 2).PadLeft(16, '0'));
            return (s.wButtons & (ushort)button) != 0;
        }
    }
}
