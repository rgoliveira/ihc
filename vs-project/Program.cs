using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ihc
{
    class Program
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

        static void SetMapping(string windowTitle)
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

        static void OnFocusChanged(IntPtr hWinEventHook, uint eventType, IntPtr hWnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            string windowTitle = GetWindowTitle(hWnd);
            if (windowTitle != "XBox Input Mapper")
            {
                SetMapping(windowTitle);
                currentWindow = hWnd;
            }
        }

        static void Main(string[] args)
        {
            //// setup fixed mappings (axis stuff)
            //actions_always_run= new List<Action>();
            //actions_always_run.Add(new Action("mouse")); // ls
            //actions_always_run.Add(new Action("wheel")); // rs
            //actions_always_run.Add(new Action("volume")); // triggers

            //// read button mappings
            //mappings = new List<ProgramMapping>();
            //foreach(string filename in File.ReadLines(@"ini\mappings.cfg"))
            //{
            //    if ((!filename.StartsWith(@"#")) && (File.Exists(filename)))
            //    {
            //        mappings.Add(ProgramMapping.BuildFromFile(filename));
            //    }
            //}
            
            //// set current mapping
            //current_mapping = mappings[0];

            //EventHook ehChanged = new EventHook(OnFocusChanged, EventHook.EVENT_SYSTEM_FOREGROUND);

            //// start timer
            //t = new System.Timers.Timer(10);
            //t.AutoReset = true;
            //t.Elapsed += OnTimedEvent;
            //t.Enabled = true;
            //Console.WriteLine("Press ENTER anytime to quit...");
            //Console.ReadLine(); // press anything to quit
            Application.EnableVisualStyles();
            Application.Run(new Form1());
            //objEventHook.Stop();
        }

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Types.XINPUT_GAMEPAD_STATE s = Controller.get_gamepad_state((int)Types.UserIndex.One);

            if (Controller.button_pressed(Types.ButtonFlags.GUIDE))
            {
                Application.OpenForms[0].WindowState = FormWindowState.Normal;
                Application.OpenForms[0].Activate();
                Application.OpenForms[0].Focus();
            }

            foreach(Action a in actions_always_run)
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
