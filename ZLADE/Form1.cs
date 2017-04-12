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
		public Image imgMinimap = Image.FromFile("./Images/DXMinimap.bmp");
		public Image imgTileset1 = Image.FromFile("./Images/DX 1 Tileset.bmp");
		public Image imgTileset2 = Image.FromFile("./Images/DX 2 Tileset.bmp");
		public Image imgTileset3 = Image.FromFile("./Images/DX 3 Tileset.bmp");
		public Image imgTileset4 = Image.FromFile("./Images/DX 4 Tileset.bmp");
		public Image imgTileset5 = Image.FromFile("./Images/DX 5 Tileset.bmp");
		public Image imgTileset6 = Image.FromFile("./Images/DX 6 Tileset.bmp");
		public Image imgTileset7 = Image.FromFile("./Images/DX 7 Tileset.bmp");
		public Image imgTileset8 = Image.FromFile("./Images/DX 8 Tileset.bmp");
		public Image imgSOG = Image.FromFile("./Images/DX SOG.bmp");
		public Image imgSide = Image.FromFile("./Images/DX Side Tileset.bmp");
		public Image imgSide2 = Image.FromFile("./Images/DX 2 Side Tileset.bmp");
		public Image imgSide6 = Image.FromFile("./Images/DX 6 Side Tileset.bmp");
		public Image imgSide7 = Image.FromFile("./Images/DX 7 Side Tileset.bmp");
		public Image imgSide8 = Image.FromFile("./Images/DX 8 Side Tileset.bmp");
		public Image imgSprites = Image.FromFile("./Images/DX Sprites.bmp");
		public Image imgDoors = Image.FromFile("./Images/DX 1 DTileset.bmp");
		public Image imgEntrances = Image.FromFile("./Images/DX Entrances.bmp");
		int room = 1;
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
		int selectedTile = 0;
		int selectedSprite = -1;
		Point mousePoint = new Point(0, 0);
		bool hoverBox = false;
		public string[] spriteNames = new string[256];
		public int fakeSpace = 0;

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
			cboLevel.SelectedIndex = 0;
			nID.Maximum = 0xFD;
			readSprites();
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
			r = m.realMinimap[i];
			minimap.SelectedMap = i;
			pMinimap.Invalidate();
			lCurrent.Text = "Current: 0x" + r.ToString("X");

			m.openRom();
			m.clearData();
			m.getData(cboLevel.SelectedIndex + 1, r);
			setTileset(cboLevel.SelectedIndex + 1);
			m.readSprites(cboLevel.SelectedIndex + 1, r + (cboLevel.SelectedIndex > 5 ? 0x100 : 0));
			if (m.sprites.Count > 0)
				nSprite.Maximum = m.sprites.Count - 1;
			else
				nSprite.Maximum = 0;
			m.readEvent(cboLevel.SelectedIndex + 1, r + (cboLevel.SelectedIndex > 5 ? 0x100 : 0));
			m.readChest(cboLevel.SelectedIndex + 1, r + (cboLevel.SelectedIndex > 5 ? 0x100 : 0));
			m.readDungeonNames();
			room = r;
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

		public void setTileset(int level)
		{
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
			m.closeRom();
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
					cDungeon.Text = cboLevel.GetItemText(cboLevel.Items[w.map]);
					cDungeon.Enabled = true;
				}
				else
					cDungeon.Enabled = false;
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
				cDungeon.Enabled = false;
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
						if (doors)
						{
							Bitmap b2 = getDoorTile(t.id);
							g.DrawImage(b2, t.x * 16, t.y * 16);
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
				string directory = "./Images/Sprites/" + s.id.ToString("X") + ".PNG";
				directory = Application.StartupPath + "/Images/Sprites/" + s.id.ToString("X") + ".PNG";
				if (!File.Exists(directory))
				{
					g.FillRectangle(Brushes.Black, s.x * 16, s.y * 16, 16, 16);
					if (selectedSprite != i)
						g.DrawRectangle(p, s.x * 16 + 1, s.y * 16 + 1, 14, 14);
					else
						g.DrawRectangle(p2, s.x * 16 + 1, s.y * 16 + 1, 14, 14);
				}
				else
				{
					Image img = Image.FromFile(directory);
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
		}

		private void threeByteObjectsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			threeobjects = threeByteObjectsToolStripMenuItem.Checked;
			pMap.Invalidate();
		}

		private void doorsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			doors = doorsToolStripMenuItem.Checked;
			pMap.Invalidate();
		}

		private void wallTemplateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			walltemplate = wallTemplateToolStripMenuItem.Checked;
			pMap.Invalidate();
		}

		private void floorTemplateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			floortemplate = floorTemplateToolStripMenuItem.Checked;
			pMap.Invalidate();
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
			setWarps();
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
			m.warps[0].map = cDungeon.SelectedIndex;
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
				frmMinimap.Show();
			}
			catch (Exception)
			{
				frmMinimap = new frmMinimap(m, this);
				frmMinimap.Show();
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
			try
			{
				frmEvent.Show();
			}
			catch (Exception)
			{
				frmEvent = new frmEvent(m);
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
			}
			catch (Exception)
			{
				frmChest = new frmChest(m);
				frmChest.Show();
			}
		}

		private void nAnimation_ValueChanged(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.animationIndex = (int)nAnimation.Value;
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

		private void toolStripMenuItem5_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			try
			{
				frmSprite.nAddress.Value = (decimal)m.spriteLocation;
				frmSprite.Show();
				frmSprite.BringToFront();
				frmSprite.level = cboLevel.SelectedIndex + 1;
				frmSprite.room = room;
			}
			catch (Exception)
			{
				frmSprite = new frmSpriteRepoint(m);
				frmSprite.nAddress.Value = (decimal)m.spriteLocation;
				frmSprite.Show();
				frmSprite.level = cboLevel.SelectedIndex + 1;
				frmSprite.room = room;
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
		}

		private void clearDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m == null)
				return;
			m.tiles = new List<Tile>();
			m.warps = new List<Warp>();
			cHasWarp.Checked = false;
			m.data = new byte[160];

			setFakeSpace();
			pMap.Invalidate();
		}

		private void clearSpritesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m.sprites = new List<Sprite>();
			pMap.Invalidate();
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
				frmPalette.Show();
			}
			m.closeRom();
		}
	}
}
