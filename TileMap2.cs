using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _27Minutes {

	class TileMap2 {

		public List<MapRow> Rows = new List<MapRow>();
		public int MapWidth = 32;
		public int MapHeight = 32;
		private int temp;

		public TileMap2(Random rand) {

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
		}
	}
}
