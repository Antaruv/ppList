using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ppList
{
    class Mods
    {

		public static string convertMod(int mods) //Converts the integer representation of a set of mods to its respective mod combination in standard osu form.
		{
			if (mods == 0)
			{
				return "NoMod";
			}
			var test = ((Mods.Enums)mods).ToString();
			test.Replace(", ", "");
			return ((Mods.Enums)mods).ToString();
		}

		public static int convertMod(string mods) //Converts a string denoting mods in the standard osu form to its integer representation.
		{
			if (mods.ToLower() == "nomod")
			{
				return 0;
			}
			mods = Regex.Replace(mods, ".{2}(?!$)", "$0,");
			return (int)Enum.Parse(typeof(Mods.Enums), mods, true);
		}

		public static bool tryConvert(string mods, out int ans)	//Tries to convert string to mods in integer form. Returns bool indicating success.
																//ans is 0 by default.
		{
			ans = 0;
			try
			{
				ans = convertMod(mods);
				return true;
			}
			catch
			{
				return false;
			}
		}

		[Flags]
		public enum Enums //Used for string<=>integer conversion for mods. Does not include some mods that should not appear in online scores.
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
