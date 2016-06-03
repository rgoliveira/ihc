using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using SharpDX.XInput;
using System.Runtime.InteropServices;

namespace ConsoleApplication1
{
    class Program
    {
        [DllImport(@"guide.dll", CallingConvention=CallingConvention.Cdecl)]
        //double __cdecl __declspec(dllexport) is_guide_button_down(double which_controller) {
        private static extern double is_guide_button_down(double which_controller);

        static void Main(string[] args)
        {
            // box setup
            Form f = new Form();
            f.Opacity = 0.3;
            f.StartPosition = FormStartPosition.CenterScreen;
            f.FormBorderStyle = FormBorderStyle.None;
            f.TopMost = true;

            bool boxVisible = false;
            bool wasPressedGuide = false;

            while (!System.Console.KeyAvailable)
            {
                SharpDX.XInput.Controller c = new SharpDX.XInput.Controller(SharpDX.XInput.UserIndex.One);
                SharpDX.XInput.State s;
                if (c.GetState(out s))
                {
                    //System.Console.WriteLine(s.Gamepad.ToString());
                    //System.Console.WriteLine(s.Gamepad.Buttons);
                    s.Gamepad.Buttons.HasFlag(SharpDX.XInput.GamepadButtonFlags.A);
                    if (!wasPressedGuide)
                    {
                        if (is_guide_button_down(0) != 0)
                        {
                            wasPressedGuide = true;
                            boxVisible = !boxVisible;
                            //SendKeys.SendWait("{RIGHT}");
                        }

                        if (boxVisible)
                        {
                            if (!f.Visible)
                            {
                                f.Show();
                                
                            }
                        }
                        else
                        {
                            f.Hide();
                        }
                    }
                    else
                    {
                        wasPressedGuide = (is_guide_button_down(0) != 0);
                    }
                }
            }
        }
    }
}
