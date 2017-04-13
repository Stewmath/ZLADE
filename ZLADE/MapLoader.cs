using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ZLADE
{
	public class MapLoader
	{
		public byte[] data = new byte[160];
		public long dataLocation = 0;
		public byte[] pointerValues = new byte[2];
		public int animationIndex = 0;
		public int borderTileIndex = 0;
		public int floorTileIndex = 0;
		public List<TwoByteTile> twoByteTiles = new List<TwoByteTile>();
		public List<ThreeByteTile> threeByteTiles = new List<ThreeByteTile>();
		public List<Tile> tiles = new List<Tile>();
		public TwoByteTile[,] borderTiles = new TwoByteTile[15, 64];
		public List<Warp> warps = new List<Warp>();
		public BinaryReader reader;
		public BinaryWriter writer;
		public byte[] minimap = new byte[64];
		public byte[] realMinimap = new byte[64];
		public double romVersion = 1.0;
		public int byteCount = 0;
		public string fname = "";
		public int highestMap = 0;
		public List<Sprite> sprites = new List<Sprite>();
		public byte[] spritePointer = new byte[2];
		public bool hasWarp = true;
		public int lastLevel = 0;
		public int lastRoom = 0;
		public byte[] spriteData = new byte[32];
		public long spriteLocation = 0;
		public Event roomEvent = new Event();
		public long eventLocation = 0;
		public byte chestValue = 0;
		public string[] dungeonNames = new string[10];
		public int last6Room = 0;
		public int startingSpace = 0;
		public byte chestX = 0;
		public byte chestY = 0;
		public byte stairsX = 0;
		public byte stairsY = 0;
		public byte keyX = 0;
		public byte keyY = 0;
		public long palleteOffset = 0;
		public int[] paletteColors = new int[32];

		public MapLoader(string filename)
		{
			try
			{
				fname = filename;
			}
			catch (Exception e)
			{
				System.Windows.Forms.MessageBox.Show("Cannot open file.\n\n" + e.Message, "Error");
			}
		}

		int getOffsetPointer(int offset)
		{
			return (int)getPointerAddress((byte)(offset / 0x4000), readByte(offset), readByte(offset + 1));
		}

		int getOffsetPointer(int offset, int bank)
		{
			return (int)getPointerAddress((byte)bank, readByte(offset), readByte(offset + 1));
		}

		public void getPalleteOffset(int level, int room)
		{
			long o = 0;
			/*switch (i)
			{
				case 1: o = 0x86080; break;
				case 2: o = 0x86120; break;
				case 3: o = 0x86270; break;
				case 4: o = 0x862C0; break;
				case 5: o = 0x86450; break;
				case 6: o = 0x86500; break;
				case 7: o = 0x865B0; break;
				case 8: o = 0x866B0; break;
			}*/
			o = 0x85540 + (room * 32);
			palleteOffset = o;
		}

		public void loadPalette(int level, int room)
		{
			getPalleteOffset(level, room);
			reader.BaseStream.Position = palleteOffset;
			for (int i = 0; i < 32; i++)
			{
				int total = reader.ReadByte() + (reader.ReadByte() << 8);
				paletteColors[i] = total;
			}
		}

		public void loadPalette(int address)
		{
			reader.BaseStream.Position = address;
			for (int i = 0; i < 32; i++)
			{
				int total = reader.ReadByte() + (reader.ReadByte() << 8);
				paletteColors[i] = total;
			}
		}

		public void savePalette()
		{
			writer.BaseStream.Position = palleteOffset;
			for (int i = 0; i < 32; i++)
			{
				byte[] bytes = BitConverter.GetBytes(paletteColors[i]);
				if (bytes.Length == 1)
				{
					writer.Write((byte)0);
					writer.Write(bytes[0]);
				}
				else
				{
					writer.Write(bytes[0]);
					writer.Write(bytes[1]);
				}
			}
		}

		public void loadEventPos()
		{/*
			if (romVersion == 1.0)
			{
				reader.BaseStream.Position = 0x9ef0;
				chestX = reader.ReadByte();
				reader.BaseStream.Position = 0x9eec;
				chestY = reader.ReadByte();
				reader.BaseStream.Position = 0x9f69;
				stairsX = reader.ReadByte();
				reader.BaseStream.Position = 0x9f61;
				stairsY = reader.ReadByte();
				reader.BaseStream.Position = 0x9442;
				keyX = reader.ReadByte();
				reader.BaseStream.Position = 09458;
				keyY = reader.ReadByte();
			}
			else if (romVersion == 1.1)
			{
				reader.BaseStream.Position = 0x9f10;
				chestX = reader.ReadByte();
				reader.BaseStream.Position = 0x9f0c;
				chestY = reader.ReadByte();
				reader.BaseStream.Position = 0x9f89;
				stairsX = reader.ReadByte();
				reader.BaseStream.Position = 0x9f81;
				stairsY = reader.ReadByte();
				reader.BaseStream.Position = 0x9462;
				keyX = reader.ReadByte();
				reader.BaseStream.Position = 0x9478;
				keyY = reader.ReadByte();
			}*/
			reader.BaseStream.Position = OffsetLoader.activeOffset.chestX;
			chestX = reader.ReadByte();
			reader.BaseStream.Position = OffsetLoader.activeOffset.chestY;
			chestY = reader.ReadByte();
			reader.BaseStream.Position = OffsetLoader.activeOffset.stairsX;
			stairsX = reader.ReadByte();
			reader.BaseStream.Position = OffsetLoader.activeOffset.stairsY;
			stairsY = reader.ReadByte();
			reader.BaseStream.Position = OffsetLoader.activeOffset.keyX;
			keyX = reader.ReadByte();
			reader.BaseStream.Position = OffsetLoader.activeOffset.keyY;
			keyY = reader.ReadByte();
		}

		public void saveEventPos()
		{
			/*if (romVersion == 1.0)
			{
				writer.BaseStream.Position = 0x9ef0;
				writer.Write(chestX);
				writer.BaseStream.Position = 0x9EAF;
				writer.Write((byte)(chestX + 8));
				writer.BaseStream.Position = 0x9eec;
				writer.Write(chestY);
				writer.BaseStream.Position = 0x9ecb;
				writer.Write((byte)(chestY + 16));
				writer.BaseStream.Position = 0x9f69;
				writer.Write(stairsX);
				writer.BaseStream.Position = 0x9f61;
				writer.Write(stairsY);
				writer.BaseStream.Position = 0x9442;
				writer.Write(keyX);
				writer.BaseStream.Position = 0x9458;
				writer.Write((byte)(keyY + 24));
			}
			else if (romVersion == 1.1)
			{
				writer.BaseStream.Position = 0x9f10;
				writer.Write(chestX);
				writer.BaseStream.Position = 0x9dfd;
				writer.Write((byte)(chestX + 8));
				writer.BaseStream.Position = 0x9f0c;
				writer.Write(chestY);
				writer.BaseStream.Position = 0x9eeb;
				writer.Write((byte)(chestY + 16));
				writer.BaseStream.Position = 0x9f89;
				writer.Write(stairsX);
				writer.BaseStream.Position = 0x9f81;
				writer.Write(stairsY);
				writer.BaseStream.Position = 0x9462;
				writer.Write(keyX);
				writer.BaseStream.Position = 0x9478;
				writer.Write((byte)(keyY + 24));
			}*/
			writer.BaseStream.Position = OffsetLoader.activeOffset.chestX;
			writer.Write(chestX);
			writer.BaseStream.Position = OffsetLoader.activeOffset.chestPoofX;
			writer.Write((byte)(chestX + 8));
			writer.BaseStream.Position = OffsetLoader.activeOffset.chestY;
			writer.Write(chestY);
			writer.BaseStream.Position = OffsetLoader.activeOffset.chestPoofY;
			writer.Write((byte)(chestY + 16));
			writer.BaseStream.Position = OffsetLoader.activeOffset.stairsX;
			writer.Write(stairsX);
			writer.BaseStream.Position = OffsetLoader.activeOffset.stairsY;
			writer.Write(stairsY);
			writer.BaseStream.Position = OffsetLoader.activeOffset.keyX;
			writer.Write(keyX);
			writer.BaseStream.Position = OffsetLoader.activeOffset.keyY;
			writer.Write((byte)(keyY + 24));
		}

		public void readHighest6Index()
		{
			int highest = 0;
			reader.BaseStream.Position = 0x50220 + (64 * 5);
			byte[] b = reader.ReadBytes(64);
			for (int i = 0; i < b.Length; i++)
			{
				if ((int)b[i] > highest)
					highest = (int)b[i];
			}
			last6Room = highest;
		}

		public void readDungeonNames()
		{
			reader.BaseStream.Position = 0x53393;
			byte[] data = reader.ReadBytes(0x116);
			string to = System.Text.Encoding.ASCII.GetString(data);
			string[] names = to.Split('?');
			dungeonNames = names;
		}

		public void saveDungeonNames()
		{
			writer.BaseStream.Position = 0x53393;
			for (int i = 0; i < 9; i++)
			{
				string s = dungeonNames[i];
				byte[] b = System.Text.Encoding.ASCII.GetBytes(s);
				writer.Write(b);
				writer.Write((byte)0xFF);
			}
		}

		public void readChest(int level, int room)
		{
			if (level != 0xA)
				reader.BaseStream.Position = 0x50660 + room;
			else
				reader.BaseStream.Position = 0x50860 + room;
			chestValue = reader.ReadByte();
		}

		public void readSprites(int level, int room)
		{
			//Pointer first
			reader.BaseStream.Position = 0x58200 + (room * 2);
			spritePointer = reader.ReadBytes(2);
			long offset = getPointerAddress(0x58000 / 0x4000, spritePointer[0], spritePointer[1]);
			if (level == 0xA)
			{
				reader.BaseStream.Position = 0x58600 + (room * 2);
				spritePointer = reader.ReadBytes(2);
				offset = getPointerAddress(0x58000 / 0x4000, spritePointer[0], spritePointer[1]);
			}
			spriteLocation = offset;
			reader.BaseStream.Position = offset;
			string off = offset.ToString("X");
			byte b;
			int ind = 0;
			spriteData = new byte[32];
			while ((b = reader.ReadByte()) != 0xFF)
			{
				spriteData[ind] = b;
				ind++;
			}
			for (int i = 0; i < ind; i++)
			{
				Sprite s = new Sprite();
				string to = spriteData[i].ToString("X");
				if(to.Length == 1)
					to = "0" + to;
				char[] ccc = to.ToCharArray();
				int x = Convert.ToInt32(ccc[1].ToString(), 16);
				int y = Convert.ToInt32(ccc[0].ToString(), 16);
				int id = spriteData[i + 1];
				s.x = x;
				s.y = y;
				s.id = id;
				sprites.Add(s);
				i++;
			}
		}

		public void readEvent(int level, int room)
		{
			long offset = 0x50000 + room;
			eventLocation = offset;
			if (level == 0xA)
			{
				offset = 0x50200 + room;
				eventLocation = offset;
			}
			reader.BaseStream.Position = offset;
			Event e = new Event();
			Byte b = reader.ReadByte();
			string to = b.ToString("X");
			if (to.Length == 1)
				to = "0" + to;
			int id = Convert.ToInt32(to.Substring(0, 1), 16);
			int trigger = Convert.ToInt32(to.Substring(1, 1), 16);
			e.id = id;
			e.trigger = trigger;
			roomEvent = e;
		}

		public void saveEvent()
		{
			writer.BaseStream.Position = eventLocation;
			Event e = roomEvent;
			string s = e.id.ToString("X") + e.trigger.ToString("X");
			byte b = (byte)Convert.ToInt32(s, 16);
			writer.Write(b);
		}

		public void readPointer(int level, int room)
		{
			if (level < 7)
			{
				byte[] buffer = new byte[2];
				room--;
				if (room + 1 > 0)
					reader.BaseStream.Position = 0x28002 + (room * 2);
				else
					reader.BaseStream.Position = 0x28000;
				buffer = reader.ReadBytes(2);
				pointerValues = buffer;
			}
			else
			{
				byte[] buffer = new byte[2];
				room--;
				if (room + 1 > 0)
					reader.BaseStream.Position = 0x2C002 + (room * 2);
				else
					reader.BaseStream.Position = 0x2C000;
				buffer = reader.ReadBytes(2);
				pointerValues = buffer;
			}
		}
		public void getData(int level, int room)
		{
			byte b;
			lastLevel = level;
			lastRoom = room;
			dataLocation = getOffset(level, room);
			reader.BaseStream.Position = dataLocation;
			data = new byte[160];
			int i = 0;
			while ((b = reader.ReadByte()) != 0xFE && i < 160)
			{
				data[i] = b;
				i++;
			}
			if (i == 160)
			{
				System.Windows.Forms.MessageBox.Show("Too many bytes in room ( > 160)!", "Invalid Room");
			}
			loadEventPos();
			loadPalette(level, lastRoom);
			byteCount = i;
			startingSpace = i;
		}

		public void openRom()
		{
			try
			{
				reader = new BinaryReader(File.Open(fname, FileMode.Open));
			}
			catch (Exception)
			{
				if (writer != null)
				{
					writer.Close();
					writer = null;
				}
				reader = new BinaryReader(File.Open(fname, FileMode.Open));
			}
		}

		public void closeRom()
		{
			reader.Close();
		}

		public void clearData()
		{
			data = new byte[160];
			threeByteTiles = new List<ThreeByteTile>();
			twoByteTiles = new List<TwoByteTile>();
			tiles = new List<Tile>();
			warps = new List<Warp>();
			sprites = new List<Sprite>();
			roomEvent = new Event();
			highestMap = 0;
			hasWarp = false;
		}

		public void readMinimap(int level)
		{
			level--;
			//if (romVersion == 1.0)
			reader.BaseStream.Position = OffsetLoader.activeOffset.dMinimap + (64 * level);
			//else if (romVersion == 1.1)
			//	reader.BaseStream.Position = 0xA49A + (64 * level);
			minimap = reader.ReadBytes(64);
		}

		public void readRealMinimap(int level)
		{
			if (level < 0xA)
			{
				level--;
				reader.BaseStream.Position = 0x50220 + (64 * level);
				realMinimap = reader.ReadBytes(64);
			}
			else
			{
				reader.BaseStream.Position = 0x504E0;
				realMinimap = reader.ReadBytes(64);
			}
		}

		public void checkHighestMap()
		{
			highestMap = 0;
			for (int i = 0; i < 64; i++)
			{
				if (realMinimap[i] > highestMap)
					highestMap = realMinimap[i];
			}
		}

		public long getOffset(int level, int room)
		{
			readPointer(level, room);
			long o = 0;
			if (level == 0xFF)
			{
				byte b1 = readByte(0x2BB77 + room * 2);
				byte b2 = readByte(0x2BB77 + 1 + room * 2);
				byte[] b = { b1, b2 };
				pointerValues = b;
				return getPointerAddress(0xA, b1, b2);
			}
			if (level < 7)
			{
				o = getPointerAddress(0x28000 / 0x4000, pointerValues[0], pointerValues[1]);//getAddressFromPointer(0x28000, pointerValues);
			}
			else
			{
				o = getPointerAddress(0x2c000 / 0x4000, pointerValues[0], pointerValues[1]);
			}
			return o;
		}

		public void formatData()
		{
			string an = data[0].ToString("X");
			animationIndex = Convert.ToInt32(an, 16);
			string d = data[1].ToString("X");
			if (d.Length == 1)
				d = "0" + d;
			char[] chars = d.ToCharArray();
			borderTileIndex = Convert.ToInt32(chars[0].ToString(), 16);
			floorTileIndex = Convert.ToInt32(chars[1].ToString(), 16);
			for (int i = 2; i < byteCount; i++) //Skip header
			{
				byte s = data[i];
				string to = s.ToString("X");
				if (to.StartsWith("E") && to.Length > 1) //Warp
				{
					Warp w = new Warp();
					char[] ccc = to.ToCharArray();
					int mt = Convert.ToInt32(ccc[1].ToString(), 16);
					w.type = Warp.getMapType(mt);
					w.map = data[i + 1];
					w.room = data[i + 2];
					w.x = data[i + 3];
					w.y = data[i + 4];
					warps.Add(w);
					hasWarp = true;
					i += 4;
					continue;
				}

				if (to.StartsWith("8") && to.Length > 1) //Horizontal 3-byte object
				{
					ThreeByteTile t = new ThreeByteTile();
					t.direction = ThreeByteTile.Direction.Horizontal;
					char[] ccc = to.ToCharArray();
					int l = Convert.ToInt32(ccc[1].ToString(), 16);
					t.length = l;
					char[] cl = data[i + 1].ToString("X").ToCharArray();
					if (cl.Length > 1)
						t.y = Convert.ToInt32(cl[0].ToString(), 16);
					else
						t.y = 0;
					if (cl.Length > 1)
						t.x = Convert.ToInt32(cl[1].ToString(), 16);
					else
						t.x = Convert.ToInt32(cl[0].ToString(), 16);
					t.id = data[i + 2];
					threeByteTiles.Add(t);
					Tile tile = new Tile();
					tile.is3Byte = true;
					tile.direction = (Tile.Direction)t.direction;
					tile.x = t.x;
					tile.y = t.y;
					tile.length = t.length;
					tile.id = t.id;
					tiles.Add(tile);
					i += 2;
					continue;
				}

				if (to.StartsWith("C")) //Vertical 3-byte object
				{
					ThreeByteTile t = new ThreeByteTile();
					t.direction = ThreeByteTile.Direction.Vertical;
					char[] ccc = to.ToCharArray();
					int l = Convert.ToInt32(ccc[1].ToString(), 16);
					t.length = l;
					char[] cl = data[i + 1].ToString("X").ToCharArray();
					if (cl.Length > 1)
						t.y = Convert.ToInt32(cl[0].ToString(), 16);
					else
						t.y = 0;
					if (cl.Length > 1)
						t.x = Convert.ToInt32(cl[1].ToString(), 16);
					else
						t.x = Convert.ToInt32(cl[0].ToString(), 16);
					t.id = data[i + 2];
					threeByteTiles.Add(t);
					Tile tile = new Tile();
					tile.is3Byte = true;
					tile.direction = (Tile.Direction)t.direction;
					tile.x = t.x;
					tile.y = t.y;
					tile.length = t.length;
					tile.id = t.id;
					tiles.Add(tile);
					i += 2;
					continue;
				}

				else //2-byte object
				{
					TwoByteTile t = new TwoByteTile();
					char[] cl = data[i].ToString("X").ToCharArray();
					if (cl.Length > 1)
					{
						t.x = Convert.ToInt32(cl[1].ToString(), 16);
						t.y = Convert.ToInt32(cl[0].ToString(), 16);
					}
					else
					{
						t.y = 0;
						t.x = Convert.ToInt32(cl[0].ToString(), 16);
					}
					t.id = data[i + 1];
					twoByteTiles.Add(t);
					Tile tile = new Tile();
					tile.is3Byte = false;
					tile.x = t.x;
					tile.y = t.y;
					tile.id = t.id;
					tiles.Add(tile);
					i += 1;
				}
			}
		}

		public void readBorderTiles()
		{
			reader.BaseStream.Position = 0x50917;
			for (int i = 0; i < 9; i++)
			{
				byte b = 0;
				int count = 0;
				List<TwoByteTile> list = new List<TwoByteTile>();
				while ((b = reader.ReadByte()) != 0xFF)
				{
					TwoByteTile t = new TwoByteTile();
					t.y = b / 16;
					t.x = b - (t.y * 16);
					list.Add(t);
					count++;
				}
				byte[] buffer = reader.ReadBytes(count);
				for (int k = 0; k < count; k++)
				{
					list[k].id = buffer[k];
					borderTiles[i, k] = list[k];
				}
			}
		}

		public TwoByteTile[] convertToTwo()
		{
			List<TwoByteTile> tiles = new List<TwoByteTile>();
			for (int i = 0; i < threeByteTiles.Count; i++)
			{
				int x = -1;
				int y = -1;
				for (int k = 0; k < threeByteTiles[i].length; k++)
				{
					TwoByteTile t = new TwoByteTile();
					if (x == -1 && y == -1)
					{
						x = threeByteTiles[i].x;
						y = threeByteTiles[i].y;
					}
					t.id = threeByteTiles[i].id;
					t.x = x;
					t.y = y;
					if (threeByteTiles[i].direction == ThreeByteTile.Direction.Horizontal)
					{
						x += 1;
					}
					else
					{
						y += 1;
					}
					tiles.Add(t);
				}
			}

			return tiles.ToArray();
		}

		public long getPointerAddress(byte bank, byte byte1, byte byte2)
		{
			byte2 -= 0x40;
			string b1 = byte1.ToString("X");
			if (b1.Length == 1)
				b1 = "0" + b1;
			string s = byte2.ToString("X") + b1;
			int l1 = Convert.ToInt32(s, 16);
			long l = (bank * 0x4000) + l1;
			string s1 = l.ToString("X");
			return l;
		}

		public void writePointer(int level, int room)
		{
			reader.Close();
			writer = new BinaryWriter(File.Open(fname, FileMode.Open));
			if (level < 7)
			{
				writer.BaseStream.Position = 0x28000 + (room * 2);
				writer.Write(pointerValues);
				writer.Flush();
			}
			else
			{
				writer.BaseStream.Position = 0x2C000 + (room * 2);
				writer.Write(pointerValues);
				writer.Flush();
			}
			writer.Close();
		}

		public void writeSpritePointer(int level, int room)
		{
			reader.Close();
			writer = new BinaryWriter(File.Open(fname, FileMode.Open));
			if (level > 6)
				room += 0x100;
			writer.BaseStream.Position = 0x58200 + (room * 2);
			writer.Write(spritePointer);
			writer.Close();
		}

		public byte[] getObjectData()
		{
			List<byte> temp = new List<byte>();
			for (int i = 0; i < tiles.Count; i++)
			{
				Tile t = tiles[i];
				if (t.is3Byte)
				{
					string dbyte;
					if (t.direction == Tile.Direction.Horizontal)
					{
						dbyte = "8";
					}
					else
					{
						dbyte = "C";
					}
					dbyte += t.length.ToString("X");
					int length = Convert.ToInt32(dbyte, 16);
					string lbyte;
					lbyte = t.y.ToString("X") + t.x.ToString("X");
					int location = Convert.ToInt32(lbyte, 16);
					int id = t.id;
					byte[] towrite = { (byte)length, (byte)location, (byte)id };
					foreach (byte b in towrite)
						temp.Add(b);
				}
				else
				{
					string lbyte;
					lbyte = t.y.ToString("X") + t.x.ToString("X");
					int location = Convert.ToInt32(lbyte, 16);
					int id = t.id;
					byte[] towrite = { (byte)location, (byte)id };
					foreach (byte b in towrite)
						temp.Add(b);
				}
			}
			for (int i = 0; i < warps.Count; i++)
			{
				if (!hasWarp || warps.Count == 0)
					break;
				Warp w = warps[i];
				byte type = 0;
				if (w.type == Warp.MapType.Dungeon)
					type = 1;
				else if (w.type == Warp.MapType.Side)
					type = 2;
				string t = "E" + type.ToString();
				type = (byte)Convert.ToInt32(t, 16);
				byte map = (byte)w.map;
				byte room = (byte)w.room;
				byte x = (byte)w.x;
				byte y = (byte)w.y;
				byte[] towrite = { type, map, room, x, y };
				writer.Write(towrite);
				foreach (byte b in towrite)
					temp.Add(b);
			}
			return temp.ToArray();
		}

		public int getFreeSpace()
		{
			try
			{
				int space = 0;
				byte b = 0;
				byte last = 0;
				reader.BaseStream.Position = dataLocation;
				while ((b = reader.ReadByte()) != 0xFE) ;
				while ((b = reader.ReadByte()) != 0xFE)
				{
					if (b == 0 && last == 0)
						space++;
					last = b;
				}
				return space;
			}
			catch (Exception)
			{
				openRom();
				int space = 0;
				byte b = 0;
				byte last = 0;
				reader.BaseStream.Position = dataLocation;
				while ((b = reader.ReadByte()) != 0xFE) ;
				while ((b = reader.ReadByte()) != 0xFE)
				{
					if (b == 0 && last == 0)
						space++;
					last = b;
				}
				closeRom();
				return space;
			}
		}

		public int getFreeOSpace()
		{
			try
			{
				int space = 0;
				byte b = 0;
				byte last = 0;
				reader.BaseStream.Position = oDataLocation;
				while ((b = reader.ReadByte()) != 0xFE);
				while ((b = reader.ReadByte()) != 0xFE)
				{
					if (b == 0 && last == 0)
						space++;
					last = b;
				}
				return space;
			}
			catch (Exception)
			{
				openRom();
				int space = 0;
				byte b = 0;
				byte last = 0;
				reader.BaseStream.Position = oDataLocation;
				while ((b = reader.ReadByte()) != 0xFE);
				while ((b = reader.ReadByte()) != 0xFE)
				{
					if (b == 0 && last == 0)
						space++;
					last = b;
				}
				closeRom();
				return space;
			}
		}

		public int getFreeISpace()
		{
			try
			{
				int space = 0;
				byte b = 0;
				byte last = 0;
				reader.BaseStream.Position = iDataLocation;
				while ((b = reader.ReadByte()) != 0xFE) ;
				while ((b = reader.ReadByte()) != 0xFE)
				{
					if (b == 0 && last == 0)
						space++;
					last = b;
				}
				return space;
			}
			catch (Exception)
			{
				openRom();
				int space = 0;
				byte b = 0;
				byte last = 0;
				reader.BaseStream.Position = iDataLocation;
				while ((b = reader.ReadByte()) != 0xFE) ;
				while ((b = reader.ReadByte()) != 0xFE)
				{
					if (b == 0 && last == 0)
						space++;
					last = b;
				}
				closeRom();
				return space;
			}
		}

		public int getNeededSpace()
		{
			int space = 3;
			for (int i = 0; i < tiles.Count; i++)
			{
				Tile t = tiles[i];
				if (t.is3Byte)
				{
					space += 3;
				}
				else
				{
					space += 2;
				}
			}
			for (int i = 0; i < warps.Count; i++)
			{
				if (warps.Count == 0 || !hasWarp)
					break;
				space += 5;
			}
			return space;
		}

		public int getONeededSpace()
		{
			int space = 3;
			for (int i = 0; i < oTiles.Count; i++)
			{
				Tile t = oTiles[i];
				if (t.is3Byte)
				{
					space += 3;
				}
				else
				{
					space += 2;
				}
			}
			for (int i = 0; i < oWarps.Count; i++)
			{
				if (oWarps.Count == 0)
					break;
				space += 5;
			}
			return space;
		}

		public int getINeededSpace()
		{
			int space = 3;
			for (int i = 0; i < iTiles.Count; i++)
			{
				Tile t = iTiles[i];
				if (t.is3Byte)
				{
					space += 3;
				}
				else
				{
					space += 2;
				}
			}
			for (int i = 0; i < iWarps.Count; i++)
			{
				if (iWarps.Count == 0 || !hasWarp)
					break;
				space += 5;
			}
			return space;
		}

		public void saveRoomData()
		{
			openRom();
			long ending = dataLocation;
			reader.BaseStream.Position = dataLocation;
			while (reader.ReadByte() != 0xFE)
				ending++;
			reader.Close();
			writer = new BinaryWriter(File.Open(fname, FileMode.Open));
			int needed = getNeededSpace();
			writer.BaseStream.Position = dataLocation;
			byte animindex = (byte)animationIndex;
			string template = borderTileIndex.ToString("X") + floorTileIndex.ToString("X");
			byte temp = (byte)Convert.ToInt32(template, 16);
			byte[] ttt = {animindex, temp};
			writer.Write(ttt);
			for (int i = 0; i < tiles.Count; i++)
			{
				Tile t = tiles[i];
				if (t.is3Byte)
				{
					string dbyte;
					if (t.direction == Tile.Direction.Horizontal)
					{
						dbyte = "8";
					}
					else
					{
						dbyte = "C";
					}
					dbyte += t.length.ToString("X");
					int length = Convert.ToInt32(dbyte, 16);
					string lbyte;
					lbyte = t.y.ToString("X") + t.x.ToString("X");
					int location = Convert.ToInt32(lbyte, 16);
					int id = t.id;
					byte[] towrite = { (byte)length, (byte)location, (byte)id };
					writer.Write(towrite);
				}
				else
				{
					string lbyte;
					lbyte = t.y.ToString("X") + t.x.ToString("X");
					int location = Convert.ToInt32(lbyte, 16);
					int id = t.id;
					byte[] towrite = { (byte)location, (byte)id };
					writer.Write(towrite);
				}
			}
			for (int i = 0; i < warps.Count; i++)
			{
				if (!hasWarp || warps.Count == 0)
					break;
				Warp w = warps[i];
				byte type = 0;
				if (w.type == Warp.MapType.Dungeon)
					type = 1;
				else if (w.type == Warp.MapType.Side)
					type = 2;
				string t = "E" + type.ToString();
				type = (byte)Convert.ToInt32(t, 16);
				byte map = (byte)w.map;
				byte room = (byte)w.room;
				byte x = (byte)w.x;
				byte y = (byte)w.y;
				byte[] towrite = { type, map, room, x, y };
				writer.Write(towrite);
			}
			long l = writer.BaseStream.Position;

			writer.Write((byte)0xFE);
			if (writer.BaseStream.Position < ending)
			{
				while (writer.BaseStream.Position != ending + 1)
					writer.Write((byte)0);
			}
			saveDungeonNames();
			saveEventPos();
			saveSprites();
			saveEvent();
			savePalette();
			saveMinimaps(lastLevel);
			saveChest(lastLevel, lastRoom);
			writer.Close();
		}

		public void saveMinimaps(int level)
		{
			level--;
			writer.BaseStream.Position = OffsetLoader.activeOffset.dMinimap + (64 * level);
			writer.Write(minimap);
			writer.BaseStream.Position = 0x50220 + (64 * level);
			writer.Write(realMinimap);
		}

		public void saveChest(int level, int room)
		{
			writer.BaseStream.Position = 0x50660 + room;
			writer.Write(chestValue);
		}

		public void saveSprites()
		{
			writer.BaseStream.Position = spriteLocation;
			for (int i = 0; i < sprites.Count; i++)
			{
				if (sprites[i] == new Sprite() || sprites[i] == null || sprites[i].id == 0)
					continue;
				Sprite s = sprites[i];
				string loc = s.y.ToString("X") + s.x.ToString("X");
				byte l = (byte)Convert.ToInt32(loc, 16);
				byte id = (byte)s.id;
				byte[] towrite = { l, id };
				writer.Write(towrite);
			}
			byte end = 0xFF;
			writer.Write(end);
		}

		public void add2ByteObject()
		{
			Tile t = new Tile();
			tiles.Add(t);
		}

		public void add3ByteObject()
		{
			Tile t = new Tile();
			t.is3Byte = true;
			t.length = 1;
			tiles.Add(t);
		}




		/*END
		 * OF
		 * DUNGEON
		 */
		//OVERWORLD STUFF
		public byte[] overlayData = new byte[80];
		int lastORoom = 0;
		public byte OTileset = 0;
		public Color[,] oMinimapPalette = new Color[8, 4];
		public byte[,] oMinimapGraphics = new byte[112, 64];
		public byte[,] oMinimapGraphics2 = new byte[16, 64];
		public byte[] oCollisionData = new byte[128];
		public byte[] oPointerValues = new byte[2];
		public long oDataLocation = 0;
		public byte oAnimIndex = 0;
		public byte oHeaderValue2 = 0;
		public byte oFloorTile = 0;
		public List<Tile> oTiles = new List<Tile>();
		public int oByteCount = 0;
		public List<Warp> oWarps = new List<Warp>();
		public List<int> oWarpIndexes = new List<int>();
		public long oSpriteLocation = 0;
		public List<Sprite> oSprites = new List<Sprite>();
		public byte oChestValue = 0;
		public byte oSpriteSet = 0;
		public struct oMinimapTile
		{
			public int gID;
			public int pID;
		}
		public oMinimapTile[] oMinimapTiles = new oMinimapTile[256];
		public void loadOverlayData(int room)
		{
			if (room < 0xCC)
				reader.BaseStream.Position = 0x98000 + (room * 80);
			else
				reader.BaseStream.Position = 0x9C000 + ((room - 0xCC) * 80);
			overlayData = reader.ReadBytes(80);
		}

		public void readOSpriteSet(int room)
		{
			reader.BaseStream.Position = 0x830D3 + room;
			oSpriteSet = reader.ReadByte();
		}

		public void writeOSpriteSet()
		{
			writer.BaseStream.Position = 0x830D3 + lastORoom;
			writer.Write(oSpriteSet);
		}

		public void writeOSpritePointer(byte b1, byte b2)
		{
			writer = new BinaryWriter(File.Open(fname, FileMode.Open));
			writer.BaseStream.Position = 0x58000 + (lastORoom * 2);
			writer.Write(b1);
			writer.Write(b2);
			writer.Close();
		}

		public void readOChest(int room)
		{
			reader.BaseStream.Position = 0x50560 + room;
			oChestValue = reader.ReadByte();
		}

		public void saveOChest()
		{
			writer.BaseStream.Position = 0x50560 + lastORoom;
			writer.Write(oChestValue);
		}

		public void saveOverlayData()
		{
			int room = lastORoom;
			if (room < 0xCC)
				writer.BaseStream.Position = 0x98000 + (room * 80);
			else
				writer.BaseStream.Position = 0x9C000 + ((room - 0xCC) * 80);
			writer.Write(overlayData);
		}

		public Color GetColor(int value)
		{
			UInt16 color2B = (UInt16)value;
			int red = (color2B & 31) << 3;
			color2B >>= 5;
			int green = (color2B & 31) << 3;
			color2B >>= 5;
			int blue = (color2B & 31) << 3;
			return Color.FromArgb(red, green, blue);
		}

		public void readSprites(int room)
		{
			byte b1 = readByte(0x58000 + (room * 2));
			byte b2 = readByte(0x58000 + (room * 2) + 1);
			if (lastLevel == 10)
			{
				b1 = readByte(0x58600 + (room * 2));
				b2 = readByte(0x58601 + (room * 2));
			}
			oSpriteLocation = getPointerAddress(0x58000 / 0x4000, b1, b2);
			reader.BaseStream.Position = oSpriteLocation;
			byte b;
			int ind = 0;
			spriteData = new byte[32];
			while ((b = reader.ReadByte()) != 0xFF)
			{
				spriteData[ind] = b;
				ind++;
			}
			for (int i = 0; i < ind; i++)
			{
				Sprite s = new Sprite();
				string to = spriteData[i].ToString("X");
				if (to.Length == 1)
					to = "0" + to;
				char[] ccc = to.ToCharArray();
				int x = Convert.ToInt32(ccc[1].ToString(), 16);
				int y = Convert.ToInt32(ccc[0].ToString(), 16);
				int id = spriteData[i + 1];
				s.x = x;
				s.y = y;
				s.id = id;
				oSprites.Add(s);
				i++;
			}
		}

		public byte[,] readTileData(int tilecount, long address)
		{
			reader.BaseStream.Position = address;
			byte[,] data = new byte[tilecount, 64];
			for (int ind = 0; ind < tilecount; ind++)
			{
				int last = 0;
				byte[] by = reader.ReadBytes(16);
				string[] bincodes = new string[2];
				for (int i = 0; i < 16; i++)
				{
					bincodes[0] = Convert.ToString(by[i], 2);
					bincodes[1] = Convert.ToString(by[i + 1], 2);
					while (bincodes[0].Length < 8)
						bincodes[0] = "0" + bincodes[0];
					while (bincodes[1].Length < 8)
						bincodes[1] = "0" + bincodes[1];
					for (int k = 0; k < 8; k++)
					{
						string s1 = bincodes[0].Substring(k, 1);
						string s2 = bincodes[1].Substring(k, 1);
						string t = s2 + s1;
						int value = (int)Convert.ToInt32(t, 2);
						data[ind, last] = (byte)value;
						last++;
					}

					i++;
				}
			}
			return data;
		}

		public void readOMinimapGraphics()
		{
			oMinimapGraphics = readTileData(112, 0xB3900);
			oMinimapGraphics2 = readTileData(16, 0xB3800);
		}


		public void readOMinimapPalette()
		{
			reader.BaseStream.Position = 0x8786E;
			for (int i = 0; i < 8; i++)
			{
				for (int k = 0; k < 4; k++)
				{
					string d = reader.ReadByte().ToString("X");
					string s = reader.ReadByte().ToString("X") + d;
					int total = Convert.ToInt32(s, 16);
					oMinimapPalette[i, k] = frmPalette.getColorFromHex(total);
				}
			}
		}

		public byte readByte(long address)
		{
			reader.BaseStream.Position = address;
			return reader.ReadByte();
		}

		public void readOMinimap()
		{
			for (int i = 0; i < 256; i++)
			{
				byte tID = readByte(OffsetLoader.activeOffset.oMinimapTile + i);
				byte pID = readByte(OffsetLoader.activeOffset.oMinimapPal + i);
				oMinimapTile o = new oMinimapTile();
				o.gID = tID;
				o.pID = pID;
				oMinimapTiles[i] = o;
			}
		}

		public void saveOMinimap()
		{
			for (int i = 0; i < 256; i++)
			{
				byte tID = (byte)oMinimapTiles[i].gID;
				byte pID = (byte)oMinimapTiles[i].pID;
				writer.BaseStream.Position = OffsetLoader.activeOffset.oMinimapTile + i;
				writer.Write(tID);
				writer.BaseStream.Position = OffsetLoader.activeOffset.oMinimapPal + i;
				writer.Write(pID);
			}
		}

		public void readOverworldCollision(int room)
		{
			lastORoom = room;
			byte pv1 = readByte(0x24000 + (room * 2));
			byte pv2 = readByte(0x24001 + (room * 2));
			byte[] pv = {pv1, pv2};
			oPointerValues = pv;
			long l = getPointerAddress(0x24000 / 0x4000, pv1, pv2);
			if (room > 0x7F)
			{
				int temp = (int)l - 0x24000;
				l = 0x68000 + temp;
			}
			oDataLocation = l;
			reader.BaseStream.Position = l;
			int i = 0;
			byte b = 0;
			while ((b = reader.ReadByte()) != 0xFE && i < 160)
			{
				oCollisionData[i] = b;
				i++;
			}
			oByteCount = i;
			if (i == 160)
			{
				System.Windows.Forms.MessageBox.Show("Too many bytes in room ( > 160)!", "Invalid Room");
			}
		}

		public void clearOData()
		{
			oTiles = new List<Tile>();
			oWarpIndexes = new List<int>();
			oWarps = new List<Warp>();
			oSprites = new List<Sprite>();
		}

		public void formatOverworldData()
		{
			oAnimIndex = oCollisionData[0];
			oHeaderValue2 = oCollisionData[1];
			int objectIndex = 0;
			string d = oCollisionData[1].ToString("X");
			if (d.Length == 1)
				d = "0" + d;
			char[] chars = d.ToCharArray();
			oFloorTile = (byte)Convert.ToInt32(chars[1].ToString(), 16);
			oHeaderValue2 = (byte)Convert.ToInt32(chars[0].ToString(), 16);
			for (int i = 2; i < oByteCount; i++)
			{
				byte b = oCollisionData[i];
				string to = b.ToString("X");
				if (to.StartsWith("E") && to.Length > 1)
				{
					Warp w = new Warp();
					char[] ccc = to.ToCharArray();
					int mt = Convert.ToInt32(ccc[1].ToString(), 16);
					w.type = Warp.getMapType(mt);
					w.map = oCollisionData[i + 1];
					w.room = oCollisionData[i + 2];
					w.x = oCollisionData[i + 3];
					w.y = oCollisionData[i + 4];
					w.after = objectIndex - 1;
					oWarps.Add(w);
					i += 4;
					oWarpIndexes.Add(objectIndex - 1);
					continue;
				}
				if (to.StartsWith("8") && to.Length > 1) //Horizontal 3-byte object
				{
					ThreeByteTile t = new ThreeByteTile();
					t.direction = ThreeByteTile.Direction.Horizontal;
					char[] ccc = to.ToCharArray();
					int l = Convert.ToInt32(ccc[1].ToString(), 16);
					t.length = l;
					char[] cl = oCollisionData[i + 1].ToString("X").ToCharArray();
					if (cl.Length > 1)
						t.y = Convert.ToInt32(cl[0].ToString(), 16);
					else
						t.y = 0;
					if (cl.Length > 1)
						t.x = Convert.ToInt32(cl[1].ToString(), 16);
					else
						t.x = Convert.ToInt32(cl[0].ToString(), 16);
					t.id = oCollisionData[i + 2];
					threeByteTiles.Add(t);
					Tile tile = new Tile();
					tile.is3Byte = true;
					tile.direction = (Tile.Direction)t.direction;
					tile.x = t.x;
					tile.y = t.y;
					tile.length = t.length;
					tile.id = t.id;
					oTiles.Add(tile);
					objectIndex++;
					i += 2;
					continue;
				}

				if (to.StartsWith("C")) //Vertical 3-byte object
				{
					ThreeByteTile t = new ThreeByteTile();
					t.direction = ThreeByteTile.Direction.Vertical;
					char[] ccc = to.ToCharArray();
					int l = Convert.ToInt32(ccc[1].ToString(), 16);
					t.length = l;
					char[] cl = oCollisionData[i + 1].ToString("X").ToCharArray();
					if (cl.Length > 1)
						t.y = Convert.ToInt32(cl[0].ToString(), 16);
					else
						t.y = 0;
					if (cl.Length > 1)
						t.x = Convert.ToInt32(cl[1].ToString(), 16);
					else
						t.x = Convert.ToInt32(cl[0].ToString(), 16);
					t.id = oCollisionData[i + 2];
					threeByteTiles.Add(t);
					Tile tile = new Tile();
					tile.is3Byte = true;
					tile.direction = (Tile.Direction)t.direction;
					tile.x = t.x;
					tile.y = t.y;
					tile.length = t.length;
					tile.id = t.id;
					oTiles.Add(tile);
					objectIndex++;
					i += 2;
					continue;
				}

				else //2-byte object
				{
					TwoByteTile t = new TwoByteTile();
					char[] cl = oCollisionData[i].ToString("X").ToCharArray();
					if (cl.Length > 1)
					{
						t.x = Convert.ToInt32(cl[1].ToString(), 16);
						t.y = Convert.ToInt32(cl[0].ToString(), 16);
					}
					else
					{
						t.y = 0;
						t.x = Convert.ToInt32(cl[0].ToString(), 16);
					}
					t.id = oCollisionData[i + 1];
					twoByteTiles.Add(t);
					Tile tile = new Tile();
					tile.is3Byte = false;
					tile.x = t.x;
					tile.y = t.y;
					tile.id = t.id;
					oTiles.Add(tile);
					objectIndex++;
					i += 1;
				}
			}
		}

		public byte[] getOObjectData()
		{
			List<byte> temp = new List<byte>();
			int warpIndex = 0;
			for (int i = 0; i < oTiles.Count; i++)
			{
				Tile t = oTiles[i];
				if (t.is3Byte)
				{
					string dbyte;
					if (t.direction == Tile.Direction.Horizontal)
					{
						dbyte = "8";
					}
					else
					{
						dbyte = "C";
					}
					dbyte += t.length.ToString("X");
					int length = Convert.ToInt32(dbyte, 16);
					string lbyte;
					lbyte = t.y.ToString("X") + t.x.ToString("X");
					int location = Convert.ToInt32(lbyte, 16);
					int id = t.id;
					byte[] towrite = { (byte)length, (byte)location, (byte)id };
					foreach (byte b in towrite)
						temp.Add(b);
				}
				else
				{
					string lbyte;
					lbyte = t.y.ToString("X") + t.x.ToString("X");
					int location = Convert.ToInt32(lbyte, 16);
					int id = t.id;
					byte[] towrite = { (byte)location, (byte)id };
					foreach (byte b in towrite)
						temp.Add(b);
				}
				if (warpIndex < oWarps.Count)
				{
					for (int k = 0; k < oWarpIndexes.Count; k++)
					{
						if (i == oWarpIndexes[k])
						{
							Warp w = oWarps[k];
							byte type = 0;
							if (w.type == Warp.MapType.Dungeon)
								type = 1;
							else if (w.type == Warp.MapType.Side)
								type = 2;
							string ts = "E" + type.ToString();
							type = (byte)Convert.ToInt32(ts, 16);
							byte map = (byte)w.map;
							byte room = (byte)w.room;
							byte x = (byte)w.x;
							byte y = (byte)w.y;
							byte[] towrite = { type, map, room, x, y };
							foreach (byte b in towrite)
								temp.Add(b);
							warpIndex++;
						}
					}
				}
			}
			return temp.ToArray();
		}

		public void saveOverworld()
		{
			openRom();
			long ending = oDataLocation;
			reader.BaseStream.Position = oDataLocation;
			while (reader.ReadByte() != 0xFE)
				ending++;
			reader.Close();
			writer = new BinaryWriter(File.Open(fname, FileMode.Open));
			int needed = getNeededSpace();
			writer.BaseStream.Position = oDataLocation;
			byte temp = (byte)oFloorTile;
			byte[] ttt = { oAnimIndex, temp };
			writer.Write(ttt);
			writer.Write(getOObjectData());
			writer.Write((byte)0xFE);
			if (writer.BaseStream.Position < ending)
			{
				while (writer.BaseStream.Position != ending + 1)
					writer.Write((byte)0);
			}
			saveOSprites();
			saveOChest();
			saveOverlayData();
			saveOMinimap();
			writeOSpriteSet();
			writer.Close();
		}

		public void writeOPointer(byte[] values)
		{
			writer = new BinaryWriter(File.Open(fname, FileMode.Open));
			writer.BaseStream.Position = 0x24000 + (lastORoom * 2);
			writer.Write(values);
			writer.Close();
		}

		public void saveOSprites()
		{
			writer.BaseStream.Position = oSpriteLocation;
			for (int i = 0; i < oSprites.Count; i++)
			{
				if (oSprites[i] == new Sprite() || oSprites[i] == null || oSprites[i].id == 0)
					continue;
				Sprite s = oSprites[i];
				string loc = s.y.ToString("X") + s.x.ToString("X");
				byte l = (byte)Convert.ToInt32(loc, 16);
				byte id = (byte)s.id;
				byte[] towrite = { l, id };
				writer.Write(towrite);
			}
			byte end = 0xFF;
			writer.Write(end);
		}


		/*END
		 * OF
		 * OVERWORLD
		 */
		//INDOOR STUFF
		public long iDataLocation = 0;
		public byte[] iData = new byte[160];
		public List<Tile> iTiles = new List<Tile>();
		public bool iSpecial = false;
		public int lastIRegion = 0;
		public int lastIRoom = 0;
		public int iByteCount = 0;
		public byte iAnimIndex = 0;
		public byte iFloorTile = 0;
		public byte iWallTemplate = 0;
		public bool iHasWarp = false;
		public List<Warp> iWarps = new List<Warp>();
		public List<Sprite> iSprites = new List<Sprite>();
		public long iSpriteLocation = 0;
		public byte iChestValue = 0;
		public Event iRoomEvent = new Event();

		public void readIChest(int room, int region)
		{
			reader.BaseStream.Position = 0x50660 + (region > 6 ? 0x100 : 0) + room;
			iChestValue = reader.ReadByte();
		}

		public void saveIChest(int room, int region)
		{
			writer.BaseStream.Position = 0x50660 + (region > 6 ? 0x100 : 0) + room;
			writer.Write(iChestValue);
		}

		public void saveISprites()
		{
			writer.BaseStream.Position = iSpriteLocation;
			for (int i = 0; i < iSprites.Count; i++)
			{
				if (iSprites[i] == new Sprite() || iSprites[i] == null || iSprites[i].id == 0)
					continue;
				Sprite s = iSprites[i];
				string loc = s.y.ToString("X") + s.x.ToString("X");
				byte l = (byte)Convert.ToInt32(loc, 16);
				byte id = (byte)s.id;
				byte[] towrite = { l, id };
				writer.Write(towrite);
			}
			byte end = 0xFF;
			writer.Write(end);
		}

		public int GetMapOffset(int dungeonId, int mapId)
		{
			if (iSpecial)
			{
				if (dungeonId == 0x1F && mapId == 0xF5)
					return 0xA * 0x4000 + 0x7855 - 0x4000;
			}

			if (dungeonId == 0xFF)
			{
				getOffsetPointer(0x2BB77 + mapId * 2);
			}

			int bank;
			if (dungeonId < 6 || dungeonId >= 0x1A)
				bank = 0xA;
			else
				bank = 0xB;
			return getOffsetPointer(bank * 0x4000 + mapId * 2);
		}

		public void writeIPointer(int region, int room, byte b1, byte b2)
		{
			reader.Close();
			byte[] tw = { b1, b2 };
			writer = new BinaryWriter(File.Open(fname, FileMode.Open));
			int bank;
			if (region < 6 || region >= 0x1A)
				bank = 0xA;
			else
				bank = 0xB;
			
			writer.BaseStream.Position = bank * 0x4000 + (room * 2);
			writer.Write(tw);
			writer.Close();
		}

		public void clearIData()
		{
			iTiles = new List<Tile>();
			iWarps = new List<Warp>();
			iSprites = new List<Sprite>();
			iHasWarp = false;
		}

		public void readIData(int room, int region)
		{
			lastIRegion = region;
			lastIRoom = room;
			int l = GetMapOffset(region, room);
			iDataLocation = l;
			reader.BaseStream.Position = l;
			int i = 0;
			byte b = 0;
			while ((b = reader.ReadByte()) != 0xFE && i < 160)
			{
				iData[i] = b;
				i++;
			}
			iByteCount = i;
			if (i == 160)
			{
				System.Windows.Forms.MessageBox.Show("Too many bytes in room ( > 160)!", "Invalid Room");
			}
		}

		public void readISprites(int region, int room)
		{
			byte b1 = readByte(0x58200 + (room * 2));
			byte b2 = readByte(0x58200 + (room * 2) + 1);
			iSpriteLocation = getPointerAddress(0x58000 / 0x4000, b1, b2);
			reader.BaseStream.Position = iSpriteLocation;
			byte b;
			int ind = 0;
			spriteData = new byte[32];
			while ((b = reader.ReadByte()) != 0xFF)
			{
				spriteData[ind] = b;
				ind++;
			}
			for (int i = 0; i < ind; i++)
			{
				Sprite s = new Sprite();
				string to = spriteData[i].ToString("X");
				if (to.Length == 1)
					to = "0" + to;
				char[] ccc = to.ToCharArray();
				int x = Convert.ToInt32(ccc[1].ToString(), 16);
				int y = Convert.ToInt32(ccc[0].ToString(), 16);
				int id = spriteData[i + 1];
				s.x = x;
				s.y = y;
				s.id = id;
				iSprites.Add(s);
				i++;
			}
		}

		public void writeISpritePointer(int region, int room, byte b1, byte b2)
		{
			writer = new BinaryWriter(File.Open(fname, FileMode.Open));
			if (region > 6)
				room += 0x100;
			writer.BaseStream.Position = 0x58200 + (room * 2);
			byte[] b = { b1, b2 };
			writer.Write(b);
			writer.Close();
		}

		public void formatIndoorData()
		{
			iAnimIndex = iData[0];
			string d = iData[1].ToString("X");
			if (d.Length == 1)
				d = "0" + d;
			char[] chars = d.ToCharArray();
			iFloorTile = (byte)Convert.ToInt32(chars[1].ToString(), 16);
			iWallTemplate = (byte)Convert.ToInt32(chars[0].ToString(), 16);
			for (int i = 2; i < iByteCount; i++)
			{
				byte b = iData[i];
				string to = b.ToString("X");
				if (to.StartsWith("E") && to.Length > 1)
				{
					Warp w = new Warp();
					char[] ccc = to.ToCharArray();
					int mt = Convert.ToInt32(ccc[1].ToString(), 16);
					w.type = Warp.getMapType(mt);
					w.map = iData[i + 1];
					w.room = iData[i + 2];
					w.x = iData[i + 3];
					w.y = iData[i + 4];
					iWarps.Add(w);
					iHasWarp = true;
					i += 4;
					continue;
				}
				if (to.StartsWith("8") && to.Length > 1) //Horizontal 3-byte object
				{
					ThreeByteTile t = new ThreeByteTile();
					t.direction = ThreeByteTile.Direction.Horizontal;
					char[] ccc = to.ToCharArray();
					int l = Convert.ToInt32(ccc[1].ToString(), 16);
					t.length = l;
					char[] cl = iData[i + 1].ToString("X").ToCharArray();
					if (cl.Length > 1)
						t.y = Convert.ToInt32(cl[0].ToString(), 16);
					else
						t.y = 0;
					if (cl.Length > 1)
						t.x = Convert.ToInt32(cl[1].ToString(), 16);
					else
						t.x = Convert.ToInt32(cl[0].ToString(), 16);
					t.id = iData[i + 2];
					Tile tile = new Tile();
					tile.is3Byte = true;
					tile.direction = (Tile.Direction)t.direction;
					tile.x = t.x;
					tile.y = t.y;
					tile.length = t.length;
					tile.id = t.id;
					iTiles.Add(tile);
					i += 2;
					continue;
				}

				if (to.StartsWith("C")) //Vertical 3-byte object
				{
					ThreeByteTile t = new ThreeByteTile();
					t.direction = ThreeByteTile.Direction.Vertical;
					char[] ccc = to.ToCharArray();
					int l = Convert.ToInt32(ccc[1].ToString(), 16);
					t.length = l;
					char[] cl = iData[i + 1].ToString("X").ToCharArray();
					if (cl.Length > 1)
						t.y = Convert.ToInt32(cl[0].ToString(), 16);
					else
						t.y = 0;
					if (cl.Length > 1)
						t.x = Convert.ToInt32(cl[1].ToString(), 16);
					else
						t.x = Convert.ToInt32(cl[0].ToString(), 16);
					t.id = iData[i + 2];
					threeByteTiles.Add(t);
					Tile tile = new Tile();
					tile.is3Byte = true;
					tile.direction = (Tile.Direction)t.direction;
					tile.x = t.x;
					tile.y = t.y;
					tile.length = t.length;
					tile.id = t.id;
					iTiles.Add(tile);
					i += 2;
					continue;
				}

				else //2-byte object
				{
					TwoByteTile t = new TwoByteTile();
					char[] cl = iData[i].ToString("X").ToCharArray();
					if (cl.Length > 1)
					{
						t.x = Convert.ToInt32(cl[1].ToString(), 16);
						t.y = Convert.ToInt32(cl[0].ToString(), 16);
					}
					else
					{
						t.y = 0;
						t.x = Convert.ToInt32(cl[0].ToString(), 16);
					}
					t.id = iData[i + 1];
					twoByteTiles.Add(t);
					Tile tile = new Tile();
					tile.is3Byte = false;
					tile.x = t.x;
					tile.y = t.y;
					tile.id = t.id;
					iTiles.Add(tile);
					i += 1;
				}
			}
		}

		public void saveIRoomData()
		{
			openRom();
			long ending = iDataLocation;
			reader.BaseStream.Position = iDataLocation;
			while (reader.ReadByte() != 0xFE)
				ending++;
			reader.Close();
			writer = new BinaryWriter(File.Open(fname, FileMode.Open));
			writer.BaseStream.Position = iDataLocation;
			string template = iWallTemplate.ToString("X") + iFloorTile.ToString("X");
			byte temp = (byte)Convert.ToInt32(template, 16);
			byte[] ttt = { iAnimIndex, temp };
			writer.Write(ttt);
			for (int i = 0; i < iTiles.Count; i++)
			{
				Tile t = iTiles[i];
				if (t.is3Byte)
				{
					string dbyte;
					if (t.direction == Tile.Direction.Horizontal)
					{
						dbyte = "8";
					}
					else
					{
						dbyte = "C";
					}
					dbyte += t.length.ToString("X");
					int length = Convert.ToInt32(dbyte, 16);
					string lbyte;
					lbyte = t.y.ToString("X") + t.x.ToString("X");
					int location = Convert.ToInt32(lbyte, 16);
					int id = t.id;
					byte[] towrite = { (byte)length, (byte)location, (byte)id };
					writer.Write(towrite);
				}
				else
				{
					string lbyte;
					lbyte = t.y.ToString("X") + t.x.ToString("X");
					int location = Convert.ToInt32(lbyte, 16);
					int id = t.id;
					byte[] towrite = { (byte)location, (byte)id };
					writer.Write(towrite);
				}
			}
			for (int i = 0; i < 1; i++)
			{
				if (!iHasWarp || iWarps.Count == 0)
					break;
				Warp w = iWarps[i];
				byte type = 0;
				if (w.type == Warp.MapType.Dungeon)
					type = 1;
				else if (w.type == Warp.MapType.Side)
					type = 2;
				string t = "E" + type.ToString();
				type = (byte)Convert.ToInt32(t, 16);
				byte map = (byte)w.map;
				byte room = (byte)w.room;
				byte x = (byte)w.x;
				byte y = (byte)w.y;
				byte[] towrite = { type, map, room, x, y };
				writer.Write(towrite);
			}
			writer.Write((byte)0xFE);
			if (writer.BaseStream.Position < ending)
			{
				while (writer.BaseStream.Position != ending + 1)
					writer.Write((byte)0);
			}
			saveISprites();
			saveIChest(lastIRoom, lastIRegion);
			saveIEvent(lastIRegion, lastIRoom);
			writer.Close();
		}

		public void readIEvent(int region, int room)
		{
			long offset = 0x50000 + (region > 6 ? room + 0x100 : room);
			reader.BaseStream.Position = offset;
			Event e = new Event();
			Byte b = reader.ReadByte();
			string to = b.ToString("X");
			if (to.Length == 1)
				to = "0" + to;
			int id = Convert.ToInt32(to.Substring(0, 1), 16);
			int trigger = Convert.ToInt32(to.Substring(1, 1), 16);
			e.id = id;
			e.trigger = trigger;
			iRoomEvent = e;
		}

		public void saveIEvent(int region, int room)
		{
			writer.BaseStream.Position = 0x50000 + (region > 6 ? room + 0x100 : room);
			Event e = iRoomEvent;
			string s = e.id.ToString("X") + e.trigger.ToString("X");
			byte b = (byte)Convert.ToInt32(s, 16);
			writer.Write(b);
		}
	}
}
