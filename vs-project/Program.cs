using System;

namespace ihc
{
    class Program
    {
        private static System.Timers.Timer t;
        private static Action a;
        static void Main(string[] args)
        {
            a = new Action("mouse");

            t = new System.Timers.Timer(10);
            t.AutoReset = true;
            t.Elapsed += OnTimedEvent;
            t.Enabled = true;
            Console.WriteLine("Press ENTER anytime to quit...");
            Console.ReadLine(); // press anything to quit
        }

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            a.execute(Controller.get_gamepad_state((int)Types.UserIndex.One));
        }
    }
}
