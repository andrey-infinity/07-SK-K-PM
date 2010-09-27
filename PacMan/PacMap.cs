using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    class Point
    {
        public int X,Y;
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    class Size{
        public int X, Y;
        public Size(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class PacMap
    {
        public char[,] Map;
        public Size MapSize = new Size(0, 0);
        public List<Point> Enemies = new List<Point>();
        public Point StartPoint = new Point(0, 0);
        public Direction StartDirection = Direction.Right;
        public int TotalDots = 0;
        
        public PacMap(int Level){
            LoadMap(Level);
        }

        void LoadMap(int Level)
        {
            string[] tMap;
            tMap = System.IO.File.ReadAllLines("Maps\\"+ Level.ToString());

            int i, j = 0;
            MapSize.Y = tMap.GetLength(0);
            MapSize.X = tMap[0].Length;

            Map = new char[MapSize.Y, MapSize.X];

            for (i = 0; i < MapSize.Y; i++)
            {
                for (j = 0; j < MapSize.X; j++)
                {
                    char NextSym = ' ';

                    #region Enemy on map 
                    if (tMap[i][j] == 'S'){
                        Enemies.Add(new Point(j,i));
                    }
                    #endregion
                    #region Super berry
                    else if (tMap[i][j] == 'N'){
                        //SpecialDots.Add(new SpecialDot(new Point(j,i)));
                        NextSym = Convert.ToChar(164);
                        TotalDots += 1;
                    }
                    #endregion
                    #region Player
                    else if (tMap[i][j] == '^'){
                        StartPoint = new Point(j,i);
                        StartDirection = Direction.Down;
                    }
                    else if (tMap[i][j] == 'v'){
                        StartPoint = new Point(j,i);
                        StartDirection = Direction.Up;
                    }
                    else if (tMap[i][j] == '<'){
                        StartPoint = new Point(j,i);
                        StartDirection = Direction.Right;
                    }
                    else if (tMap[i][j] == '>'){
                        StartPoint = new Point(j, i);
                        StartDirection = Direction.Left;
                    }
                    #endregion
                    #region Wall
                    else if (tMap[i][j] == '#'){
                        NextSym = '#';
                    }
                    #endregion
                    #region Berry
                    else {
                        TotalDots+=1;
                        NextSym = Convert.ToChar(183);
                    }
                    #endregion
                    Map[i, j] = NextSym;
                }
            }
        }

        public void Draw()
        {
            for (int i = 0; i < MapSize.Y; i++)
            {
                System.Console.SetCursorPosition(Game.play.Offset.X, Game.play.Offset.Y + i);
                for (int j = 0; j < MapSize.X; j++)
                {
                    DrawObjectInSquare(j, i);
                }
            }
        }
        public void DrawObjectInSquare(int j, int i)
        {
            if (Map[i, j] == '#')
            {
                System.Console.ForegroundColor = ConsoleColor.Gray;
                System.Console.BackgroundColor = ConsoleColor.Gray;
            }
            if (Map[i, j] == Convert.ToChar(183))
            {
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            if (Map[i, j] == Convert.ToChar(164))
            {
                System.Console.ForegroundColor = ConsoleColor.Magenta;
            }
            System.Console.Write(Map[i, j]);
            System.Console.ResetColor();
        }
    }
}
