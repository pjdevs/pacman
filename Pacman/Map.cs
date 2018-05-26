using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static Pacman.Constants // Pour avoir accès aux constantes

namespace Pacman
{
    public enum Case // Enumération associant un nombre à une case de la map
    {
        Vide = 0, Mur = 1, Point = 2, Super = 3
    }

    public class Map // Class représentant la map
    {
        //Variables membres de la classe
        private Texture2D map;
        private Texture2D items;

        public bool IsEmpty { get; private set; } // Vaut true si il ny a plus de points
        public int[,] MapInfo { get; private set; } // Tableau de nombres représentant la map
        
        // Constructeur
        public Map()
        {
            ResetMap();
        }

        // Fonction qui remet la map d'origine
        public void ResetMap()
        {
            MapInfo = new int[25, 25]
            {
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 1},
                {1, 2, 1, 1, 1, 2, 1, 1, 2, 1, 1, 2, 1, 2, 1, 1, 2, 1, 1, 2, 1, 1, 1, 2, 1},
                {1, 2, 1, 1, 1, 2, 1, 1, 2, 1, 1, 2, 1, 2, 1, 1, 2, 1, 1, 2, 1, 1, 1, 2, 1},
                {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
                {1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1},
                {1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1},
                {1, 1, 2, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 2, 1, 1},
                {1, 1, 2, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1},
                {1, 1, 1, 2, 1, 1, 1, 1, 2, 1, 1, 1, 0, 1, 1, 1, 2, 1, 1, 1, 1, 2, 1, 1, 1},
                {0, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 0},
                {1, 2, 1, 2, 1, 1, 1, 1, 2, 1, 0, 0, 0, 0, 0, 1, 2, 1, 1, 1, 1, 2, 1, 2, 1},
                {1, 2, 1, 2, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 1, 2, 1},
                {1, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 1},
                {1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1},
                {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
                {1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1},
                {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
                {1, 1, 1, 1, 2, 1, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1},
                {1, 2, 2, 2, 3, 1, 2, 2, 2, 1, 2, 1, 1, 1, 2, 1, 2, 2, 2, 1, 2, 2, 2, 2, 1},
                {1, 2, 1, 1, 1, 1, 1, 1, 2, 1, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 2, 1},
                {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };
            IsEmpty = false;
        }

        // Fonction qui charge les textures
        public void LoadContent(ContentManager content) 
        {
            map = content.Load<Texture2D>("map");
            items = content.Load<Texture2D>("items");
        }
        // Fonction pour dessiner la map et les points
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(map, Vector2.Zero, Color.White);

            // Affiche tout les points et regarde si la map est vide
            bool point = false;

            for (int i=0; i<MAP_LENGHT; i++)
            {
                for (int j=0; j<MAP_LENGHT; j++)
                {
                    if (MapInfo[j, i] == (int)Case.Point)
                    {
                        spriteBatch.Draw(items, new Vector2(i * TILE_WIDTH, j * TILE_HEIGHT), new Rectangle(0, 0, TILE_WIDTH, TILE_HEIGHT), Color.White);
                        point = true;
                    }
                    else if (MapInfo[j, i] == (int)Case.Super)
                    {
                        spriteBatch.Draw(items, new Vector2(i * TILE_WIDTH, j * TILE_HEIGHT), new Rectangle(TILE_WIDTH, 0, TILE_WIDTH, TILE_HEIGHT), Color.White);
                        point = true;
                    }
                }
            }

            if (point == false)
                IsEmpty = true;
        }
    }
}
