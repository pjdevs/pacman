using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using static Pacman.Constants;

namespace Pacman
{
    public class Player : Entity
    {
        private SpriteFont font;

        private Vector2 direction;
        private Vector2 savedDirection;
        private Animator animator;
        private int score;
        private bool invincible;
        private int invincibleTime;

        //On récupère les coord de la case actuelle à partir du centre
        private int ActualCaseX { get { return ((int)position.X + TILE_WIDTH / 2) / TILE_WIDTH; } }
        private int ActualCaseY { get { return ((int)position.Y + TILE_HEIGHT / 2) / TILE_HEIGHT; } }        

        public Player(float x, float y)
            : base(x, y)
        {
            direction = Vector2.Zero;
            savedDirection = Vector2.Zero;
            score = 0;
            invincible = false;
            invincibleTime = 0;

            animator = new Animator();

            animator.AddAnimation(new Animation("IdleRight", 0, 0, TILE_WIDTH));
            animator.AddAnimation(new Animation("IdleDown", 1, 0, TILE_WIDTH));
            animator.AddAnimation(new Animation("IdleLeft", 2, 0, TILE_WIDTH));
            animator.AddAnimation(new Animation("IdleUp", 3, 0, TILE_WIDTH));

            animator.AddAnimation(new Animation("Right", 0, 3, TILE_WIDTH));
            animator.AddAnimation(new Animation("Down", 1, 3, TILE_WIDTH));
            animator.AddAnimation(new Animation("Left", 2, 3, TILE_WIDTH));
            animator.AddAnimation(new Animation("Up", 3, 3, TILE_WIDTH));

            animator.SetAnimation("IdleRight");
        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("text");
            texture = content.Load<Texture2D>("pacmen");
        }
        public override void Update(Map map)
        {
            if (InputManager.IsKeyDown(Keys.Left))
            {
                direction.X = -1f;
                savedDirection = -Vector2.UnitX;
            }
            else if (InputManager.IsKeyDown(Keys.Right))
            {
                direction.X = 1f;
                savedDirection = Vector2.UnitX;
            }
            else if (InputManager.IsKeyDown(Keys.Up))
            {
                direction.Y = -1f;
                savedDirection = -Vector2.UnitY;
            }
            else if (InputManager.IsKeyDown(Keys.Down))
            {
                direction.Y = 1f;
                savedDirection = Vector2.UnitY;
            }

            //Gere les collisions
            ManageCollisions(map);

            //Gestion score avec points
            if (!IsOut())
            {
                if ((position.X % TILE_WIDTH == 0 && position.Y % TILE_HEIGHT == 0) && GetCaseType(ActualCaseX, ActualCaseY, map) == Case.Point)
                {
                    map.MapInfo[ActualCaseY, ActualCaseX] = (int)Case.Vide;
                    score += 20;
                }
                else if ((position.X % TILE_WIDTH == 0 && position.Y % TILE_HEIGHT == 0) && GetCaseType(ActualCaseX, ActualCaseY, map) == Case.Super)
                {
                    map.MapInfo[ActualCaseY, ActualCaseX] = (int)Case.Vide;
                    score += 100;
                    //Mettre invincible
                }
            }

            //Animations
            if (direction == Vector2.Zero)
            {
                if (animator.CurrentAnimation.Name == "Right")
                    animator.SetAnimation("IdleRight");
                else if (animator.CurrentAnimation.Name == "Left")
                    animator.SetAnimation("IdleLeft");
                else if (animator.CurrentAnimation.Name == "Up")
                    animator.SetAnimation("IdleUp");
                else if (animator.CurrentAnimation.Name == "Down")
                    animator.SetAnimation("IdleDown");
            }
            else if (direction == Vector2.UnitX)
                animator.SetAnimation("Right");
            else if (direction == -Vector2.UnitX)
                animator.SetAnimation("Left");
            else if (direction == Vector2.UnitY)
                animator.SetAnimation("Down");
            else if (direction == -Vector2.UnitY)
                animator.SetAnimation("Up");

            animator.Animate(); 

            //Fais avancer
            position += Speed * direction;
        }
        public override void Draw(SpriteBatch batch)
        {
            batch.DrawString(font, "SCORE : " + score.ToString(), new Vector2(5, 1), Color.White);
            batch.Draw(texture, position, animator.CurrentAnimation.SourceRectangle, Color.White);
        }

        private void ManageCollisions(Map map)
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
        private Case GetCaseType(int x, int y, Map map)
        {
            return (Case)map.MapInfo[y, x];
        }
        private bool IsOut()
        {
            if (position.X <= 0 || position.X >= WINDOW_WIDTH - TILE_WIDTH - Speed)
                return true;
            else return false;
        }
    }
}
