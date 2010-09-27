using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    enum Direction { Up, Down, Left, Right };
    abstract class DynamicObject
    {
        private int sX, sY = 0;
        public int X
        {
            set
            {
                System.Console.SetCursorPosition(sX + Game.play.Offset.X, sY + Game.play.Offset.Y);
                Game.play.Map.DrawObjectInSquare(sX, sY);
                sX = value;
            }
            get
            {
                return sX;
            }
        }
        public int Y
        {
            set
            {
                System.Console.SetCursorPosition(sX + Game.play.Offset.X, sY + Game.play.Offset.Y);
                Game.play.Map.DrawObjectInSquare(sX, sY);
                sY = value;
            }
            get
            {
                return sY;
            }
        }
        public Direction D = Direction.Down;
        public ConsoleColor Color = ConsoleColor.Yellow;
        public ConsoleColor Back = ConsoleColor.Black;

        public virtual void Create(Point coord)
        {
            sX = coord.X;
            sY = coord.Y;
        }
        public virtual void Draw()
        {
            System.Console.SetCursorPosition(X + Game.play.Offset.X, Y + Game.play.Offset.Y);
            System.Console.ForegroundColor = Color;
            System.Console.BackgroundColor = Back;
        }
        public virtual void Move(Direction md)
        {
            switch (md)
            {
                case Direction.Up: MoveUp();
                    break;
                case Direction.Down: MoveDown();
                    break;
                case Direction.Left: MoveLeft();
                    break;
                case Direction.Right: MoveRight();
                    break;
            }
        }
        public virtual void MoveUp()
        {
            if (CanMove(Direction.Up))
            {
                D = Direction.Up;
                Y -= 1;
            }
            else if ((Y == 0) && (Game.play.Map.Map[Game.play.Map.MapSize.Y - 1, X] != '#'))
            {
                D = Direction.Up;
                Y = Game.play.Map.MapSize.Y - 1;
            }
        }
        public virtual void MoveDown()
        {
            if (CanMove(Direction.Down))
            {
                D = Direction.Down;
                Y += 1;
            }
            else if ((Y == Game.play.Map.MapSize.Y - 1) && (Game.play.Map.Map[0, X] != '#'))
            {
                D = Direction.Right;
                Y = 0;
            }
        }
        public virtual void MoveLeft()
        {
            if (CanMove(Direction.Left))
            {
                D = Direction.Left;
                X -=1;
            }
            else if ((X == 0) && (Game.play.Map.Map[Y, Game.play.Map.MapSize.X - 1] != '#'))
            {
                D = Direction.Left;
                X = Game.play.Map.MapSize.X - 1;
            }
        }
        public virtual void MoveRight()
        {
            if (CanMove(Direction.Right))
            {
                D = Direction.Right;
                X +=1;
            }
            else if ((X == Game.play.Map.MapSize.X - 1) && (Game.play.Map.Map[Y, 0] != '#'))
            {
                D = Direction.Right;
                X = 0;
            }
        }

        public bool CanMove(Direction d)
        {
            switch (d)
            {
                case Direction.Down:
                    if ((Y < Game.play.Map.MapSize.Y - 1) &&
                        (Game.play.Map.Map[Y + 1, X] != '#')) return true;
                    else return false;

                case Direction.Left:
                    if ((X > 0) && 
                        (Game.play.Map.Map[Y, X - 1] != '#')) return true;
                    else return false;

                case Direction.Right:
                    if ((X < Game.play.Map.MapSize.X - 1) && (
                        Game.play.Map.Map[Y, X + 1] != '#')) return true;
                    else return false;

                case Direction.Up:
                    if ((Y > 0) &&
                        (Game.play.Map.Map[Y - 1, X] != '#')) return true;
                    else return false;
            }
            return false;
        }

    }

    class PacMan : DynamicObject
    {
        public bool Super = false;
        public int SuperLeft = 0;
        public PacMan(Point coord)
        {
            base.Create(coord);
        }
        public override void Draw()
        {
            base.Draw();

            char ObjectChar = ' ';
            switch (D)
            {
                case Direction.Down: ObjectChar = '^';
                    break;
                case Direction.Up: ObjectChar = 'v';
                    break;
                case Direction.Left: ObjectChar = '>';
                    break;
                case Direction.Right: ObjectChar = '<';
                    break;
            }
            System.Console.Write(ObjectChar);
            System.Console.ResetColor();
        }

        public override void MoveUp()
        {
            base.MoveUp();
            CheckDots();
        }
        public override void MoveDown()
        {
            base.MoveDown();
            CheckDots();
        }
        public override void MoveLeft()
        {
            base.MoveLeft();
            CheckDots();
        }
        public override void MoveRight()
        {
            base.MoveRight();
            CheckDots();
        }

        public void CheckDots()
        {
            Game.play.WriteInfo();
            if (Game.play.Map.Map[Y, X] == Convert.ToChar(183))
            {
                Game.play.Map.Map[Y, X] = ' ';
                Game.play.Map.TotalDots -= 1;
                Game.play.Score += 10;
            }
            else if (Game.play.Map.Map[Y, X] == Convert.ToChar(164))
            {
                Game.play.Map.Map[Y, X] = ' ';
                Game.play.Map.TotalDots -= 1;
                Game.play.Score += 100;
                UseSpecial();
            }
            if (Game.play.Map.TotalDots == 0)
            {
                if (Game.play.Level < Game.play.Levels)
                {
                    Game.play.NextLevel();
                    Game.play.Show();
                }
                else
                {
                    Game.play.Win();
                }
            }
        }
        public void UseSpecial()
        {
            SuperLeft = 30;
            Super = true;
            Color = ConsoleColor.DarkYellow;
        }
        public void GoSpecial()
        {
            if (Super == true)
            {
                SuperLeft -= 1;
            }
            if (SuperLeft == 0)
            {
                Super = false;
                Color = ConsoleColor.Yellow;
            }
        }

    }
    class Ghost : DynamicObject
    {
        public bool White = true;
        public bool Dead = false;

        public int TimeOut = 40;
        public bool Hunter = false; 

        public Ghost(Point coord, bool white)
        {
            base.Create(coord);
            White = white;
        }
        public void FindDirection()
        {
            TimeOut--;
            if (TimeOut == 0)
            {
                if (Hunter)
                {
                    Hunter = false;
                    Color = ConsoleColor.White;
                    TimeOut = 40;
                }
                else
                {
                    if (!Game.play.Player.Super)
                    {
                        Hunter = true;
                        Color = ConsoleColor.Red;
                        TimeOut = 40;
                    }
                    else
                    {
                        Color = ConsoleColor.White;
                        TimeOut = Game.play.Player.SuperLeft + 5;
                    }
                }
            }
            else
            {
                if ((Hunter) && (Game.play.Player.Super))
                {
                    Hunter = false;
                    Color = ConsoleColor.White;
                    TimeOut = Game.play.Player.SuperLeft + 5;
                    D = ReverseMove(D);
                }
            }
            if (!Hunter)
            {
                RandomMove();
            }
            else
            {
                ShortestMove();
            }
        }
        public void RandomMove()
        {
            bool Able = false;
            while (!Able)
            {
                int i = Game.randomizer.Next(5);
                int n = 0;
                foreach (Direction f in Enum.GetValues(typeof(Direction)))
                {
                    if (n == i)
                    {
                        if (CanMove(f))
                        {
                            int av = CountAvaliable();
                            if (((av > 1) &&
                                (f != ReverseMove(D)))
                                || (av == 1))
                            {
                                Able = true;
                                D = f;
                            }
                        }
                    }
                    n++;
                }
            }
        }
        public void ShortestMove()
        {
            #region make the matrix
            int[,] matrix = new int[Game.play.Map.MapSize.X, Game.play.Map.MapSize.Y];
            for (int i = 0; i < Game.play.Map.MapSize.X; i++)
            {
                for (int j = 0; j < Game.play.Map.MapSize.Y; j++)
                {
                    if (Game.play.Map.Map[j, i] == '#')
                    {
                        matrix[i, j] = -100;
                    }
                    else
                    {
                        matrix[i, j] = -1;
                    }
                }
            }
            #endregion
            #region Find shortest path
            matrix[Game.play.Player.X, Game.play.Player.Y] = 0;
            int step = 0;
            bool cont = true;
            while ((matrix[X, Y] == -1) && cont)
            {
                cont = false;
                for (int i = 0; i < Game.play.Map.MapSize.X; i++)
                {
                    for (int j = 0; j < Game.play.Map.MapSize.Y; j++)
                    {
                        if (matrix[i, j] == step)
                        {
                            if (i - 1 >= 0)
                                if (matrix[i - 1, j] == -1)
                                {
                                    matrix[i - 1, j] = step + 1;
                                    cont = true;
                                }
                            if (i + 1 < Game.play.Map.MapSize.X)
                                if (matrix[i + 1, j] == -1)
                                {
                                    matrix[i + 1, j] = step + 1;
                                    cont = true;
                                }
                            if (j - 1 >= 0)
                                if (matrix[i, j - 1] == -1)
                                {
                                    matrix[i, j - 1] = step + 1;
                                    cont = true;
                                }
                            if (j + 1 < Game.play.Map.MapSize.Y)
                                if (matrix[i, j + 1] == -1)
                                {
                                    matrix[i, j + 1] = step + 1;
                                    cont = true;
                                }
                        }
                    }
                }
                step++;
            }
            #endregion
            if (matrix[X, Y] == -1) // If path to pacman wasnt found
            {
                RandomMove();
            }
            else
            {
                D = ShortestStep(matrix);
            }
        }
        public Direction ShortestStep(int[,] matrix)
        {
            Direction result = Direction.Down;

            if (X - 1 >= 0)
                if (matrix[X - 1, Y] == matrix[X, Y] - 1) result = Direction.Left;
            if (X + 1 < Game.play.Map.MapSize.X)
                if (matrix[X + 1, Y] == matrix[X, Y] - 1) result = Direction.Right;
            if (Y - 1 >= 0)
                if (matrix[X, Y - 1] == matrix[X, Y] - 1) result = Direction.Up;
            if (Y + 1 < Game.play.Map.MapSize.Y)
                if (matrix[X, Y + 1] == matrix[X, Y] - 1) result = Direction.Down;

            return result;
        }
        public int CountAvaliable()
        {
            int c = 0;
            foreach (Direction f in Enum.GetValues(typeof(Direction)))
            {
                if (CanMove(f)) c++;
            }
            return c;
        }
        public Direction ReverseMove(Direction d)
        {
            switch (d)
            {
                case Direction.Left:
                    return Direction.Right;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Left;
            }

            return Direction.Left;
        }
        public override void Draw()
        {
            if (!Dead)
            {
                base.Draw();

                char ObjectChar = ' ';
                if (White)
                    ObjectChar = Convert.ToChar(1);
                else
                    ObjectChar = Convert.ToChar(2);

                System.Console.Write(ObjectChar);
                System.Console.ResetColor();
            }
        }
    }
}
