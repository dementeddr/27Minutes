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

	public enum tileType { SOLID, AIR, LADDER, HAZARD, DOOR }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		Texture2D wall;
		Texture2D floor;
        Texture2D hero;
		Texture2D quad;
        Vector2 heroPos;
        Vector2 heroSpeed;
		Random rand;
		LinkedList<Room> rooms;
		Room exit;

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
            rand = new Random(); //TODO Add seeds
            heroPos = Vector2.Zero;
			heroSpeed = new Vector2(0, 0);

			generateMap(rand);

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
			wall = Content.Load<Texture2D>("grey_dirt3");
			floor = Content.Load<Texture2D>("floor_vines3");
			quad = new Texture2D(GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
			quad.SetData<Color>(new Color[] { Color.White });
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
        protected override void Draw(GameTime gameTime) {
			Rectangle rect;
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
			//spriteBatch.Draw(hero, heroPos, null, Color.White, 0f, Vector2.Zero, 3.0f, SpriteEffects.None, 1f);
			drawRoom(exit);
			
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

		private Rectangle sizeForDraw(Room room) {
			Rectangle rect = new Rectangle();
			rect.X = (int) room.getPosition().X;
			rect.Y = (int)room.getPosition().Y;
			rect.Width = (int)room.getSize().X * 32;
			rect.Height = (int)room.getSize().Y * 32;
			return rect;
		}

		private void drawRoom(Room room) {
			Tile[,] grid = room.getTileGrid();
			Rectangle temp = new Rectangle();
			for (int i = 0; i < grid.GetLength(0); i++) {
				for (int j = 0; j < grid.GetLength(1); j++) {
					temp.Height = 32;
					temp.Width = 32;
					temp.X = j*32;
					temp.Y = i*32;
					if (grid[i, j].type == tileType.AIR)
						spriteBatch.Draw(wall, temp, Color.White);
					else
						spriteBatch.Draw(floor, temp, Color.White);
				}
			}
		}

		protected void generateMap(Random rand) {
			int mapAreaLimit = 200;
			int totalArea = 0;
			Room temp;
			Rectangle rect;
			Vector2 size;

			//rooms = new LinkedList<Room>();

			/*for (int i = 0; i < 5; i++) {
				rect = new Rectangle();
				rect.X = rand.Next(GraphicsDevice.Viewport.Width);
				rect.Y = rand.Next(GraphicsDevice.Viewport.Height);
				size = Room.getRandSize(rand);
				rect.Width = (int) size.X;
				rect.Height = (int) size.Y;
				temp = new Room(rect);
				rooms.AddLast(temp);
			}
			*/
			//exit = rooms.First.Value;
			//exit.setPosition(50, 50);
			exit = new Room(0, 0, 2, 1);
		}
	}
}