using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Tools;
using static Tools.Writer;

namespace ApiPart
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();

		/// <summary>
		/// Start job and keep tempory file to delete after getting result of the filtering 
		/// </summary>
		/// <param name="csvUri"></param>
		/// <returns></returns>
		public static byte[] DoFilter(string csvUri, TypeOfResult type = TypeOfResult.JSON)
		{

			// Create unique id for tempory file
			Guid uniqueId = Guid.NewGuid();
			// Keep the tempory file to delete later
			string pathIn = "./InFile/" + uniqueId.ToString() + ".csv";
			string pathOut = "./OutFile/" + uniqueId.ToString();

			// use the good extension
			switch (type)
			{
				case TypeOfResult.JSON:
					pathOut += ".json";
					break;
				case TypeOfResult.XML:
					pathOut += ".xml";
					break;
			}

			// Get the csv file by uri
			Downloader.DownloadCsvFile(csvUri, pathIn);

			// Do the work on csv file
			Worker.WorkFile(pathIn, pathOut, type);

			//Get result
			byte[] data = File.ReadAllBytes(pathOut);

			// Delete temporry file
			Writer.DeleteFileIfExist(pathIn);
			Writer.DeleteFileIfExist(pathOut);

			return data;
		}
	}
}
