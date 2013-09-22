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
	//public enum tileType { SOLID, AIR, LADDER, HAZARD, DOOR }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Manager : Microsoft.Xna.Framework.Game
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

		int winWidth; 
		int winHeight; 

		int scalar = 32;
		int cameraSpeed = 4;
		TileMap myMap;
		int squaresAcross;
		int squaresDown;

        public Manager()
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
			myMap = new TileMap(rand);

			winWidth = graphics.GraphicsDevice.Viewport.Width;
			winHeight = graphics.GraphicsDevice.Viewport.Height;

            heroPos = new Vector2(128, 128);
			heroSpeed = new Vector2(0, 0);
			squaresDown = 2 + (int)Math.Ceiling((double)(winHeight / scalar));
			squaresAcross = 1 + (int)Math.Ceiling((double)(winWidth / scalar));
			
			//generateMap(rand);

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
			TileManager.initialize(Content);

			hero = Content.Load<Texture2D>("frogMario");

			//quad = new Texture2D(GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
			//quad.SetData<Color>(new Color[] { Color.White });

			
			//Tile.TileSetTexture = Content.Load<Texture2D>(@"Textures\TileSets\part1_tileset");
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


			KeyboardState ks = Keyboard.GetState();
			if (ks.IsKeyDown(Keys.Left)) {
				if (heroPos.X > 128 || Camera.Location.X == 0 && heroPos.X >= 0) {
					heroPos.X -= cameraSpeed;
				} else {
					Camera.Location.X = MathHelper.Clamp(Camera.Location.X - cameraSpeed, 0, (myMap.MapWidth - squaresAcross) * scalar);
				}
			}

			if (ks.IsKeyDown(Keys.Right)) {
				if (heroPos.X + 32 < winWidth - 128 || Camera.Location.X + 32 >= myMap.MapWidth * scalar - winWidth && heroPos.X + 32 < winWidth)  {
					heroPos.X += cameraSpeed;
				} else {
					Camera.Location.X = MathHelper.Clamp(Camera.Location.X + cameraSpeed, 0, (myMap.MapWidth - squaresAcross) * scalar);
				}
			}

			if (ks.IsKeyDown(Keys.Up)) {
				if (heroPos.Y > 128 || Camera.Location.Y == 0 && heroPos.Y >= 0) {
					heroPos.Y -= cameraSpeed;
				} else {
					Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y - cameraSpeed, 0, (myMap.MapHeight - squaresDown) * scalar);
				}
			}

			if (ks.IsKeyDown(Keys.Down)) {
				if (heroPos.Y + 64 < winHeight - 128 || Camera.Location.Y + 40 >= myMap.MapHeight * scalar - winHeight && heroPos.Y + 64 < winHeight) {
					heroPos.Y += cameraSpeed;
				} else {
					Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y + cameraSpeed, 0, (myMap.MapHeight - squaresDown) * scalar);
				}
			}

			if (ks.IsKeyDown(Keys.OemTilde)) {
				myMap = new TileMap(rand);
			}

            /*
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
			*/
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
			
			//Rectangle rect;
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

			//drawRoom(exit);

			//spriteBatch.Begin();

			Vector2 firstSquare = new Vector2(Camera.Location.X / scalar, Camera.Location.Y / scalar);
			int firstX = (int) firstSquare.X;
			int firstY = (int) firstSquare.Y;

			Vector2 squareOffset = new Vector2(Camera.Location.X % scalar, Camera.Location.Y % scalar);
			int offsetX = (int) squareOffset.X;
			int offsetY = (int) squareOffset.Y;

			for (int y = 0; y < squaresDown; y++) {
				for (int x = 0; x < squaresAcross; x++) {
					foreach (int tileID in myMap.Rows[y + firstY].Columns[x + firstX].BaseTiles) {
						spriteBatch.Draw(
							TileManager.getTexture(tileID),
							new Rectangle((x * scalar) - offsetX, (y * scalar) - offsetY, scalar, scalar),
							TileManager.GetSourceRectangle(),
							Color.White);
					}
				}
			}

			spriteBatch.Draw(hero, heroPos, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
			
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
/*
		private Rectangle sizeForDraw(Room room) {
			Rectangle rect = new Rectangle();
			rect.X = (int) room.getPosition().X;
			rect.Y = (int)room.getPosition().Y;
			rect.Width = (int)room.getSize().X * scalar;
			rect.Height = (int)room.getSize().Y * scalar;
			return rect;
		}*/
		/*
		private void drawRoom(Room room) {
			Tile[,] grid = room.getTileGrid();
			Rectangle temp = new Rectangle();
			for (int i = 0; i < grid.GetLength(0); i++) {
				for (int j = 0; j < grid.GetLength(1); j++) {
					temp.Height = scalar;
					temp.Width = scalar;
					temp.X = j*scalar;
					temp.Y = i*scalar;
					if (grid[i, j].type == tileType.AIR)
						spriteBatch.Draw(wall, temp, Color.White);
					else
						spriteBatch.Draw(floor, temp, Color.White);
				}
			}
		}
		*/
/*
		protected void generateMap(Random rand) {
			int mapAreaLimit = 200;
			int totalArea = 0;
			Room temp;
			Rectangle rect;
			Vector2 size;

			//rooms = new LinkedList<Room>();

			for (int i = 0; i < 5; i++) {
				rect = new Rectangle();
				rect.X = rand.Next(GraphicsDevice.Viewport.Width);
				rect.Y = rand.Next(GraphicsDevice.Viewport.Height);
				size = Room.getRandSize(rand);
				rect.Width = (int) size.X;
				rect.Height = (int) size.Y;
				temp = new Room(rect);
				rooms.AddLast(temp);
			}
			
			//exit = rooms.First.Value;
			//exit.setPosition(50, 50);
			exit = new Room(0, 0, 2, 1);
		}*/
	}
}
