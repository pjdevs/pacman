using System;

namespace Pacman
{
    static class Program // Classe de demmarage du proramme
    {
        static void Main(string[] args) // Fonction d'entrée du programme
        {
            // On décare le jeu et on le lance
            using (PacmanGame game = new PacmanGame())
            {
                game.Run();
            }
        }
    }
}

