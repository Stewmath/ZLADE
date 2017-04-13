using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZLADE
{
	public partial class frmDump : Form
	{
		Form1 frm;
		public frmDump(Form1 f)
		{
			InitializeComponent();
			frm = f;
		}

		private void frmDump_Load(object sender, EventArgs e)
		{
			button1.Left = (this.Width / 2) - (button1.Width / 2);
			for (int i = 0; i < OffsetLoader.loadedOffsets.Count; i++)
			{
				comboBox1.Items.Add(OffsetLoader.loadedOffsets[i].name);
			}
			if (comboBox1.Items.Count > 0)
				comboBox1.SelectedIndex = 0;
			else
			{
				comboBox1.Items.Add("Default");
				comboBox1.SelectedIndex = 0;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (OffsetLoader.loadedOffsets.Count > 0)
				OffsetLoader.activeOffset = OffsetLoader.loadedOffsets[comboBox1.SelectedIndex];
			else
				OffsetLoader.activeOffset = new LoadedOffset();
			frm.dumpOK();
			this.Close();
		}
	}
}
