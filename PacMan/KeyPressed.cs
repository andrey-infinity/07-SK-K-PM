using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PacMan
{
    static class KeyPressed
    {
        const uint STD_INPUT_HANDLE = 0xFFFFFFFF - 9;
        const ushort KEY_EVENT = 0x0001;
        static IntPtr stdIn = GetStdHandle(STD_INPUT_HANDLE);

        [DllImport("Kernel32")]
        private static extern IntPtr GetStdHandle(uint nStdHandle);
        [DllImport("Kernel32")]
        private static extern bool ReadConsoleInput(IntPtr hConsoleInput, [Out]
      INPUT_RECORD[] lpBuffer, uint nLength, out uint lpNumberOfEventsRead);
        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT_RECORD
        {
            public ushort EventType;
            public uint bKeyDown;
            public ushort wRepeatCount;
            public ushort wVirtualKeyCode;
            public ushort wVirtualScanCode;
            public char UnicodeChar;
            public uint dwControlKeyState;
        }
        public static ushort GetLastPressedCode()
        {
            uint nRead = 0;
            ushort result = 0;
            INPUT_RECORD[] iRecord = new INPUT_RECORD[128];
            if (ReadConsoleInput(stdIn, iRecord, 128, out nRead))
                for (int n = (int)nRead -1; n >= 0; n--)
                    if (iRecord[n].EventType == KEY_EVENT)
                        if (iRecord[n].bKeyDown != 0)
                        {
                            result = iRecord[n].wVirtualKeyCode;
                            break;
                        }
            return result;
        }
    }
}
