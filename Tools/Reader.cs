using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tools
{
	public class Reader
	{
		/// <summary>
		/// Read each line and do specifique work on value on column to render result
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public async static Task ReadAndConsole(IEnumerable<string> data)
		{
			// Value of column C
			int valueC = 0;
			// Value of column D
			int valueD = 0;
			// Bool of parsing succeded colums C and D
			bool successParseValueC = false;
			bool successParseValueD = false;

			foreach (string line in data)
			{
				string[] l = line.Split(";");
				if (l.Length >= 5)
				{
					successParseValueC = int.TryParse(l[3], out valueC);
					successParseValueD = int.TryParse(l[4], out valueD);
					if (successParseValueC && successParseValueD && (valueC + valueD) > 100)
						Console.WriteLine(string.Concat(l[1], l[2]));
				}
			}
			return;
		}

		// Just for information
		// In the beggining i wanted split the file beacause ReadAllLines take too much memory.
		// But i skip it, and i just use ReadLines who uses a lot less memory
		//public async static Task ReadAndParse(IEnumerable<string> data, string path, string prefixeFileName, int idFile)
		//{
		//	StreamWriter ffs = new StreamWriter(path + "\\" + prefixeFileName + idFile + ".csv");
		//	foreach (string line in data)
		//	{
		//		ffs.WriteLine(line);
		//	}
		//	ffs.Close();
		//	ffs.Dispose();
		//	return;
		//}
	}
}
