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
		string fname = "";
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
				string d = reader.ReadByte().ToString("X");
				string s = reader.ReadByte().ToString("X") + d;
				int total = Convert.ToInt32(s, 16);
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
		{
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
			}
		}

		public void saveEventPos()
		{
			if (romVersion == 1.0)
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
			}
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
			reader.BaseStream.Position = 0x50660 + room;
			chestValue = reader.ReadByte();
		}

		public void readSprites(int level, int room)
		{
			//Pointer first
			reader.BaseStream.Position = 0x58200 + (room * 2);
			spritePointer = reader.ReadBytes(2);
			long offset = getPointerAddress(0x58000 / 0x4000, spritePointer[0], spritePointer[1]);
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
			reader = new BinaryReader(File.Open(fname, FileMode.Open));
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
			if (romVersion == 1.0)
				reader.BaseStream.Position = 0xA479 + (64 * level);
			else if (romVersion == 1.1)
				reader.BaseStream.Position = 0xA49A + (64 * level);
			minimap = reader.ReadBytes(64);
		}

		public void readRealMinimap(int level)
		{
			level--;
			reader.BaseStream.Position = 0x50220 + (64 * level);
			realMinimap = reader.ReadBytes(64);
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
				if (!hasWarp)
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
			if (romVersion == 1.0)
				writer.BaseStream.Position = 0xA479 + (64 * level);
			else if (romVersion == 1.1)
				writer.BaseStream.Position = 0xA49A + (64 * level);
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
	}
}
