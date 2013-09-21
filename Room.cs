using System;
using Microsoft.Xna.Framework;

namespace _27Minutes {
	public class Room {
		private Door[] doors;
		private Rectangle rect;
		public Tile[,] grid;

		public static Vector2[] dimensions = {new Vector2(1,1),
							   new Vector2(2,1),
							   new Vector2(1,2),
							   new Vector2(1,3),
							   new Vector2(2,2),
							   new Vector2(3,1),
							   new Vector2(4,1),
							   new Vector2(3,2),
							   new Vector2(2,3),
							   new Vector2(1,4),
							   new Vector2(2,4),
							   new Vector2(3,3),
							   new Vector2(4,2),
							   new Vector2(5,1),
							   new Vector2(6,1),
							   new Vector2(5,2),
							   new Vector2(4,3),
							   new Vector2(3,4),
							   new Vector2(4,4),
							   new Vector2(5,3),
							   new Vector2(5,4)};

		public Room(Rectangle rectangle) {
			rect = rectangle;
			init();
		}

		public Room(int X, int Y, int Width, int Height) {
			rect = new Rectangle(X, Y, Height, Width);
			init();
		}

		private void init() {
			doors = new Door[4];
			generateRoom();
		}

		public Vector2 getSize() {
			return new Vector2(rect.Width, rect.Height);
		}

		public Vector2 getPosition() {
			return new Vector2(rect.X, rect.Y);
		}

		public Rectangle getRectangle() {
			return rect;
		}

		public Tile[,] getTileGrid() {
			return grid;
		}

		public void setRectangle(Rectangle rectangle) {
			rect = rectangle;
		}

		public void setRectangle(int X, int Y, int Width, int Height) {
			rect = new Rectangle(X, Y, Width, Height);
		}

		public void setPosition(int X, int Y) {
			rect.X = X;
			rect.Y = Y;
		}

		public void setSize(int X, int Y) {
			rect.Width = X;
			rect.Height = Y;
		}

		public static Vector2 getRandSize(Random rand) {
			return dimensions[rand.Next(dimensions.Length)];
		}

		private void generateRoom() {
			//grid = new Tile[rect.Width * 10, rect.Height * 10];
			grid = new Tile[12,16];

			for (int i = 0; i < grid.GetLength(0); i++) {
				for (int j = 0; j < grid.GetLength(1); j++) {
					grid[i, j] = new Tile();
				}
			}

			for (int i = 0; i < grid.GetLength(1); i++) {
				grid[4, i].type = tileType.SOLID;
			}
			//int numDoor = 2;
			//int random = rand.Next(grid.GetLength(1) - 5);


			/* int random = rand.Next(10);
			if (random <= 6)
				numDoor = 2;
			else if (random <= 8)
				numDoor = 3;
			 */

		}

	}
}