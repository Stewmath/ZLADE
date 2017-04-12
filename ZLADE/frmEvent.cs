using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZLADE
{
	public partial class frmEvent : Form
	{
		MapLoader m;
		public frmEvent(MapLoader l)
		{
			InitializeComponent();
			m = l;
		}

		private void frmEvent_Load(object sender, EventArgs e)
		{
			button1.Left = (groupBox1.Width / 2) - (button1.Width / 2);
			cEvent.SelectedIndex = m.roomEvent.id / 2;
			cTrigger.SelectedIndex = m.roomEvent.trigger;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Event ev = new Event();
			ev.id = cEvent.SelectedIndex * 2;
			ev.trigger = cTrigger.SelectedIndex;
			m.roomEvent = ev;
			this.Close();
		}
	}
}
