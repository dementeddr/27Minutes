using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _27Minutes {
	
	public class Tile {
		public Texture2D texture;
		public Vector2 position;
		public tileType type = tileType.AIR;

		public Tile() {

		}

		public Tile(int x, int y, tileType type) {
			position = new Vector2(x, y);
			this.type = type;
		}

	}
}
