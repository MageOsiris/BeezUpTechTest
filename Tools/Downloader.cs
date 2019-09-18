using System.Net;

namespace Tools
{
	public class Downloader
	{
		/// <summary>
		/// Download a file by uri
		/// </summary>
		/// <param name="url"></param>
		/// <param name="pathDownload"></param>
		public static void DownloadCsvFile(string url, string pathDownload)
		{
			using (System.Net.WebClient client = new WebClient())
				client.DownloadFile(url, pathDownload);
		}
	}
}
