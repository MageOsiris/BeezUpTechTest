using System;
using System.IO;
using System.Linq;
using Tools;
using static Tools.Writer;

namespace ConsoleAppPart
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				System.Console.WriteLine("No argument found. Json result use by default");
				System.Console.WriteLine("(-json for json, -xml for xml)");
			}

			TypeOfResult type = TypeOfResult.JSON;

			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == "-xml")
				{
					type = TypeOfResult.XML;
				}

				else if (args[i] == "-json")
				{
					//Already set by default
					//type = TypeOfResult.JSON
				}
			}

			Console.WriteLine("Start Reading File");
			Console.WriteLine("Enter a specifique path of csv file, if empty it use InFile folder in ConsoleAppPart");

			string pathIn = Console.ReadLine();

			if (string.IsNullOrEmpty(pathIn) || string.IsNullOrWhiteSpace(pathIn))
				pathIn = "../../../InFile/bigfile.csv";

			Console.WriteLine("Found the file result in the OutFile folder in ConsoleAppPart");
			string pathOut = "../../../OutFile/result";

			switch(type)
			{
				case TypeOfResult.JSON:
					pathOut += ".json";
					break;
				case TypeOfResult.XML:
					pathOut += ".xml";
					break;
			}

			try
			{
				Worker.WorkFile(pathIn, pathOut, type);
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
	}
}
