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

namespace Pacman.Core.Entities
{
    public abstract class Entity
    {
        public float Speed { get; set; }

        protected Vector2 position;
        protected Vector2 velocity;
        protected Texture2D texture;

        public Entity(float x, float y)
        {
            position = new Vector2(x, y);
            velocity = new Vector2(0f, 0f);
            Speed = 2f;
        }

        public abstract void LoadContent(ContentManager content);
        public abstract void Update();
        public abstract void Draw(SpriteBatch batch);
    }
}
