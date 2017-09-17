using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ppList
{
	class Program
	{

		static void Main(string[] args)
		{
			//Asks for requirements, now with added input checking so you can't simply break it.
			//I'm sure there's still some way to break it, but beats me!

			var a = float.TryParse("1.fssdfasfsd", out float ans);

			float minpp = promptFloat("Min pp:");
			float maxpp = promptFloat("Max pp:");

			float minacc = promptFloat("Min acc:")/100.0f;
			float maxacc = promptFloat("Max acc:")/100.0f;

			int reqmods = promptMod("Required mods:");
			int dismods = promptMod("Excluded mods:");

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

				Score curScore = new Score(scoreText);

				if ((curScore.mods & reqmods) == reqmods && (curScore.mods & dismods) == 0 &&       //Checks bitwise if the mod requirements fit.
					curScore.acc > minacc && curScore.acc < maxacc &&
					curScore.pp > minpp && curScore.pp < maxpp)
				{
					if (mapTable.ContainsKey(curScore.map_id))
					{
						mapTable[curScore.map_id]++;		//Increments map frequency by 1 if the map is found in the dictionary.
					}
					else
					{
						mapTable[curScore.map_id] = 1;		//Adds map to the dictionary using its ID as a key and sets its value to 1.
					}
				}
			}
			var sorted = mapTable.OrderByDescending(pair => pair.Value);		//Gets sorted enumerable of map frequencies.

			foreach (var map in sorted)
			{
				Console.WriteLine($"{map.Key}".PadRight(12) + $"{map.Value}");	//Outputs the map ID followed by its frequency.
			}
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Finished!");

			//TODO?: Maybe write it to a file for now? Not like this is supposed to produce the final product yet.
			Console.ReadLine();
		}

		public static float calcAcc(int h300, int h100, int h50, int hmiss)	//Calculates accuracy given 300s, 100s, 50s and misses.
		{
			return (h300 * 300.0f + h100 * 100.0f + h50 * 50.0f) / ((h300 + h100 + h50 + hmiss) * 300.0f); 
		}

		public static float promptFloat(string askText) //Asks for a float using the askText parameter as the prompt. Retries until a valid float is given.
		{
			
			Console.Write(askText.PadRight(20));
			if (float.TryParse(Console.ReadLine(), out var ans))
			{
				return ans;
			} else
			{
				clearLine(Console.CursorTop-1);
				promptFloat(askText);
			}
			return 1.0f;
		}

		public static int promptMod(string askText) //Just like promptFloat, but for prompting the mods. Having both of these feels wrong but I don't know how not to!
		{

			Console.Write(askText.PadRight(20));
			if (Mods.tryConvert(Console.ReadLine(), out var ans))
			{
				return ans;
			}
			else
			{
				clearLine(Console.CursorTop - 1);
				promptMod(askText);
			}
			return 1;
		}


		public static void clearLine(int top) //Clears line indicated by top
		{
			Console.SetCursorPosition(0, top);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, top);
		}

		public static void clearLine() => clearLine(Console.CursorTop); //Clears current line
	}
}
