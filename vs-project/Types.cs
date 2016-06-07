namespace ihc
{
    namespace Types
    {
        public enum Velocities : int
        {
            Mouse_Movement = 15,
            Wheel_Rotation = 10
        }

        // xinput own thresholds/deadzones
        public enum Thresholds : int
        {
            XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE = 7849,
            XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE = 8689,
            XINPUT_GAMEPAD_TRIGGER_THRESHOLD = 30
        }

        public struct XINPUT_GAMEPAD_STATE
        {
            public uint eventCount;
            public ushort wButtons;
            public byte bLeftTrigger;
            public byte bRightTrigger;
            public short sThumbLX;
            public short sThumbLY;
            public short sThumbRX;
            public short sThumbRY;
        }

        public enum ButtonFlags : uint
        {
            DPAD_UP = 0x0001,
            DPAD_DOWN = 0x0002,
            DPAD_LEFT = 0x0004,
            DPAD_RIGHT = 0x0008,
            START = 0x0010,
            BACK = 0x0020,
            LEFT_THUMB = 0x0040,
            RIGHT_THUMB = 0x0080,
            LEFT_SHOULDER = 0x0100,
            RIGHT_SHOULDER = 0x0200,
            GUIDE = 0x0400,
            A = 0x1000,
            B = 0x2000,
            X = 0x4000,
            Y = 0x8000
        };

        public enum UserIndex : int
        {
            One = 0x00,
            Two = 0x01,
            Three = 0x02,
            Four = 0x03,
            Any = 0x000000FF
        };
    }
}
