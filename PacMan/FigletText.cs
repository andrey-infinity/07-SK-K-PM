using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PacMan
{
    class FigletText
    {
        int sX = 0;
        int sY = 0;
        string sName = "pacman";
        bool sTransparent = true;
        ConsoleColor sColor = ConsoleColor.White;

        public FigletText(int X, int Y, string Name, bool Transparent, ConsoleColor Color){
            sX = X;
            sY = Y;
            sName = Name;
            sTransparent = Transparent;
            sColor = Color;
            Draw();
        }

        void Draw()
        {
            int tY = sY;

            try
            {
                System.Console.ForegroundColor = sColor;
                string[] file = File.ReadAllLines("Texts\\" + sName + ".ascii");
                for (int i = 0; i < file.Length; i++)
                {
                    if (sTransparent)
                    {
                        for (int c = 0; c < file[i].Length; c++)
                        {
                            if (file[i][c] != ' ')
                            {
                                System.Console.SetCursorPosition(sX + c, tY);
                                System.Console.WriteLine(file[i][c]);
                            }
                        }
                    }
                    else
                    {
                        System.Console.SetCursorPosition(sX, tY);
                        System.Console.WriteLine(file[i]);
                    }
                    tY++;
                }
            }
            finally
            {
                System.Console.ResetColor();
            }
        }
    }
}
