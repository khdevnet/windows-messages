using System;

namespace Common.Converters
{
    public class MouseCoordinatesConverter
    {
        public IntPtr Convert(MousePoint point)
        {
            return (IntPtr)(((short)point.Y << 16) | (point.X & 0xffff));
        }

        public MousePoint Convert(IntPtr point)
        {
            var x = point.ToInt32() & 0x0000FFFF;
            var y = (int)((point.ToInt32() & 0xFFFF0000) >> 16);
            return new MousePoint(x, y);
        }
    }
}