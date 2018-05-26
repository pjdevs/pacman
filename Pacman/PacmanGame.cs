using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using static Pacman.Constants; // Pour avoir accès aux constantes

namespace Pacman
{
    public class PacmanGame : Game // Classe principale de jeu qui hérite de la classe Game de MonoGame 
    {
        //Pour gérer l'affichage
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Ressources
        private SpriteFont font;
        private Texture2D gameoverTexture; 

        //Variables
        private bool gameover;

        //Objets du jeu
        private Map map;
        private Entity[] entities;

        // Constructeur
        public PacmanGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

            TargetElapsedTime = TimeSpan.FromTicks(166666); //60fps
        }

        // Fonction qui initialise le jeu, elle se lance toute seule au début du jeu après le constructeur
        protected override void Initialize()
        {
            entities = new Entity[]
            {
                new Fantom(12, 11, Color.Red, 485),
                new Fantom(12, 11, Color.Yellow, 167),
                new Fantom(12, 11, Color.Blue, 10005),
                new Fantom(12, 11, Color.Green, 9954),
                new Player(12, 13)
            };

            map = new Map();

            gameover = false;

            base.Initialize();
        }

        // Fonction qui charge les ressources, elle se lance toute seule après Initialize() au début du jeu
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            map.LoadContent(Content);
            foreach (Entity e in entities)
                e.LoadContent(Content);
            font = Content.Load<SpriteFont>("text");
            gameoverTexture = Content.Load<Texture2D>("gameover");
        }
        
        // Fonction pour mettre à jour le jeu
        protected override void Update(GameTime gameTime)
        {
            //Inputs
            InputManager.GetInputs();
            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();
            
            //Update du jeu si on n'a pas perdu
            if (!gameover)
            {
                Player player = (Player)entities[PLAYER_INDEX];

                //Gestion de la map vide
                if (map.IsEmpty)
                {
                    map.ResetMap();
                    for (int i = 0; i < FANTOMS_INDEX; i++)
                        entities[i].ResetPosition();
                    player.ResetPosition();
                }

                //Gestion de la mort
                if (player.IsDead)
                {
                    for (int i = 0; i < FANTOMS_INDEX; i++)
                        entities[i].ResetPosition();
                    player.ResetPosition();
                    player.IsDead = false;
                }

                //Update de tout le monde
                foreach (Entity e in entities)
                    e.Update(map, entities);

                //Gestion de fin de partie
                if (player.Lives == 0)
                {
                    gameover = true;
                }
            }
            else // Sinon on attend juste un appuie sur R ou Echap
            {
                if (InputManager.IsKeyDown(Keys.R))
                {
                    gameover = false;
                    Initialize();
                }
            }

            base.Update(gameTime);
        }

        // Fontion pour afficher le jeu
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            Player player = (Player)entities[PLAYER_INDEX];

            spriteBatch.Begin(); //Début du dessin

            if (!gameover)
            {
                map.Draw(spriteBatch);
                foreach (Entity e in entities)
                    e.Draw(spriteBatch);
                spriteBatch.DrawString(font, "LIVES : " + player.Lives.ToString() + " SCORE : " + player.Score.ToString(), new Vector2(5, 0), Color.White);
            }
            else
            {
                spriteBatch.Draw(gameoverTexture, Vector2.Zero, Color.White);
                spriteBatch.DrawString(font, "SCORE : " + player.Score.ToString(), new Vector2(100, 320), Color.White);
            }

            spriteBatch.End(); //Fin du dessin

            base.Draw(gameTime);
        }
    }
}
