using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace _27Minutes {
	
	public static class TileManager {

		static List<Texture2D> tiles = new List<Texture2D>();

		static public Rectangle GetSourceRectangle() {
			return new Rectangle(0, 0, 32, 32);
		}

		public static void initialize(ContentManager Content) { 

			tiles.Add(Content.Load<Texture2D>("grey_dirt3"));
			tiles.Add(Content.Load<Texture2D>("cobble_blood3"));
			tiles.Add(Content.Load<Texture2D>("floor_vines3"));
			tiles.Add(Content.Load<Texture2D>("brick_brown2"));
			tiles.Add(Content.Load<Texture2D>("grey_dirt5"));
			tiles.Add(Content.Load<Texture2D>("brick_dark1"));
			tiles.Add(Content.Load<Texture2D>("zot_blue3"));
			tiles.Add(Content.Load<Texture2D>("grey_dirt1"));
			tiles.Add(Content.Load<Texture2D>("brick_brown3"));
		}

		public static Texture2D getTexture(int TileID) {
				return tiles[TileID];
		}
	}
}
