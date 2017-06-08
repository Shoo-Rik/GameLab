using System;
using System.Drawing;

namespace Data
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

        public static Direction GetAttackerDirection(this Coordinates defender, Coordinates attacker)
        {
            if (Math.Abs(defender.X - attacker.X) + Math.Abs(defender.Y - attacker.Y) != 1)
                return Direction.None;

            if (defender.X < attacker.X)
                return Direction.Ost;

            if (defender.X > attacker.X)
                return Direction.West;

            if (defender.Y < attacker.Y)
                return Direction.South;

            if (defender.Y > attacker.Y)
                return Direction.Nord;

            return Direction.None;
        }
    }
}
