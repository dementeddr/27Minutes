using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _27Minutes {

	public class TileMap {

		public List<MapRow> Rows = new List<MapRow>();
		public int MapWidth = 64;
		public int MapHeight = 64;

		public TileMap(Random rand) {
			for (int y = 0; y < MapHeight; y++) {

				MapRow thisRow = new MapRow();

				for (int x = 0; x < MapWidth; x++) {
					if (rand.Next(5) >= 4)
						thisRow.Columns.Add(new MapCell(1));
					thisRow.Columns.Add(new MapCell(0));
				}
				Rows.Add(thisRow);
			}

			

			// End Create Sample Map Data
		}
	}
}
