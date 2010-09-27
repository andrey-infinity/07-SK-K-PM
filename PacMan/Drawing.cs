using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    static class Drawing
    {
        public static void DrawBorder(Point position, Size size, ConsoleColor Fc, ConsoleColor Bc)
        {
            size = new Size(size.X - 1, size.Y - 1);
            System.Console.ForegroundColor = Fc;
            System.Console.BackgroundColor = Bc;

            System.Console.SetCursorPosition(position.X, position.Y);
            System.Console.Write('+');
            System.Console.SetCursorPosition(position.X + size.X, position.Y);
            System.Console.Write('+');
            System.Console.SetCursorPosition(position.X, position.Y + size.Y);
            System.Console.Write('+');
            System.Console.SetCursorPosition(position.X + size.X, position.Y + size.Y);
            System.Console.Write('+');
            for (int i = 1; i < size.X; i++)
            {
                System.Console.SetCursorPosition(position.X + i, position.Y);
                System.Console.Write('-');
            }
            for (int i = 1; i < size.X; i++)
            {
                System.Console.SetCursorPosition(position.X + i, position.Y + size.Y);
                System.Console.Write('-');
            }
            for (int i = 1; i < size.Y; i++)
            {
                System.Console.SetCursorPosition(position.X, position.Y + i);
                System.Console.Write('|');
            }
            for (int i = 1; i < size.Y; i++)
            {
                System.Console.SetCursorPosition(position.X + size.X, position.Y + i);
                System.Console.Write('|');
            }
            System.Console.ResetColor();
        }
        public static void DrawHorLine(int x, int y, int width)
        {
            string l = "";
            for (int i = x; i < width + x; i++) l = l + ' ';
            System.Console.SetCursorPosition(x, y);
            System.Console.Write(l);
        }
        public static void DrawVertLine(int x, int y, int height)
        {
            for (int i = y; i < height + y; i++)
            {
                System.Console.SetCursorPosition(x, i);
                System.Console.Write(' ');
            }
        }
        public static void DrawRect(int x, int y, int width, int height)
        {
            for (int i = y; i < height + y; i++)
                DrawHorLine(x, i, width);
        }
        public static string FillZeros(Int64 Number, int Length)
        {
            string s = Number.ToString();
            while (s.Length < Length) s = '0' + s;
            return s;
        }
        public static void  Write(int x, int y, string Text){
            System.Console.SetCursorPosition(x, y);
            System.Console.Write(Text);
        }
    }
}
