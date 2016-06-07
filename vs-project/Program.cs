using System;
using System.Collections.Generic;

namespace ihc
{
    class Program
    {
        private static System.Timers.Timer t;
        
        private static List<Action> actions_always_run;
        private static Dictionary<Types.ButtonFlags, Action> mapper_press;
        //private static Dictionary<Types.ButtonFlags, Action> mapper_can_hold;

        static void Main(string[] args)
        {
            actions_always_run= new List<Action>();
            actions_always_run.Add(new Action("mouse")); // ls
            actions_always_run.Add(new Action("wheel")); // rs
            actions_always_run.Add(new Action("volume")); // triggers

            //mapper_can_hold = new Dictionary<Types.ButtonFlags, Action>();
            //mapper_can_hold.Add(Types.ButtonFlags.A, new Action("mouse left"));
            //mapper_can_hold.Add(Types.ButtonFlags.B, new Action("mouse right"));

            mapper_press = new Dictionary<Types.ButtonFlags, Action>();
            mapper_press.Add(Types.ButtonFlags.START, new Action("osk"));
            mapper_press.Add(Types.ButtonFlags.A, new Action("mouse left"));
            mapper_press.Add(Types.ButtonFlags.B, new Action("mouse right"));
            mapper_press.Add(Types.ButtonFlags.Y, new Action("^=")); // zoom in
            mapper_press.Add(Types.ButtonFlags.X, new Action("^-")); // zoom out
            mapper_press.Add(Types.ButtonFlags.LEFT_SHOULDER, new Action("^{PGUP}")); // cycle tabs forward
            mapper_press.Add(Types.ButtonFlags.RIGHT_SHOULDER, new Action("^{PGDN}")); // cycle tabs backward
            mapper_press.Add(Types.ButtonFlags.DPAD_DOWN, new Action("{DOWN}"));
            mapper_press.Add(Types.ButtonFlags.DPAD_LEFT, new Action("{LEFT}"));
            mapper_press.Add(Types.ButtonFlags.DPAD_RIGHT, new Action("{RIGHT}"));
            mapper_press.Add(Types.ButtonFlags.DPAD_UP, new Action("{UP}"));

            t = new System.Timers.Timer(10);
            t.AutoReset = true;
            t.Elapsed += OnTimedEvent;
            t.Enabled = true;
            Console.WriteLine("Press ENTER anytime to quit...");
            Console.ReadLine(); // press anything to quit
        }

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Types.XINPUT_GAMEPAD_STATE s = Controller.get_gamepad_state((int)Types.UserIndex.One);
            foreach(Action a in actions_always_run)
            {
                a.execute(s);
            }

            foreach (Types.ButtonFlags b in mapper_press.Keys)
            {
                if (Controller.button_pressed(b))
                {
                    if (mapper_press.ContainsKey(b))
                    {
                        mapper_press[b].execute(s);
                    }
                }
            }

            //foreach (Types.ButtonFlags b in mapper_can_hold.Keys)
            //{
            //    if (Controller.is_button_down(b))
            //    {
            //        if (mapper_can_hold.ContainsKey(b))
            //        {
            //            mapper_can_hold[b].execute(s);
            //        }
            //    }
            //}
        }
    }
}
