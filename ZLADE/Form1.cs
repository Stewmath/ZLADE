using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ZLADE
{
	public partial class Form1 : Form
	{
		private MinimapSelector minimap = new MinimapSelector();
		bool ddrawGrid = true;
		MapLoader m;
		public TilesetLoader tloader = new TilesetLoader();
		IndoorTilesetLoader iloader = new IndoorTilesetLoader();
		public Image imgMinimap = Image.FromFile("./Images/DXMinimap.bmp");
		public Image imgTileset1 = Image.FromFile("./Images/DX 1 Tileset.bmp");
		public Image imgTileset2 = Image.FromFile("./Images/DX 2 Tileset.bmp");
		public Image imgTileset3 = Image.FromFile("./Images/DX 3 Tileset.bmp");
		public Image imgTileset4 = Image.FromFile("./Images/DX 4 Tileset.bmp");
		public Image imgTileset5 = Image.FromFile("./Images/DX 5 Tileset.bmp");
		public Image imgTileset6 = Image.FromFile("./Images/DX 6 Tileset.bmp");
		public Image imgTileset7 = Image.FromFile("./Images/DX 7 Tileset.bmp");
		public Image imgTileset8 = Image.FromFile("./Images/DX 8 Tileset.bmp");
		public Image imgTileset9 = Image.FromFile("./Images/DX 9 Tileset.bmp");
		public Image imgSOG = Image.FromFile("./Images/DX SOG.bmp");
		public Image imgSide = Image.FromFile("./Images/DX Side Tileset.bmp");
		public Image imgSide2 = Image.FromFile("./Images/DX 2 Side Tileset.bmp");
		public Image imgSide6 = Image.FromFile("./Images/DX 6 Side Tileset.bmp");
		public Image imgSide7 = Image.FromFile("./Images/DX 7 Side Tileset.bmp");
		public Image imgSide8 = Image.FromFile("./Images/DX 8 Side Tileset.bmp");
		public Image imgSprites = Image.FromFile("./Images/DX Sprites.bmp");
		public Image imgDoors = Image.FromFile("./Images/DX 1 DTileset.bmp");
		public Image imgEntrances = Image.FromFile("./Images/DX Entrances.bmp");
		public Image[] spriteImages = new Bitmap[256];
		int room = 1;
		int oRoom = 0;
		public bool threeobjects = true;
		public bool twoobjects = true;
		public bool doors = true;
		public bool walltemplate = true;
		public bool floortemplate = true;
		public bool objectborders = true;
		public bool spriteborders = true;
		public int mouseStyle = 0;
		frmGraphic frmGraphics;
		frmRepoint frmRepoint;
		frmMinimap frmMinimap;
		frmEvent frmEvent;
		frmChest frmChest;
		frmNameEditor frmNameEditor;
		frmSpriteRepoint frmSprite;
		frmEventPos frmEventPos;
		frmPalette frmPalette;
		frmOMinimapEditor frmOMinimap;
		int selectedTile = 0;
		int selectedSprite = -1;
		Point mousePoint = new Point(0, 0);
		bool hoverBox = false;
		public string[] spriteNames = new string[256];
		public int fakeSpace = 0;
		public int oFakeSpace = 0;
		public int iFakeSpace = 0;
		int oMouse = 1;
		int oSelectedTile = 0;
		int selectedOSprite = -1;
		public List<DuoTile> duoTiles = new List<DuoTile>();
		public List<DuoTile> doorTiles = new List<DuoTile>();
		public List<int> duoTileIDs = new List<int>();
		public List<int> doorTileIDs = new List<int>();
		int xyroom = 0;
		int roomx = 0;
		int roomy = 0;
		int iRoom = 0;
		int iRegion = 0;
		int iMouse = 1;
		int iSelectedTile = 0;
		int selectedISprite = -1;
		frmDump frmDump;
		bool dungeonTilesetsFromRom = true;

		public void loadSpriteImages()
		{
			for (int i = 0; i < 256; i++)
			{
				try
				{
					Image img = Image.FromFile(Application.StartupPath + "/Images/Sprites/" + i.ToString("X") + ".png");
					spriteImages[i] = img;
				}
				catch (Exception) { }
			}
		}

		public int getDuoIndex(int id)
		{
			for (int i = 0; i < duoTileIDs.Count; i++)
			{
				if (duoTiles[i].originalID == id)
					return i;
			}
			return -1;
		}
		public int getDoorIndex(int id)
		{
			for (int i = 0; i < doorTileIDs.Count; i++)
			{
				if (doorTiles[i].originalID == id)
					return i;
			}
			return -1;
		}

		public void loadDuoTiles()
		{
			try
			{
				StreamReader s = new StreamReader(Application.StartupPath + "/ObjectCombos.txt");
				string sss = s.ReadToEnd();
				string[] lines = sss.Replace("\r", "").Split('\n');
				s.Close();
				for (int i = 0; i < lines.Length; i++)
				{
					string command = lines[i].Split('=')[0];
					if (command.Equals("obj"))
					{
						DuoTile t = new DuoTile();
						int tid = int.Parse(lines[i].Split('=')[1]);
						duoTileIDs.Add(tid);
						int tcount = int.Parse(lines[i + 1].Split('=')[1]);
						int w = int.Parse(lines[i + 2].Split('=')[1]);
						int h = int.Parse(lines[i + 3].Split('=')[1]);
						t.originalID = tid;
						t.tileCount = tcount;
						t.hTiles = w;
						t.vTiles = h;
						for (int k = 0; k < tcount; k++)
						{
							int v = int.Parse(lines[i + 4 + k].Split('=')[1]);
							t.addTile(v);
						}
						duoTiles.Add(t);
					}
				}
			}
			catch (IOException e)
			{
				MessageBox.Show("An error occured while loading ObjectCombos.txt.\n\n" + e.Message, "Error");
			}
		}

		public void loadDoorTiles()
		{
			try
			{
				StreamReader s = new StreamReader(Application.StartupPath + "/DoorCombos.txt");
				string sss = s.ReadToEnd();
				string[] lines = sss.Replace("\r", "").Split('\n');
				s.Close();
				for (int i = 0; i < lines.Length; i++)
				{
					string command = lines[i].Split('=')[0];
					if (command.Equals("obj"))
					{
						DuoTile t = new DuoTile();
						int tid = int.Parse(lines[i].Split('=')[1]);
						doorTileIDs.Add(tid);
						int tcount = int.Parse(lines[i + 1].Split('=')[1]);
						int w = int.Parse(lines[i + 2].Split('=')[1]);
						int h = int.Parse(lines[i + 3].Split('=')[1]);
						t.originalID = tid;
						t.tileCount = tcount;
						t.hTiles = w;
						t.vTiles = h;
						for (int k = 0; k < tcount; k++)
						{
							int v = int.Parse(lines[i + 4 + k].Split('=')[1]);
							t.addTile(v);
						}
						doorTiles.Add(t);
					}
				}
			}
			catch (IOException e)
			{
				MessageBox.Show("An error occured while loading ObjectCombos.txt.\n\n" + e.Message, "Error");
			}
		}

		public bool isSpecialObject(int id, int level)
		{
			if (level == 1)
			{
				if (id == 0xDD || id == 0xDE)
					return true;
			}
			if (level == 3)
			{
				if (id == 0xDE || id == 0x4F || id == 0x88)
					return true;
			}
			if (level == 4)
			{
				if (id == 0xDE)
					return true;
			}
			if (level == 5)
			{
				if (id == 0xDD || id == 0x9E || id == 0xDE || id == 0x4F)
					return true;
			}
			if (level == 6)
			{
				if (id == 0xDE)
					return true;
			}
			if (level == 7)
			{
				if (id == 0xDE)
					return true;
			}
			if (level == 8)
			{
				if (id == 0xDE)
					return true;
			}
			return false;
		}

		public int getSOG(int id, int level)
		{
			if (level == 1)
			{
				if (id == 0xDD)
					return 0;
				if (id == 0xDE)
					return 1;
			}
			if (level == 3)
			{
				if (id == 0xDE)
					return 1;
				if (id == 0x4F)
					return 2;
				if (id == 0x88)
					return 3;
			}
			if (level == 4)
			{
				if (id == 0xDE)
					return 1;
			}
			if (level == 5)
			{
				if (id == 0xDD)
					return 4;
				if (id == 0x9E)
					return 5;
				if (id == 0xDE)
					return 1;
				if (id == 0x4F)
					return 6;
			}
			if (level == 6)
			{
				if (id == 0xDE)
					return 1;
			}
			if (level == 7)
			{
				if (id == 0xDE)
					return 1;
			}
			if (level == 8)
			{
				if (id == 0xDE)
					return 1;
			}
			return -1;
		}

		public void readSprites()
		{
			StreamReader s = new StreamReader("./Sprites.txt");
			string[] lines = s.ReadToEnd().Split('\n');
			for (int i = 0; i < lines.Length; i++)
			{
				string ind = lines[i].Substring(0, 2);
				int index = Convert.ToInt32(ind, 16);
				string desc = lines[i].Substring(3);
				spriteNames[index] = desc;
			}
			s.Close();
		}

		public Color MinimapColor
		{
			get
			{
				return Color.FromArgb(40, 48, 80);
			}
		}
		public Color SquareColor
		{
			get
			{
				return Color.FromArgb(255, 255, 173);
			}
		}
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			OffsetLoader.loadOffsets();
			cboLevel.SelectedIndex = 0;
			nID.Maximum = 0xFD;
			readSprites();
			loadDoorTiles();
			loadSpriteImages();
		}

		private void openROMToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openRom.ShowDialog();
		}

		private void drawGrid(Graphics g)
		{
			for (int x = 0; x < 9; x++)
			{
				g.DrawLine(Pens.Yellow, x * 8, 0, x * 8, 64);
			}
			for (int y = 0; y < 9; y++)
			{
				g.DrawLine(Pens.Yellow, 0, y * 8, 64, y * 8);
			}
		}

		private void pMinimap_Paint(object sender, PaintEventArgs e)
		{
			drawMinimap(e.Graphics);
			if(ddrawGrid)
				drawGrid(e.Graphics);
			drawMinimapBox(e.Graphics);
		}

		private void pMinimap_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.X >= 64 || e.Y >= 64 || m == null)
				return;
			int i = 0;
			int x = e.X / 8;
			int y = e.Y / 8;
			i = x + (y * 8);
			int r = 0;
			xyroom = i;
			roomx = x;
			roomy = y;
			r = m.realMinimap[i];
			minimap.SelectedMap = i;
			pMinimap.Invalidate();
			lCurrent.Text = "Current: 0x" + r.ToString("X");

			m.openRom();
			m.clearData();
			
			m.getData((cboLevel.SelectedIndex < 9 ? cboLevel.SelectedIndex + 1 : 0xFF), r);
			setTileset(cboLevel.SelectedIndex + 1);
			m.readSprites(cboLevel.SelectedIndex + 1, r + (cboLevel.SelectedIndex > 5 && cboLevel.SelectedIndex < 9 ? 0x100 : 0));
			if (m.sprites.Count > 0)
				nSprite.Maximum = m.sprites.Count - 1;
			else
				nSprite.Maximum = 0;
			m.readEvent(cboLevel.SelectedIndex + 1, r + (cboLevel.SelectedIndex > 5 && cboLevel.SelectedIndex < 9 ? 0x100 : 0));
			m.readChest(cboLevel.SelectedIndex + 1, r + (cboLevel.SelectedIndex > 5 && cboLevel.SelectedIndex < 9 ? 0x100 : 0));
			m.readDungeonNames();
			room = r;
			if (dungeonTilesetsFromRom)
				setTileset(cboLevel.SelectedIndex);
			m.formatData();
			selectedSprite = -1;
			nTile.Maximum = m.tiles.Count;
			mouseStyle = 0;
			button1.BackColor = SystemColors.ButtonHighlight;
			button3.BackColor = SystemColors.Control;
			button5.BackColor = SystemColors.Control;
			pMap.Invalidate();
			setHeader();
			setWarps();
			setSpace();
			m.closeRom();
		}

		public void setAnimTileset(int level, int anim)
		{
			if (dungeonTilesetsFromRom)
			{
				try
				{
					if (m.reader == null)
					{
						m.openRom();
						iloader.loadMap(m.reader, cboLevel.SelectedIndex, room, true, chkSide.Checked, anim);
						pTileset.Image = iloader.blockBmp;
						m.closeRom();
					}
					else
					{
						iloader.loadMap(m.reader, cboLevel.SelectedIndex, room, true, chkSide.Checked, anim);
						pTileset.Image = iloader.blockBmp;
					}
				}
				catch (Exception)
				{
					m.openRom();
					iloader.loadMap(m.reader, cboLevel.SelectedIndex, room, true, chkSide.Checked, anim);
					pTileset.Image = iloader.blockBmp;
					m.closeRom();
				}
				return;
			}
		}

		public void setTileset(int level)
		{
			if (dungeonTilesetsFromRom)
			{
				try
				{
					if (m.reader == null)
					{
						m.openRom();
						if (cboLevel.SelectedIndex < 9)
							iloader.loadMap(m.reader, cboLevel.SelectedIndex, room, false, chkSide.Checked);
						else
							iloader.loadMap(m.reader, 0xFF, room, false, chkSide.Checked);
						pTileset.Image = iloader.blockBmp;
						m.closeRom();
					}
					else
					{
						if (cboLevel.SelectedIndex < 9)
							iloader.loadMap(m.reader, cboLevel.SelectedIndex, room, false, chkSide.Checked);
						else
							iloader.loadMap(m.reader, 0xFF, room, false, chkSide.Checked);
						pTileset.Image = iloader.blockBmp;
					}
				}
				catch (Exception)
				{
					m.openRom();
					if (cboLevel.SelectedIndex < 9)
						iloader.loadMap(m.reader, cboLevel.SelectedIndex, room, false, chkSide.Checked);
					else
						iloader.loadMap(m.reader, 0xFF, room, false, chkSide.Checked);
					pTileset.Image = iloader.blockBmp;
					m.closeRom();
				}
				return;
			}
			if (!chkSide.Checked)
			{
				if (level == 1)
				{
					pTileset.Image = imgTileset1;
				}
				if (level == 2)
				{
					pTileset.Image = imgTileset2;
				}
				if (level == 3)
				{
					pTileset.Image = imgTileset3;
				}
				if (level == 4)
				{
					pTileset.Image = imgTileset4;
				}
				if (level == 5)
				{
					pTileset.Image = imgTileset5;
				}
				if (level == 6)
				{
					pTileset.Image = imgTileset6;
				}
				if (level == 7)
				{
					pTileset.Image = imgTileset7;
				}
				if (level == 8)
				{
					pTileset.Image = imgTileset8;
				}
				if (level == 9)
				{
					pTileset.Image = imgTileset9;
				}
			}
			else
			{
				if (level == 2 || level == 4)
				{
					pTileset.Image = imgSide2;
				}
				else if (level == 6)
				{
					pTileset.Image = imgSide6;
				}
				else if (level == 7)
				{
					pTileset.Image = imgSide7;
				}
				else if (level == 8)
				{
					pTileset.Image = imgSide8;
				}
				else
				{
					pTileset.Image = imgSide;
				}
			}
			setDoorTileset();
		}

		public int GetMap(int x, int y)
		{
			if (room < 0xB)
				return m.readByte(0x50220 + cboLevel.SelectedIndex * 0x40 + x + y * 8);
			else
				return m.readByte(0x504E0 + x + y * 8);
		}

		private void pMinimap_Click(object sender, EventArgs e)
		{

		}

		private void drawMinimapBox(Graphics g)
		{
			Point p = minimap.GetSelectedPoint();
			g.DrawRectangle(Pens.Red, p.X, p.Y, 8, 8);
		}

		private void minimapGridToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			bool b = minimapGridToolStripMenuItem.Checked;
			ddrawGrid = b;
			pMinimap.Invalidate();
		}

		void loadRoom(int level, int room)
		{
			m = new MapLoader(openRom.FileName);
			m.openRom();
			m.readBorderTiles();
			m.getData(level, room);
			m.readMinimap(level);
			m.readRealMinimap(level);
			m.formatData();
			m.readEvent(level, room);
			m.readChest(level, room);
			m.readDungeonNames();
			nTile.Maximum = m.tiles.Count;
			mouseStyle = 0;
			button1.BackColor = SystemColors.ButtonHighlight;
			button3.BackColor = SystemColors.Control;
			if (m.sprites.Count > 0)
				nSprite.Maximum = m.sprites.Count - 1;
			else
				nSprite.Maximum = 0;
			setTileset(level);
			pMinimap.Invalidate();
			pMap.Invalidate();
			setHeader();
			setWarps();
			m.closeRom();
		}

		private void openRom_FileOk(object sender, CancelEventArgs e)
		{
			frmDump = new frmDump(this);
			frmDump.ShowDialog();
		}

		public void setWarps()
		{
			Warp w;
			if (m.warps.Count > 0 && m.hasWarp)
			{
				w = m.warps[0];
				cHasWarp.Checked = true;
				if (w.type == Warp.MapType.Dungeon)
					cMapType.SelectedIndex = 1;
				else if (w.type == Warp.MapType.Overworld)
					cMapType.SelectedIndex = 0;
				else
					cMapType.SelectedIndex = 2;
				if (w.type != Warp.MapType.Overworld)
				{
					nDRegion.Value = w.map;
					nDRegion.Enabled = true;
				}
				else
					nDRegion.Enabled = false;
				nRoom.Value = (decimal)w.room;
				nDestX.Value = (decimal)w.x;
				nDestY.Value = (decimal)w.y;
				cMapType.Enabled = true;
				nRoom.Enabled = true;
				nDestX.Enabled = true;
				nDestY.Enabled = true;
			}
			else
			{
				cHasWarp.Checked = false;
				cMapType.Enabled = false;
				nDRegion.Enabled = false;
				nRoom.Enabled = false;
				nDestX.Enabled = false;
				nDestY.Enabled = false;
			}
		}

		public void drawMinimap(Graphics g)
		{
			int x = 0;
			int y = 0;
			if (m == null)
				return;
			for (int i = 0; i < 64; i++)
			{
				byte b = m.minimap[i];
				if (b == 0x7D) //Empty
					g.FillRectangle(new SolidBrush(MinimapColor), (x * 8), (y * 8), 8, 8);
				if(b == 0xEF) //Room
					g.FillRectangle(new SolidBrush(SquareColor), (x * 8), (y * 8), 8, 8);
				if (b == 0xEE) //Boss
					g.DrawImage(getMinimapIcon(16, 0), x * 8, y * 8);
				if (b == 0xED) //Chest
					g.DrawImage(getMinimapIcon(8, 0), x * 8, y * 8);
				x++;
				if (x == 8)
				{
					x = 0;
					y++;
				}
			}
		}

		public Bitmap getMinimapIcon(int x, int y)
		{
			Bitmap i = new Bitmap(8, 8);
			Graphics g = Graphics.FromImage(i);
			Rectangle r = new Rectangle(0,0,8,8);
			g.DrawImage(imgMinimap, r, x, y, 8, 8, GraphicsUnit.Pixel);
			return i;
		}

		private void cboLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.openRom();
			int i = cboLevel.SelectedIndex + 1;
			m.readMinimap(i);
			m.readRealMinimap(i);
			setTileset(cboLevel.SelectedIndex + 1);
			pMinimap.Invalidate();
			m.closeRom();
		}

		public Bitmap getOTile(int id)
		{
			char[] c = id.ToString("X").ToCharArray();
			int y = 0;
			int x = 0;
			if (c.Length > 1)
			{
				y = Convert.ToInt32(c[0].ToString(), 16);
				x = Convert.ToInt32(c[1].ToString(), 16);
			}
			else
			{
				x = Convert.ToInt32(c[0].ToString(), 16);
			}

			Bitmap b = new Bitmap(16, 16);
			Graphics g = Graphics.FromImage(b);
			Rectangle r = new Rectangle(0, 0, 16, 16);
			g.DrawImage(pOTileset.Image, r, x * 16, y * 16, 16, 16, GraphicsUnit.Pixel);
			return b;
		}

		public Bitmap getTile(int id)
		{
			if (cSOG.Checked)
			{
				if (!isSpecialObject(id, cboLevel.SelectedIndex + 1))
				{
					char[] c = id.ToString("X").ToCharArray();
					int y = 0;
					int x = 0;
					if (c.Length > 1)
					{
						y = Convert.ToInt32(c[0].ToString(), 16);
						x = Convert.ToInt32(c[1].ToString(), 16);
					}
					else
					{
						x = Convert.ToInt32(c[0].ToString(), 16);
					}

					Bitmap b = new Bitmap(16, 16);
					Graphics g = Graphics.FromImage(b);
					Rectangle r = new Rectangle(0, 0, 16, 16);
					g.DrawImage(pTileset.Image, r, x * 16, y * 16, 16, 16, GraphicsUnit.Pixel);
					return b;
				}
				else
				{
					int i = getSOG(id, cboLevel.SelectedIndex + 1);
					Bitmap b = new Bitmap(16, 16);
					Graphics g = Graphics.FromImage(b);
					Rectangle r = new Rectangle(0, 0, 16, 16);
					g.DrawImage(imgSOG, r, i * 16, 0, 16, 16, GraphicsUnit.Pixel);
					return b;
				}
			}
			else
			{
				char[] c = id.ToString("X").ToCharArray();
				int y = 0;
				int x = 0;
				if (c.Length > 1)
				{
					y = Convert.ToInt32(c[0].ToString(), 16);
					x = Convert.ToInt32(c[1].ToString(), 16);
				}
				else
				{
					x = Convert.ToInt32(c[0].ToString(), 16);
				}

				Bitmap b = new Bitmap(16, 16);
				Graphics g = Graphics.FromImage(b);
				Rectangle r = new Rectangle(0, 0, 16, 16);
				g.DrawImage(pTileset.Image, r, x * 16, y * 16, 16, 16, GraphicsUnit.Pixel);
				return b;
			}
		}

		public Bitmap getITile(int id)
		{
			char[] c = id.ToString("X").ToCharArray();
			int y = 0;
			int x = 0;
			if (c.Length > 1)
			{
				y = Convert.ToInt32(c[0].ToString(), 16);
				x = Convert.ToInt32(c[1].ToString(), 16);
			}
			else
			{
				x = Convert.ToInt32(c[0].ToString(), 16);
			}

			Bitmap b = new Bitmap(16, 16);
			Graphics g = Graphics.FromImage(b);
			Rectangle r = new Rectangle(0, 0, 16, 16);
			g.DrawImage(pITileset.Image, r, x * 16, y * 16, 16, 16, GraphicsUnit.Pixel);
			return b;
			/*if (cSOG.Checked)
			{
				if (!isSpecialObject(id, cboLevel.SelectedIndex + 1))
				{
					char[] c = id.ToString("X").ToCharArray();
					int y = 0;
					int x = 0;
					if (c.Length > 1)
					{
						y = Convert.ToInt32(c[0].ToString(), 16);
						x = Convert.ToInt32(c[1].ToString(), 16);
					}
					else
					{
						x = Convert.ToInt32(c[0].ToString(), 16);
					}

					Bitmap b = new Bitmap(16, 16);
					Graphics g = Graphics.FromImage(b);
					Rectangle r = new Rectangle(0, 0, 16, 16);
					g.DrawImage(pTileset.Image, r, x * 16, y * 16, 16, 16, GraphicsUnit.Pixel);
					return b;
				}
				else
				{
					int i = getSOG(id, cboLevel.SelectedIndex + 1);
					Bitmap b = new Bitmap(16, 16);
					Graphics g = Graphics.FromImage(b);
					Rectangle r = new Rectangle(0, 0, 16, 16);
					g.DrawImage(imgSOG, r, i * 16, 0, 16, 16, GraphicsUnit.Pixel);
					return b;
				}
			}
			else
			{
				char[] c = id.ToString("X").ToCharArray();
				int y = 0;
				int x = 0;
				if (c.Length > 1)
				{
					y = Convert.ToInt32(c[0].ToString(), 16);
					x = Convert.ToInt32(c[1].ToString(), 16);
				}
				else
				{
					x = Convert.ToInt32(c[0].ToString(), 16);
				}

				Bitmap b = new Bitmap(16, 16);
				Graphics g = Graphics.FromImage(b);
				Rectangle r = new Rectangle(0, 0, 16, 16);
				g.DrawImage(pTileset.Image, r, x * 16, y * 16, 16, 16, GraphicsUnit.Pixel);
				return b;
			}*/
		}

		public void setDoorTileset()
		{
			int id = (int)nDoor.Value;
			int i = id - 0xEE;
			int x = 0;
			if (id != 0xFC)
			{
				if (i == -2)
					i = 1;
				else if (i == -1)
					i = 0;
				else
					i += 2;
				x = i * 32;
				Bitmap b = new Bitmap(32, 32);
				Graphics g = Graphics.FromImage(b);
				Rectangle r = new Rectangle(0, 0, 32, 32);
				int y = cboLevel.SelectedIndex * 32;
				g.DrawImage(imgDoors, r, x, y, 32, 32, GraphicsUnit.Pixel);
				b.MakeTransparent(Color.Magenta);
				pDoor.Image = b;
			}
			else
			{
				Bitmap b = new Bitmap(64, 48);
				Graphics g = Graphics.FromImage(b);
				Rectangle r = new Rectangle(0, 0, 64, 48);
				x = cboLevel.SelectedIndex * 64;
				g.DrawImage(imgEntrances, r, x, 0, 64, 48, GraphicsUnit.Pixel);
				pDoor.Image = b;
			}
		}

		public Bitmap getDoorTile(int id)
		{
			int i = id - 0xEE;
			int x = 0;
			if (id != 0xFC)
			{
				if (i == -2)
					i = 1;
				else if (i == -1)
					i = 0;
				else
					i += 2;
				x = i * 32;
				Bitmap b = new Bitmap(32, 32);
				Graphics g = Graphics.FromImage(b);
				Rectangle r = new Rectangle(0, 0, 32, 32);
				int y = cboLevel.SelectedIndex * 32;
				g.DrawImage(imgDoors, r, x, y, 32, 32, GraphicsUnit.Pixel);
				b.MakeTransparent(Color.Magenta);
				return b;
			}
			else
			{
				Bitmap b = new Bitmap(64, 48);
				Graphics g = Graphics.FromImage(b);
				Rectangle r = new Rectangle(0, 0, 64, 48);
				x = cboLevel.SelectedIndex * 64;
				g.DrawImage(imgEntrances, r, x, 0, 64, 48, GraphicsUnit.Pixel);
				return b;
			}
		}

		public void drawMap(int level, int room, Graphics g)
		{
			if (m == null)
				return;
			g.Clear(Color.Black);
			if (floortemplate)
			{
				Bitmap b = getTile(m.floorTileIndex);
				for (int x = 0; x < 10; x++)
				{
					for (int y = 0; y < 9; y++)
					{
						g.DrawImage(b, x * 16,y * 16);
					}
				}
			}
			if (walltemplate)
			{
				for (int i = 0; i < 64; i++)
				{
					TwoByteTile t = m.borderTiles[m.borderTileIndex, i];
					if (t == null)
						continue;
					Bitmap b = getTile(t.id);
					g.DrawImage(b, t.x * 16, t.y * 16);
				}
			}
			for (int i = 0; i < m.tiles.Count; i++)
			{
				Tile t = m.tiles[i];
				if (!t.is3Byte && twoobjects)
				{
					if (t.id >= 0xEC)
					{
						if (dungeonTilesetsFromRom)
						{
							if (doorTileIDs.Contains(t.id))
							{
								if (doors)
								{
									Bitmap b2 = getDDoorTile(getDoorIndex(t.id));
									g.DrawImage(b2, t.x * 16, t.y * 16);
								}
							}
						}
						else
						{
							if (doors)
							{
								Bitmap b2 = getDoorTile(t.id);
								g.DrawImage(b2, t.x * 16, t.y * 16);
							}
						}
						continue;
					}
					Bitmap b = getTile(t.id);
					g.DrawImage(b, t.x * 16, t.y * 16);
					continue;
				}
				if (t.is3Byte && threeobjects)
				{
					int x = t.x;
					int y = t.y;
					Bitmap b = getTile(t.id);
					for (int k = 0; k < t.length; k++)
					{
						g.DrawImage(b, x * 16, y * 16);
						if (t.direction == Tile.Direction.Horizontal)
							x++;
						else
							y++;
					}
				}
			}
			if (objectborders)
			{
				for (int i = 0; i < m.tiles.Count; i++)
				{
					Tile t = m.tiles[i];
					if (t.is3Byte)
					{
						if (t.direction == Tile.Direction.Horizontal)
						{
							g.DrawRectangle(Pens.Red, t.x * 16, t.y * 16, t.length * 16, 16);
						}
						else
						{
							g.DrawRectangle(Pens.Blue, t.x * 16, t.y * 16, 16, t.length * 16);
						}
					}
					else
					{
						g.DrawRectangle(Pens.Green, t.x * 16, t.y * 16, 16, 16);
					}
				}
			}
			if(cSprites.Checked)
				drawSprites(g);
		}

		public void drawSprites(Graphics g)
		{
			if (m == null)
				return;
			Pen p = new Pen(Brushes.White, 2);
			Pen p2 = new Pen(Brushes.Red, 2);
			for (int i = 0; i < m.sprites.Count; i++)
			{
				Sprite s = m.sprites[i];
				if (spriteImages[s.id] == null)
				{
					g.FillRectangle(Brushes.Black, s.x * 16, s.y * 16, 16, 16);
					if (selectedSprite != i)
						g.DrawRectangle(p, s.x * 16 + 1, s.y * 16 + 1, 14, 14);
					else
						g.DrawRectangle(p2, s.x * 16 + 1, s.y * 16 + 1, 14, 14);
				}
				else
				{
					Image img = spriteImages[s.id];
					Bitmap b = new Bitmap(img);
					b.MakeTransparent(Color.Magenta);
					g.DrawImage(b, s.x * 16, s.y * 16);
					if (spriteborders)
					{
						if (selectedSprite == i)
							g.DrawRectangle(Pens.Red, s.x * 16, s.y * 16, b.Width, b.Height);
						else
							g.DrawRectangle(Pens.White, s.x * 16, s.y * 16, b.Width, b.Height);
					}
				}
			}
		}

		public Bitmap getSpriteImage(int id)
		{
			Bitmap b = new Bitmap(16, 16);
			Graphics g = Graphics.FromImage(b);
			Rectangle r = new Rectangle(0, 0, 16, 16);
			g.DrawImage(imgSprites, r, id * 16, 0, 16, 16, GraphicsUnit.Pixel);
			b.MakeTransparent(Color.Magenta);
			return b;
		}

		public int getTileIndex(int x, int y)
		{
			for (int i = m.tiles.Count - 1; i > -1; i--)
			{
				if (m.tiles[i].x == x / 16 && m.tiles[i].y == y / 16)
				{
					return i;
				}
				else if(m.tiles[i].is3Byte)
				{
					int tx = m.tiles[i].x;
					int ty = m.tiles[i].y;
					for (int k = 0; k < m.tiles[i].length; k++)
					{
						if (tx == x / 16 && ty == y / 16)
						{
							return i;
						}
						if (m.tiles[i].direction == Tile.Direction.Horizontal)
							tx++;
						else
							ty++;
					}
				}
			}
			return -1;
		}

		public int getITileIndex(int x, int y)
		{
			for (int i = m.iTiles.Count - 1; i > -1; i--)
			{
				if (m.iTiles[i].x == x / 16 && m.iTiles[i].y == y / 16)
				{
					return i;
				}
				else if (!doorTileIDs.Contains(m.iTiles[i].id))
				{
					if (m.iTiles[i].is3Byte)
					{
						int tx = m.iTiles[i].x;
						int ty = m.iTiles[i].y;
						for (int k = 0; k < m.iTiles[i].length; k++)
						{
							if (tx == x / 16 && ty == y / 16)
							{
								return i;
							}
							if (m.iTiles[i].direction == Tile.Direction.Horizontal)
								tx++;
							else
								ty++;
						}
					}
				}
				else
				{
					DuoTile d = doorTiles[getDoorIndex(m.iTiles[i].id)];
					int tx = m.iTiles[i].x;
					int ty = m.iTiles[i].y;
					int rx = (x / 16);
					int ry = (y / 16);
					if (tx == 0xF)
						tx = -1;
					if (ty == 0xF)
						ty = -1;
					if (!m.iTiles[i].is3Byte)
					{
						if (tx <= rx && tx + d.hTiles > rx && ty <= ry && ty + d.vTiles - 1 >= ry)
							return i;
					}
					else
					{
						int lh = m.iTiles[i].length * d.hTiles;
						int lv = m.iTiles[i].length * d.vTiles;
						if (m.iTiles[i].direction == Tile.Direction.Horizontal)
						{
							if (tx <= rx && tx + lh > rx && ty <= ry && ty + d.vTiles - 1 >= ry)
								return i;
						}
						else
						{
							if (tx <= rx && tx + d.hTiles > rx && ty <= ry && ty + lv - 1 >= ry)
								return i;
						}
					}
				}
			}
			return -1;
		}

		public int getOTileIndex(int x, int y)
		{
			for (int i = m.oTiles.Count - 1; i > -1; i--)
			{
				if (m.oTiles[i].x == x / 16 && m.oTiles[i].y == y / 16)
				{
					return i;
				}
				else if (!duoTileIDs.Contains(m.oTiles[i].id))
				{
					if (m.oTiles[i].is3Byte)
					{
						int tx = m.oTiles[i].x;
						int ty = m.oTiles[i].y;
						for (int k = 0; k < m.oTiles[i].length; k++)
						{
							if (tx == x / 16 && ty == y / 16)
							{
								return i;
							}
							if (m.oTiles[i].direction == Tile.Direction.Horizontal)
								tx++;
							else
								ty++;
						}
					}
				}
				else
				{
					DuoTile d = duoTiles[getDuoIndex(m.oTiles[i].id)];
					int tx = m.oTiles[i].x;
					int ty = m.oTiles[i].y;
					int rx = (x / 16);
					int ry = (y / 16);
					if (tx == 0xF)
						tx = -1;
					if (ty == 0xF)
						ty = -1;
					if (!m.oTiles[i].is3Byte)
					{
						if (tx <= rx && tx + d.hTiles > rx && ty <= ry && ty + d.vTiles - 1 >= ry)
							return i;
					}
					else
					{
						int lh = m.oTiles[i].length * d.hTiles;
						int lv = m.oTiles[i].length * d.vTiles;
						if (m.oTiles[i].direction == Tile.Direction.Horizontal)
						{
							if (tx <= rx && tx + lh > rx && ty <= ry && ty + d.vTiles - 1 >= ry)
								return i;
						}
						else
						{
							if (tx <= rx && tx + d.hTiles > rx && ty <= ry && ty + lv - 1 >= ry)
								return i;
						}
					}
				}
			}
			return -1;
		}

		public int getSpriteWidth(int id)
		{
			if (File.Exists(Application.StartupPath + "/Images/Sprites/" + id.ToString("X") + ".PNG"))
			{
				Bitmap b = new Bitmap(Application.StartupPath + "/Images/Sprites/" + id.ToString("X") + ".PNG");
				return b.Width;
			}
			else
				return 16;
		}

		public int getSpriteHeight(int id)
		{
			if (File.Exists(Application.StartupPath + "/Images/Sprites/" + id.ToString("X") + ".PNG"))
			{
				Bitmap b = new Bitmap(Application.StartupPath + "/Images/Sprites/" + id.ToString("X") + ".PNG");
				return b.Height;
			}
			else
				return 16;
		}

		public int getSpriteIndex(int x, int y)
		{
			for (int i = m.sprites.Count - 1; i > -1; i--)
			{
				if (m.sprites[i].x == x / 16 && m.sprites[i].y == y / 16)
				{
					return i;
				}
				int w = getSpriteWidth(m.sprites[i].id);
				int h = getSpriteHeight(m.sprites[i].id);
				int x3 = m.sprites[i].x * 16;
				int y3 = m.sprites[i].y * 16;
				if (x >= x3 && y >= y3 && x < x3 + w && y < y3 + h)
				{
					return i;
				}
			}
			return -1;
		}

		public int getISpriteIndex(int x, int y)
		{
			for (int i = m.iSprites.Count - 1; i > -1; i--)
			{
				if (m.iSprites[i].x == x / 16 && m.iSprites[i].y == y / 16)
				{
					return i;
				}
				int w = getSpriteWidth(m.iSprites[i].id);
				int h = getSpriteHeight(m.iSprites[i].id);
				int x3 = m.iSprites[i].x * 16;
				int y3 = m.iSprites[i].y * 16;
				if (x >= x3 && y >= y3 && x < x3 + w && y < y3 + h)
				{
					return i;
				}
			}
			return -1;
		}

		public int getOSpriteIndex(int x, int y)
		{
			for (int i = m.oSprites.Count - 1; i > -1; i--)
			{
				if (m.oSprites[i].x == x / 16 && m.oSprites[i].y == y / 16)
				{
					return i;
				}
				int w = getSpriteWidth(m.oSprites[i].id);
				int h = getSpriteHeight(m.oSprites[i].id);
				int x3 = m.oSprites[i].x * 16;
				int y3 = m.oSprites[i].y * 16;
				if (x >= x3 && y >= y3 && x < x3 + w && y < y3 + h)
				{
					return i;
				}
			}
			return -1;
		}

		public void setHeader()
		{
			try
			{
				nAnimation.Value = (decimal)m.animationIndex;
				nWall.Value = (decimal)m.borderTileIndex;
				nFloor.Value = m.floorTileIndex;
			}
			catch (Exception)
			{
				MessageBox.Show("Invalid header.", "Error");
			}
		}

		private void pMap_Paint(object sender, PaintEventArgs e)
		{
			if (tabControl1.SelectedIndex != 0)
				return;
			e.Graphics.Clear(Color.Black);
			drawMap(1, room - 1, e.Graphics);
			if (hoverBox)
			{
				int x = mousePoint.X / 16;
				x *= 16;
				int y = mousePoint.Y / 16;
				y *= 16;
				e.Graphics.DrawRectangle(Pens.White, x, y, 16, 16);
			}
		}

		private void pTileset_MouseDown(object sender, MouseEventArgs e)
		{
			int i = 0;
			int x = e.X / 16;
			int y = e.Y / 16;
			i = x + (y * 16);
			if (i >= 0xEC)
				return;
			label2.Text = "Selected: " + "0x" + i.ToString("X");
			selectedTile = i;
		}

		private void pTileset_Click(object sender, EventArgs e)
		{

		}

		private void twoByteObjectsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			twoobjects = twoByteObjectsToolStripMenuItem.Checked;
			pMap.Invalidate();
			pOMap.Invalidate();
			pIMap.Invalidate();
		}

		private void threeByteObjectsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			threeobjects = threeByteObjectsToolStripMenuItem.Checked;
			pMap.Invalidate();
			pOMap.Invalidate();
			pIMap.Invalidate();
		}

		private void doorsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			doors = doorsToolStripMenuItem.Checked;
			pMap.Invalidate();
			pIMap.Invalidate();
		}

		private void wallTemplateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			walltemplate = wallTemplateToolStripMenuItem.Checked;
			pMap.Invalidate();
			pIMap.Invalidate();
		}

		private void floorTemplateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			floortemplate = floorTemplateToolStripMenuItem.Checked;
			pMap.Invalidate();
			pOMap.Invalidate();
			pIMap.Invalidate();
		}

		private void nFloor_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.floorTileIndex = (int)nFloor.Value;
			pMap.Invalidate();
		}

		private void graphicsViewerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			try
			{
				frmGraphics.Show();
				frmGraphics.BringToFront();
			}
			catch (Exception)
			{
				frmGraphics = new frmGraphic(m);
				frmGraphics.Show();
			}
			
		}

		private void objectBordersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			objectborders = objectBordersToolStripMenuItem.Checked;
			pMap.Invalidate();
			pIMap.Invalidate();
			pOMap.Invalidate();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			mouseStyle = 0;
			button1.BackColor = SystemColors.ButtonHighlight;
			button3.BackColor = SystemColors.Control;
			button5.BackColor = SystemColors.Control;
		}

		public string getDescription(int i)
		{
			string s = "";
			if (spriteNames[i] == null)
				s = "None";
			else
				s = spriteNames[i];
			return s;
		}

		private void pMap_MouseDown(object sender, MouseEventArgs e)
		{
			if (m == null)
				return;
			if (mouseStyle == 0) //Select
			{
				if (e.Button == MouseButtons.Left)
				{
					if (cSprites.Checked)
					{
						int i = getSpriteIndex(e.X, e.Y);
						if (i == -1)
						{
							nSprite.Value = -1;
							nSpriteID.Value = 0;
							lSpriteName.Text = "Description: None";
							selectedSprite = -1;
							drawSprites(pMap.CreateGraphics());
							return;
						}
						Sprite s = m.sprites[i];
						nSprite.Value = (decimal)i;
						nSpriteID.Value = (decimal)s.id;
						lSpriteName.Text = "Description: " + getDescription(s.id);
						selectedSprite = i;
						button3.Enabled = true;
						drawSprites(pMap.CreateGraphics());
					}
					else
					{
						int i = getTileIndex(e.X, e.Y);
						if (i == -1)
						{
							nTile.Value = -1;
							cDirection.Enabled = false;
							nLength.Enabled = false;
							return;
						}
						Tile t = m.tiles[i];
						nTile.Value = (decimal)i;
						nID.Value = (decimal)t.id;
						if (!t.is3Byte)
						{
							cDirection.Enabled = false;
							nLength.Enabled = false;
						}
						else
						{
							cDirection.Enabled = true;
							nLength.Enabled = true;
							cDirection.Text = t.direction.ToString();
							nLength.Value = (decimal)t.length;
						}
						button3.Enabled = true;
					}
				}
				else if (e.Button == MouseButtons.Right)
				{
					if (cSprites.Checked)
					{
						int si = getSpriteIndex(e.X, e.Y);
						if (si == -1)
							return;
						m.sprites.RemoveAt(si);
						pMap.Invalidate();
						return;
					}
					int i = getTileIndex(e.X, e.Y);
					if (i == -1)
						return;
					if (m.tiles[i].is3Byte)
						fakeSpace += 3;
					else
						fakeSpace += 2;
					m.tiles.RemoveAt(i);
					setFakeSpace();
					pMap.Invalidate();
				}
				else if (e.Button == MouseButtons.Middle)
				{
					if (cSprites.Checked)
						return;
					int i = getTileIndex(e.X, e.Y);
					if (i == -1)
						return;
					if (!cDoor.Checked)
						m.tiles[i].id = selectedTile;
					else
						m.tiles[i].id = (int)nDoor.Value;
					nID.Value = (decimal)m.tiles[i].id;
					pMap.Invalidate();
				}
			}
			else if (mouseStyle == 2) //Move
			{
				if (e.Button == MouseButtons.Left)
				{
					if (cSprites.Checked)
					{
						int si = (int)nSprite.Value;
						if (si == -1)
							return;
						int sx = e.X / 16;
						int sy = e.Y / 16;
						m.sprites[si].x = sx;
						m.sprites[si].y = sy;
						pMap.Invalidate();
						return;
					}
					int i = (int)nTile.Value;
					if (i == -1)
						return;
					int x = e.X / 16;
					int y = e.Y / 16;
					m.tiles[i].x = x;
					m.tiles[i].y = y;
					pMap.Invalidate();
				}
				else if (e.Button == MouseButtons.Middle)
				{
					if (cSprites.Checked)
						return;
					int i = getTileIndex(e.X, e.Y);
					if (i == -1)
						return;
					m.tiles[i].id = selectedTile;
					nID.Value = (decimal)m.tiles[i].id;
					pMap.Invalidate();
				}
			}
			else if (mouseStyle == 3) //Set
			{
				if (cSprites.Checked)
					return;
				int i = getTileIndex(e.X, e.Y);
				if (i == -1)
					return;
				if (cDoor.Checked)
					m.tiles[i].id = (int)nDoor.Value;
				else
					m.tiles[i].id = selectedTile;
				nID.Value = (decimal)m.tiles[i].id;
				pMap.Invalidate();
			}
		}

		public void selectTile(int i)
		{
			if(i == -1)
			{
				nTile.Value = -1;
				cDirection.Enabled = false;
				nLength.Enabled = false;
				return;
			}
			Tile t = m.tiles[i];
			nTile.Maximum = m.tiles.Count - 1;
			nTile.Value = (decimal)i;
			nID.Value = (decimal)t.id;
			if (!t.is3Byte)
			{
				cDirection.Enabled = false;
				nLength.Enabled = false;
			}
			else
			{
				cDirection.Enabled = true;
				nLength.Enabled = true;
				cDirection.Text = t.direction.ToString();
				nLength.Value = (decimal)t.length;
			}
			button3.Enabled = true;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			mouseStyle = 2;
			button1.BackColor = SystemColors.Control;
			button3.BackColor = SystemColors.ButtonHighlight;
			button5.BackColor = SystemColors.Control;
		}

		private void nLength_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if(nTile.Value == -1)
				return;
			int i = (int)nTile.Value;
			m.tiles[i].length = (int)nLength.Value;
			pMap.Invalidate();
		}

		private void cDirection_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (nTile.Value == -1)
				return;
			int i = (int)nTile.Value;
			Tile.Direction d;
			if(cDirection.Text == "Horizontal")
				d = Tile.Direction.Horizontal;
			else
				d = Tile.Direction.Vertical;
			m.tiles[i].direction = d;
			pMap.Invalidate();
		}

		private void nID_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (nTile.Value == -1)
				return;
			int i = (int)nTile.Value;
			m.tiles[i].id = (int)nID.Value;
			pMap.Invalidate();
		}

		private void pMap_Click(object sender, EventArgs e)
		{
			
		}

		private void nID_KeyDown(object sender, KeyEventArgs e)
		{
			
		}

		private void repointRoomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			try
			{
				frmRepoint.nAddress.Value = (decimal)m.dataLocation;
				frmRepoint.Show();
				frmRepoint.BringToFront();
				frmRepoint.level = cboLevel.SelectedIndex + 1;
				frmRepoint.room = room;
			}
			catch (Exception)
			{
				frmRepoint = new frmRepoint(m);
				frmRepoint.nAddress.Value = (decimal)m.dataLocation;
				frmRepoint.Show();
				frmRepoint.level = cboLevel.SelectedIndex + 1;
				frmRepoint.room = room;
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.saveRoomData();
			m.saveOverworld();
			m.saveIRoomData();
			setWarps();
			setIWarps();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.add2ByteObject();
			selectTile(m.tiles.Count - 1);
			nTile.Maximum = m.tiles.Count - 1;
			pMap.Invalidate();
			fakeSpace -= 2;
			setFakeSpace();
		}

		private void btnCopy_Click(object sender, EventArgs e)
		{
			button1.BackColor = SystemColors.Control;
		}

		private void button5_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (nTile.Value == -1)
				return;
			if (!cDoor.Checked)
			{
				m.tiles[(int)nTile.Value].id = selectedTile;
				nID.Value = (decimal)selectedTile;
			}
			else
			{
				m.tiles[(int)nTile.Value].id = (int)nDoor.Value;
				nID.Value = nDoor.Value;
			}
			pMap.Invalidate();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.add3ByteObject();
			selectTile(m.tiles.Count - 1);
			nTile.Maximum = m.tiles.Count - 1;
			pMap.Invalidate();
			fakeSpace -= 3;
			setFakeSpace();
		}

		private void groupBox6_Enter(object sender, EventArgs e)
		{

		}

		private void button6_Click(object sender, EventArgs e)
		{
			if (nTile.Value < 1)
				return;
			int i = (int)nTile.Value;
			Tile t = m.tiles[i];
			m.tiles.RemoveAt(i);
			m.tiles.Insert(0, t);
			pMap.Invalidate();
			selectTile(0);
		}

		private void button7_Click(object sender, EventArgs e)
		{
			if (nTile.Value == -1)
				return;
			int i = (int)nTile.Value;
			Tile t = m.tiles[i];
			m.tiles.RemoveAt(i);
			m.tiles.Add(t);
			pMap.Invalidate();
			selectTile(m.tiles.Count - 1);
		}

		private void nWall_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.borderTileIndex = (int)nWall.Value;
			pMap.Invalidate();
		}

		private void button8_Click(object sender, EventArgs e)
		{
			if (m == null || nTile.Value == -1)
				return;
			Tile t = new Tile();
			Tile p = m.tiles[(int)nTile.Value];
			t.x = p.x;
			t.y = p.y;
			t.direction = p.direction;
			t.id = p.id;
			t.is3Byte = p.is3Byte;
			t.length = p.length;
			m.tiles.Add(t);
			selectTile(m.tiles.Count - 1);
			nTile.Maximum = m.tiles.Count - 1;
			pMap.Invalidate();
			if (t.is3Byte)
				fakeSpace -= 3;
			else
				fakeSpace -= 2;
			setFakeSpace();
		}

		private void cMapType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (m.warps.Count == 0)
				return;
			Warp.MapType t = Warp.MapType.Dungeon;
			if(cMapType.SelectedIndex == 0)
				t = Warp.MapType.Overworld;
			else if(cMapType.SelectedIndex == 2)
				t = Warp.MapType.Side;
			m.warps[0].type = t;
		}

		private void cDungeon_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (m.warps.Count == 0)
				return;
			m.warps[0].map = (int)nDRegion.Value;
		}

		private void nRoom_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (m.warps.Count == 0)
				return;
			m.warps[0].room = (int)nRoom.Value;
		}

		private void nDestX_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (m.warps.Count == 0)
				return;
			m.warps[0].x = (int)nDestX.Value;
		}

		private void nDestY_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (m.warps.Count == 0)
				return;
			m.warps[0].y = (int)nDestY.Value;
		}

		private void cHasWarp_CheckedChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.hasWarp = cHasWarp.Checked;
			if (m.hasWarp)
				fakeSpace -= 5;
			else
				fakeSpace += 5;
			if (m.warps.Count == 0)
			{
				Warp w = new Warp();
				w.type = Warp.MapType.Dungeon;
				m.warps.Add(w);
			}
			setWarps();
			setFakeSpace();
		}

		private void chkSide_CheckedChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			setTileset(cboLevel.SelectedIndex + 1);
			pMap.Invalidate();
		}

		private void cSOG_CheckedChanged(object sender, EventArgs e)
		{
			pMap.Invalidate();
		}

		private void minimapEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			try
			{
				if (tabControl1.SelectedIndex == 0)
					frmMinimap.Show();
				else if (tabControl1.SelectedIndex == 1)
					frmOMinimap.Show();
			}
			catch (Exception)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					frmMinimap = new frmMinimap(m, this);
					frmMinimap.Show();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					frmOMinimap = new frmOMinimapEditor(this, m);
					frmOMinimap.Show();
				}
			}
		}

		private void pMap_MouseMove(object sender, MouseEventArgs e)
		{
			if (hoverBox)
			{
				mousePoint = new Point(e.X, e.Y);
				pMap.Invalidate();
			}
			if (m == null)
				return;
			if (mouseStyle == 0)
			{
				if (e.Button == MouseButtons.Left)
				{
					if (!cSprites.Checked)
					{
						int i = (int)nTile.Value;
						if (i == -1)
							return;
						if (m.tiles[i].x != (e.X / 16) || m.tiles[i].y != (e.Y / 16))
						{
							m.tiles[i].x = (e.X / 16);
							m.tiles[i].y = (e.Y / 16);
							pMap.Invalidate();
						}
					}
					else
					{
						int i = (int)nSprite.Value;
						if (i == -1)
							return;
						if (m.sprites[i].x != (e.X / 16) || m.sprites[i].y != (e.Y / 16))
						{
							m.sprites[i].x = (e.X / 16);
							m.sprites[i].y = (e.Y / 16);
							pMap.Invalidate();
						}
					}
				}
			}
		}

		private void toolStripMenuItem4_Click(object sender, EventArgs e)
		{
			hoverBox = !hoverBox;
			pMap.Invalidate();
		}

		private void cSprites_CheckedChanged(object sender, EventArgs e)
		{
			pMap.Invalidate();
		}

		private void nSpriteID_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (nSprite.Value == -1)
				return;
			m.sprites[(int)nSprite.Value].id = (int)nSpriteID.Value;
			pMap.Invalidate();
			lSpriteName.Text = "Description: " + getDescription((int)nSpriteID.Value);
		}

		private void eventEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (tabControl1.SelectedIndex == 1)
				return;
			try
			{
				frmEvent.Show();
			}
			catch (Exception)
			{
				frmEvent = new frmEvent(m, tabControl1.SelectedIndex);
				frmEvent.Show();
			}
		}

		private void button9_Click(object sender, EventArgs e)
		{
			Sprite s = new Sprite();
			m.sprites.Add(s);
			nSprite.Maximum = m.sprites.Count - 1;
			pMap.Invalidate();
		}

		private void button10_Click(object sender, EventArgs e)
		{
			if (nSprite.Value == -1)
				return;
			Sprite s = new Sprite();
			Sprite from = m.sprites[(int)nSprite.Value];
			s.id = from.id;
			s.x = from.x;
			s.y = from.y;
			m.sprites.Add(s);
			nSprite.Maximum = m.sprites.Count - 1;
			pMap.Invalidate();
		}

		private void nDoor_ValueChanged(object sender, EventArgs e)
		{
			setDoorTileset();
		}

		private void chestEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			try
			{
				frmChest.Show();
				frmChest.BringToFront();
			}
			catch (Exception)
			{
				frmChest = new frmChest(m, tabControl1.SelectedIndex);
				frmChest.Show();
			}
		}

		private void nAnimation_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.animationIndex = (int)nAnimation.Value;
			if (dungeonTilesetsFromRom)
			{
				setAnimTileset(cboLevel.SelectedIndex, (int)nAnimation.Value);
				pMap.Invalidate();
			}
		}

		private void editDungeonNameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			try
			{
				frmNameEditor.Show();
			}
			catch (Exception)
			{
				frmNameEditor = new frmNameEditor(m);
				frmNameEditor.Show();
			}
		}

		private void repointRoomToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			try
			{
				if (tabControl1.SelectedIndex == 0)
				{
					frmRepoint.nAddress.Value = (decimal)m.dataLocation;
					frmRepoint.Show();
					frmRepoint.BringToFront();
					frmRepoint.level = cboLevel.SelectedIndex + 1;
					frmRepoint.room = room;
				}
				else if(tabControl1.SelectedIndex == 1)
				{
					frmRepoint.nAddress.Value = (decimal)m.oDataLocation;
					frmRepoint.Show();
					frmRepoint.BringToFront();
					frmRepoint.level = -1;
					frmRepoint.room = oRoom;
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					frmRepoint.nAddress.Value = (decimal)m.iDataLocation;
					frmRepoint.Show();
					frmRepoint.BringToFront();
					frmRepoint.indoor = true;
					frmRepoint.level = iRegion;
					frmRepoint.room = iRoom;
				}
			}
			catch (Exception)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					frmRepoint = new frmRepoint(m);
					frmRepoint.nAddress.Value = (decimal)m.dataLocation;
					frmRepoint.Show();
					frmRepoint.level = cboLevel.SelectedIndex + 1;
					frmRepoint.room = room;
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					frmRepoint = new frmRepoint(m);
					frmRepoint.nAddress.Value = (decimal)m.oDataLocation;
					frmRepoint.Show();
					frmRepoint.BringToFront();
					frmRepoint.level = -1;
					frmRepoint.room = oRoom;
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					frmRepoint = new frmRepoint(m);
					frmRepoint.nAddress.Value = (decimal)m.iDataLocation;
					frmRepoint.Show();
					frmRepoint.BringToFront();
					frmRepoint.indoor = true;
					frmRepoint.level = iRegion;
					frmRepoint.room = iRoom;
				}
			}
		}

		private void toolStripMenuItem5_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			try
			{
				frmSprite.Show();
				frmSprite.BringToFront();
			}
			catch (Exception)
			{
				frmSprite = new frmSpriteRepoint(m, this);
				if (tabControl1.SelectedIndex == 0)
				{
					frmSprite.nAddress.Value = (decimal)m.spriteLocation;
					frmSprite.Show();
					frmSprite.level = cboLevel.SelectedIndex + 1;
					frmSprite.room = room;
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					frmSprite.nAddress.Value = (decimal)m.oSpriteLocation;
					frmSprite.Show();
					frmSprite.level = -1;
					frmSprite.room = oRoom;
				}
				else if (tabControl1.SelectedIndex == 2)
				{
					frmSprite.nAddress.Value = (decimal)m.iSpriteLocation;
					frmSprite.Show();
					frmSprite.level = iRegion;
					frmSprite.room = iRoom;
					frmSprite.indoor = true;
				}
			}
		}

		private void objectDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			mapDumper d = new mapDumper();
			d.objectData = m.getObjectData();
			d.byteCount = d.objectData.Length + 2;
			d.animIndex = (byte)m.animationIndex;
			d.borderTileIndex = (byte)m.borderTileIndex;
			d.floorTileIndex = (byte)m.floorTileIndex;
			SaveFileDialog o = new SaveFileDialog();
			o.Title = "Export Object Data";
			o.Filter = "LAM Files (*.lam)|*.lam";
			DialogResult r = o.ShowDialog();
			if (r == DialogResult.OK)
			{
				d.writeFile(o.FileName);
			}
		}

		public void setSpace()
		{
			if (m == null)
				return;
			int needed = m.getNeededSpace();
			fakeSpace = m.getFreeSpace();
			lSpace.Text = "Free/Needed: " + fakeSpace + "/" + needed;
		}

		public void setISpace()
		{
			if (m == null)
				return;
			int needed = m.getINeededSpace();
			iFakeSpace = m.getFreeISpace();
			lISpace.Text = "Free/Needed: " + iFakeSpace + "/" + needed;
		}

		public void setFakeSpace()
		{
			if (m == null)
				return;
			int needed = m.getNeededSpace();
			if (fakeSpace < 0)
				lSpace.Text = "Free/Needed: 0" + "/" + needed;
			else
				lSpace.Text = "Free/Needed: " + fakeSpace + "/" + needed;
		}

		public void setIFakeSpace()
		{
			if (m == null)
				return;
			int needed = m.getINeededSpace();
			if (iFakeSpace < 0)
				lISpace.Text = "Free/Needed: 0" + "/" + needed;
			else
				lISpace.Text = "Free/Needed: " + iFakeSpace + "/" + needed;
		}

		private void objectDataToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			mapDumper d = new mapDumper();
			OpenFileDialog o = new OpenFileDialog();
			o.Title = "Import Object Data";
			o.Filter = "LAM Files (*.lam)|*.lam";
			DialogResult r = o.ShowDialog();
			if (r == DialogResult.OK)
			{
				m.tiles = new List<Tile>();
				m.warps = new List<Warp>();
				byte[] temp = d.loadData(o.FileName);
				m.data = temp;
				m.byteCount = temp.Length;
				m.formatData();
				pMap.Invalidate();
			}
		}

		private void button5_Click_1(object sender, EventArgs e)
		{
			mouseStyle = 3;
			button5.BackColor = SystemColors.ButtonHighlight;
			button3.BackColor = SystemColors.Control;
			button1.BackColor = SystemColors.Control;
		}

		private void spriteBordersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			spriteborders = spriteBordersToolStripMenuItem.Checked;
			pMap.Invalidate();
			pOMap.Invalidate();
		}

		private void clearDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (tabControl1.SelectedIndex == 0)
			{
				fakeSpace = fakeSpace + m.getNeededSpace() - 3;
				m.tiles = new List<Tile>();
				m.warps = new List<Warp>();
				cHasWarp.Checked = false;
				m.data = new byte[160];
				setFakeSpace();
				setWarps();
				pMap.Invalidate();
			}
			else if(tabControl1.SelectedIndex  == 1)
			{
				oFakeSpace = oFakeSpace + m.getONeededSpace() - 3;
				m.oTiles = new List<Tile>();
				m.oWarps = new List<Warp>();
				m.oCollisionData = new byte[160];
				setOWarps();
				setOFakeSpace();
				pOMap.Invalidate();
			}
		}

		private void clearSpritesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				m.sprites = new List<Sprite>();
				pMap.Invalidate();
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				m.oSprites = new List<Sprite>();
				pOMap.Invalidate();
			}
		}

		private void toolStripMenuItem7_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process p = new System.Diagnostics.Process();
			p.StartInfo.FileName = Application.StartupPath + "/Map Planner.exe";
			p.Start();
		}

		private void toolStripMenuItem8_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			try
			{
				frmEventPos.Show();
				frmEventPos.BringToFront();
			}
			catch (Exception)
			{
				frmEventPos = new frmEventPos(imgTileset1, m);
				frmEventPos.Show();
				frmEventPos.BringToFront();
			}
		}

		private void u11ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m.openRom();
			u10ToolStripMenuItem.Checked = false;
			m.romVersion = 1.1;
			m.readRealMinimap(cboLevel.SelectedIndex + 1);
			m.readMinimap(cboLevel.SelectedIndex + 1);
			m.loadEventPos();
			pMinimap.Invalidate();
			m.closeRom();
		}

		private void u10ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m.openRom();
			u11ToolStripMenuItem.Checked = false;
			m.romVersion = 1.0;
			m.readRealMinimap(cboLevel.SelectedIndex + 1);
			m.readMinimap(cboLevel.SelectedIndex + 1);
			m.loadEventPos();
			pMinimap.Invalidate();
			m.closeRom();
		}

		private void nID_KeyUp(object sender, KeyEventArgs e)
		{
			if (m == null)
				return;
			if (nTile.Value == -1)
				return;
			int i = (int)nTile.Value;
			m.tiles[i].id = (int)nID.Value;
			pMap.Invalidate();
		}

		private void toolStripMenuItem10_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.openRom();
			try
			{
				frmPalette.Show();
				frmPalette.BringToFront();
			}
			catch (Exception)
			{
				frmPalette = new frmPalette(m);
				if (tabControl1.SelectedIndex == 1)
				{
					tloader.reader = m.reader;
					int address = tloader.GetPalOffset(oRoom);
					m.loadPalette(address);
					m.closeRom();
					frmPalette.nIndex.Value = tloader.lastPal1;
				}
				frmPalette.Show();
			}
		}

		public void setOWarps()
		{
			nOWarpIndex.Maximum = m.oWarps.Count - 1;
			nOWarpAfter.Maximum = m.oTiles.Count;
			nOWarpIndex.Value = m.oWarps.Count - 1;
		}

		public void loadWarp(int index)
		{
			if (index == -1)
			{
				clearOWarp();
				return;
			}
			Warp w = m.oWarps[index];
			int ti = 0;
			if(w.type == Warp.MapType.Dungeon)
				ti = 1;
			if(w.type == Warp.MapType.Side)
				ti = 2;
			cOType.SelectedIndex = ti;
			nORoom.Value = w.room;
			nRegion.Value = w.map;
			nODestX.Value = w.x;
			nODestY.Value = w.y;
			if (w.after > nOWarpAfter.Maximum)
			{
				nOWarpAfter.Maximum = m.oTiles.Count;
			}
			try
			{
				nOWarpAfter.Value = w.after;
			}
			catch (Exception)
			{
				MessageBox.Show("An error occurred while trying to load warp " + index + ".");
				nOWarpAfter.Value = 0;
			}
		}

		void clearOWarp()
		{
			nOWarpIndex.Value = -1;
			nOWarpAfter.Value = 0;
			nRegion.Value = 0;
			nORoom.Value = 0;
			nODestX.Value = 0;
			nODestY.Value = 0;
		}

		private void pOMinimap_MouseDown(object sender, MouseEventArgs e)
		{
			if (m == null)
				return;
			int x = e.X / 8;
			int y = e.Y / 8;
			int s = x + (y * 16);
			oRoom = s;
			loadOverworldMap(s);
		}

		void updateSpaceLabel()
		{
			
		}

		public void loadOverworldMap(int id)
		{
			try
			{
				m.openRom();
			}
			catch (Exception) { }
			int s = id;
			room = s;
			lblOSelected.Text = "Current: 0x" + s.ToString("X");
			m.clearOData();
			m.loadOverlayData(s);
			m.readOverworldCollision(s);
			m.formatOverworldData();
			m.readSprites(s);
			nOSprite.Maximum = m.oSprites.Count - 1;
			m.readOChest(s);
			m.readOSpriteSet(s);
			nOAnimation.Value = (decimal)m.oAnimIndex;
			nOFloor.Value = (decimal)m.oFloorTile;
			nOUnknown.Value = (decimal)m.oHeaderValue2;
			nOSpriteSet.Value = (decimal)m.oSpriteSet;
			tloader.readMap(m.reader, s);
			pOTileset.Image = tloader.blockBmp;
			setOSpace();
			m.closeRom();
			setOWarps();
			if (m.oWarps.Count > 0)
				loadWarp(m.oWarps.Count - 1);
			else
				clearOWarp();
			pOMap.Invalidate();
		}

		private void pOMinimap_Paint(object sender, PaintEventArgs e)
		{
			/*if (ddrawGrid)
			{
				for (int x = 0; x < 16; x++)
					e.Graphics.DrawLine(Pens.Yellow, x * 8, 0, x * 8, pOMinimap.Height);
				for (int y = 0; y < 16; y++)
					e.Graphics.DrawLine(Pens.Yellow, 0, y * 8, pOMinimap.Width, y * 8);
			}*/
		}

		void drawOverlay(Graphics g)
		{
			for (int y = 0; y < 10; y++)
			{
				for (int x = 0; x < 10; x++)
				{
					int i = x + (y * 10);
					if (i > 79)
						i = 79;
					Bitmap tile = getOTile(m.overlayData[i]);
					g.DrawImage(tile, x * 16, y * 16);
				}
			}
		}

		private void tabPage1_Click(object sender, EventArgs e)
		{

		}

		public void setOMinimapImage()
		{
			Bitmap b = new Bitmap(128, 128);

			for (int tY = 0; tY < 16; tY++)
			{
				for (int tX = 0; tX < 16; tX++)
				{
					int i = tX + (tY * 16);
					for (int y = 0; y < 8; y++)
					{
						for (int x = 0; x < 8; x++)
						{
							int si = x + (y * 8);
							int t = m.oMinimapTiles[i].gID;
							int p = m.oMinimapTiles[i].pID;
							if (t < 0x70)
								b.SetPixel((tX * 8) + x, (tY * 8) + y, m.oMinimapPalette[p, m.oMinimapGraphics[t, si]]);
							else
								b.SetPixel((tX * 8) + x, (tY * 8) + y, m.oMinimapPalette[p, m.oMinimapGraphics2[t & 0xF, si]]);
						}
					}
				}
			}
			pOMinimap.Image = b;
		}

		public void drawOverworld(Graphics g)
		{
			if (floortemplate)
			{
				Bitmap b = getOTile(m.oFloorTile);
				for (int x = 0; x < 10; x++)
				{
					for (int y = 0; y < 9; y++)
					{
						g.DrawImage(b, x * 16, y * 16);
					}
				}
			}
			for (int i = 0; i < m.oTiles.Count; i++)
			{
				Tile t = m.oTiles[i];
				if (!t.is3Byte && twoobjects)
				{
					if (!duoTileIDs.Contains(t.id))
					{
						Bitmap b = getOTile(t.id);
						g.DrawImage(b, t.x * 16, t.y * 16);
					}
					else
					{
						int index = getDuoIndex(t.id);
						if(index == -1)
							continue;
						DuoTile d = duoTiles[index];
						Bitmap b = new Bitmap(d.hTiles * 16, d.vTiles * 16);
						Graphics graphics = Graphics.FromImage(b);
						int x = 0;
						int y = 0;
						for (int k = 0; k < d.tileCount; k++)
						{
							Bitmap b2 = getOTile(d.tileIDs[k]);
							graphics.DrawImage(b2, x * 16, y * 16);
							x++;
							if (x == d.hTiles)
							{
								x = 0;
								y++;
							}
						}
						int tdx = t.x * 16;
						int tdy = t.y * 16;
						if (t.x == 0xF)
						{
							tdx = -16;
						}
						if (t.y == 0xF)
							tdy = -16;
						g.DrawImage(b, tdx, tdy);
					}
					continue;
				}
				if (t.is3Byte && threeobjects)
				{
					if (!duoTileIDs.Contains(t.id))
					{
						int x = t.x;
						int y = t.y;
						Bitmap b = getOTile(t.id);
						for (int k = 0; k < t.length; k++)
						{
							g.DrawImage(b, x * 16, y * 16);
							if (t.direction == Tile.Direction.Horizontal)
								x++;
							else
								y++;
						}
					}
					else
					{
						int index = getDuoIndex(t.id);
						if (index == -1)
							continue;
						DuoTile d = duoTiles[index];
						Bitmap b = new Bitmap(d.hTiles * 16, d.vTiles * 16);
						Graphics graphics = Graphics.FromImage(b);
						int x = 0;
						int y = 0;
						for (int k = 0; k < d.tileCount; k++)
						{
							Bitmap b2 = getOTile(d.tileIDs[k]);
							graphics.DrawImage(b2, x * 16, y * 16);
							x++;
							if (x == d.hTiles)
							{
								x = 0;
								y++;
							}
						}
						int xx = 0;
						int yy = 0;
						int tdx = t.x * 16;
						int tdy = t.y * 16;
						if (t.x == 0xF)
							tdx = -16;
						if (t.y == 0xF)
							tdy = -16;
						for (int l = 0; l < t.length; l++)
						{
							if (t.direction == Tile.Direction.Horizontal)
								xx = l * d.hTiles;
							else
								yy = l * d.vTiles;
							g.DrawImage(b, tdx + (16 * xx), tdy + (16 * yy));
						}
					}
				}
			}
			if (objectborders)
			{
				for (int i = 0; i < m.oTiles.Count; i++)
				{
					Tile t = m.oTiles[i];
					if (t.is3Byte)
					{
						if (t.direction == Tile.Direction.Horizontal)
						{
							if (!duoTileIDs.Contains(t.id))
								g.DrawRectangle(Pens.Red, t.x * 16, t.y * 16, t.length * 16, 16);
							else
							{
								int rx = t.x * 16;
								int ry = t.y * 16;
								if (t.x == 0xF)
									rx = -16;
								if (t.y == 0xF)
									ry = -16;
								DuoTile d = duoTiles[getDuoIndex(t.id)];
								g.DrawRectangle(Pens.Orange, rx, ry, t.length * (d.hTiles * 16), d.vTiles * 16);
							}
						}
						else
						{
							if (!duoTileIDs.Contains(t.id))
								g.DrawRectangle(Pens.Blue, t.x * 16, t.y * 16, 16, t.length * 16);
							else
							{
								int rx = t.x * 16;
								int ry = t.y * 16;
								if (t.x == 0xF)
									rx = -16;
								if (t.y == 0xF)
									ry = -16;
								DuoTile d = duoTiles[getDuoIndex(t.id)];
								g.DrawRectangle(Pens.LightBlue, rx, ry, d.hTiles * 16, t.length * (d.vTiles * 16));
							}
						}
					}
					else
					{
						if (!duoTileIDs.Contains(t.id))
							g.DrawRectangle(Pens.Green, t.x * 16, t.y * 16, 16, 16);
						else
						{
							int rx = t.x * 16;
							int ry = t.y * 16;
							if (t.x == 0xF)
								rx = -16;
							if (t.y == 0xF)
								ry = -16;
							DuoTile d = duoTiles[getDuoIndex(t.id)];
							g.DrawRectangle(Pens.Magenta, rx, ry, d.hTiles * 16, d.vTiles * 16);
						}
					}
				}
			}
		}

		private void pOMap_Paint(object sender, PaintEventArgs e)
		{
			if (m == null || tabControl1.SelectedIndex != 1)
				return;
			e.Graphics.Clear(Color.Black);
			if (rbOverlay.Checked)
				drawOverlay(e.Graphics);
			else
				drawOverworld(e.Graphics);
			if (cOSprites.Checked)
			{
				drawOSprites(e.Graphics);
			}
		}

		private void rbOverlay_CheckedChanged(object sender, EventArgs e)
		{
			nOTile.Enabled = false;
			cODirection.Enabled = false;
			nOLength.Enabled = false;
			nOID.Enabled = false;
			bOSelect.Enabled = false;
			bOMove.Enabled = false;
			bOSet.Enabled = false;
			bOBack.Enabled = false;
			bOFront.Enabled = false;
			groupBox12.Enabled = false;
			pOMap.Invalidate();
		}

		private void rbCollision_CheckedChanged(object sender, EventArgs e)
		{
			nOTile.Enabled = true;
			cODirection.Enabled = true;
			nOLength.Enabled = true;
			nOID.Enabled = true;
			bOSelect.Enabled = true;
			bOMove.Enabled = true;
			bOSet.Enabled = true;
			bOBack.Enabled = true;
			bOFront.Enabled = true;
			groupBox12.Enabled = true;
			pOMap.Invalidate();
		}

		private void pOMap_MouseDown(object sender, MouseEventArgs e)
		{
			if (m == null)
				return;
			if (cOSprites.Checked)
			{
				int i = getOSpriteIndex(e.X, e.Y);
				selectedOSprite = i;
				if (i == -1)
				{
					nOSprite.Value = -1;
					nOSpriteID.Value = 0;
					lOSpriteDescription.Text = "Description: None";
					pOMap.Invalidate();
					return;
				}
				if (e.Button == MouseButtons.Left)
				{
					Sprite s = m.oSprites[i];
					nOSprite.Maximum = m.oSprites.Count - 1;
					nOSprite.Value = i;
					nOSpriteID.Value = s.id;
					lOSpriteDescription.Text = "Description: " + getDescription(s.id);
					pOMap.Invalidate();
				}
				else if (e.Button == MouseButtons.Right)
				{
					m.oSprites.RemoveAt(i);
					selectedOSprite = -1;
					nOSprite.Value = -1;
					nOSpriteID.Value = 0;
					lOSpriteDescription.Text = "";
					pOMap.Invalidate();
				}
				return;
			}
			if (rbCollision.Checked)
			{
				if (e.Button == MouseButtons.Middle)
				{
					int i = getOTileIndex(e.X, e.Y);
					if (i == -1)
						return;
					if (!cOSpecialTile.Checked)
						m.oTiles[i].id = oSelectedTile;
					else
						m.oTiles[i].id = (int)nOSpecialTile.Value;
					pOMap.Invalidate();
					return;
				}
				if (e.Button == MouseButtons.Right)
				{
					int i = getOTileIndex(e.X, e.Y);
					if (i == -1)
						return;
					if (m.oTiles[i].is3Byte)
						oFakeSpace += 3;
					else
						oFakeSpace += 2;
					m.oTiles.RemoveAt(i);
					setOFakeSpace();
					pOMap.Invalidate();
					return;
				}
				if (oMouse == 1)
				{
					int i = getOTileIndex(e.X, e.Y);
					nOTile.Value = i;
					if (i == -1)
						return;
					selectOTile(i);
				}
				else if (oMouse == 2)
				{
					int x = e.X / 16;
					int y = e.Y / 16;
					if (nOTile.Value == -1)
						return;
					m.oTiles[(int)nOTile.Value].x = x;
					m.oTiles[(int)nOTile.Value].y = y;
					pOMap.Invalidate();
				}
				else if (oMouse == 3)
				{
					int i = getOTileIndex(e.X, e.Y);
					if (i == -1)
						return;
					if (!cOSpecialTile.Checked)
						m.oTiles[i].id = oSelectedTile;
					else
						m.oTiles[i].id = (int)nOSpecialTile.Value;
					pOMap.Invalidate();
				}
			}
			else
			{
				if (e.Button == MouseButtons.Left)
				{
					int s = (e.X / 16) + ((e.Y / 16) * 10);
					m.overlayData[s] = (byte)oSelectedTile;
					pOMap.CreateGraphics().DrawImage(getOTile(oSelectedTile), (e.X / 16) * 16, (e.Y / 16) * 16);
				}
				else if(e.Button == MouseButtons.Right)
				{
					int s = (e.X / 16) + ((e.Y / 16) * 10);
					oSelectedTile = m.overlayData[s];
				}
			}
		}

		private void pOTileset_MouseDown(object sender, MouseEventArgs e)
		{
			int x = e.X / 16;
			int y = e.Y / 16;
			int s = x + (y * 16);
			oSelectedTile = s;
			lOTSelected.Text = "Selected: 0x" + s.ToString("X");
		}

		void drawSpecialTile(Graphics g)
		{
			int index = getDuoIndex((int)nOSpecialTile.Value);
			if (index == -1)
				return;
			DuoTile d = duoTiles[index];
			Bitmap b = new Bitmap(d.hTiles * 16, d.vTiles * 16);
			Graphics graphics = Graphics.FromImage(b);
			int x = 0;
			int y = 0;
			for (int k = 0; k < d.tileCount; k++)
			{
				Bitmap b2 = getOTile(d.tileIDs[k]);
				graphics.DrawImage(b2, x * 16, y * 16);
				x++;
				if (x == d.hTiles)
				{
					x = 0;
					y++;
				}
			}
			g.DrawImage(b, 0, 0);
		}

		private void cOSpecialTile_CheckedChanged(object sender, EventArgs e)
		{
			pOSpecialTile.Invalidate();
		}

		private void nOSpecialTile_ValueChanged(object sender, EventArgs e)
		{
			pOSpecialTile.Invalidate();
		}

		private void pOSpecialTile_Paint(object sender, PaintEventArgs e)
		{
			drawSpecialTile(e.Graphics);
		}

		private void bOSelect_Click(object sender, EventArgs e)
		{
			bOSelect.BackColor = SystemColors.ButtonHighlight;
			bOMove.BackColor = SystemColors.Control;
			bOSet.BackColor = SystemColors.Control;
			oMouse = 1;
		}

		private void bOMove_Click(object sender, EventArgs e)
		{
			bOSelect.BackColor = SystemColors.Control;
			bOMove.BackColor = SystemColors.ButtonHighlight;
			bOSet.BackColor = SystemColors.Control;
			oMouse = 2;
		}

		private void bOSet_Click(object sender, EventArgs e)
		{
			bOSelect.BackColor = SystemColors.Control;
			bOMove.BackColor = SystemColors.Control;
			bOSet.BackColor = SystemColors.ButtonHighlight;
			oMouse = 3;
		}

		private void bOBack_Click(object sender, EventArgs e)
		{
			if (nOTile.Value < 1)
				return;
			int i = (int)nOTile.Value;
			Tile t = m.oTiles[i];
			m.oTiles.RemoveAt(i);
			m.oTiles.Insert(0, t);
			pOMap.Invalidate();
			nOTile.Value = 0;
		}

		private void bOFront_Click(object sender, EventArgs e)
		{
			if (nOTile.Value == -1)
				return;
			int i = (int)nOTile.Value;
			Tile t = m.oTiles[i];
			m.oTiles.RemoveAt(i);
			m.oTiles.Add(t);
			pOMap.Invalidate();
			nOTile.Value = m.oTiles.Count - 1;
		}

		private void pOMap_MouseMove(object sender, MouseEventArgs e)
		{
			if ((nOTile.Value == -1 && !cOSprites.Checked && rbCollision.Checked) || e.Button != MouseButtons.Left)
				return;
			int x = e.X / 16;
			int y = e.Y / 16;
			if (cOSprites.Checked)
			{
				if (oMouse == 1)
				{
					if (nOSprite.Value == -1)
						return;
					Sprite s = m.oSprites[(int)nOSprite.Value];
					if (x == s.x && y == s.y)
						return;
					m.oSprites[(int)nOSprite.Value].x = x;
					m.oSprites[(int)nOSprite.Value].y = y;
					pOMap.Invalidate();
				}
				return;
			}
			if (rbCollision.Checked)
			{
				if (x < 0 || x > 9)
					x = 0xF;
				if (y < 0 || y > 8)
					y = 0xF;
				if (x == m.oTiles[(int)nOTile.Value].x && y == m.oTiles[(int)nOTile.Value].y)
					return;
				m.oTiles[(int)nOTile.Value].x = x;
				m.oTiles[(int)nOTile.Value].y = y;
				pOMap.Invalidate();
			}
			else
			{
				int s = (e.X / 16) + ((e.Y / 16) * 10);
				if (s < 0 || s > 79)
					return;
				m.overlayData[s] = (byte)oSelectedTile;
				pOMap.CreateGraphics().DrawImage(getOTile(oSelectedTile), (e.X / 16) * 16, (e.Y / 16) * 16);
			}
		}

		public void selectOTile(int id)
		{
			nOTile.Value = id;
			Tile t = m.oTiles[id];
			nOID.Value = (decimal)t.id;
			if (t.is3Byte)
			{
				cODirection.Enabled = true;
				nOLength.Enabled = true;
				cODirection.SelectedIndex = (t.direction == Tile.Direction.Horizontal ? 0 : 1);
				nOLength.Value = t.length;
			}
			else
			{
				cODirection.Enabled = false;
				nOLength.Enabled = false;
			}
		}

		private void button12_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			Tile t = new Tile();
			m.oTiles.Add(t);
			selectOTile(m.oTiles.Count - 1);
			pOMap.Invalidate();
			oFakeSpace -= 2;
			setOFakeSpace();
			setWarps();
		}

		private void button13_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			Tile t = new Tile();
			t.is3Byte = true;
			t.length = 1;
			m.oTiles.Add(t);
			selectOTile(m.oTiles.Count - 1);
			pOMap.Invalidate();
			oFakeSpace -= 2;
			setOFakeSpace();
			setWarps();
		}

		private void button11_Click(object sender, EventArgs e)
		{
			if (m == null || nOTile.Value == -1)
				return;
			Tile t = new Tile();
			Tile from = m.oTiles[(int)nOTile.Value];
			t.id = from.id;
			t.is3Byte = from.is3Byte;
			t.length = from.length;
			t.direction = from.direction;
			t.x = from.x;
			t.y = from.y;
			m.oTiles.Add(t);
			selectOTile(m.oTiles.Count - 1);
			pOMap.Invalidate();
			if (t.is3Byte)
				oFakeSpace -= 3;
			else
				oFakeSpace -= 2;
			setOFakeSpace();
			setWarps();
		}

		private void nOLength_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || nOTile.Value == -1 || nOTile.Value > m.tiles.Count)
				return;
			m.oTiles[(int)nOTile.Value].length = (byte)nOLength.Value;
			pOMap.Invalidate();
		}

		private void nOID_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || nOTile.Value == -1)
				return;
			m.oTiles[(int)nOTile.Value].id = (byte)nOID.Value;
			pOMap.Invalidate();
		}

		private void cODirection_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m == null || nOTile.Value == -1)
				return;
			Tile.Direction d = Tile.Direction.Horizontal;
			Tile t = m.oTiles[(int)nOTile.Value];
			if (cODirection.SelectedIndex == 1)
				d = Tile.Direction.Vertical;
			t.direction = d;
			pOMap.Invalidate();
		}

		public void drawOSprites(Graphics g)
		{
			if (m == null)
				return;
			Pen p = new Pen(Brushes.White, 2);
			Pen p2 = new Pen(Brushes.Red, 2);
			for (int i = 0; i < m.oSprites.Count; i++)
			{
				Sprite s = m.oSprites[i];
				if (spriteImages[s.id] == null)
				{
					g.FillRectangle(Brushes.Black, s.x * 16, s.y * 16, 16, 16);
					if (selectedOSprite != i)
						g.DrawRectangle(p, s.x * 16 + 1, s.y * 16 + 1, 14, 14);
					else
						g.DrawRectangle(p2, s.x * 16 + 1, s.y * 16 + 1, 14, 14);
				}
				else
				{
					Image img = spriteImages[s.id];
					Bitmap b = new Bitmap(img);
					b.MakeTransparent(Color.Magenta);
					g.DrawImage(b, s.x * 16, s.y * 16);
					if (spriteborders)
					{
						if (selectedOSprite == i)
							g.DrawRectangle(Pens.Red, s.x * 16, s.y * 16, b.Width, b.Height);
						else
							g.DrawRectangle(Pens.White, s.x * 16, s.y * 16, b.Width, b.Height);
					}
				}
			}
		}

		private void cOSprites_CheckedChanged(object sender, EventArgs e)
		{
			pOMap.Invalidate();
		}

		private void nOSpriteID_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || nOSprite.Value == -1)
				return;
			m.oSprites[(int)nOSprite.Value].id = (byte)nOSpriteID.Value;
			pOMap.Invalidate();
		}

		private void nOAnimation_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.oAnimIndex = (byte)nOAnimation.Value;
			try
			{
				m.openRom();
				tloader.readMap(m.reader, oRoom, (int)nOAnimation.Value);
				pOTileset.Image = tloader.blockBmp;
				m.closeRom();
				pOMap.Invalidate();
			}
			catch (Exception) { }
		}

		private void nOFloor_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.oFloorTile = (byte)nOFloor.Value;
			pOMap.Invalidate();
		}

		private void nOWarpIndex_ValueChanged(object sender, EventArgs e)
		{
			loadWarp((int)nOWarpIndex.Value);
		}

		private void cOType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m.oWarps.Count == 0 || nOWarpIndex.Value == -1)
				return;
			Warp.MapType d = Warp.MapType.Overworld;
			if (cOType.SelectedIndex == 1)
				d = Warp.MapType.Dungeon;
			else if (cOType.SelectedIndex == 2)
				d = Warp.MapType.Side;
			m.oWarps[(int)nOWarpIndex.Value].type = d;
		}

		private void button16_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			Warp w = new Warp();
			m.oWarps.Add(w);
			m.oWarpIndexes.Add(0);
			setOWarps();
			loadWarp(m.oWarps.Count - 1);
		}

		private void nOWarpAfter_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || nOWarpIndex.Value == -1)
				return;
			m.oWarpIndexes[(int)nOWarpIndex.Value] = (int)nOWarpAfter.Value;
		}

		void setOSpace()
		{
			if (m == null)
				return;
			int needed = m.getONeededSpace();
			oFakeSpace = m.getFreeOSpace();
			lOSpace.Text = "Free/Needed: " + oFakeSpace + "/" + needed;
		}

		void setOFakeSpace()
		{
			if (m == null)
				return;
			int needed = m.getONeededSpace();
			if (oFakeSpace < 0)
				lOSpace.Text = "Free/Needed: 0" + "/" + needed;
			else
				lOSpace.Text = "Free/Needed: " + oFakeSpace + "/" + needed;
		}

		private void button18_Click(object sender, EventArgs e)
		{
			if (m == null || nOWarpIndex.Value == -1)
				return;
			m.oWarps.RemoveAt((int)nOWarpIndex.Value);
			m.oWarpIndexes.RemoveAt((int)nOWarpIndex.Value);
			setOWarps();
			if (m.oWarps.Count > 0)
				loadWarp(m.oWarps.Count - 1);
			else
				clearOWarp();
		}

		private void nRegion_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || nOWarpIndex.Value == -1)
				return;
			m.oWarps[(int)nOWarpIndex.Value].map = (byte)nRegion.Value;
		}

		private void nORoom_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || nOWarpIndex.Value == -1)
				return;
			m.oWarps[(int)nOWarpIndex.Value].room = (int)nORoom.Value;
		}

		private void nODestX_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || nOWarpIndex.Value == -1)
				return;
			m.oWarps[(int)nOWarpIndex.Value].x = (int)nODestX.Value;
		}

		private void nODestY_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || nOWarpIndex.Value == -1)
				return;
			m.oWarps[(int)nOWarpIndex.Value].y = (int)nODestY.Value;
		}

		private void button15_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			Sprite s = new Sprite();
			m.oSprites.Add(s);
			nOSprite.Maximum = m.oSprites.Count - 1;
			pOMap.Invalidate();
		}

		private void button19_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			loadIndoorRoom((int)nIRegion.Value, (int)nIRoom.Value);
		}

		public void loadIndoorRoom(int region, int room)
		{
			if (m == null)
				return;
			try
			{
				m.openRom();
			}
			catch (Exception) { }
			iloader = new IndoorTilesetLoader();
			iRegion = region;
			iRoom = room;
			nIRoom.Value = room;
			nIRegion.Value = region;
			iloader.loadMap(m.reader, region, room, false, chkISide.Checked);
			pITileset.Image = iloader.blockBmp;
			m.clearIData();
			m.readIData(room, region);
			m.formatIndoorData();
			setISpace();
			m.readISprites(iRegion, iRoom + (iRoom > 5 ? 0x100 : 0));
			m.readIChest(iRoom, iRegion);
			m.readIEvent(iRegion, iRoom);
			try
			{
				nIAnimation.Value = (decimal)m.iAnimIndex;
				nIWall.Value = (decimal)m.iWallTemplate;
				nIFloor.Value = (decimal)m.iFloorTile;
			}
			catch (Exception)
			{
				MessageBox.Show("Invalid indoor header!", "Invalid Header");
				nIAnimation.Value = 0;
				nIWall.Value = 0;
				nIFloor.Value = 0;
				m.iAnimIndex = 0;
				m.iFloorTile = 0;
				m.iWallTemplate = 0;
			}
			setIWarps();
			m.closeRom();
			pIMap.Invalidate();
		}

		public void setIWarps()
		{
			if (m.iHasWarp)
			{
				chkIHasWarp.Checked = true;
				Warp w = m.iWarps[0];
				nIWarpRegion.Value = w.map;
				nIWarpRoom.Value = w.room;
				nIDestX.Value = w.x;
				nIDestY.Value = w.y;
				int i = 0;
				if (w.type == Warp.MapType.Dungeon)
					i = 1;
				else if (w.type == Warp.MapType.Side)
					i = 2;
				cIType.SelectedIndex = i;
			}
			else
			{
				chkIHasWarp.Checked = false;
			}
		}

		private void nIRegion_KeyUp(object sender, KeyEventArgs e)
		{
			
		}

		private void chkISide_CheckedChanged(object sender, EventArgs e)
		{
			loadIndoorRoom((int)nIRegion.Value, (int)nIRoom.Value);
		}

		/*END
		 * OF
		 * OVERWORLD
		 */
		//INDOOR
		public void drawIMap(Graphics g)
		{
			if (m == null)
				return;
			g.Clear(Color.Black);
			if (floortemplate)
			{
				Bitmap b = getITile(m.iFloorTile);
				for (int x = 0; x < 10; x++)
				{
					for (int y = 0; y < 9; y++)
					{
						g.DrawImage(b, x * 16, y * 16);
					}
				}
			}
			if (walltemplate)
			{
				try
				{
					for (int i = 0; i < 64; i++)
					{
						TwoByteTile t = m.borderTiles[m.iWallTemplate, i];
						if (t == null)
							continue;
						Bitmap b = getITile(t.id);
						g.DrawImage(b, t.x * 16, t.y * 16);
					}
				}
				catch (Exception) { }
			}
			for (int i = 0; i < m.iTiles.Count; i++)
			{
				Tile t = m.iTiles[i];
				if (!t.is3Byte && twoobjects)
				{
					if (doorTileIDs.Contains(t.id))
					{
						if (doors)
						{
							Bitmap b2 = getIDoorTile(getDoorIndex(t.id));
							g.DrawImage(b2, t.x * 16, t.y * 16);
						}
						continue;
					}
					Bitmap b = getITile(t.id);
					g.DrawImage(b, t.x * 16, t.y * 16);
					continue;
				}
				if (t.is3Byte && threeobjects)
				{
					int x = t.x;
					int y = t.y;
					Bitmap b = getITile(t.id);
					for (int k = 0; k < t.length; k++)
					{
						g.DrawImage(b, x * 16, y * 16);
						if (t.direction == Tile.Direction.Horizontal)
							x++;
						else
							y++;
					}
				}
			}
			if (objectborders)
			{
				for (int i = 0; i < m.iTiles.Count; i++)
				{
					Tile t = m.iTiles[i];
					if (t.is3Byte)
					{
						if (t.direction == Tile.Direction.Horizontal)
						{
							g.DrawRectangle(Pens.Red, t.x * 16, t.y * 16, t.length * 16, 16);
						}
						else
						{
							g.DrawRectangle(Pens.Blue, t.x * 16, t.y * 16, 16, t.length * 16);
						}
					}
					else
					{
						if(!doorTileIDs.Contains(m.iTiles[i].id))
							g.DrawRectangle(Pens.Green, t.x * 16, t.y * 16, 16, 16);
						else
						{
							DuoTile d = doorTiles[getDoorIndex(m.iTiles[i].id)];
							g.DrawRectangle(Pens.Magenta, t.x * 16, t.y * 16, d.hTiles * 16, d.vTiles * 16);
						}
					}
				}
			}

			if (chkISprites.Checked)
				drawISprites(g);
		}

		private void pIMap_Paint(object sender, PaintEventArgs e)
		{
			if (m == null || tabControl1.SelectedIndex != 2)
				return;
			drawIMap(e.Graphics);
		}

		private void pIMap_MouseDown(object sender, MouseEventArgs e)
		{
			if (m == null)
				return;
			int i = getITileIndex(e.X, e.Y);

			if (chkISprites.Checked)
			{
				if (iMouse == 1)
				{
					int si = getISpriteIndex(e.X, e.Y);
					if (e.Button == MouseButtons.Left)
					{
						selectedISprite = si;
						nISprite.Value = si;
						if (si == -1)
						{
							nISpriteID.Value = 0;
							lISpriteDescription.Text = "Description: None";
							pIMap.Invalidate();
							bIMove.Enabled = false;
							return;
						}
						Sprite s = m.iSprites[si];
						nISpriteID.Value = (decimal)s.id;
						lISpriteDescription.Text = "Description: " + getDescription(s.id);
						bIMove.Enabled = true;
						pIMap.Invalidate();
					}
					else if (e.Button == MouseButtons.Right)
					{
						if (si == -1)
							return;
						m.iSprites.RemoveAt(si);
						nISprite.Value = -1;
						nISpriteID.Value = 0;
						lISpriteDescription.Text = "Description: None";
						selectedISprite = -1;
						pIMap.Invalidate();
					}
				}
				else if (iMouse == 2)
				{
					if (nISprite.Value == -1)
						return;
					m.iSprites[(int)nISprite.Value].x = (e.X / 16);
					m.iSprites[(int)nISprite.Value].y = (e.Y / 16);
					pIMap.Invalidate();
				}
				return;
			}
			if (e.Button == MouseButtons.Right)
			{
				if (i == -1)
					return;
				if (m.iTiles[i].is3Byte)
					iFakeSpace += 3;
				else
					iFakeSpace += 2;
				m.iTiles.RemoveAt(i);
				nITile.Value = -1;
				nIID.Value = 0;
				cIDirection.Enabled = false;
				nILength.Enabled = false;
				nILength.Value = 0;
				bIMove.Enabled = false;
				pIMap.Invalidate();
				setIFakeSpace();
				return;
			}
			if (e.Button == MouseButtons.Middle)
			{
				if (i == -1)
					return;
				if (!cIUseDoor.Checked)
					m.iTiles[i].id = iSelectedTile;
				else
					m.iTiles[i].id = (int)nIDoor.Value;
				pIMap.Invalidate();
				return;
			}

			if (iMouse == 1)
			{
				if (e.Button == MouseButtons.Left)
				{
					nITile.Value = i;
					if (i == -1)
					{
						nIID.Value = 0;
						cIDirection.Enabled = false;
						nILength.Enabled = false;
						nILength.Value = 0;
						bIMove.Enabled = false;
						return;
					}
					bIMove.Enabled = true;
					Tile t = m.iTiles[i];
					nIID.Value = t.id;
					if (t.is3Byte)
					{
						cIDirection.Enabled = true;
						nILength.Enabled = true;
						if (t.direction == Tile.Direction.Horizontal)
							cIDirection.SelectedIndex = 0;
						else
							cIDirection.SelectedIndex = 1;
						nILength.Value = t.length;
					}
					else
					{
						cIDirection.Enabled = false;
						nILength.Enabled = false;
					}
				}
				return;
			}
			if (iMouse == 2)
			{
				if (nITile.Value == -1)
					return;
				m.iTiles[(int)nITile.Value].x = e.X / 16;
				m.iTiles[(int)nITile.Value].y = e.Y / 16;
				pIMap.Invalidate();
				return;
			}
			if (iMouse == 3)
			{
				if (i == -1)
					return;
				if (!cIUseDoor.Checked)
					m.iTiles[i].id = iSelectedTile;
				else
					m.iTiles[i].id = (int)nIDoor.Value;
				pIMap.Invalidate();
				return;
			}
		}

		Bitmap getIDoorTile(int tileIndex)
		{
			DuoTile d = doorTiles[tileIndex];
			Bitmap b = new Bitmap(d.hTiles * 16, d.vTiles * 16);
			Graphics graphics = Graphics.FromImage(b);
			int x = 0;
			int y = 0;
			for (int k = 0; k < d.tileCount; k++)
			{
				Bitmap b2 = getITile(d.tileIDs[k]);
				graphics.DrawImage(b2, x * 16, y * 16);
				x++;
				if (x == d.hTiles)
				{
					x = 0;
					y++;
				}
			}
			return b;
		}

		Bitmap getDDoorTile(int tileIndex)
		{
			DuoTile d = doorTiles[tileIndex];
			Bitmap b = new Bitmap(d.hTiles * 16, d.vTiles * 16);
			Graphics graphics = Graphics.FromImage(b);
			int x = 0;
			int y = 0;
			for (int k = 0; k < d.tileCount; k++)
			{
				Bitmap b2 = getTile(d.tileIDs[k]);
				graphics.DrawImage(b2, x * 16, y * 16);
				x++;
				if (x == d.hTiles)
				{
					x = 0;
					y++;
				}
			}
			return b;
		}

		private void bIMove_Click(object sender, EventArgs e)
		{
			iMouse = 2;
			bISelectDrag.BackColor = SystemColors.Control;
			bIMove.BackColor = SystemColors.ButtonHighlight;
			bISet.BackColor = SystemColors.Control;
		}

		private void bISelectDrag_Click(object sender, EventArgs e)
		{
			iMouse = 1;
			bISelectDrag.BackColor = SystemColors.ButtonHighlight;
			bIMove.BackColor = SystemColors.Control;
			bISet.BackColor = SystemColors.Control;
		}

		private void bISet_Click(object sender, EventArgs e)
		{
			iMouse = 3;
			bISelectDrag.BackColor = SystemColors.Control;
			bIMove.BackColor = SystemColors.Control;
			bISet.BackColor = SystemColors.ButtonHighlight;
		}

		private void pIMap_MouseMove(object sender, MouseEventArgs e)
		{
			if (m == null || e.Button != MouseButtons.Left)
				return;

			int x = e.X / 16;
			int y = e.Y / 16;
			if (chkISprites.Checked)
			{
				if (nISprite.Value == -1 || iMouse != 1)
					return;
				if (x == m.iSprites[(int)nISprite.Value].x && y == m.iSprites[(int)nISprite.Value].y)
					return;
				m.iSprites[(int)nISprite.Value].x = x;
				m.iSprites[(int)nISprite.Value].y = y;
				pIMap.Invalidate();
				return;
			}
			if (nITile.Value == -1 || iMouse != 1)
				return;
			if (x == m.iTiles[(int)nITile.Value].x && y == m.iTiles[(int)nITile.Value].y)
				return;
			m.iTiles[(int)nITile.Value].x = x;
			m.iTiles[(int)nITile.Value].y = y;
			pIMap.Invalidate();
		}

		private void pITileset_MouseDown(object sender, MouseEventArgs e)
		{
			int x = e.X / 16;
			int y = e.Y / 16;
			iSelectedTile = x + (y * 16);
			lISelectedTile.Text = "Selected: 0x" + iSelectedTile.ToString("X");
		}

		private void button22_Click(object sender, EventArgs e)
		{
			if (nITile.Value < 1)
				return;
			int i = (int)nITile.Value;
			Tile t = m.iTiles[i];
			m.iTiles.RemoveAt(i);
			m.iTiles.Insert(0, t);
			pIMap.Invalidate();
			nITile.Value = 0;
		}

		private void button21_Click(object sender, EventArgs e)
		{
			if (nITile.Value == -1)
				return;
			int i = (int)nITile.Value;
			Tile t = m.iTiles[i];
			m.iTiles.RemoveAt(i);
			m.iTiles.Add(t);
			pIMap.Invalidate();
			nITile.Value = m.iTiles.Count - 1;
		}

		private void chkIHasWarp_CheckedChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.iHasWarp = chkIHasWarp.Checked;
			if (m.iHasWarp && m.iWarps.Count == 0)
			{
				Warp w = new Warp();
				m.iWarps.Add(w);
			}
		}

		private void cIType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m == null || !m.hasWarp)
				return;
			Warp.MapType mt = Warp.MapType.Overworld;
			if (cIType.SelectedIndex == 1)
				mt = Warp.MapType.Dungeon;
			else if (cIType.SelectedIndex == 2)
				mt = Warp.MapType.Side;
			m.warps[0].type = mt;
		}

		private void nIWarpRegion_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || !m.hasWarp)
				return;
			m.iWarps[0].map = (int)nIWarpRegion.Value;
		}

		private void nIWarpRoom_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || !m.hasWarp)
				return;
			m.iWarps[0].room = (int)nIWarpRoom.Value;
		}

		private void nIDestX_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || !m.hasWarp)
				return;
			m.iWarps[0].x = (int)nIDestX.Value;
		}

		private void nIDestY_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || !m.hasWarp)
				return;
			m.iWarps[0].y = (int)nIDestY.Value;
		}

		public void selectITile(int index)
		{
			bIMove.Enabled = true;
			Tile t = m.iTiles[index];
			nITile.Value = index;
			nIID.Value = t.id;
			if (t.is3Byte)
			{
				cIDirection.Enabled = true;
				nILength.Enabled = true;
				if (t.direction == Tile.Direction.Horizontal)
					cIDirection.SelectedIndex = 0;
				else
					cIDirection.SelectedIndex = 1;
				nILength.Value = t.length;
			}
			else
			{
				cIDirection.Enabled = false;
				nILength.Enabled = false;
			}
		}

		private void button23_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			Tile t = new Tile();
			m.iTiles.Add(t);
			selectITile(m.iTiles.Count - 1);
			pIMap.Invalidate();
			iFakeSpace -= 2;
			setIFakeSpace();
		}

		private void button24_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			Tile t = new Tile();
			t.is3Byte = true;
			t.length = 1;
			m.iTiles.Add(t);
			nITile.Maximum = m.iTiles.Count - 1;
			selectITile(m.iTiles.Count - 1);
			pIMap.Invalidate();
			iFakeSpace -= 3;
			setIFakeSpace();
		}

		private void button20_Click(object sender, EventArgs e)
		{
			if (m == null || nITile.Value == -1)
				return;
			Tile t = new Tile();
			Tile from = m.iTiles[(int)nITile.Value];
			t.id = from.id;
			t.is3Byte = from.is3Byte;
			t.length = from.length;
			t.x = from.x;
			t.y = from.y;
			t.direction = from.direction;
			m.iTiles.Add(t);
			selectITile(m.iTiles.Count - 1);
			pIMap.Invalidate();
			if (from.is3Byte)
				iFakeSpace -= 3;
			else
				iFakeSpace -= 2;
			setIFakeSpace();
		}

		private void nIAnimation_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (nIAnimation.Value == m.iAnimIndex)
				return;
			m.openRom();
			iloader.loadMap(m.reader, iRegion, iRoom, false, chkISide.Checked, (int)nIAnimation.Value);
			pITileset.Image = iloader.blockBmp;
			m.iAnimIndex = (byte)nIAnimation.Value;
			pIMap.Invalidate();
			m.closeRom();
		}

		private void nIWall_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.iWallTemplate = (byte)nIWall.Value;
			pIMap.Invalidate();
		}

		private void nIFloor_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.iFloorTile = (byte)nIFloor.Value;
			pIMap.Invalidate();
		}

		private void chkISprites_CheckedChanged(object sender, EventArgs e)
		{
			if (chkISprites.Checked)
			{
				bISet.Enabled = false;
			}
			else
			{
				bISet.Enabled = true;
			}
			pIMap.Invalidate();
		}

		public void drawISprites(Graphics g)
		{
			if (m == null)
				return;
			Pen p = new Pen(Brushes.White, 2);
			Pen p2 = new Pen(Brushes.Red, 2);
			for (int i = 0; i < m.iSprites.Count; i++)
			{
				Sprite s = m.iSprites[i];
				if (spriteImages[s.id] == null)
				{
					g.FillRectangle(Brushes.Black, s.x * 16, s.y * 16, 16, 16);
					if (selectedISprite != i)
						g.DrawRectangle(p, s.x * 16 + 1, s.y * 16 + 1, 14, 14);
					else
						g.DrawRectangle(p2, s.x * 16 + 1, s.y * 16 + 1, 14, 14);
				}
				else
				{
					Image img = spriteImages[s.id];
					Bitmap b = new Bitmap(img);
					b.MakeTransparent(Color.Magenta);
					g.DrawImage(b, s.x * 16, s.y * 16);
					if (spriteborders)
					{
						if (selectedISprite == i)
							g.DrawRectangle(Pens.Red, s.x * 16, s.y * 16, b.Width, b.Height);
						else
							g.DrawRectangle(Pens.White, s.x * 16, s.y * 16, b.Width, b.Height);
					}
				}
			}
		}

		private void nISpriteID_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || nISprite.Value == -1)
				return;
			m.iSprites[(int)nISprite.Value].id = (int)nISpriteID.Value;
			pIMap.Invalidate();
		}

		private void button26_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			Sprite s = new Sprite();
			m.iSprites.Add(s);
			nISprite.Maximum = m.iSprites.Count - 1;
			nISprite.Value = m.iSprites.Count - 1;
			selectedISprite = (int)nISprite.Value;
			nISpriteID.Value = 0;
			lISpriteDescription.Text = "Description: None";
			pIMap.Invalidate();
		}

		private void nILength_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || nITile.Value == -1)
				return;
			m.iTiles[(int)nITile.Value].length = (int)nILength.Value;
			pIMap.Invalidate();
		}

		private void cIDirection_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (m == null || nITile.Value == -1)
				return;
			Tile.Direction d = Tile.Direction.Horizontal;
			if (cIDirection.SelectedIndex == 1)
				d = Tile.Direction.Vertical;
			m.iTiles[(int)nITile.Value].direction = d;
			pIMap.Invalidate();
		}

		private void button25_Click(object sender, EventArgs e)
		{
			if (m == null || nISprite.Value == -1)
				return;
			Sprite s = new Sprite();
			Sprite from = m.iSprites[(int)nISprite.Value];
			s.id = from.id;
			s.x = from.x;
			s.y = from.y;
			m.iSprites.Add(s);
			nISprite.Maximum = m.iSprites.Count - 1;
			nISprite.Value = m.iSprites.Count - 1;
			selectedISprite = (int)nISprite.Value;
			pIMap.Invalidate();
		}

		private void nISprite_ValueChanged(object sender, EventArgs e)
		{
			selectedISprite = (int)nISprite.Value;
			pIMap.Invalidate();
		}

		private void nIDoor_ValueChanged(object sender, EventArgs e)
		{
			if (m == null || !cIUseDoor.Checked)
				return;
			if(doorTileIDs.Contains((int)nIDoor.Value))
				pIDoor.Image = getIDoorTile(getDoorIndex((int)nIDoor.Value));
			else
				pIDoor.CreateGraphics().Clear(pIDoor.BackColor);
		}

		private void toolStripMenuItem11_Click(object sender, EventArgs e)
		{
			if (tabControl1.SelectedIndex != 1)
				return;
			frmOTileset f = new frmOTileset(this, m, oRoom);
			f.Text = "Overworld Tileset Editor - Room " + oRoom.ToString("X");
			int i = tloader.lastPrimary;
			int pa = tloader.palAddress;
			f.nAddress.Value = pa;
			f.nPalette.Value = tloader.lastPal1;
			f.nPrimary.Value = i;
			f.Show();
		}


		public void dumpOK()
		{
			m = new MapLoader(openRom.FileName);
			m.openRom();
			m.clearData();
			m.readHighest6Index();
			m.getData(1, 1);
			m.readMinimap(1);
			m.readRealMinimap(1);
			pMinimap.Invalidate();
			setTileset(1);
			m.readSprites(1, 1);
			if (m.sprites.Count > 0)
				nSprite.Maximum = m.sprites.Count - 1;
			else
				nSprite.Maximum = 0;
			m.readEvent(1, 1);
			m.readChest(1, 1);
			m.readDungeonNames();
			m.readBorderTiles();
			room = 1;
			m.formatData();
			nTile.Maximum = m.tiles.Count;
			mouseStyle = 0;
			button1.BackColor = SystemColors.ButtonHighlight;
			button3.BackColor = SystemColors.Control;
			pMap.Invalidate();
			setHeader();
			setWarps();
			setSpace();

			//Overworld stuff
			m.readOMinimapGraphics();
			m.readOMinimapPalette();
			m.readOMinimap();
			setOMinimapImage();
			loadOverworldMap(0);
			loadDuoTiles();

			//Indoor stuff
			loadIndoorRoom(0x10, 0xA3);

			m.closeRom();
		}

		private void toolStripMenuItem12_Click(object sender, EventArgs e)
		{
			dungeonTilesetsFromRom = toolStripMenuItem12.Checked;
			setTileset(cboLevel.SelectedIndex + 1);
			pMap.Invalidate();
		}

		private void nOSpriteSet_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.oSpriteSet = (byte)nOSpriteSet.Value;
		}

		private void button27_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (nIRoom.Value - 0x10 >= 0)
				nIRoom.Value -= 0x10;
			else
				nIRoom.Value = 0;
		}

		private void button28_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (nIRoom.Value > 0)
				nIRoom.Value--;
		}

		private void button29_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (nIRoom.Value < 0xFF)
				nIRoom.Value++;
		}

		private void button30_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			if (nIRoom.Value + 0x10 <= 0xFF)
				nIRoom.Value += 0x10;
			else
				nIRoom.Value = 0xFF;
		}

		private void nDRegion_ValueChanged(object sender, EventArgs e)
		{

		}
	}
}
