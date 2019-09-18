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
			Console.WriteLine("Enter a specifique path of csv file, if empty it use InFile folder in ConsoleAppPart");

			string pathIn = Console.ReadLine();

			if (string.IsNullOrEmpty(pathIn) || string.IsNullOrWhiteSpace(pathIn))
				pathIn = "../../../InFile/bigfile.csv";

			Console.WriteLine("Found the json file result in the OutFile folder in ConsoleAppPart");
			string pathOut = "../../../OutFile/result.json";

			try
			{
				WorkFile(pathIn, pathOut);
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
		/// Proceed a csv file and write create a result json file
		/// </summary>
		/// <param name="info"></param>
		/// <param name="maxline"></param>
		public static async void WorkFile(string pathIn, string pathOut, int maxline = 1000000)
		{
			// Get the file
			var lines = File.ReadLines(pathIn);
			string directoryPath = Path.GetDirectoryName(pathIn);
			string filenameok = Path.GetFileNameWithoutExtension(pathIn);

			// Get number of lines of the file
			var linesNumber = lines.Count();
			Console.WriteLine("Number of lines :" + linesNumber);

			// Get number of occurence
			int numberOfOccurence = (linesNumber / maxline);
			// Console.WriteLine("Number of occurences :" + numberOfOccurence);

			// Prepare loop
			int cptOccurence = 0;
			int cptLine = 0;
			int lastLines = linesNumber - ((numberOfOccurence - 1) * maxline);

			//Delete file if exist
			Writer.DeleteFileIfExist(pathOut);

			// Begin the array of json
			Writer.WriteChar(pathOut, '[');

			bool writeInLastOccurence = false;

			// Loop and call specifique method by batch of data/lines
			while (cptOccurence <= numberOfOccurence && cptLine < linesNumber)
			{
				cptLine = cptOccurence * maxline;

				if (cptOccurence == numberOfOccurence)
					writeInLastOccurence = Writer.WriteJsonData(pathOut, await Reader.ReadLinesAndCreateResponse(lines.Skip(cptLine).Take(lastLines), cptLine), writeInLastOccurence);
				else
					writeInLastOccurence = Writer.WriteJsonData(pathOut, await Reader.ReadLinesAndCreateResponse(lines.Skip(cptLine).Take(maxline), cptLine), writeInLastOccurence);
				cptOccurence++;
			}

			// Close the array of json
			Writer.WriteChar(pathOut, ']');
		}
	}
}
