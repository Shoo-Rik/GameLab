using System;
using System.Drawing;

namespace Common.Data
{
    public static class CoordinateHelper
    {
        public static string HorizontalIndexToCoordinate(int hIndex)
        {
            return new string((char)('А' + ((hIndex == 9) ? hIndex + 1 : hIndex)), 1);
        }

        public static string VerticalIndexToCoordinate(int vIndex)
        {
            return (vIndex + 1).ToString();
        }

        public static string GetColorName(Color color)
        {
            return color.IsNamedColor ? color.Name : String.Format($"{color.ToArgb():X}");
        }
    }
}
