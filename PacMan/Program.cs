using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PacMan
{
    enum GameState { Game, Menu, Exit };
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.CursorVisible = false;
            System.Console.SetBufferSize(110, 35);
            System.Console.SetWindowSize(110, 35);
            System.Console.Title = "Pacman.NET";

            Game.Start();
        }

    }


    static class Game
    {
        public static GameState State = GameState.Menu;
        public static Menu menu = new Menu();
        public static Play play = new Play();
        public static SettingParser settings = new SettingParser();
        public static Random randomizer = new Random();


        public static void Show()
        {
            if (State == GameState.Menu)
            {
                menu.Show();
            }else if (State == GameState.Game)
            {
                play.Show();
            }
        }
        public static void KeyHandler()
        {
            ConsoleKeyInfo Key;
            if (Game.State == GameState.Menu)
            {
                while (Game.State == GameState.Menu)
                {
                    Key = System.Console.ReadKey(true);
                    menu.KeyHandler(Key);
                }
            }
            else if (Game.State == GameState.Game)
            {
                play.KeyHandler();
            }

            if (Game.State != GameState.Exit)
            {
                Create();
                Show();
                KeyHandler();
            }
        }

        public static void Create()
        {
            menu = new Menu();
            play = new Play();
        }

        public static void Start()
        {
            Show();
            KeyHandler();
        }
    }
}
