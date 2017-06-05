using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class main
    {
        static void Main(string[] args)
        {
            GameLoop game = new GameLoop();
            game.Init();
            game.Loop();
        }
    }
}
