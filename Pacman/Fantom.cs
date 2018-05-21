using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using static Pacman.Constants;

namespace Pacman
{
    public class Fantom : Entity
    {
        private Rectangle sourceRect;
        private Color color;
        private int turnCounter;
        private Random random;

        public Fantom(float x, float y, Color fcolor, int seed) : base(x, y)
        {
            color = fcolor;
            turnCounter = TURN_TIME_MIN;
            sourceRect = new Rectangle(0, 0, TILE_WIDTH, TILE_HEIGHT);
            random = new Random(seed);
        }

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
        public override void Update(Map map)
        {
            if (turnCounter == 0)
            {
                int dir = random.Next(1, 4);

                if (dir == 1)
                {
                    direction.X = 1f;
                    savedDirection = Vector2.UnitX;
                }
                else if (dir == 2)
                {
                    direction.X = -1f;
                    savedDirection = -Vector2.UnitX;
                }
                else if (dir == 3)
                {
                    direction.Y = 1f;
                    savedDirection = Vector2.UnitY;
                }
                else if (dir == 4)
                {
                    direction.Y = -1f;
                    savedDirection = -Vector2.UnitY;
                }

                turnCounter = random.Next(TURN_TIME_MIN, TURN_TIME_MIN);
            }

            ManageCollisions(map);

            turnCounter--;
            position += Speed * direction;
        }
        public override void Draw(SpriteBatch batch)
        {
            if (direction.Y == -1f)
                sourceRect.X = 0;
            else if (direction.X == 1f)
                sourceRect.X = TILE_WIDTH;
            else if (direction.Y == 1f)
                sourceRect.X = 2 * TILE_WIDTH;
            else if (direction.X == -1f)
                sourceRect.X = 3 * TILE_WIDTH;


            batch.Draw(texture, position, sourceRect, Color.White);
        }
    }
}
