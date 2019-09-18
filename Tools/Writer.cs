using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tools
{
	public class Writer
	{
		/// <summary>
		/// Delete a file on a specifique path if exist
		/// </summary>
		/// <param name="path"></param>
		public static void DeleteFileIfExist(string path)
		{
			if (File.Exists(path))
				File.Delete(path);
			return;
		}

		/// <summary>
		/// Write a char on file (create file if don't exist)
		/// </summary>
		/// <param name="path"></param>
		/// <param name="c"></param>
		public static void WriteChar(string path, char c)
		{
			if (File.Exists(path))
				using (StreamWriter sw = File.AppendText(path))
					sw.Write(c);
			else
				using (StreamWriter sw = new StreamWriter(path))
					sw.Write(c);
			return;
		}

		/// <summary>
		/// Write a string on file (create file if don't exist)
		/// </summary>
		/// <param name="path"></param>
		/// <param name="s"></param>
		public static void WriteString(string path, string s)
		{
			if (File.Exists(path))
				using (StreamWriter sw = File.AppendText(path))
					sw.Write(s);
			else
				using (StreamWriter sw = new StreamWriter(path))
					sw.Write(s);
			return;
		}

		/// <summary>
		/// Convert a enumation of BeezUpData to json format
		/// </summary>
		/// <param name="path"></param>
		/// <param name="data"></param>
		/// <param name="haveAlreadyData"></param>
		/// <returns></returns>
		public static bool WriteJsonData(string path, IEnumerable<BeezUpData> data, bool haveAlreadyData)
		{
			// If there are no data return false but if the last occurence have data return true
			// because if occurence 1 have data, occurence 2 not. I need for the occurence 3 this information (see after)
			if (data == null || data.Count() == 0)
				return haveAlreadyData ? true : false;

			using (StringWriter sw = new StringWriter())
			using (JsonTextWriter writer = new JsonTextWriter(sw))
			{
				// If on occurence write data, we need seperate them
				if (haveAlreadyData)
					writer.WriteValue(',');

				// Use after to detect if it's the last data of the current occurence
				BeezUpData lastobj = data.Last();

				foreach (BeezUpData beezUpData in data)
				{
					// Manual serialize

					writer.WriteStartObject();

					// Commun data
					writer.WritePropertyName("lineNumber");
					writer.WriteValue(beezUpData.LineNumber);
					writer.WritePropertyName("type");
					writer.WriteValue(Enum.GetName(typeof(Model.Type), beezUpData.Type));

					switch (beezUpData.Type)
					{
						// Specifique error part
						case Model.Type.error:
							writer.WritePropertyName("errorMessage");
							writer.WriteValue(beezUpData.ErrorMessage);
							break;
						// Specifique ok type
						case Model.Type.ok:
							writer.WritePropertyName("concatAB");
							writer.WriteValue(beezUpData.ConcatAB);
							writer.WritePropertyName("sumCD");
							writer.WriteValue(beezUpData.SumCD);
							break;
					}
					writer.WriteEndObject();

					// Seperation of data
					if (lastobj.LineNumber != beezUpData.LineNumber)
						writer.WriteValue(',');
				}

				// Call the method to write in file
				WriteString(path, sw.ToString());
			}

			return true;
		}

		// I'm beggin to use the JsonSerializer package but in this link, it's more performant to serialize manualy : https://www.newtonsoft.com/json/help/html/Performance.htm
		//public static void WriteJsonData(string path, IEnumerable<BeezUpData> data)
		//{
		//	JsonSerializer serializer = new JsonSerializer();
		//	serializer.Converters.Add(new JavaScriptDateTimeConverter());
		//	serializer.NullValueHandling = NullValueHandling.Ignore;

		//	if (File.Exists(path))
		//		using (StreamWriter sw = File.AppendText(path))
		//		using (JsonWriter writer = new JsonTextWriter(sw))
		//		{
		//			serializer.Serialize(writer, data);
		//		}
		//	else
		//		using (StreamWriter sw = new StreamWriter(path))
		//		using (JsonWriter writer = new JsonTextWriter(sw))
		//		{
		//			serializer.Serialize(writer, data);
		//		}

		//	return;
		//}
	}
}
