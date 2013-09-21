using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace _27Minutes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D hero;
        Vector2 heroPos;
        Vector2 heroSpeed;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Random rand = new Random(); //TODO Add seeds
            heroPos = Vector2.Zero;
			heroSpeed = new Vector2(0, 0);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hero = Content.Load<Texture2D>("marioSprite1");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            
            int MaxX = graphics.GraphicsDevice.Viewport.Width - hero.Width;
            int MinX = 0;
            int MaxY = graphics.GraphicsDevice.Viewport.Height - hero.Height;
            int MinY = 0;

            // Check for bounce.
			KeyboardState keyState = Keyboard.GetState();
			if (keyState.IsKeyDown(Keys.Right))
				heroSpeed.X = 50;
			if (keyState.IsKeyDown(Keys.Left))
				heroSpeed.X = -50;
			if (keyState.IsKeyDown(Keys.Up))
				heroSpeed.Y = -50;
			if (keyState.IsKeyDown(Keys.Down))
				heroSpeed.Y = 50;
			if (!keyState.IsKeyDown(Keys.Right) && !keyState.IsKeyDown(Keys.Left))
				heroSpeed.X = 0;
			if (!keyState.IsKeyDown(Keys.Up) && !keyState.IsKeyDown(Keys.Down))
				heroSpeed.Y = 0;

			// Move the sprite by speed, scaled by elapsed time.
			heroPos.X += heroSpeed.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
			heroPos.Y += heroSpeed.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
			spriteBatch.Draw(hero, heroPos, null, Color.White, 0f, Vector2.Zero, 3.0f, SpriteEffects.None, 0f);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        //protected 
    }
}
