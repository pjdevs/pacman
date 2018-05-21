using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    public static class InputManager
    {
        private static KeyboardState state;

        public static void GetInputs()
        {
            state = Keyboard.GetState();
        }

        public static bool IsKeyDown(Keys key)
        {
            return state.IsKeyDown(key);
        }
        public static bool IsKeyUp(Keys key)
        {
            return state.IsKeyUp(key);
        }
    }
}
