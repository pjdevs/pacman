using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using static Pacman.Constants; // Pour avoir accès aux constantes

namespace Pacman
{
    public abstract class Entity // Classe abstraite de base pour une entitée (pacman ou fantomes)
    {
        // Variables membres de la classe
        protected float speed;
        protected Vector2 position;
        protected Vector2 initialPosition;
        protected Vector2 direction;
        protected Vector2 savedDirection;
        protected Texture2D texture;

        // On récupère les coordonnées de la case actuelle à partir du centre
        protected int ActualCaseX { get { return ((int)position.X + TILE_WIDTH / 2) / TILE_WIDTH; } }
        protected int ActualCaseY { get { return ((int)position.Y + TILE_HEIGHT / 2) / TILE_HEIGHT; } }

        public Rectangle Box { get { return new Rectangle((int)position.X, (int)position.Y, TILE_WIDTH, TILE_HEIGHT); } } // Rectangle autour de l'entitée pour les collision entre elles

        // Constructeur
        public Entity(float x, float y)
        {
            position = new Vector2(TILE_WIDTH * x, TILE_HEIGHT * y);
            initialPosition = position;
            direction = Vector2.Zero;
            savedDirection = Vector2.Zero;
            speed = MOVE_SPEED;
        }

        // Fonction virtuelles pures
        public abstract void LoadContent(ContentManager content); // Pour charger les ressources (texture etc...)
        public abstract void Update(Map map, Entity[] entites); // Pour mettre à jour
        public abstract void Draw(SpriteBatch batch); // Pour afficher

        //Fonction qui remet la position à celle du début
        public void ResetPosition()
        {
            position = initialPosition;
            direction = -Vector2.UnitY;
        }

        //Fonction qui gère les collisions avec la map
        protected void ManageCollisions(Map map)
        {
            //Hors de la fenetre
            if (IsOut())
            {
                if (position.X <= -TILE_HEIGHT)
                {
                    position.X = WINDOW_WIDTH;
                    return;
                }
                else if (position.X >= WINDOW_WIDTH)
                {
                    position.X = -TILE_WIDTH;
                    return;
                }
                else return;
            }

            //Collisions dans toutes les directions et gestions des directions
            if (position.X % TILE_WIDTH == 0 && direction.X == 1f)
            {
                if (GetCaseType(ActualCaseX + 1, ActualCaseY, map) == Case.Mur) //Collision obligatiore pour pas traverser les murs
                    direction.X = 0f;

                if ((savedDirection == -Vector2.UnitY && GetCaseType(ActualCaseX, ActualCaseY - 1, map) != Case.Mur) //Veut aller en haut et peut
                    || (savedDirection == Vector2.UnitY && GetCaseType(ActualCaseX, ActualCaseY + 1, map) != Case.Mur)) //Veut aller en bas et peut
                {
                    direction.X = 0f;
                    direction.Y = savedDirection.Y;
                    savedDirection = Vector2.Zero;
                }
            }
            if (position.X % TILE_WIDTH == 0 && direction.X == -1f)
            {
                if (GetCaseType(ActualCaseX - 1, ActualCaseY, map) == Case.Mur) //Collision obligatiore pour pas traverser les murs
                    direction.X = 0f;

                if ((savedDirection == -Vector2.UnitY && GetCaseType(ActualCaseX, ActualCaseY - 1, map) != Case.Mur) //Veut aller en haut et peut
                    || (savedDirection == Vector2.UnitY && GetCaseType(ActualCaseX, ActualCaseY + 1, map) != Case.Mur)) //Veut aller en bas et peut
                {
                    direction.X = 0f;
                    direction.Y = savedDirection.Y;
                    savedDirection = Vector2.Zero;
                }
            }
            if (position.Y % TILE_HEIGHT == 0 && direction.Y == 1f)
            {
                if (GetCaseType(ActualCaseX, ActualCaseY + 1, map) == Case.Mur) //Collision obligatiore pour pas traverser les murs
                    direction.Y = 0f;

                if ((savedDirection == -Vector2.UnitX && GetCaseType(ActualCaseX - 1, ActualCaseY, map) != Case.Mur) //Veut aller à gauche et peut
                    || (savedDirection == Vector2.UnitX && GetCaseType(ActualCaseX + 1, ActualCaseY, map) != Case.Mur)) //Veut aller à droite et peut
                {
                    direction.Y = 0f;
                    direction.X = savedDirection.X;
                    savedDirection = Vector2.Zero;
                }
            }
            if (position.Y % TILE_HEIGHT == 0 && direction.Y == -1f)
            {
                if (GetCaseType(ActualCaseX, ActualCaseY - 1, map) == Case.Mur) //Collision obligatiore pour pas traverser les murs
                    direction.Y = 0f;

                if ((savedDirection == -Vector2.UnitX && GetCaseType(ActualCaseX - 1, ActualCaseY, map) != Case.Mur) //Veut aller à gauche et peut
                    || (savedDirection == Vector2.UnitX && GetCaseType(ActualCaseX + 1, ActualCaseY, map) != Case.Mur)) //Veut aller à droite et peut
                {
                    direction.Y = 0f;
                    direction.X = savedDirection.X;
                    savedDirection = Vector2.Zero;
                }
            }

        }
        // Fonction qui récupère la case actuelle
        protected Case GetCaseType(int x, int y, Map map)
        {
            return (Case)map.MapInfo[y, x];
        }
        // Fonction retournant vrai si l'entitée en hors fenetre et faux sinon
        protected bool IsOut()
        {
            if (position.X <= 0 || position.X >= WINDOW_WIDTH - TILE_WIDTH)
                return true;
            else return false;
        }
    }
}
