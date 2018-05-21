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
        private Animator animator;
        private int score;
        private int lives;
        private bool invincible;
        private int invincibleTime;

        public Player(float x, float y)
            : base(x, y)
        {
            direction = Vector2.Zero;
            savedDirection = Vector2.Zero;
            score = 0;
            lives = 3;
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
                    score += POINT_SCORE;
                }
                else if ((position.X % TILE_WIDTH == 0 && position.Y % TILE_HEIGHT == 0) && GetCaseType(ActualCaseX, ActualCaseY, map) == Case.Super)
                {
                    map.MapInfo[ActualCaseY, ActualCaseX] = (int)Case.Vide;
                    score += SUPER_SCORE;
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
            batch.DrawString(font, "LIVES : " + lives.ToString() + " SCORE : " + score.ToString(), new Vector2(5, 0), Color.White);
            batch.Draw(texture, position, animator.CurrentAnimation.SourceRectangle, Color.White);
        }

    }
}
