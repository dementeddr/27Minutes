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
	public enum tileType { AIR, SOLID, LADDER, HAZARD, DOOR }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Manager : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		TileMap myMap;

		//Texture2D wall;
		//Texture2D floor;
        Texture2D hero;
		//Texture2D quad;
        
		Vector2 heroPos;
        Vector2 heroSpeed;
		long decel;
		bool onGround;
		
		Random rand;
		//LinkedList<Room> rooms;
		//Room exit;

		int winWidth; 
		int winHeight; 

		int scalar = 32;
		int cameraSpeed = 4;
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
			decel = 0;
			onGround = false;

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
			KeyboardState ks = Keyboard.GetState();
			
			if (ks.IsKeyDown(Keys.Escape))
				this.Exit();

			if (ks.IsKeyDown(Keys.Left))
				heroSpeed.X -= cameraSpeed;

			if (ks.IsKeyDown(Keys.Right))
				heroSpeed.X += cameraSpeed;

			if (ks.IsKeyDown(Keys.Up) && onGround)
				heroSpeed.Y -= 8;
			
			collisionDetect();

			if (ks.IsKeyDown(Keys.Left)) {
			//if (heroSpeed.X < 0) {
				if (heroPos.X > 128 || Camera.Location.X == 0 && heroPos.X >= 0) {
					heroPos.X -= cameraSpeed;
					//heroPos.X += heroSpeed.X;
				} else {
					Camera.Location.X = MathHelper.Clamp(Camera.Location.X - cameraSpeed, 0, (myMap.MapWidth - squaresAcross) * scalar);
				}
			}

			if (ks.IsKeyDown(Keys.Right)) {
			//if (heroSpeed.X > 0) {
				if (heroPos.X + 32 < winWidth - 128 || Camera.Location.X + 32 >= myMap.MapWidth * scalar - winWidth && heroPos.X + 32 < winWidth)  {
					heroPos.X += cameraSpeed;
					//heroPos.X += heroSpeed.X;
				} else {
					Camera.Location.X = MathHelper.Clamp(Camera.Location.X + cameraSpeed, 0, (myMap.MapWidth - squaresAcross) * scalar);
				}
			}

			if (ks.IsKeyDown(Keys.Up)) {
			//if (heroSpeed.Y < 0) {
				if (heroPos.Y > 128 || Camera.Location.Y == 0 && heroPos.Y >= 0) {
					//heroSpeed.Y = 8;
					//heroPos.Y += heroSpeed.Y;
					heroPos.Y -= cameraSpeed;
				} else {
					Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y - cameraSpeed, 0, (myMap.MapHeight - squaresDown) * scalar);
				}
			}

			if (ks.IsKeyDown(Keys.Down)) {
			//if (heroSpeed.Y > 0) {
				if (heroPos.Y + 64 < winHeight - 128 || Camera.Location.Y + 40 >= myMap.MapHeight * scalar - winHeight && heroPos.Y + 64 < winHeight) {
					heroPos.Y += cameraSpeed;
					//heroPos.Y += heroSpeed.Y;
				} else {
					Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y + cameraSpeed, 0, (myMap.MapHeight - squaresDown) * scalar);
				}
			}

			if (ks.IsKeyDown(Keys.OemTilde)) {
				myMap = new TileMap(rand);
			}
			/*
			decel++;
			if (decel % 15 == 0) {
				heroSpeed.Y += 1;
			}
			*/
			//heroSpeed = Vector2.Zero;
			
            base.Update(gameTime);
        }

		private void collisionDetect() {

			List<Rectangle> rects = new List<Rectangle>();
			
			int x = (int) (heroPos.X /*+ heroSpeed.X*/ + Camera.Location.X) / scalar;
			int y = (int) (heroPos.Y /*+ heroSpeed.Y*/ + Camera.Location.Y) / scalar;

			//Console.Write(myMap.Rows[y].Columns[x].getTileType());

			for (int i=0; i<3; i++) {
				if ((int)myMap.Rows[y + i].Columns[x].getTileType() == (int)tileType.SOLID) {
					rects.Add(new Rectangle(x * scalar, (y + i) * scalar, scalar, scalar));
				}
			}
			
			for (int i = 0; i < 3; i++) {
				if ((int)myMap.Rows[y + i].Columns[x+1].getTileType() == (int)tileType.SOLID)
					rects.Add(new Rectangle((x+1) * scalar, (y + i) * scalar, scalar, scalar));
			}

			if (rects.Count > 0) {
				foreach (Rectangle r in rects) {
					if (heroSpeed.X < 0 && heroPos.X > r.X) {
						heroPos.X = r.X + 32;
						heroSpeed.X = 0;
					}

					if (heroSpeed.X > 0 && heroPos.X < r.X) {
						heroPos.X = r.X - 32;
						heroSpeed.X = 0;
					}

					if (heroSpeed.Y < 0 && heroPos.Y > r.Y) {
						heroPos.Y = r.Y + 64;
						heroSpeed.Y = 0;
					}

					if (heroSpeed.Y > 0 && heroPos.Y < r.Y) {
						heroPos.Y = r.Y - 64;
						heroSpeed.Y = 0;
					}
				}
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			
			//Rectangle rect;
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

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
	}
}
