using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using static Pacman.Constants;

namespace Pacman
{
    public class PacmanGame : Game
    {
        //Pour gérer l'affichage
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Objets du jeu
        Map map;
        Entity[] entities;

        public PacmanGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

            TargetElapsedTime = TimeSpan.FromTicks(166666); //60fps
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            entities = new Entity[]
            {
                new Fantom(5, 1, Color.Red, 485),
                new Fantom(1, 23, Color.Yellow, 167),
                new Fantom(23, 1, Color.Blue, 10005),
                new Fantom(23, 23, Color.Green, 9954),
                new Player(1, 1)
            };

            map = new Map();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            map.LoadContent(Content);
            foreach (Entity e in entities)
                e.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            //Inputs
            InputManager.GetInputs();
            if (InputManager.IsKeyPressed(Keys.Escape))
                Exit();

            //Update des entities
            foreach (Entity e in entities)
                e.Update(map);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            map.Draw(spriteBatch);
            foreach (Entity e in entities)
                e.Draw(spriteBatch);          

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
