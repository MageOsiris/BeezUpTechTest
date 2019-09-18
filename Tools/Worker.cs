using System;
using System.IO;
using System.Linq;
using static Tools.Writer;

namespace Tools
{
	public class Worker
	{
		/// <summary>
		/// Proceed a csv file and write create a result file
		/// </summary>
		/// <param name="info"></param>
		/// <param name="maxline"></param>
		public static async void WorkFile(string pathIn, string pathOut, TypeOfResult type, int maxline = 1000000)
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

			if (type == TypeOfResult.JSON)
				// Begin the array of json
				Writer.WriteChar(pathOut, '[');

			bool writeInLastOccurence = false;

			// Loop and call specifique method by batch of data/lines
			while (cptOccurence <= numberOfOccurence && cptLine < linesNumber)
			{
				cptLine = cptOccurence * maxline;

				if (cptOccurence == numberOfOccurence)
					writeInLastOccurence = Writer.WriteData(pathOut, await Reader.ReadLinesAndCreateResponse(lines.Skip(cptLine).Take(lastLines), cptLine), writeInLastOccurence, type);
				else
					writeInLastOccurence = Writer.WriteData(pathOut, await Reader.ReadLinesAndCreateResponse(lines.Skip(cptLine).Take(maxline), cptLine), writeInLastOccurence, type);
				cptOccurence++;
			}

			if (type == TypeOfResult.JSON)
				// Close the array of json
				Writer.WriteChar(pathOut, ']');
		}
	}
}