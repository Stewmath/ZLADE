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
		int ind = 0;
		public frmEvent(MapLoader l, int i)
		{
			InitializeComponent();
			m = l;
			ind = i;
		}

		private void frmEvent_Load(object sender, EventArgs e)
		{
			button1.Left = (groupBox1.Width / 2) - (button1.Width / 2);
			if (ind == 2)
			{
				cEvent.SelectedIndex = m.iRoomEvent.id / 2;
				cTrigger.SelectedIndex = m.iRoomEvent.trigger;
			}
			else if(ind == 0)
			{
				cEvent.SelectedIndex = m.roomEvent.id / 2;
				cTrigger.SelectedIndex = m.roomEvent.trigger;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Event ev = new Event();
			ev.id = cEvent.SelectedIndex * 2;
			ev.trigger = cTrigger.SelectedIndex;
			if (ind == 0)
				m.roomEvent = ev;
			else
				m.iRoomEvent = ev;
			this.Close();
		}
	}
}
