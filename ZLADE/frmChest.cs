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
		public frmChest(MapLoader l)
		{
			InitializeComponent();
			m = l;
		}

		private void frmChest_Load(object sender, EventArgs e)
		{
			button1.Left = (this.Width / 2) - (button1.Width / 2);
			nChest.Value = (decimal)m.chestValue;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			m.chestValue = (byte)nChest.Value;
			this.Close();
		}
	}
}
