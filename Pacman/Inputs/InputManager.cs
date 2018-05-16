using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    static class InputManager
    {
        private static KeyboardState lastState;
        private static KeyboardState state;

        public static void GetState()
        {
            lastState = state;
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

        public static bool IsKeyReleased(Keys key)
        {
            return lastState.IsKeyDown(key) && state.IsKeyUp(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return state.IsKeyDown(key) && lastState.IsKeyUp(key);
        }
    }
}
