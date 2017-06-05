using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class UserMenu
    {
        public List<string> MenuItems { get; set; }
        public int SelectedIndex { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public ConsoleColor TextColour { get; set; }
        public ConsoleColor SelectedColour { get; set; }

        private ConsoleKeyInfo KeyInput;

        public void Create(List<string> menuItems, int x = 0, int y = 0, int selected = 0, ConsoleColor textColour = ConsoleColor.Gray, ConsoleColor selectedColour = ConsoleColor.Red)
        {
            MenuItems = menuItems;
            StartX = x;
            StartY = y;
            SelectedIndex = selected;
            TextColour = textColour;
            SelectedColour = selectedColour;
        }

        public bool Update()
        {
            KeyInput = Console.ReadKey(true);
                
            // Scroll the menu
            switch (KeyInput.Key)
            {
                case ConsoleKey.UpArrow:
                    SelectedIndex = (SelectedIndex > 0) ? SelectedIndex -= 1 : MenuItems.Count - 1;
                    return false;
                case ConsoleKey.DownArrow:
                    SelectedIndex = (SelectedIndex < MenuItems.Count - 1) ? SelectedIndex += 1 : 0;
                    return false;
                case ConsoleKey.Enter:
                    return true;
            }
            return false;
        }

        public void Draw()
        {
            Console.SetCursorPosition(StartX, StartY + 2);

            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.SetCursorPosition(StartX + 2, Console.CursorTop + 1);
                if (i == SelectedIndex)
                {
                    Console.ForegroundColor = SelectedColour;
                    Console.Write("-> ");
                }
                else
                {
                    Console.ForegroundColor = TextColour;
                    Console.Write("   ");
                }

                Console.Write(MenuItems[i]);
            }
        }
    }
}
