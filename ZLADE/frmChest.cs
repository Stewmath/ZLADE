using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZLADE
{
	public partial class frmChest : Form
	{
		MapLoader m;
		int tabindex = 0;
		public frmChest(MapLoader l, int t)
		{
			InitializeComponent();
			m = l;
			tabindex = t;
		}

		private void frmChest_Load(object sender, EventArgs e)
		{
			button1.Left = (this.Width / 2) - (button1.Width / 2);
			if (tabindex == 0)
				nChest.Value = (decimal)m.chestValue;
			else if (tabindex == 1)
				nChest.Value = (decimal)m.oChestValue;
			else if (tabindex == 2)
				nChest.Value = (decimal)m.iChestValue;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (tabindex == 0)
				m.chestValue = (byte)nChest.Value;
			else if (tabindex == 1)
				m.oChestValue = (byte)nChest.Value;
			else if (tabindex == 2)
				m.iChestValue = (byte)nChest.Value;
			this.Close();
		}
	}
}
