using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace Hangman
{
    class Game
    {
        private GameLoop loop;

        // Array to read in the words
        private string[] Words;

        private int Guess;
        private int MainUIx, MainUIy, MainUIwidth, MainUIheight;
        private int StatusX, StatusY, StatusWidth, StatusHeight;
        private int GameWinX, GameWinY, GameWinWidth, GameWinHeight;
        private int GameOverX, GameOverY, GameOverWidth, GameOverHeight;
        private int HangmanX, HangmanY, HangStage, MaxHangStage;
        private string WordToGuess, LettersUsed, PlayerName;
        private char[] PlayerGuess;
        private ConsoleKeyInfo KeyInput;

        private Random randomGenerator;

        public Game(GameLoop loop)
        {
            this.loop = loop;

            // Load the words from the file
            Words = File.ReadAllLines("words.txt");

            randomGenerator = new Random();
            MainUIx = 1;
            MainUIy = 4;
            MainUIwidth = Console.WindowWidth - 4;
            MainUIheight = 20;
            StatusWidth = 45;
            StatusHeight = 2;
            // Set the status box in the center
            StatusX = (Console.WindowWidth / 2) - (StatusWidth / 2);
            // Set the status box just above the bottom of the screen
            StatusY = Console.WindowHeight - 10;

            GameWinWidth = 30;
            GameWinHeight = 5;
            // Center the game win box
            GameWinX = (Console.WindowWidth / 2) - (GameWinWidth / 2);
            GameWinY = (Console.WindowHeight / 2) - (GameWinHeight / 2);

            GameOverWidth = 30;
            GameOverHeight = 5;
            // Center the game over box
            GameOverX = (Console.WindowWidth / 2) - (GameOverWidth / 2);
            GameOverY = (Console.WindowHeight / 2) - (GameOverHeight / 2);

            HangmanX = MainUIx + 2;
            HangmanY = MainUIy + 3;
            // The maximum hangman stage, if you increase it you may want to edit the hangman drawing
            MaxHangStage = 5;
        }

        public void Init(string name)
        {
            HangStage = 0;
            Guess = 0;
            // Choose a random word
            WordToGuess = Words[randomGenerator.Next(0, Words.Length)];
            // Set the player guess array to the length of the word
            PlayerGuess = new char[WordToGuess.Length];
            // Loop through each letter, if is is a space, add '/' to the player guess, otherwise add a '?'
            for (int i = 0; i < WordToGuess.Length; i++)
                PlayerGuess[i] = (WordToGuess[i] == ' ') ? '/' : '?';

            PlayerName = name;
            // A string of letters used by the player
            LettersUsed = "";

            Console.Clear();
            Draw();
            DrawStatus("Press a key");
            DrawPlayerGuess();
        }

        public void Update()
        {
            // Get a key press
            KeyInput = Console.ReadKey(true);
            // If the key is a letter continue, otherwise state to
            if (char.IsLetter(KeyInput.KeyChar))
            {
                // Check if the user has already used the letter
                if (LettersUsed.Contains(KeyInput.KeyChar))
                    DrawStatus("'" + KeyInput.KeyChar + "' Already used, please choose another");
                else
                {
                    Guess++;
                    DrawGuess();

                    // Add the letter to the used string
                    LettersUsed += KeyInput.KeyChar;
                    DrawStatus("Press a letter");

                    // If the letter is in the word, update the player guess array
                    if (WordToGuess.Contains(KeyInput.KeyChar))
                    {
                        for (int i = 0; i < WordToGuess.Length; i++)
                        {
                            if (WordToGuess[i] == KeyInput.KeyChar)
                                PlayerGuess[i] = KeyInput.KeyChar;
                        }

                        DrawPlayerGuess();

                        // Check the player guess array for any '?', if so the game has been won!
                        if (PlayerGuess.Count(x => x == '?') == 0)
                            GameWon();
                    }
                    else
                    {
                        // The letter is not in the word, choose a random position and write it in
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.SetCursorPosition(randomGenerator.Next(MainUIx + 31, MainUIwidth), randomGenerator.Next(MainUIy + 7, MainUIheight));
                        Console.Write(KeyInput.KeyChar);
                        Console.BackgroundColor = ConsoleColor.Black;
                        HangStage++;
                    }
                }
            }
            else
                DrawStatus("Please press a letter");

            // Draw the hangman stage
            if (HangStage > 0)
            {
                DrawHangman();
                // If the hang stage equals the maximum, the game is over!
                if (HangStage == MaxHangStage)
                {
                    // Wait 1 second for the player to see the full hangman
                    Thread.Sleep(1000);
                    GameOver();
                }
            }
        }

        private void Draw()
        {
            // Draw player name
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(0, 0);
            Console.Write("Player: {0}   ", PlayerName);
            // Draw player guess count
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            Console.Write("║"); // Alt 186

            DrawGuess();

            // Draw the top border
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(0, 1);
            for (int w = 0; w < Console.WindowWidth; w++)
            {
                if (w == 10 + PlayerName.Length)
                    Console.Write("╩"); // Alt code 202
                else
                    Console.Write("═"); // Alt code 205, not =
            }

            ui.DrawBox(MainUIx, MainUIy, MainUIwidth, MainUIheight, ConsoleColor.Green);

            // Draw the inner box UI
            Console.SetCursorPosition(30, MainUIy);
            Console.Write("╦"); // Alt code 203
            Console.SetCursorPosition(30, MainUIy + MainUIheight);
            Console.Write("╩"); // Alt code 202
            Console.SetCursorPosition(Console.WindowWidth - 3, 9);
            Console.Write("╣"); // Alt code 185

            Console.SetCursorPosition(30, MainUIy);
            for(int h = 0; h < 19; h++)
            {
                Console.SetCursorPosition(30, Console.CursorTop + 1);
                if (h == 4)
                    Console.Write("╠"); // Alt code 204
                else
                    Console.Write("║"); // Alt code 186
            }

            Console.SetCursorPosition(31, 9);
            for (int w = 0; w < MainUIwidth - 30; w++)
                Console.Write("═"); // Alt code 205, not =

            // Draw status box
            ui.DrawBox(StatusX, StatusY, StatusWidth, StatusHeight, ConsoleColor.Yellow);

            // Draw UI strings
            Console.SetCursorPosition(MainUIx + 31, MainUIy + 1);
            Console.Write("Word to guess");
            Console.SetCursorPosition(MainUIx + 31, MainUIy + 6);
            Console.Write("Letter bin");
        }

        private void DrawGuess()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(12 + PlayerName.Length, 0);
            Console.Write("Guess No: {0}", Guess);
        }

        private void DrawStatus(string text)
        {
            // Clear the status box
            Console.SetCursorPosition(StatusX + 2, StatusY + 1);
            for (int i = 0; i < StatusWidth - 2; i++)
                Console.Write(' ');

            Console.SetCursorPosition(StatusX + 2, StatusY + 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(text);
        }

        private void DrawPlayerGuess()
        {
            Console.SetCursorPosition(MainUIx + 32, MainUIy + 3);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(PlayerGuess);
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private void GameWon()
        {
            Console.Clear();

            ui.DrawDialog(GameWinX, GameWinY, GameWinWidth, GameWinHeight, "You Win!", ConsoleColor.Green, ConsoleColor.Yellow);
            Console.SetCursorPosition(GameWinX + 2, GameWinY + 3);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Yippee! You did in {0} goes!", Guess);
            Console.ReadLine();
            // Change to the main menu state to view it after
            loop.CurrentState = GameLoop.GameStates.MainMenu;
        }

        private void GameOver()
        {
            Console.Clear();

            ui.DrawDialog(GameOverX, GameOverY, GameOverWidth, GameOverHeight, "Game Over!", ConsoleColor.Green, ConsoleColor.Yellow);
            Console.SetCursorPosition(GameOverX + 2, GameOverY + 3);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("The word was '{0}'", WordToGuess);
            Console.ReadLine();
            // Change to the main menu state to view it after
            loop.CurrentState = GameLoop.GameStates.MainMenu;
        }

        private void DrawHangman()
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;

            // Draw the hangman, each stage is drawn only once. DO NOT CALL Console.Clear() while updating
            switch(HangStage)
            {
                case 1:
                    #region STAGE 1
                    Console.ForegroundColor = ConsoleColor.White;
                    for(int w = 0; w < 12; w++)
                    {
                        Console.SetCursorPosition(HangmanX + w, HangmanY);
                        if (w == 0)
                            Console.Write("╔"); // Alt code 201
                        else if (w == 11)
                            Console.Write("╗"); // Alt code 187
                        else
                            Console.Write("═"); // Alt code 205, not =
                    }

                    for (int h = 1; h < 17; h++)
                    {
                        Console.SetCursorPosition(HangmanX, HangmanY + h);
                        Console.Write("║"); // Alt code 187
                    }

                    Console.SetCursorPosition(HangmanX, Console.CursorTop);
                    Console.Write("╚"); // Alt code 200

                    for (int w = 0; w < 12; w++)
                        Console.Write("═"); // Alt code 205, not =
                    break;
                    #endregion
                #region STAGE 2
                case 2:
                    Console.SetCursorPosition(HangmanX + 11, HangmanY + 1);
                    Console.Write("║"); // Alt code 186
                    Console.SetCursorPosition(HangmanX + 10, HangmanY + 2);
                    Console.Write("/ \\");
                    Console.SetCursorPosition(HangmanX + 9, HangmanY + 3);
                    Console.Write("|@ @|");
                    Console.SetCursorPosition(HangmanX + 10, HangmanY + 4);
                    Console.Write("\\_/");
                    break;
                #endregion
                #region STAGE 3
                case 3:
                    Console.SetCursorPosition(HangmanX + 11, HangmanY + 5);
                    Console.Write("|");
                    Console.SetCursorPosition(HangmanX + 11, HangmanY + 6);
                    Console.Write("|");
                    Console.SetCursorPosition(HangmanX + 11, HangmanY + 7);
                    Console.Write("|");
                    Console.SetCursorPosition(HangmanX + 11, HangmanY + 8);
                    Console.Write("|");
                    Console.SetCursorPosition(HangmanX + 11, HangmanY + 9);
                    Console.Write("|");
                    Console.SetCursorPosition(HangmanX + 11, HangmanY + 10);
                    Console.Write("|");
                    break;
                #endregion
                #region STAGE 4
                case 4:
                    Console.SetCursorPosition(HangmanX + 10, HangmanY + 11);
                    Console.Write("/ \\");
                    Console.SetCursorPosition(HangmanX + 9, HangmanY + 12);
                    Console.Write("/   \\");
                    break;
                #endregion
                #region STAGE 5
                case 5:
                    Console.SetCursorPosition(HangmanX + 10, HangmanY + 6);
                    Console.Write("/|\\");
                    Console.SetCursorPosition(HangmanX + 9, HangmanY + 7);
                    Console.Write("/ | \\");
                    Console.SetCursorPosition(HangmanX + 8, HangmanY + 8);
                    Console.Write("/  |  \\");
                    break;
                #endregion
            }
        }
    }
}
