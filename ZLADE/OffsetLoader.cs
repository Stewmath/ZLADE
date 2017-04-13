using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ZLADE
{
	public class OffsetLoader
	{
		public static List<LoadedOffset> loadedOffsets = new List<LoadedOffset>();
		public static LoadedOffset activeOffset = new LoadedOffset();
		public static bool loadOffsets()
		{
			try
			{
				StreamReader s = new StreamReader(System.Windows.Forms.Application.StartupPath + "/VersionOffsets.txt");
				string[] lines = s.ReadToEnd().Replace("\r", "").Split('\n');
				s.Close();
				LoadedOffset current = new LoadedOffset();
				for (int i = 0; i < lines.Length; i++)
				{
					if(lines[i] == null || lines[i] == "")
						continue;
					string l = lines[i];
					string[] values = l.Split('=');
					string key = values[0];
					string value = "";
					if (values.Length > 1)
						value = values[1];
					int ivalue = 0;
					try
					{
						ivalue = Convert.ToInt32(value, 16);
					}
					catch (Exception) { }
					
					switch (key)
					{
						case "dump":
							current = new LoadedOffset();
							current.name = value;
							break;
						case "chestx":
							current.chestX = ivalue;
							break;
						case "chestY":
							current.chestY = ivalue;
							break;
						case "stairsx":
							current.stairsX = ivalue;
							break;
						case "stairsy":
							current.stairsY = ivalue;
							break;
						case "keyx":
							current.keyX = ivalue;
							break;
						case "keyy":
							current.keyY = ivalue;
							break;
						case "chestpoofx":
							current.chestPoofX = ivalue;
							break;
						case "chestpoofy":
							current.chestPoofY = ivalue;
							break;
						case "ominimaptile":
							current.oMinimapTile = ivalue;
							break;
						case "ominimappal":
							current.oMinimapPal = ivalue;
							break;
						case "dminimap":
							current.dMinimap = ivalue;
							break;
						case "droomindex":
							current.dRoomIndexes = ivalue;
							break;
						case "tileanim":
							current.tileAnim = ivalue;
							break;
						case "tile1loc":
							current.tile1Loc = ivalue;
							break;
						case "itile1loc":
							current.iTileLoc = ivalue;
							break;
						case "itile2loc":
							current.iTileLoc2 = ivalue;
							break;
						case "end":
							loadedOffsets.Add(current);
							break;
					}
				}

				activeOffset = loadedOffsets[0];
			}
			catch (IOException e)
			{
				msgbox("Error loading ROM addresses.\n\n" + e.Message, "Error");
				return false;
			}
			return true;
		}

		public static void msgbox(string text, string caption)
		{
			System.Windows.Forms.MessageBox.Show(text, caption);
		}
	}
}
