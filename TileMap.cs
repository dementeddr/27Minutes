using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _27Minutes {

	public class TileMap {

		public List<MapRow> Rows = new List<MapRow>();
		public int MapWidth = 40;
		public int MapHeight = 20;
		private int[] path;
		private int dist = 0;
		private int wait = 0;

		public TileMap(Random rand) {

			path = new int[MapWidth - 6];

			for (int y = 0; y < MapHeight; y++) {

				MapRow thisRow = new MapRow();

				for (int x = 0; x < MapWidth; x++) {
					if (y == 0 || y >= (MapHeight - 2) || x == 0 || x >= (MapWidth - 2)) {
						thisRow.Columns.Add(new MapCell(3));
					} else { 
						if (rand.Next(5) >= 4) {
							thisRow.Columns.Add(new MapCell(4));
						} else {
							thisRow.Columns.Add(new MapCell(0));
						}
					}

					
				}

				Rows.Add(thisRow);
			}

			Rows[MapHeight - 7].Columns[1].BaseTiles[0] = 3;
			Rows[MapHeight - 7].Columns[2].BaseTiles[0] = 3;

			Rows[MapHeight - 9].Columns[MapWidth - 3].BaseTiles[0] = 3;
			Rows[MapHeight - 9].Columns[MapWidth - 4].BaseTiles[0] = 3;

			Rows[MapHeight - 8].Columns[0].BaseTiles[0] = 6;
			Rows[MapHeight - 9].Columns[0].BaseTiles[0] = 6;

			Rows[MapHeight - 10].Columns[MapWidth - 2].BaseTiles[0] = 6;
			Rows[MapHeight - 11].Columns[MapWidth - 2].BaseTiles[0] = 6;

			for (int i = 0; i < (MapWidth - 7); i++) {
				//path[i] =;
				Rows[ rand.Next(MapHeight - 6)].Columns[i+3].BaseTiles[0] = 5;
			}
		}
	}
}
