using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _27Minutes {
	
	public static class Tile {
		public static Texture2D texture;
		public  static tileType type;

		static public Rectangle GetSourceRectangle(int tileIndex, int scalar) {
			return new Rectangle(tileIndex * 32, 0, 32, 32);
		}

	}
}
