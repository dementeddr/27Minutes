using System;
using Microsoft.Xna.Framework;


public class Room
{
	Door[] doors;
	Rectangle rect;

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
	
	public Room(Rectangle rectangle)
	{
		rect = rectangle;
		init();
	}

	public Room(int X, int Y, int Width, int Height) {
		rect = new Rectangle(X, Y, Height, Width);
		init();
	}

	private void init() {
		doors = new Door[4];
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

	public static Vector2 getRandSize(Random rand) {
		return dimensions[rand.Next(dimensions.Length)];
	}

}
