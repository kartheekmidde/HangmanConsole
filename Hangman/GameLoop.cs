using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class GameLoop
    {
        public enum GameStates
        {
            MainMenu, Game
        };

        public string GameName { get; private set; }
        public int GameWidth { get; private set; }
        public int GameHeight { get; private set; }

        public bool GameRunning { get; set; }

        private GameStates currentState;
        public GameStates CurrentState
        {
            get { return currentState; }
            set
            {
                // When the state is changed, initalise it
                currentState = value;
                Console.Clear();
                if (value == GameStates.MainMenu)
                    mainMenu.Init();
                else if (value == GameStates.Game)
                    game.Init(mainMenu.PlayerName);
            }
        }

        private mainmenu mainMenu;
        private Game game;

        public void Init()
        {
            GameName = "Hangman";
            GameWidth = 90;
            GameHeight = 40;
            GameRunning = true;
            
            // Setup the window
            Console.SetWindowSize(GameWidth, GameHeight);
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.CursorVisible = false;
            Console.Title = GameName;

            // Create new instances of the main menu and game
            mainMenu = new mainmenu(this);
            game = new Game(this);

            // Set the main menu as the current state
            CurrentState = GameStates.MainMenu;
        }

        public void Loop()
        {
            while(GameRunning)
            {
                switch(CurrentState)
                {
                    case GameStates.MainMenu:
                        mainMenu.Update();
                        break;
                    case GameStates.Game:
                        game.Update();
                        break;
                }
            }
        }
    }
}
