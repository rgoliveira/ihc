using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ihc;

namespace ihc
{
    class Action
    {
        private string _action_string;

        private delegate void Act(Types.XINPUT_GAMEPAD_STATE state);
        private Act _act;

        private System.Drawing.Point get_new_mouse_pos(Types.XINPUT_GAMEPAD_STATE state)
        {
            int deltaX = 0, deltaY = 0;
            if (state.sThumbLX > (int)Types.Thresholds.XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE)
            {
                deltaX = 5;
            } else if (state.sThumbLX < (-1) * (int)Types.Thresholds.XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE)
            {
                deltaX = -5;
            }
            if (state.sThumbLY > (int)Types.Thresholds.XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE)
            {
                deltaY = -5;
            } else if (state.sThumbLY < (-1) * (int)Types.Thresholds.XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE)
            {
                deltaY = 5;
            }
            return new System.Drawing.Point(Cursor.Position.X + deltaX, Cursor.Position.Y + deltaY);
        }

        public Action(string action_string)
        {
            this._action_string = action_string;

            // parse action string

            // simple keys
            Match m = Regex.Match(action_string, @"^([+%^])?([a-z]|BKSP|CAPSLOCK|DEL|DOWN|END|ENTER|ESC|HOME|INS|LEFT|NUMLOCK|PGDN|PGUP|PRTSC|RIGHT|TAB|UP|F\d+)$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                _act = (x) => SendKeys.SendWait("{" + action_string + "}");
            }

            // mouse buttons
            m = Regex.Match(action_string, @"^mouse$");
            if (m.Success)
            {
                _act = (x) => Cursor.Position = get_new_mouse_pos(x);
            }

        }

        public void execute(Types.XINPUT_GAMEPAD_STATE state)
        {
            this._act(state);
        }
    }
}
