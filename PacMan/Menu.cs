using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan
{
    enum MenuState { Main, Options, Help, Records }
    class Menu
    {
        public short Selected = 0;
        public Options Opts;
        private MenuState sState = MenuState.Main;
        public MenuState State
        {
            set
            {
                sState = value;
                fShow();
            }
            get { return sState; }
        }
        public MenuEntry[] Entrys = new MenuEntry[5];

        public Menu()
        {
            Selected = 0;
        }
        public void Show()
        {
            System.Console.Clear();

            FigletText pacman = new FigletText(35, 1, "pacman", true, ConsoleColor.Yellow);
            Entrys[0] = new MenuEntry("New game", 25, true);
            Entrys[1] = new MenuEntry("Settings", 26, false);
            Entrys[2] = new MenuEntry("Hall of Fame", 27, false);
            Entrys[3] = new MenuEntry("Help", 28, false);
            Entrys[4] = new MenuEntry("Exit", 30, false);
            ShowMain();
        }
        public void fShow()
        {
            switch (State)
            {
                case MenuState.Main: ShowMain();
                    break;
                case MenuState.Options: ShowOptions();
                    break;
                case MenuState.Help: ShowHelp();
                    break;
                case MenuState.Records: ShowRecords();
                    break;
            }
        }
        public void ShowMain()
        {
            ClearSquare();
            Drawing.DrawHorLine(0, 33, System.Console.BufferWidth);
            Drawing.DrawHorLine(0, 13, System.Console.BufferWidth);
            Entrys[0].Show();
            Entrys[1].Show();
            Entrys[2].Show();
            Entrys[3].Show();
            Entrys[4].Show();
        }
        public void ShowHelp()
        {
            string h = "Help";
            Drawing.Write((Console.BufferWidth - h.Length) / 2, 13, h);

            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            ClearSquare();
            System.Console.ResetColor();
            Drawing.DrawBorder(new Point(20, 15), new Size(70, 17), ConsoleColor.DarkYellow, ConsoleColor.DarkYellow);
            Game.play.InnerMessage("Press space to return back");

            Drawing.Write(24, 17, "PacMan.NET is a remake on a popular arcade game called Pac-Man.");
            Drawing.Write(22, 18, "You are playing as a yellow charracter. The point of the game is to");
            Drawing.Write(22, 19, "beat the score record. To reach the next level you have to eat all");
            Drawing.Write(22, 20, "of the berrys(dots) seen on the screen.");
            Drawing.Write(24, 21, "Be aware of the ghosts. By touching them you lose one life, and");
            Drawing.Write(22, 22, "have to replay the whole level. They move randomly, but suddenly");
            Drawing.Write(22, 23, "they become red and start chasing you for some time. To defend");
            Drawing.Write(22, 24, "yourself, you can eat special berrys(purple), after that you become");
            Drawing.Write(22, 25, "a ghost-hunter for some time and by touching ghosts you kill them.");
            Drawing.Write(24, 28, "To control PacMan use arrow keys on your keyboard. To pause press");
            Drawing.Write(22, 29, "spacebar. To finnish the game and return to the menu press Escape.");
        }
        public void ShowOptions()
        {
            Opts = new Options();
        }
        public void ShowRecords()
        {
            string h = "Hall of Fame";
            Drawing.Write((Console.BufferWidth - h.Length) / 2, 13, h);

            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            ClearSquare();
            System.Console.ResetColor();
            Drawing.DrawBorder(new Point(20, 15), new Size(70, 17), ConsoleColor.DarkYellow, ConsoleColor.DarkYellow);
            Game.play.InnerMessage("Press space to return back");

            Drawing.DrawHorLine(22, 20, 66);
            Drawing.DrawVertLine(29, 17, 12);
            Drawing.DrawVertLine(56, 17, 12);

            Drawing.Write(23, 18, "Place");
            Drawing.Write(39, 18, "Nickname");
            Drawing.Write(70, 18, "Score");
            for (int i = 1; i < 4; i++)
            {
                Record c = Game.settings.GetRecord(i);
                Drawing.Write(25, 20 + i * 2, i.ToString());
                Drawing.Write(32, 20 + i * 2, c.Name);
                Drawing.Write(69, 20 + i * 2, Drawing.FillZeros(c.Score,8));
            }
        }
        public void ClearSquare()
        {
            Drawing.DrawRect(20, 15, 70, 17);
        }
        public void KeyHandler(ConsoleKeyInfo Key)
        {
            #region Hanler for main menu
            if (State == MenuState.Main)
            {
                if (Key.Key == ConsoleKey.DownArrow)
                {
                    Entrys[Selected].Selected = false;
                    if (Selected == 4) { Selected = 0; }
                    else { Selected++; }
                    Entrys[Selected].Selected = true;
                }
                else if (Key.Key == ConsoleKey.UpArrow)
                {
                    Entrys[Selected].Selected = false;
                    if (Selected == 0) { Selected = 4; }
                    else { Selected--; }
                    Entrys[Selected].Selected = true;
                }
                else if (Key.Key == ConsoleKey.Enter)
                {
                    if (Selected == 0) Game.State = GameState.Game;
                    else if (Selected == 1) State = MenuState.Options;
                    else if (Selected == 2) State = MenuState.Records;
                    else if (Selected == 3) State = MenuState.Help;
                    else if (Selected == 4) Game.State = GameState.Exit;
                }
            }
            #endregion
            #region Handler for Help or Record table
            else if ((State == MenuState.Help) || (State == MenuState.Records))
            {
                if (Key.Key == ConsoleKey.Spacebar)
                {
                    State = MenuState.Main;
                }
            }
            #endregion
            else if (State == MenuState.Options)
            {
                Opts.KeyHandler(Key);
            }
        }
    }
    class Options
    {
        public int Selected = 0;
        public Edit NickName = new Edit(25, 20, 15);
        public Button Save = new Button("Save", new Point(83, 26), false);
        public Button Back = new Button("Back", new Point(83, 28), false);
        public Options()
        {
            string h = "Settings";
            Drawing.Write((Console.BufferWidth - h.Length) / 2, 13, h);
            Game.menu.ClearSquare();
            Drawing.DrawBorder(new Point(20, 15), new Size(70, 17), ConsoleColor.DarkYellow, ConsoleColor.DarkYellow);

            Drawing.Write(25, 19, "Nickname:");


            NickName.Draw();
            NickName.Selected = true;
            NickName.MaxLen = 14;
            NickName.Text = Game.settings.Nick;
            NickName.Cursor = NickName.Text.Length;
            Save.Show();
            Back.Show();
        }
        public void KeyHandler(ConsoleKeyInfo Key)
        {
            if (Key.Key == ConsoleKey.UpArrow)
            {
                if (Selected == 0) { Selected = 2; }
                else { Selected--; }
            }
            else if (Key.Key == ConsoleKey.DownArrow)
            {
                if (Selected == 2) { Selected = 0; }
                else { Selected++; }
            }
            if (Selected == 0)
            {
                NickName.Selected = true;
                Save.Selected = false;
                Back.Selected = false;
                NickName.KeyHandler(Key);
            }
            if (Selected == 1)
            {
                NickName.Selected = false;
                Save.Selected = true;
                Back.Selected = false;
                if (Key.Key == ConsoleKey.Enter)
                {
                    Game.settings.Nick = NickName.Text;
                }
            }
            if (Selected == 2)
            {
                NickName.Selected = false;
                Save.Selected = false;
                Back.Selected = true;
                if (Key.Key == ConsoleKey.Enter)
                {
                    Game.menu.State = MenuState.Main;
                   // Selected = 0;
                }
            }
        }
    }
}
