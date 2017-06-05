using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class ui
    {
        static public void DrawBox(int x, int y, int width, int height, ConsoleColor colour = ConsoleColor.Gray)
        {
            Console.ForegroundColor = colour;

            // Draw a box with the given position & size
            for (int w = 0; w <= width; w++)
            {
                for (int h = 0; h <= height; h++)
                {
                    Console.SetCursorPosition(x + w, y + h);

                    if (w == 0 && h == 0)
                        Console.Write("╔"); // Alt code 201
                    else if (w == width && h == 0)
                        Console.Write("╗"); // Alt code 187
                    else if (w == 0 && h == height)
                        Console.Write("╚"); // Alt code 200
                    else if (w == width && h == height)
                        Console.Write("╝"); // Alt code 188
                    else if (h == 0 || h == height)
                        Console.Write("═"); // Alt code 205, not =
                    else if (w == 0 || w == width)
                        Console.Write("║"); // Alt code 186
                }
            }
        }

        static public void DrawDialog(int x, int y, int width, int height, string title, ConsoleColor titleColour = ConsoleColor.Green, ConsoleColor colour = ConsoleColor.Gray)
        {
            // Make sure the title can fit in, if not update the width
            if (title.Length > width)
                width = title.Length;

            DrawBox(x, y, width, height, colour);

            Console.ForegroundColor = titleColour;
            Console.SetCursorPosition(Console.WindowWidth / 2 - title.Length / 2, y + 1);
            Console.Write(title);

            // Draw the horizontal line to seperate the title
            Console.SetCursorPosition(x + 1, y + 2);
            Console.ForegroundColor = colour;
            for (int i = 0; i < width - 1; i++) Console.Write("═");

            Console.SetCursorPosition(x, y + 2);
            Console.Write("╠"); // Alt code 204
            Console.SetCursorPosition(x + width, y + 2);
            Console.Write("╣"); // Alt code 185
        }
    }
}
