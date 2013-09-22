using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MapCell {

	public List<int> BaseTiles = new List<int>();
	public int TileID;
	public enum tileType { SOLID, AIR, LADDER, HAZARD, DOOR }

	public int getTileID() { 
			return BaseTiles.Count > 0 ? BaseTiles[0] : 0; 
	}

	public void	setTileID(int value) {
			if (BaseTiles.Count > 0)
				BaseTiles[0] = value;
			else
				AddBaseTile(value);
		}

	public void AddBaseTile(int tileID) {
		BaseTiles.Add(tileID);
	}

	public MapCell(int tileID) {
		TileID = tileID;
	}

	public tileType getTileType() {
		if (TileID == 0 || TileID == 4 || TileID == 7)
			return tileType.AIR;
		else 
			return tileType.SOLID;
	}
}