using System;
using System.Collections.Generic;
using System.Text;

namespace ppList
{
    class Score
    {
		//I really feel like there should be an easier way here.
		public string map_id { get; }
		public int combo { get; }
		public int hit300 { get; }
		public int hit100 { get; }
		public int hit50 { get; }
		public int hitmiss { get; }
		public bool perfect { get; }
		public int mods { get; }
		public string player_id { get; }
		public string date { get; }
		public string rank { get; }
		public float pp { get; }
		public float acc { get; }


		public Score(string line) //Constructs score object out of line following the format of scores.txt.
		{
			string[] data = line.Split('\t');

			map_id = data[0];
			combo = Int32.Parse(data[1]);
			hit300 = Int32.Parse(data[2]);
			hit100 = Int32.Parse(data[3]);
			hit50 = Int32.Parse(data[4]);
			hitmiss = Int32.Parse(data[5]);
			perfect = data[6] == "1";
			mods = Int32.Parse(data[7]);
			player_id = data[8];
			date = data[9];
			rank = data[10];
			pp = float.Parse(data[11]);
			acc = Program.calcAcc(hit300, hit100, hit50, hitmiss);
		}

    }
}
