using System;
using System.Collections.Generic;
using System.Drawing;

namespace ZLADE
{
	class MinimapSelector
	{
		int selected = 0;
		public int SelectedMap
		{
			get { return selected; }
			set { selected = value; }
		}

		public Point GetSelectedPoint()
		{
            Point returnPoint;
            int y = 0;
            int x = 0;
            int s = selected;
            y = s / 8;
            x = s - (y * 8);
            x = x * 8;
            y = (y * 8);
            returnPoint = new Point(x, y);
            return returnPoint;
		}
	}
}
