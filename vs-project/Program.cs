using System;

namespace ihc
{
    class Program
    {
        private static System.Timers.Timer t;
        static void Main(string[] args)
        {
            t = new System.Timers.Timer(25);
            t.AutoReset = true;
            t.Elapsed += OnTimedEvent;
            t.Enabled = true;
            Console.ReadLine(); // press anything to quit
        }

        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (Controller.is_button_down(Types.UserIndex.Any, Types.ButtonFlags.A))
            {
                Console.WriteLine("A!");
            }
            if (Controller.is_button_down(Types.UserIndex.Any, Types.ButtonFlags.GUIDE))
            {
                Console.WriteLine("GUIDE!");
            }
        }
    }
}
