using System;
using System.Runtime.InteropServices;

namespace ihc
{
    class EventHook
    {
        public delegate void WinEventDelegate(
                        IntPtr hWinEventHook, uint eventType,
                        IntPtr hWnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(
              uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc,
              uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        public const uint EVENT_SYSTEM_FOREGROUND = 3;
        public const uint WINEVENT_OUTOFCONTEXT = 0;
        public const uint EVENT_OBJECT_CREATE = 0x8000;

        readonly WinEventDelegate _procDelegate;
        readonly IntPtr _hWinEventHook;

        public EventHook(WinEventDelegate handler, uint eventMin, uint eventMax)
        {
            _procDelegate = handler;
            _hWinEventHook = SetWinEventHook(eventMin, eventMax, IntPtr.Zero, handler, 0, 0, WINEVENT_OUTOFCONTEXT);
        }

        public EventHook(WinEventDelegate handler, uint eventMin)
                : this(handler, eventMin, eventMin) {
        }

        public void Stop()
        {
            UnhookWinEvent(_hWinEventHook);
        }

        // Usage Example for EVENT_OBJECT_CREATE (http://msdn.microsoft.com/en-us/library/windows/desktop/dd318066%28v=vs.85%29.aspx)
        // var _objectCreateHook = new EventHook(OnObjectCreate, EventHook.EVENT_OBJECT_CREATE);
        // ...
        // static void OnObjectCreate(IntPtr hWinEventHook, uint eventType, IntPtr hWnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime) {
        //    if (!Win32.GetClassName(hWnd).StartsWith("ClassICareAbout"))
        //        return;
        // Note - in Console program, doesn't fire if you have a Console.ReadLine active, so use a Form
    }
}
