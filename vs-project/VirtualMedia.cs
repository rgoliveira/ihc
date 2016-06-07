using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ihc
{
    static class VirtualMedia
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        public static void volume_up()
        {
            keybd_event((byte)Keys.VolumeUp, 0, 0, 0);            
        }

        public static void volume_down()
        {
            keybd_event((byte)Keys.VolumeDown, 0, 0, 0);
        }

        public static void change_volume(int delta)
        {
            if (delta > 0)
            {
                VirtualMedia.volume_up();
            } else if (delta < 0)
            {
                VirtualMedia.volume_down();
            }
        }
    }
}
