using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    public static class InputManager // Classe statique qui donne les entrées au clavier à tout moment
    {
        private static KeyboardState state;

        //Fonction qui récupère l'état du clavier
        public static void GetInputs()
        {
            state = Keyboard.GetState();
        }

        // Fonction qui retourne vrai si la touche donnée est enfoncée et faux sinon
        public static bool IsKeyDown(Keys key)
        {
            return state.IsKeyDown(key);
        }
        // Fonction qui retourne vrai si la touche donnée est pas enfoncée et faux sinon
        public static bool IsKeyUp(Keys key)
        {
            return state.IsKeyUp(key);
        }
    }
}
