using System;
using System.Collections.Generic;

namespace ZLADE
{
	public class Tile
	{
		public enum Direction
		{
			Horizontal,
			Vertical
		};
		public int length = 0;
		public Direction direction = Direction.Horizontal;
		public bool is3Byte = false;
		public int x = 0;
		public int y = 0;
		public int id = 0;
	}
}
