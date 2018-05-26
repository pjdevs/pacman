using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    public static class Constants // Classe qui définit toutes les constantes du jeu
    {
        public static readonly int WINDOW_WIDTH = 400,
                                   WINDOW_HEIGHT = 400,

                                   TILE_WIDTH = 16,
                                   TILE_HEIGHT = 16,

                                   MAP_LENGHT = 25,

                                   ANIMATION_SPEED = 4,

                                   POINT_SCORE = 10,
                                   SUPER_SCORE = 200,

                                   MOVE_SPEED = 2,
                                   SLOW_MOVE_SPEED = 1,
                                   INVINCIBLE_TIME = 300,
                                   TURN_TIME_MIN = 40,
                                   TURN_TIME_MAX = 150,

                                   FANTOM_LEFT = 0,
                                   FANTOM_UP = TILE_WIDTH,
                                   FANTOM_RIGHT = 2 * TILE_WIDTH,
                                   FANTOM_DOWN = 3 * TILE_WIDTH,

                                   FANTOMS_INDEX = 3,
                                   PLAYER_INDEX = 4;

    }
}
