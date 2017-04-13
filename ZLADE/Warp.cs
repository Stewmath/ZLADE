using System;

namespace ZLADE
{
	public class Warp
	{
		public enum MapType
		{
			Overworld,
			Dungeon,
			Side
		}
		public MapType type = 0;
		public int map = 0;
		public int room = 0;
		public int x = 0;
		public int y = 0;
		public int after = 0;

		public static MapType getMapType(int i)
		{
			if (i == 0)
				return MapType.Overworld;
			if (i == 1)
				return MapType.Dungeon;
			if (i == 2)
				return MapType.Side;
			return MapType.Overworld;
		}
	}
}
