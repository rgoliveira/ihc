using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.IO;

namespace ihc
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern int GetWindowTextA(IntPtr hWnd, System.Text.StringBuilder Buffer, int Lg);

        private static System.Timers.Timer t;

        private static IntPtr currentWindow;

        private static List<Action> actions_always_run;

        private static List<ProgramMapping> mappings;
        private static ProgramMapping current_mapping;

        private static string GetWindowTitle(IntPtr hWnd)
        {
            System.Text.StringBuilder SB = new System.Text.StringBuilder(200);
            int Hresult = GetWindowTextA(hWnd, SB, 200);
            return (Hresult != 0) ? SB.ToString() : "";
        }

        void SetMapping(string windowTitle)
        {
            Console.WriteLine(windowTitle);
            foreach (ProgramMapping pm in mappings)
            {
                Match m = Regex.Match(windowTitle, pm.program_regex_str, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    current_mapping = pm;
                    break;
                }
            }
        }

        string ActionStringToStr(string ActionString)
        {
            if (ActionString == " ")
            {
                return "{SPACE}";
            }
            else
            {
                return ActionString;
            }
        }

        void setTextBoxToMapping(TextBox tb, Types.ButtonFlags button)
        {
            if (current_mapping.mappings.ContainsKey(button))
            {
                tb.Text = current_mapping.mappings[button].label;
                if (tb.TextLength == 0)
                {
                    tb.Text = current_mapping.mappings[button]._action_string;
                }
            }
            else
            {
                tb.Clear();
            }
        }

        void updateScreen()
        {
            lblActiveWindowTitle.Text = current_mapping.name;

            setTextBoxToMapping(tbLB, Types.ButtonFlags.LEFT_SHOULDER);
            setTextBoxToMapping(tbRB, Types.ButtonFlags.RIGHT_SHOULDER);
            setTextBoxToMapping(tbBack, Types.ButtonFlags.BACK);
            setTextBoxToMapping(tbStart, Types.ButtonFlags.START);
            setTextBoxToMapping(tbA, Types.ButtonFlags.A);
            setTextBoxToMapping(tbB, Types.ButtonFlags.B);
            setTextBoxToMapping(tbX, Types.ButtonFlags.X);
            setTextBoxToMapping(tbY, Types.ButtonFlags.Y);
            setTextBoxToMapping(tbDPAD_DOWN, Types.ButtonFlags.DPAD_DOWN);
            setTextBoxToMapping(tbDPAD_LEFT, Types.ButtonFlags.DPAD_LEFT);
            setTextBoxToMapping(tbDPAD_RIGHT, Types.ButtonFlags.DPAD_RIGHT);
            setTextBoxToMapping(tbDPAD_UP, Types.ButtonFlags.DPAD_UP);
        }

        void OnFocusChanged(IntPtr hWinEventHook, uint eventType, IntPtr hWnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            string windowTitle = GetWindowTitle(hWnd);
            if (windowTitle != "XBox Input Mapper")
            {
                SetMapping(windowTitle);
                updateScreen();
                currentWindow = hWnd;
            }
        }

        public Form1()
        {
            InitializeComponent();

            // setup fixed mappings (axis stuff)
            actions_always_run = new List<Action>();
            actions_always_run.Add(new Action("mouse", "Mouse Cursor")); // ls
            actions_always_run.Add(new Action("wheel", "Scrolling")); // rs
            actions_always_run.Add(new Action("volume", "Volume")); // triggers

            // read button mappings
            mappings = new List<ProgramMapping>();
            foreach (string filename in File.ReadLines(@"ini\mappings.cfg"))
            {
                if ((!filename.StartsWith(@"#")) && (File.Exists(filename)))
                {
                    mappings.Add(ProgramMapping.BuildFromFile(filename));
                }
            }

            // set current mapping
            current_mapping = mappings.Last();
            updateScreen();

            EventHook ehChanged = new EventHook(OnFocusChanged, EventHook.EVENT_SYSTEM_FOREGROUND);

            // start timer
            t = new System.Timers.Timer(10);
            t.AutoReset = true;
            t.Elapsed += OnTimedEvent;
            t.Enabled = true;            
        }

        private void lblActiveWindowTitle_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Types.XINPUT_GAMEPAD_STATE s = Controller.get_gamepad_state((int)Types.UserIndex.One);

            if (Controller.button_pressed(Types.ButtonFlags.GUIDE))
            {
                Application.OpenForms[0].WindowState = FormWindowState.Normal;
                Application.OpenForms[0].Activate();
                Application.OpenForms[0].Focus();
            }

            foreach (Action a in actions_always_run)
            {
                a.execute(s);
            }

            foreach (Types.ButtonFlags b in current_mapping.mappings.Keys)
            {
                if (Controller.button_pressed(b))
                {
                    if (current_mapping.mappings.ContainsKey(b))
                    {
                        current_mapping.mappings[b].execute(s);
                    }
                }
            }
        }
    }
}
