using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ihc
{
    class Action
    {
        public string _action_string { get; private set; }
        public string label { get; private set; }

        private delegate void Act(Types.XINPUT_GAMEPAD_STATE state);
        private Act _act;

        private double get_axis_delta(short axis_value, short deadzone)
        {
            double delta;
            if (Math.Abs(axis_value) > deadzone)
            {
                delta = axis_value / 32767.0;
                if (delta < 0)
                {
                    delta *= -delta;
                } else
                {
                    delta *= delta;
                }
            }
            else
            {
                delta = 0.0;
            }
            return delta;
        }

        private int get_mouse_deltaX(Types.XINPUT_GAMEPAD_STATE state)
        {            
            double delta = get_axis_delta(state.sThumbLX, (short)Types.Thresholds.XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE);
            return (int)(delta * (int)Types.Velocities.Mouse_Movement);
        }

        private int get_mouse_deltaY(Types.XINPUT_GAMEPAD_STATE state)
        {
            double delta = get_axis_delta(state.sThumbLY, (short)Types.Thresholds.XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE);
            return (int)(-delta * (int)Types.Velocities.Mouse_Movement);         
        }

        private int get_wheel_deltaX(Types.XINPUT_GAMEPAD_STATE state)
        {
            double delta = get_axis_delta(state.sThumbRX, (short)Types.Thresholds.XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE);
            return (int)(delta * (int)Types.Velocities.Wheel_Rotation);
        }

        private int get_wheel_deltaY(Types.XINPUT_GAMEPAD_STATE state)
        {
            double delta = get_axis_delta(state.sThumbRY, (short)Types.Thresholds.XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE);
            return (int)(delta * (int)Types.Velocities.Wheel_Rotation);
        }

        private int get_volume_delta(Types.XINPUT_GAMEPAD_STATE state)
        {
            if (state.bLeftTrigger > (byte)Types.Thresholds.XINPUT_GAMEPAD_TRIGGER_THRESHOLD)
            {
                return -1;
            } else if (state.bRightTrigger > (byte)Types.Thresholds.XINPUT_GAMEPAD_TRIGGER_THRESHOLD) {
                return +1;
            } else {
                return 0;
            }
        }

        private void invoke_osk()
        {
            // activates on-screen-keyboard.

            // A T T E N T I O N ! ! !
            // for this to work, and to be able to interact with "especial" software,
            // run this cmd: "icacls <binary> /setintegritylevel High"

            // "classic" screen keyboard
            //System.Diagnostics.Process.Start(@"osk.exe");

            // new tablet screen keyboard (not a full keyboard; lacks ALT, WinKey, ...)
            // much better to write on than the other.
            //ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"TabTip.exe");
            //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(startInfo);
        }

        public Action(string action_string, string label)
        {
            this._action_string = action_string;
            this.label = label;

            // parse action string

            // simple keys
            // modifiers: shift = +, ctrl = ^, alt = %
            Match m = Regex.Match(action_string, @"^([+%^]*)(.|\{(BKSP|CAPSLOCK|DEL|DOWN|END|ENTER|ESC|HOME|INS|LEFT|NUMLOCK|PGDN|PGUP|PRTSC|RIGHT|TAB|UP|F\d+)\})$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                _act = (x) => SendKeys.SendWait(action_string);              
            }

            // virtual keyboard
            m = Regex.Match(action_string, @"^osk$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                _act = (x) => invoke_osk();
            }

            // wheel rotation (vertical and horizontal scrolling)
            m = Regex.Match(action_string, @"^wheel$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                _act = (x) => VirtualMouse.Wheel(get_wheel_deltaX(x), get_wheel_deltaY(x));
            }

            // mouse movement
            m = Regex.Match(action_string, @"^mouse$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                _act = (x) => VirtualMouse.Move(get_mouse_deltaX(x), get_mouse_deltaY(x));
            }

            // mouse buttons
            m = Regex.Match(action_string, @"^mouse left$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                _act = (x) => VirtualMouse.LeftClick();
            }
            m = Regex.Match(action_string, @"^mouse right$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                _act = (x) => VirtualMouse.RightClick();
            }
            m = Regex.Match(action_string, @"^mouse middle", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                _act = (x) => VirtualMouse.MiddleClick();
            }

            // volume            
            m = Regex.Match(action_string, @"^volume$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                _act = (x) => VirtualMedia.change_volume(get_volume_delta(x));
            }

        }

        public void execute(Types.XINPUT_GAMEPAD_STATE state)
        {
            this._act(state);
        }
    }
}
