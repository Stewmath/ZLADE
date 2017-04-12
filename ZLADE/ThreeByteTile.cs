using System;

namespace ZLADE
{
	public class ThreeByteTile : TwoByteTile
	{
		public enum Direction
		{
			Horizontal,
			Vertical
		};
		public int length = 0;
		public Direction direction = Direction.Horizontal;
	}
}
