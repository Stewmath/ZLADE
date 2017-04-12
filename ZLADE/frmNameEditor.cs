using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZLADE
{
	public partial class frmNameEditor : Form
	{
		MapLoader m;
		public frmNameEditor(MapLoader l)
		{
			InitializeComponent();
			m = l;
		}

		private void frmNameEditor_Load(object sender, EventArgs e)
		{
			cDungeon.SelectedIndex = 0;
		}

		private void cDungeon_SelectedIndexChanged(object sender, EventArgs e)
		{
			tText.Text = m.dungeonNames[cDungeon.SelectedIndex];
		}

		private void tText_TextChanged(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			m.dungeonNames[cDungeon.SelectedIndex] = tText.Text;
			this.Close();
		}
	}
}
