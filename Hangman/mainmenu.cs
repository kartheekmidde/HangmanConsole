using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

namespace Hangman
{
    class mainmenu
    {
        private GameLoop loop;
        private UserMenu menu;

        private List<string> MenuItems = new List<string>
        {
            "New Game", "Credits", "Exit"
        };

        private string[] Credits = new string[]
        {
            "Hangman - Console Version", "Created by: Dheeraj, Anusha, Kartheek", "University of Cincinnati", "", "© UCinn 2016"
        };

        private int MenuWidth;
        private int MenuHeight;
        private int StartX;
        private int StartY;
        private int NewGameX;
        private int NewGameY;
        private int NewGameWidth;

        private int NewGameHeight;
        private int CreditsX, CreditsY, CreditsWidth, CreditsHeight;

        public string PlayerName { get; private set; }

        public mainmenu(GameLoop loop)
        {
            // Refrence to the game loop
            this.loop = loop;
            
            menu = new UserMenu();
        }

        public void Init()
        {
            /* Get the minimum size for the menu
             * The initial value is the length of the game name
             */
            MenuWidth = loop.GameName.Length;
            MenuHeight = 3 + MenuItems.Count();

            NewGameWidth = 20;
            NewGameHeight = 4;

            // Center the new game window
            NewGameX = (Console.WindowWidth / 2) - NewGameWidth / 2;
            NewGameY = (Console.WindowHeight / 2) - NewGameHeight / 2;

            // Calculate the size of the credits menu depending on the length of the credits
            CreditsWidth = 0;
            CreditsHeight = Credits.Length + 3;

            foreach (string credit in Credits)
                if (credit.Length > CreditsWidth) CreditsWidth = credit.Length;

            CreditsWidth += 3;
            // Center the credits menu
            CreditsX = (Console.WindowWidth / 2) - CreditsWidth / 2;
            CreditsY = (Console.WindowHeight / 2) - CreditsHeight / 2;

            foreach (string item in MenuItems)
            {
                if (item.Length > MenuWidth)
                    MenuWidth = item.Length;
            }

            MenuWidth += 6;

            StartX = (Console.WindowWidth - MenuWidth) / 2;
            StartY = (Console.WindowHeight - MenuHeight) / 2;

            menu.Create(MenuItems, StartX, StartY);

            Draw();
        }

        public void Update()
        {
            if (menu.Update())
            {
                switch (menu.SelectedIndex)
                {
                    case 0:
                        SetupNewGame();
                        break;
                    case 1:
                        ShowCredits();
                        break;
                    case 2:
                        Environment.Exit(0);
                        return;
                }
            }
            else
                menu.Draw(); // Redraw the menu
        }

        public void Draw()
        {
            Console.Clear();

            ui.DrawDialog(StartX, StartY, MenuWidth, MenuHeight, loop.GameName, ConsoleColor.Green, ConsoleColor.Yellow);

            menu.Draw();

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Created by: Dheeraj Inampudi, Durga Anusha Inukonda, Kartheek Midde");
        }

        public void SetupNewGame()
        {
            Console.Clear();

            ui.DrawDialog(NewGameX, NewGameY, NewGameWidth, NewGameHeight, "New Game", ConsoleColor.Green, ConsoleColor.Yellow);
            Console.SetCursorPosition(NewGameX + 2, NewGameY + 3);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.CursorVisible = true;
            PlayerName = Console.ReadLine();
            Console.CursorVisible = false;

            // Change the game state to start the game!
            loop.CurrentState = GameLoop.GameStates.Game;
        }

        public void ShowCredits()
        {
            Console.Clear();

            ui.DrawDialog(CreditsX, CreditsY, CreditsWidth, CreditsHeight, "Credits", ConsoleColor.Green, ConsoleColor.Yellow);

            Console.SetCursorPosition(CreditsX, CreditsY + 2);
            Console.ForegroundColor = ConsoleColor.Gray;

            foreach(string credit in Credits)
            {
                Console.SetCursorPosition(CreditsX + 2, Console.CursorTop + 1);
                Console.Write(credit);
            }

            Console.ReadLine();
            
            // Redraw the main menu
            Draw();
        }
    }
}
