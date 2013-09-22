using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace _27Minutes {

	public class TileMap {

		public List<MapRow> Rows = new List<MapRow>();
		public int MapWidth = 60;
		public int MapHeight = 20;
		//private int[] path;
		private int dist = 0;
		private int wait = 0;
		private int length = 0;
		private int height = 0;
		private int prev = 9;
		private int temp;

		public TileMap(Random rand) {

			//path = new int[MapWidth - 6];

			for (int y = 0; y < MapHeight; y++) {

				MapRow thisRow = new MapRow();

				for (int x = 0; x < MapWidth; x++) {
					if (y == 0 || y >= (MapHeight - 2) || x == 0 || x >= (MapWidth - 2)) {
						thisRow.Columns.Add(new MapCell(5));
					} else { 
						if ((temp = rand.Next(10)) < 7) {
							thisRow.Columns.Add(new MapCell(0));
						} else {
							if (temp < 9)
								thisRow.Columns.Add(new MapCell(4));
							else
								thisRow.Columns.Add(new MapCell(7));
						}
					}
				}

				Rows.Add(thisRow);
			}

			Rows[MapHeight - 8].Columns[0].setTileID(3);
			Rows[MapHeight - 8].Columns[1].setTileID(3);
			Rows[MapHeight - 8].Columns[2].setTileID(3);

			Rows[MapHeight - 10].Columns[MapWidth - 2].setTileID(3);
			Rows[MapHeight - 10].Columns[MapWidth - 3].setTileID(3);
			Rows[MapHeight - 10].Columns[MapWidth - 4].setTileID(3);

			Rows[MapHeight - 10].Columns[0].setTileID(6);
			Rows[MapHeight - 9].Columns[0].setTileID(6);

			Rows[MapHeight - 12].Columns[MapWidth - 2].setTileID(6);
			Rows[MapHeight - 11].Columns[MapWidth - 2].setTileID(6);

			for (int i = 4; i < (MapWidth - 5); i++) {

				if (wait == 0) {
					length = rand.Next(4) + 2;
					wait = length + rand.Next(3) + 2;
					prev = nextHeight(rand);
					Rows[MapHeight - prev].Columns[i].setTileID(3);

				} else {
					if (length > 0) {
						Rows[MapHeight - prev].Columns[i].setTileID(3);
						length--;
					}
					wait--;
				}
			}
		}

		private int nextHeight(Random rand) {
			int max = MapHeight - 4;
			int min = 5;

			temp = prev - 4;
			min = (int) MathHelper.Max(min, temp);

			temp = prev + 4;
			max = (int) MathHelper.Min(max, temp);

			do {
				temp = rand.Next(max - min) + min;
			} while (temp == prev);

			return temp;
		}
	}
}
