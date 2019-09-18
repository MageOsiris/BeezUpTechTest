using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Tools
{
	public class Writer
	{
		public enum TypeOfResult{
			JSON = 0,
			XML = 1
		}

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
		/// Convert a enumation of BeezUpData to a specifique format
		/// </summary>
		/// <param name="path"></param>
		/// <param name="data"></param>
		/// <param name="haveAlreadyData"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool WriteData(string path, IEnumerable<BeezUpData> data, bool haveAlreadyData, TypeOfResult type){
			switch(type)
			{
				case TypeOfResult.JSON:
					return WriteJsonData(path, data, haveAlreadyData);
				case TypeOfResult.XML:
					WriteXmlData(path, data);
					return false;
				default:
					return false;

			}
		}

		/// <summary>
		/// Convert a enumation of BeezUpData to json format
		/// </summary>
		/// <param name="path"></param>
		/// <param name="data"></param>
		/// <param name="haveAlreadyData"></param>
		/// <returns></returns>
		private static bool WriteJsonData(string path, IEnumerable<BeezUpData> data, bool haveAlreadyData)
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
					sw.Write(',');

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
						sw.Write(',');
				}

				// Call the method to write in file
				WriteString(path, sw.ToString());
			}

			return true;
		}

		/// <summary>
		/// Convert a enumation of BeezUpData to xml format
		/// </summary>
		/// <param name="path"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		private static void WriteXmlData(string path, IEnumerable<BeezUpData> data)
		{
			if (!File.Exists(path))
			{
				//Specifique code for existing file
				using (XmlWriter xmlWriter = XmlWriter.Create(path))
				{
					// Manual serialize
					xmlWriter.WriteStartDocument();
					xmlWriter.WriteStartElement("BeezUpData");

					foreach (BeezUpData beezUpData in data)
					{
						xmlWriter.WriteStartElement("Data");

						// Commun data
						xmlWriter.WriteElementString("lineNumber", beezUpData.LineNumber.ToString());
						xmlWriter.WriteElementString("type", Enum.GetName(typeof(Model.Type), beezUpData.Type));

						switch (beezUpData.Type)
						{
							// Specifique error part
							case Model.Type.error:
								xmlWriter.WriteElementString("errorMessage", beezUpData.ErrorMessage);
								break;
							// Specifique ok type
							case Model.Type.ok:
								xmlWriter.WriteElementString("concatAB", beezUpData.ConcatAB);
								xmlWriter.WriteElementString("sumCD", beezUpData.SumCD.ToString());
								break;
						}
						xmlWriter.WriteEndElement();
					}

					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndDocument();
					xmlWriter.Flush();
					xmlWriter.Close();
				}
			}
			else
			{
				//Specifique code for not existing file

				XDocument xDocument = XDocument.Load(path);
				XElement root = xDocument.Element("BeezUpData");
				IEnumerable<XElement> rows = root.Descendants("Data");
				XElement firstRow = rows.First();

				List<XElement> rowToAdd = new List<XElement>();

				foreach (BeezUpData beezUpData in data)
				{
					rowToAdd = new List<XElement>();

					// Commun data
					rowToAdd.Add(new XElement("LineNumber", beezUpData.LineNumber.ToString()));
					rowToAdd.Add(new XElement("Type", Enum.GetName(typeof(Model.Type), beezUpData.Type)));

					switch (beezUpData.Type)
					{
						// Specifique error part
						case Model.Type.error:
							rowToAdd.Add(new XElement("ErrorMessage", beezUpData.ErrorMessage));
							break;
						// Specifique ok type
						case Model.Type.ok:
							rowToAdd.Add(new XElement("ConcatAB", beezUpData.ConcatAB));
							rowToAdd.Add(new XElement("SumCD", beezUpData.SumCD.ToString()));
							break;
					}

					firstRow.AddBeforeSelf(rowToAdd);
				}

				xDocument.Save(path);
			}
		}
	}
}
