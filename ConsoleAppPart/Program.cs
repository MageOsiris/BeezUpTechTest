using System;
using System.IO;
using System.Linq;
using Tools;

namespace ConsoleAppPart
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Start Reading File");
			Console.WriteLine("Enter a specifique path of csv file. If empty it use /InFile/bigfile.csv in ConsoleAppPart");

			string pathIn = Console.ReadLine();

			if (string.IsNullOrEmpty(pathIn) || string.IsNullOrWhiteSpace(pathIn))
				pathIn = "../../../InFile/bigfile.csv";

			try
			{
				WorkFile(pathIn);
				Console.WriteLine();
				Console.WriteLine("Reading finish");
			}
			catch (Exception ex)
			{
				Console.WriteLine();
				Console.WriteLine(ex.InnerException);
				Console.WriteLine(ex.Message);
			}
			finally
			{
				Console.WriteLine();
				Console.WriteLine("Please enter to quit");
				Console.ReadLine();
			}
		}

		/// <summary>
		/// Method do the work for the csv file.
		/// I use 1 000 000 for the maxline (i test it with 80 mo of csv and it's really performant)
		/// </summary>
		/// <param name="info"></param>
		/// <param name="maxline"></param>
		public static async void WorkFile(string info, int maxline = 1000000)
		{
			// Get the file
			var lines = File.ReadLines(info);
			string directoryPath = Path.GetDirectoryName(info);
			string filenameok = Path.GetFileNameWithoutExtension(info);

			// Get number of lines of the file
			var linesNumber = lines.Count();
			Console.WriteLine("Number of lines :" + linesNumber);

			// Get number of occurence
			int numberOfOccurence = (linesNumber / maxline);
			// Console.WriteLine("Number of occurences :" + numberOfOccurence);

			// Prepare loop
			int cptOccurence = 0;
			int lastLines = linesNumber - ((numberOfOccurence) * maxline);

			// Loop and call specifique method by batch of data/lines
			while (cptOccurence <= numberOfOccurence)
			{
				if (cptOccurence == numberOfOccurence)
					await Reader.ReadAndConsole(lines.Skip(cptOccurence * maxline).Take(lastLines));
				else
					await Reader.ReadAndConsole(lines.Skip(cptOccurence * maxline).Take(maxline));
				cptOccurence++;
			}
		}
	}
}
