using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Threading;

namespace PacMan
{
    enum PlayState { Play, Pause, Win, Dead, Level, Lost };
    class Play
    {
        public int Level = 1;
        public int Levels = 1;
        public int Lives = 3;
        public Int64 Score = 0;
        public Point Offset = new Point(0, 0);
        public PlayState State = PlayState.Level;
        public PacMap Map;

        public PacMan Player = new PacMan(new Point(0,0));
        public List<Ghost> Enemies = new List<Ghost>();

        public Play()
        {
            Levels = System.IO.Directory.GetFiles("Maps").Length;
            Level = 0;

            NextLevel();

            Offset.Y = 5;
        }
        public void Pause()
        {
            State = PlayState.Pause;
            WriteLevelInfo();
            int y = System.Console.BufferHeight / 2 - 5;
            FigletText Plogo = new FigletText(40, y, "pause", false, ConsoleColor.DarkMagenta);
            Drawing.DrawBorder(new Point(39, y-1), new Size(31, 8), ConsoleColor.Blue, ConsoleColor.DarkBlue);
        }
        public void UnPause()
        {
            State = PlayState.Play;
            Show();
            WriteLevelInfo();
            DrawLives();
        }

        public void Show(){
            System.Console.Clear();
            Drawing.DrawBorder(new Point(Offset.X - 1, Offset.Y - 1), new Size(Map.MapSize.X + 2, Map.MapSize.Y + 2), ConsoleColor.Black, ConsoleColor.DarkCyan);
            Map.Draw();
            Player.Draw();
            WriteLevelInfo();
            WriteInfo();
            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Draw();
            }
            DrawLives();
            Drawing.DrawBorder(new Point(Offset.X / 2 - 9, Offset.Y - 1), new Size(18, 7), ConsoleColor.Black, ConsoleColor.DarkCyan);
            Drawing.DrawBorder(new Point(Offset.X / 2 - 9, Offset.Y + 7), new Size(18, 7), ConsoleColor.Black, ConsoleColor.DarkCyan);
            Drawing.Write(Offset.X / 2 - 7, Offset.Y + 1, "Current score:");
            Drawing.Write(Offset.X / 2 - 7, Offset.Y + 9, "Score to beat:");

            System.Console.ResetColor();

        }
        public void DrawObjects()
        {

            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Draw();
            }
            if (State != PlayState.Dead)
            {
                Player.Draw();
            }
        }
        public void WriteLevelInfo()
        {
            if (State == PlayState.Level)
            {
                UpperMessage("Entering level " + Level.ToString() + "...");
                InnerMessage("Press spacebar to start");
            }
            else if (State == PlayState.Lost)
            {
                UpperMessage("Reentering level " + Level.ToString() + "...");
                InnerMessage("Press spacebar to restart");
            }
            else if (State == PlayState.Play)
            {
                UpperMessage("Level " + Level.ToString());
                InnerMessage("Press spacebar to pause");
            }
            else if (State == PlayState.Pause)
            {
                InnerMessage("Press spacebar to pause");
            }
            else if (State == PlayState.Dead)
            {
                UpperMessage("You're dead!");
                InnerMessage("Press spacebar to return to the menu");
            }
            else if (State == PlayState.Win)
            {
                UpperMessage("Congratulations, you have won!");
                InnerMessage("Press space to return to the menu");
            }
        }
        public void WriteInfo()
        {
            int y = Offset.Y;
            int x = Offset.X + Map.MapSize.X + 3;

            Drawing.DrawHorLine(x + 16, y, 6);
            Drawing.DrawHorLine(x + 14, y + 1, 7);

            Drawing.Write(x, y, "Dots remaining: " + Map.TotalDots.ToString());
            Drawing.Write(x, y + 1, "Ghost hunting: " + Player.SuperLeft.ToString());

            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.BackgroundColor = ConsoleColor.DarkCyan;
            Drawing.Write(Offset.X / 2 - 4, Offset.Y + 3, Drawing.FillZeros(Score, 8));

            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.BackgroundColor = ConsoleColor.DarkRed;

            Drawing.Write(Offset.X / 2 - 4, Offset.Y + 11, Drawing.FillZeros(Game.settings.GetRecord(3).Score, 8));

            System.Console.ResetColor();
        }
        public void DrawLives()
        {
            int y = Offset.Y;
            int x = Offset.X + Map.MapSize.X + 3;
            Drawing.Write(x, y+3, "Pacman lifes: ");

            System.Console.ForegroundColor = ConsoleColor.Yellow;
            for (int l = 0; l < 5; l++)
            {
                if (Lives <= l)
                {
                    System.Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                System.Console.Write('<');
            }
            System.Console.ResetColor();
        }
        public void UpperMessage(string Text)
        {
            UpperMessage(Text, Offset.Y - 3, ConsoleColor.Gray);
        }
        public void UpperMessage(string Text, int Line, ConsoleColor bg)
        {
            string info = Text;
            int mx = (System.Console.BufferWidth - info.Length) / 2;

            Drawing.DrawHorLine(0, Line, System.Console.BufferWidth);

            System.Console.BackgroundColor = bg;
            System.Console.ForegroundColor = ConsoleColor.Black;

            Drawing.DrawHorLine(Offset.X - 1, Line, Map.MapSize.X + 2);
            Drawing.Write(mx, Line, info);

            System.Console.ResetColor();
        }
        public void InnerMessage(string Text){
            System.Console.BackgroundColor = ConsoleColor.Gray;
            System.Console.ForegroundColor = ConsoleColor.Black;

            string info = Text;
            int my = System.Console.BufferHeight - 2;
            int mx = (System.Console.BufferWidth - info.Length) / 2;

            Drawing.DrawHorLine(0, my, System.Console.BufferWidth);
            Drawing.Write(mx, my, info);

            System.Console.ResetColor();
        }

        public void KeyHandler()
        {
            if ((State == PlayState.Level) || (State == PlayState.Lost))
            {
                #region Key handler while entering new level or life lost
                while ((State == PlayState.Level) || (State == PlayState.Lost))
                {
                    ConsoleKeyInfo Key = System.Console.ReadKey(true);
                    if (Key.Key == ConsoleKey.Spacebar)
                    {
                        Show();
                        State = PlayState.Play;
                        WriteLevelInfo();
                    }
                }
                KeyHandler();
                #endregion
            }
            else if (State == PlayState.Play)
            {
                #region Key handler while playing
                

                Direction md = Player.D;
                while (State == PlayState.Play)
                {
                    if (Console.KeyAvailable)
                    {
                        ushort Key = KeyPressed.GetLastPressedCode();
                        if (Key == 40) md = Direction.Down;
                        else if (Key == 38) md = Direction.Up;
                        else if (Key == 37) md = Direction.Left;
                        else if (Key == 39) md = Direction.Right;
                        else if (Key == 32)
                        {
                            Pause();
                        }
                        else if (Key == 27)
                        {
                            GameOver();
                        }
                    }
                    if (State != PlayState.Dead)
                    {
                        if (State != PlayState.Pause)
                        {
                            MoveGhosts();
                            Player.Move(md);
                            CheckCollisions();
                        }
                        if (State == PlayState.Play || State == PlayState.Level)
                        {
                            DrawObjects();
                            if (Player.Super) Player.GoSpecial();
                        }
                    }
                    System.Threading.Thread.Sleep(250);
                }
                KeyHandler();
                #endregion
            }
            else if (State == PlayState.Pause)
            {
                #region Key handler while paused
                while (State == PlayState.Pause)
                {
                    ConsoleKeyInfo Key = System.Console.ReadKey(true);
                    if (Key.Key == ConsoleKey.Spacebar)
                    {
                        UnPause();
                    }
                }
                KeyHandler();
                #endregion
            }
            else if ((State == PlayState.Win) || (State == PlayState.Dead))
            {
                #region Key handler when won or died
                while ((State == PlayState.Win) || (State == PlayState.Dead))
                {
                    ConsoleKeyInfo Key = System.Console.ReadKey(true);
                    if (Key.Key == ConsoleKey.Spacebar)
                    {
                        State = PlayState.Level;
                        Game.State = GameState.Menu;
                    }
                }
                #endregion
            }
        }

        public void CheckCollisions()
        {
            foreach (Ghost g in Enemies)
            {
                if (EnemyPlayerCollision(g))
                {
                    if (Player.Super)
                    {
                        g.Dead = true;
                    }
                    else if (!g.Dead)
                    {
                        Lost();
                        break;
                    }
                }
            }
            if (State == PlayState.Lost)
            {
                //LoadLevel();
                LoadObjects();
                Show();
                int y = System.Console.BufferHeight / 2 - 5;
                FigletText go = new FigletText(36, y, "lo", false, ConsoleColor.Magenta);
                Drawing.DrawBorder(new Point(35, y - 1), new Size(40, 8), ConsoleColor.White, ConsoleColor.DarkMagenta);
            }
        }
        public bool EnemyPlayerCollision(Ghost enemy)
        {
            if ((enemy.X == Player.X) && (enemy.Y == Player.Y))
                return true;
            else
            {
                //Horizontal - enemy on left
                if ((enemy.X + 1 == Player.X) &&
                    (enemy.Y == Player.Y) &&
                    (enemy.D == Direction.Left) &&
                    (Player.D == Direction.Right)) return true;
                //Horizontal - enemy on right
                else if ((enemy.X - 1 == Player.X) &&
                    (enemy.Y == Player.Y) &&
                    (enemy.D == Direction.Right) &&
                    (Player.D == Direction.Left)) return true;
                //Vertical - enemy up
                else if ((enemy.Y + 1 == Player.Y) &&
                    (enemy.X == Player.X) &&
                    (enemy.D == Direction.Up) &&
                    (Player.D == Direction.Down)) return true;
                //Vertical - enemy down
                else if ((enemy.Y - 1 == Player.Y) &&
                    (enemy.X == Player.X) &&
                    (enemy.D == Direction.Down) &&
                    (Player.D == Direction.Up)) return true;
                else return false;
            }
        }

        public void MoveGhosts()
        {
            foreach (Ghost g in Enemies){
                g.FindDirection();
                g.Move(g.D);
            }
        }

        public void NextLevel()
        {
            Level += 1;
            LoadLevel();
            State = PlayState.Level;
        }
        public void LoadObjects()
        {
            Player = new PacMan(Map.StartPoint);
            Player.D = Map.StartDirection;

            Enemies.Clear();
            for (int n = 0; n < Map.Enemies.Count; n++)
            {
                Ghost en = new Ghost(Map.Enemies[n], false);
                en.Color = ConsoleColor.White;
                if (n % 2 != 0) en.White = true;
                Enemies.Add(en);
            }
        }
        public void LoadLevel()
        {
            Map = new PacMap(Level);
            Offset.X = (System.Console.BufferWidth - Map.MapSize.X) / 2;

            LoadObjects();
        }

        public void Win()
        {
            State = PlayState.Win;
            System.Console.Clear();
            WriteLevelInfo();
            NewRecord();
        }
        public void Lost()
        {
            if (Lives > 1)
            {
                Lives -= 1;
                DrawLives();
                State = PlayState.Lost;
            }
            else
            {
                GameOver();
            }
        }
        public void GameOver()
        {
            Console.ResetColor();

            State = PlayState.Dead;
            WriteInfo();
            int y = System.Console.BufferHeight / 2 - 5;
            FigletText go = new FigletText(28, y, "go", false, ConsoleColor.Red);
            Drawing.DrawBorder(new Point(27, y - 1), new Size(54, 8), ConsoleColor.White, ConsoleColor.DarkRed);
            WriteLevelInfo();
            NewRecord();

        }
        public void NewRecord()
        {
            for (int i = 1; i <=3; i++)
            {
                if (Score > Game.settings.GetRecord(i).Score)
                {
                    UpperMessage("You've set new high score on "+ i.ToString() +" place with "+ Score.ToString() +" points!!!", System.Console.BufferHeight -4, ConsoleColor.Green);
                    Game.settings.SetRecord(i, Game.settings.Nick, Score);
                    break;
                }
            }
        }

    }
}