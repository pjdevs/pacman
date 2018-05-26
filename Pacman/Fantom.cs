using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using static Pacman.Constants; // Pour avoir accès aux constantes

namespace Pacman
{
    public class Fantom : Entity // Classe représentant un fantôme
    {
        // Variables membres de la classe
        private Rectangle sourceRect; // Rectangle source pour la sens du fantôme
        private Color color;
        private int turnCounter; // Compteur avant que le fantôme tourne 
        private Random random; // Objet pour générer un nombre aléatoire

        // Constructeur
        public Fantom(float x, float y, Color fcolor, int seed) : base(x, y)
        {
            color = fcolor;
            turnCounter = TURN_TIME_MAX;
            sourceRect = new Rectangle(0, 0, TILE_WIDTH, TILE_HEIGHT);
            random = new Random(seed); // Pour ne pas générer les mêmes nombres entre les fantômes
            direction = -Vector2.UnitY;
        }

        // Fontion pour charger la texture
        public override void LoadContent(ContentManager content)
        {
            if (color == Color.Red)
                texture = content.Load<Texture2D>("redfantom");
            else if (color == Color.Blue)
                texture = content.Load<Texture2D>("bluefantom");
            else if (color == Color.Yellow)
                texture = content.Load<Texture2D>("yellowfantom");
            else if (color == Color.Green)
                texture = content.Load<Texture2D>("greenfantom");
        }
        // Fonction pour mettre à jour
        public override void Update(Map map, Entity[] entities)
        {
            //Gestion de "l'IA" pour tourner
            if (direction == Vector2.Zero) // Si il est aretté on tourne directement
            {
                if (GetCaseType(ActualCaseX, ActualCaseY - 1, map) != Case.Mur)
                {
                    direction.Y = -1f;
                }
                else if (GetCaseType(ActualCaseX + 1, ActualCaseY, map) != Case.Mur)
                {
                    direction.X = 1f;
                }
                else if (GetCaseType(ActualCaseX, ActualCaseY + 1, map) != Case.Mur)
                {
                    direction.Y = 1f;
                }
                else if (GetCaseType(ActualCaseX - 1, ActualCaseY, map) != Case.Mur)
                {
                    direction.X = -1f;
                }
                
            }

            if (turnCounter == 0) // Si compteur à 0 on tourne
            {
                int dir = random.Next(1, 8);

                if (dir == 1 || dir == 2)
                {
                    direction.X = 1f;
                    savedDirection = Vector2.UnitX;
                }
                else if (dir == 3 || dir == 4)
                {
                    direction.X = -1f;
                    savedDirection = -Vector2.UnitX;
                }
                else if (dir == 5 || dir == 6)
                {
                    direction.Y = 1f;
                    savedDirection = Vector2.UnitY;
                }
                else if (dir == 7 || dir == 8)
                {
                    direction.Y = -1f;
                    savedDirection = -Vector2.UnitY;
                }

                turnCounter = random.Next(TURN_TIME_MIN, TURN_TIME_MAX);
            }

            //Collision
            ManageCollisions(map);

            Player player = (Player)entities[PLAYER_INDEX]; // On récupère le joueur dans le tableau

            //Collision avec le player
            if (Box.Intersects(player.Box))
            {
                if (player.IsInvincible)
                    ResetPosition();
            }

            turnCounter--;
            position += speed * direction;
        }
        // Fonction pour afficher
        public override void Draw(SpriteBatch batch)
        {
            if (direction.Y == -1f)
                sourceRect.X = FANTOM_LEFT;
            else if (direction.X == 1f)
                sourceRect.X = FANTOM_UP;
            else if (direction.Y == 1f)
                sourceRect.X = FANTOM_RIGHT;
            else if (direction.X == -1f)
                sourceRect.X = FANTOM_DOWN;

            batch.Draw(texture, position, sourceRect, Color.White);
        }
    }
}
