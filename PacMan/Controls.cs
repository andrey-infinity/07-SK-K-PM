using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    class Edit
    {
        private bool tSelected = false;
        public bool Selected
        {
            get { return tSelected; }
            set
            {
                tSelected = value;
                Draw();
            }
        }
        public int Width = 10;
        public int MaxLen = 10;
        private int tCursor = 0;
        private string tText = "";
        public string Text
        {
            get { return tText; }
            set
            {
                tText = value;
                DrawText();
            }
        }
        public int Cursor
        {
            get { return tCursor; }
            set
            {
                tCursor = value;
                DrawText();
            }
        }
        public Point Coord;

        public Edit(int x, int y, int width)
        {
            Coord = new Point(x, y);
            Width = width;
        }
        public void Add(char l)
        {
            if (Text.Length < MaxLen)
            {
                string tmp = "";
                if (Cursor == Text.Length)
                {
                    tmp = Text + l;
                }
                else
                {
                    for (int i = 0; i < Text.Length; i++)
                    {
                        if (i == Cursor) tmp += l;
                        tmp += Text[i];
                    }
                }
                Text = tmp;
            }
            MoveCursor(Direction.Right);
        }
        public void DeleteChar()
        {
            MoveCursor(Direction.Left);
            if (Text.Length >= Cursor)
            {
                string tmp = "";
                for (int i = 0; i < Text.Length; i++)
                {
                    if (i != Cursor) tmp += Text[i];
                }
                Text = tmp;
            }

        }
        public void MoveCursor(Direction d)
        {
            if (d == Direction.Left)
            {
                if (Cursor > 0) Cursor--;
            }
            if (d == Direction.Right)
            {
                if (Text.Length > Cursor)
                {
                    Cursor++;
                }
            }
            DrawText();
        }

        public void Draw()
        {
            ConsoleColor bc = ConsoleColor.Black;
            if (Selected) bc = ConsoleColor.DarkCyan;
            Drawing.DrawBorder(Coord, new Size(Width + 2, 3), ConsoleColor.White, bc);
            DrawText();
        }
        public void DrawText()
        {
            Drawing.DrawHorLine(Coord.X + 1, Coord.Y + 1, Width);
            System.Console.SetCursorPosition(Coord.X + 1, Coord.Y + 1);
            for (int i = 0; i < Text.Length; i++)
            {
                if (i == Cursor)
                    if (Selected)
                        System.Console.BackgroundColor = ConsoleColor.Cyan;

                System.Console.Write(Text[i]);
                System.Console.ResetColor();
            }
            if (Text.Length == Cursor)
            {
                System.Console.BackgroundColor = ConsoleColor.Cyan;
                System.Console.Write(' ');
                System.Console.ResetColor();
            }
        }

        public void KeyHandler(ConsoleKeyInfo Key)
        {
            if (Key.Key == ConsoleKey.Backspace)
            {
                DeleteChar();
            }
            else if (Key.Key == ConsoleKey.LeftArrow)
            {
                MoveCursor(Direction.Left);
            }
            else if (Key.Key == ConsoleKey.RightArrow)
            {
                MoveCursor(Direction.Right);
            }
            else
            {
                if (Char.IsLetterOrDigit(Key.KeyChar) || Char.IsPunctuation(Key.KeyChar))
                {
                    Add(Key.KeyChar);
                }
            }
        }
    }
    class Button
    {
        public string Text = "";
        public short X,Y = 0;
        bool sSelected = false;
        public bool Selected
        {
            set
            {
                sSelected = value;
                this.Show();
            }
            get
            {
                return sSelected;
            }
        }

        public Button(string Txt, Point Coord, bool Sel)
        {
            Text = Txt;
            sSelected = Sel;
            Y = (short)Coord.Y;
            X = (short)Coord.X;
        }
        public void Show()
        {
            System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
            System.Console.SetCursorPosition(X, Y);
            System.Console.Write(Text);
            System.Console.SetCursorPosition(X - 2, Y);
            if (sSelected)
            {
                System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                System.Console.Write("> ");
            }
            else
            {
                System.Console.Write("  ");
            }
            System.Console.ResetColor();
        }
    }
    class MenuEntry  :Button
    {

        public MenuEntry(string Txt, short Coord, bool Sel):base(Txt, new Point(0,Coord), Sel)
        {
            X = (short)((System.Console.WindowWidth - Text.Length) / 2);
        }
    }
}
