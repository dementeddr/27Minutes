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
	//Defines the ways that entities can interact with tiles
	public enum tileType { AIR, SOLID, LADDER, HAZARD, DOOR }
   
	/// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Manager : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		TileMap myMap;		//Current map. Needs to be generalized

		Texture2D hero;		//Character sprite
		Texture2D water;	//Don't drown
        
		Vector2 heroPos;	//Position of the character in relation to the viewport
        Vector2 heroSpeed;	//How many pixels the hero will move in each direction next frame
		long decel;			//Value that helps govern how fast entities fall
		bool onGround;		//Is the hero on the ground?
		
		Random rand;		//Seed-based random number generator
		//LinkedList<Room> rooms;
		//Room exit;
		Rectangle[] temp;

		int winWidth;		//Pixel-width of the viewport
		int winHeight;		//Pizel-height of the viewport

		int scalar = 32;	//Pixel-length of the side of a tile
		int cameraSpeed = 4;//How many pixels the hero will move each frame
		int depth;			//Current depth of the water

		int squaresAcross;	//How many tiles fit on the screen horizontally
		int squaresDown;	//How many tiles fit on the screen vertically

		/*
		 * Constructor 
		 */
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
            rand = new Random();		//TODO Add seeds
			myMap = new TileMap(rand);	//TODO generalize the maps

			//Getting the screen dimensions
			winWidth = graphics.GraphicsDevice.Viewport.Width;
			winHeight = graphics.GraphicsDevice.Viewport.Height;

			//Initial speed and position of the hero, and it's relation to gravity. Needs to be generalized
			//for different maps
            heroPos = new Vector2(64, (myMap.MapHeight - 11) * scalar);
			heroSpeed = new Vector2(0, 0);
			decel = 0;
			onGround = false;

			//The tile-in-screen dimensions are defined by the scalar value
			squaresDown = 2 + (int)Math.Ceiling((double)(winHeight / scalar));
			squaresAcross = 1 + (int)Math.Ceiling((double)(winWidth / scalar));

			temp = new Rectangle[2];

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

			//This holds the tile textures for the map
			TileManager.initialize(Content);

			//Boing boing
			hero = Content.Load<Texture2D>("frogMario");

			//Creates a plain blue texture for the water
			water = new Texture2D(GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
			water.SetData<Color>(new Color[] { new Color(72, 61, 139, 200) });
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
			//All the keys that are pressed. Or not.
			KeyboardState ks = Keyboard.GetState();
			
			//Exit on escape
			if (ks.IsKeyDown(Keys.Escape))
				this.Exit();

			//Randomize the map on space
			if (ks.IsKeyDown(Keys.Space)) {
				myMap = new TileMap(rand);
				return;
			}

			//Set the potential movement for the next frame depending upon the keys pressed
			if (ks.IsKeyDown(Keys.Left))
				heroSpeed.X -= cameraSpeed;

			if (ks.IsKeyDown(Keys.Right))
				heroSpeed.X += cameraSpeed;

			//Jumping, only if in contact with ground
			if (ks.IsKeyDown(Keys.Up) && onGround)
				heroSpeed.Y -= 8;
			
			//Is the character touching the terrain? Modifies or zeroes out the heroSpeed vectors as necessary, depending upon contact with terrain
			collisionDetect();

			//LEFT. Moves the camera or the hero left depeding upon where the hero is on screen
			if (heroSpeed.X < 0) {
				//Moving the hero
				if (heroPos.X > 128 || Camera.Location.X == 0 && heroPos.X >= 0) {
					heroPos.X += heroSpeed.X;
					//Moving the camera
				} else {
					Camera.Location.X = MathHelper.Clamp(Camera.Location.X + heroSpeed.X, 0, (myMap.MapWidth - squaresAcross) * scalar);
				}
			}

			//RIGHT. Moves the camera or the hero left depeding upon where the hero is on screen
			if (heroSpeed.X > 0) {
				//Moving the hero
				if (heroPos.X + 32 < winWidth - 128 || Camera.Location.X + 32 >= myMap.MapWidth * scalar - winWidth && heroPos.X + 32 < winWidth)  {
					heroPos.X += heroSpeed.X;
					//Moving the camera
				} else {
					Camera.Location.X = MathHelper.Clamp(Camera.Location.X + heroSpeed.X, 0, (myMap.MapWidth - squaresAcross) * scalar);
				}
			}

			//UP. Moves the camera or the hero left depeding upon where the hero is on screen
			if (heroSpeed.Y < 0) {
				//Moving the hero
				if (heroPos.Y > 128 || Camera.Location.Y == 0 && heroPos.Y >= 0) {
					heroPos.Y += heroSpeed.Y;
					//Moving the camera
				} else {
					Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y + heroSpeed.Y, 0, (myMap.MapHeight - squaresDown) * scalar);
				}
			}

			//DOWN. Moves the camera or the hero left depeding upon where the hero is on screen
			if (heroSpeed.Y > 0) {
				//Moving the hero
				if (heroPos.Y + 64 < winHeight - 128 || Camera.Location.Y + 40 >= myMap.MapHeight * scalar - winHeight && heroPos.Y + 64 < winHeight) {
					heroPos.Y += heroSpeed.Y;
					//Moving the camera
				} else {
					Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y + heroSpeed.Y, 0, (myMap.MapHeight - squaresDown) * scalar);
				}
			}
			
			//Accelerates the hero downwords at a rate of 1 pixel per frame per frame
			decel++;
			if (decel % 4 == 0 && !onGround) {
				heroSpeed.Y += 1;
			}
			//Zeroes out the horizontal movement at the end of each frame
			heroSpeed.X = 0;

			//gurgle gurgle
			if (winHeight - depth < heroPos.Y) {
				this.Exit();
			}

			//Update
            base.Update(gameTime);
        }

		/*
		 * Detects collision between the hero and any solid tiles around it. If the hero would collide with a
		 * solid tile, it (should) zero out movement in that direction, then sets the position of the sprite so
		 * it is right next to the tile in question.
		 */
		private void collisionDetect() {

			Rectangle[] rects;
			Rectangle heroRect = new Rectangle((int)heroPos.X, (int)heroPos.Y, 32, 64);
			
			int x = (int) (heroPos.X + heroSpeed.X + Camera.Location.X) / scalar;
			int y = (int) (heroPos.Y + heroSpeed.Y + Camera.Location.Y) / scalar;

			//DEBUG
			Console.WriteLine("X: " + x + "  Y: " + y);
			Console.WriteLine("heroPos.X: " + heroPos.X + "  heroPos.Y: " + heroPos.Y);


			/*
			for (int i=0; i<3; i++) {
				if ((int)myMap.Rows[y + i].Columns[x].getTileType() == (int)tileType.SOLID) {
					rects.Add(new Rectangle(x * scalar, (y + i) * scalar, scalar, scalar));
				}
			}
			
			for (int i = 0; i < 3; i++) {
				if ((int)myMap.Rows[y + i].Columns[x+1].getTileType() == (int)tileType.SOLID)
					rects.Add(new Rectangle((x+1) * scalar, (y + i) * scalar, scalar, scalar));
			}*/
			/*
			//if (heroSpeed.X < 0) {
			rects = new Rectangle[3];

				for (int i = 0; i < 3; i++) {
					if ((int)myMap.Rows[y + i].Columns[x].getTileType() == (int)tileType.SOLID) {
						rects[i] = new Rectangle((x) * scalar, (y + i) * scalar, scalar, scalar);
					}
				}

				//if (rects.Length > 0) {
					foreach (Rectangle r in rects) {

						if (heroSpeed.X < 0 && heroPos.X <= r.X + 32 && heroPos.X > r.X) {
							heroPos.X = r.X + 32;
							heroSpeed.X = 0;
						}
					}
				//}
			//}
			
			//if (heroSpeed.Y < 0) {
			rects = new Rectangle[2];

				if ((int)myMap.Rows[y].Columns[x].getTileType() == (int)tileType.SOLID) {
					rects[0] = new Rectangle(x * scalar, y * scalar, scalar, scalar);
				}

				if ((int)myMap.Rows[y].Columns[x + 1].getTileType() == (int)tileType.SOLID) {
					rects[1] = new Rectangle((x + 1) * scalar, y * scalar, scalar, scalar);
				}

				//if (rects.Length > 0) {
					foreach (Rectangle r in rects) {

						if (heroSpeed.Y < 0 && heroPos.Y < r.Y + 32 && heroPos.Y > r.Y) {
							heroPos.Y = r.Y + 32;
							heroSpeed.Y = 0;
						}
					}
				//}
			//}
			
			*/
			
			//if (heroSpeed.Y >= 0) {
			rects = new Rectangle[2];
				
				onGround = false;

				if ((int)myMap.Rows[y + 2].Columns[x].getTileType() == (int)tileType.SOLID) {
					rects[0] = new Rectangle(x * scalar, (y + 2) * scalar, scalar, scalar);
				}

				if ((int)myMap.Rows[y + 2].Columns[x + 1].getTileType() == (int)tileType.SOLID) {
					rects[1] = new Rectangle((x + 1) * scalar, (y + 2) * scalar, scalar, scalar);
				}

				//if (rects.Length > 0) {
					foreach (Rectangle r in rects) {

						if (heroSpeed.Y > 0 && heroPos.Y + 60 >= r.Y && heroPos.Y < r.Y) {
						//if (heroSpeed.Y > 0 && heroRect.Intersects(r)) {
							heroPos.Y = r.Y - 60;
							heroSpeed.Y = 0;
							onGround = true;
						}
					}

					temp = rects;
				//}
			//}
			//Rectangle r = rects[0];
			/*
				

				if (heroSpeed.X > 0 && heroPos.X + 32 >= r.X && heroPos.X < r.X) {
					heroPos.X = r.X - 32;
					heroSpeed.X = 0;
				}

				
			*/
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

			spriteBatch.Draw(water, new Rectangle(0, winHeight - depth, winWidth, depth), Color.DarkSeaGreen);

			spriteBatch.Draw(water, temp[0], Color.DarkSeaGreen);
			spriteBatch.Draw(water, temp[1], Color.DarkSeaGreen);

			//decel++;
			//if (decel % 15 == 0)
			//	depth++;

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
	}
}