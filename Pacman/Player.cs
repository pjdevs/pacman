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
    public class Player : Entity // Class du joueur qui est en fait le pacman
    {
        // Variables membres de la classe
        private Animator animator;      
        private int invincibleTime;

        public bool IsInvincible { get; private set; }
        public bool IsDead { get; set; }
        public int Score { get; private set; }
        public int Lives { get; private set; }

        // Constructeur
        public Player(float x, float y)
            : base(x, y)
        {
            direction = Vector2.Zero;
            savedDirection = Vector2.Zero;
            Lives = 3;
            IsInvincible = false;
            invincibleTime = INVINCIBLE_TIME;

            IsDead = false;
            Score = 0;

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

        // Fonction pour charger la texture
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("pacmen");
        }
        // Fonction pour mettre à jour le pacman
        public override void Update(Map map, Entity[] entities)
        {
            // Gestion des déplacements au clavier
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

            //Gestion invincibilité
            if (IsInvincible)
            {
                invincibleTime--;

                if (invincibleTime <= 0)
                {
                    IsInvincible = false;
                    invincibleTime = INVINCIBLE_TIME;
                }
            }

            // Gestion de la collision avec les fantomes
            for (int i=0; i<=FANTOMS_INDEX; i++)
            {
                if (Box.Intersects(entities[i].Box))
                {
                    if (IsInvincible)
                        Score += 500;
                    else
                    {
                        IsDead = true;
                        Lives--;
                    }
                }
            }

            //Gestion du score avec les points
            if (!IsOut())
            {
                if ((position.X % TILE_WIDTH == 0 && position.Y % TILE_HEIGHT == 0) && GetCaseType(ActualCaseX, ActualCaseY, map) == Case.Point)
                {
                    map.MapInfo[ActualCaseY, ActualCaseX] = (int)Case.Vide;
                    Score += POINT_SCORE;
                }
                else if ((position.X % TILE_WIDTH == 0 && position.Y % TILE_HEIGHT == 0) && GetCaseType(ActualCaseX, ActualCaseY, map) == Case.Super)
                {
                    map.MapInfo[ActualCaseY, ActualCaseX] = (int)Case.Vide;
                    Score += SUPER_SCORE;
                    IsInvincible = true;
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

            //Déplacement
            position += speed * direction;
        }
        // Fonction pour dessiner le pacman
        public override void Draw(SpriteBatch batch)
        {
            if (!IsInvincible)
                batch.Draw(texture, position, animator.CurrentAnimation.SourceRectangle, Color.White);
            else batch.Draw(texture, position, animator.CurrentAnimation.SourceRectangle, Color.Green);
        }

    }
}
