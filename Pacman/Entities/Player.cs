using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Pacman.Core.Inputs;

namespace Pacman.Core.Entities
{
    public class Player : Entity
    {
        private bool wantTurn;
        private Vector2 direction;

        public Player(float x, float y)
            : base(x, y)
        {
            wantTurn = false;
            direction = Vector2.Zero;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Pacmid");
        }

        public override void Update()
        {
            ManageCollision();

            if (InputManager.IsKeyDown(Keys.Left))
            {
                wantTurn = true;
                direction = new Vector2(-1f, 0f);               
            }
            if (InputManager.IsKeyDown(Keys.Right))
            {
                wantTurn = true;
                direction = new Vector2(1f, 0f);
            }
            if (InputManager.IsKeyDown(Keys.Up))
            {
                wantTurn = true;
                direction = new Vector2(0f, -1f);
            }
            if (InputManager.IsKeyDown(Keys.Down))
            {
                wantTurn = true;
                direction = new Vector2(0f, 1f);
            }

            if (wantTurn)
            {
                if (canTurn())
                {
                    velocity = Speed * direction;
                    wantTurn = false;
                    direction = Vector2.Zero;
                }
            }
         
            position += velocity;
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, position, Color.White);
        }

        private void ManageCollision()
        {
            if (position.X < 16)
                position.X = 16;

            if (position.X > 400 - 32)
                position.X = 400 - 32;

            if (position.Y < 16)
                position.Y = 16;

            if (position.Y > 400 - 32)
                position.Y = 400 - 32;
        }

        private bool canTurn()
        {
            return true;
        }
    }
}
