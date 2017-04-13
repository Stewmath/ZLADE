using System;
using System.Collections.Generic;

namespace ZLADE
{
	public class DuoTile
	{
		public int originalID = 0;
		public int tileCount = 0;
		public int hTiles = 0;
		public int vTiles = 0;
		public List<int> tileIDs = new List<int>();

		public DuoTile(int oID, int tC, int hT, int vT)
		{
			originalID = oID;
			tileCount = tC;
			hTiles = hT;
			vTiles = vT;
		}

		public DuoTile()
		{
		}

		public void addTile(int id)
		{
			tileIDs.Add(id);
		}
	}
}
