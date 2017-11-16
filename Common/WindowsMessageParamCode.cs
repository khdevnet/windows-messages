using System;

namespace Common
{
    public static class WindowsMessageParamCode
    {
        public static IntPtr LeftMouseClick => (IntPtr)0x0008;

        public static IntPtr RightMouseClick => (IntPtr)0x0002;
    }
}