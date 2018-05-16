using System;

namespace Pacman
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (PacmanGame game = new PacmanGame())
            {
                game.Run();
            }
        }
    }
}

