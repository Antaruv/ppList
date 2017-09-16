using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace ppList
{
	class Program
	{
		static void Main(string[] args)
		{
			//Asks for requirements, no kind of input checking so with an incorrect value it just shits itself.	
			Console.Write("Min pp:".PadRight(20));
			var minpp = float.Parse(Console.ReadLine());
			Console.Write("Max pp:".PadRight(20));
			var maxpp = float.Parse(Console.ReadLine());

			Console.Write("Min acc:".PadRight(20));
			var minacc = float.Parse(Console.ReadLine())/100.0f;
			Console.Write("Max acc:".PadRight(20));
			var maxacc = float.Parse(Console.ReadLine())/100.0f;

			Console.Write("Required mods:".PadRight(20));
			var reqmods = convertMod(Console.ReadLine());
			Console.Write("Excluded mods:".PadRight(20));
			var dismods = convertMod(Console.ReadLine());

			DirectoryInfo currDir = new DirectoryInfo(".");
			string playerPath = $"{currDir.FullName}\\test_players.txt";
			string scorePath = $"{currDir.FullName}\\scores.txt";
			
			//Creating a dictionary for all the maps with their respective frequencies to be stored.
			Dictionary<string, int> mapTable = new Dictionary<string, int>();
			string date = "";

			Console.WriteLine("Loading...");
			foreach (string scoreText in File.ReadLines(scorePath))
			{
				if (date == "")
				{
					date = scoreText;					//First line of the file is the date the scores were loaded.
					continue;
				}
				string[] data = scoreText.Split('\t');	//Individual scores are tab-separated. Not all data stored is required here.

				var map_id = data[0];
				var hit300 = Int32.Parse(data[2]);
				var hit100 = Int32.Parse(data[3]);
				var hit50 = Int32.Parse(data[4]);
				var hitmiss = Int32.Parse(data[5]);
				var mods = Int32.Parse(data[7]);
				var pp = float.Parse(data[11]);

				float acc = calcAcc(hit300, hit100, hit50, hitmiss);

				if ((mods & reqmods) == reqmods && (mods & dismods) == 0 &&		//Checks bitwise if the mod requirements fit.
					acc > minacc && acc < maxacc &&
					pp > minpp && pp < maxpp)
				{
					if (mapTable.ContainsKey(data[0]))
					{
						mapTable[data[0]]++;		//Increments map frequency by 1 if the map is found in the dictionary.
					}
					else
					{
						mapTable[data[0]] = 1;		//Adds map to the dictionary using its ID as a key and sets its value to 1.
					}
				}
			}
			var sorted = mapTable.OrderByDescending(pair => pair.Value);		//Gets sorted enumerable of map frequencies.

			foreach (var map in sorted)
			{
				Console.WriteLine($"{map.Key}".PadRight(12) + $"{map.Value}");	//Outputs the map ID followed by its frequency.
			}
			//TODO?: Maybe write it to a file for now? Not like this is supposed to produce the final product yet.
			Console.ReadLine();
		}

		static float calcAcc(int h300, int h100, int h50, int hmiss)	//Calculates accuracy given 300s, 100s, 50s and misses.
		{
			return (h300 * 300.0f + h100 * 100.0f + h50 * 50.0f) / ((h300 + h100 + h50 + hmiss) * 300.0f); 
		}

		static string convertMod(int mods) //Converts the integer representation of a set of mods to its respective mod combination in standard osu form.
		{
			if (mods == 0)
			{
				return "NoMod";
			}
			var test = ((Mods)mods).ToString();
			test.Replace(", ", "");
			return ((Mods)mods).ToString();
		}

		static int convertMod(string mods) //Converts a string denoting mods in the standard osu form to its integer representation.
		{
			if (mods.ToLower() == "nomod")
			{
				return 0;
			}
			mods = Regex.Replace(mods, ".{2}(?!$)", "$0,");
			return (int)Enum.Parse(typeof(Mods), mods, true);
		}

		[Flags]
		enum Mods //Used for string<=>integer conversion for mods. Does not include some mods that should not appear in online scores.
		{
			NoFail = 1,
			NF = 1,

			Easy = 2,
			EZ = 2,

			NoVideo = 4, // Not used anymore but can show up on older plays
			NV = 4,

			Hidden = 8,
			HD = 8,

			HardRock = 16,
			HR = 16,

			SuddenDeath = 32,
			SD = 32,

			DoubleTime = 64,
			DT = 64,

			Relax = 128,
			RX = 128,

			HalfTime = 256,
			HT = 256,

			Nightcore = 512,
			NC = 512,

			Flashlight = 1024,
			FL = 1024,

			Autoplay = 2048, // Should never show up here
			AU = 2048,

			SpunOut = 4096,
			SO = 4096,

			Relax2 = 8192,  // Autopilot, should never show up here
			AP = 8192,

			Perfect = 16384, // Only set along with SuddenDeath. i.e: PF only gives 16416  
			PF = 16384

		}
	}
}
