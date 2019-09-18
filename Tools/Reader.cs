using Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tools
{
	public class Reader
	{
		/// <summary>
		/// Read each line and do specifique work to create data
		/// </summary>
		/// <param name="data"></param>
		/// <param name="cptLine"></param>
		/// <returns></returns>
		public async static Task<IEnumerable<BeezUpData>> ReadLinesAndCreateResponse(IEnumerable<string> data, int cptLine)
		{
			// Concat of colums A and B
			string concatAB = "";
			// Value of column C
			int valueC = 0;
			// Value of column D
			int valueD = 0;
			// Bool of parsing succeded colums C and D
			bool successParseValueC = false;
			bool successParseValueD = false;
			// Sum of colums C and D
			int sumCD = 0;
			// ErrrorMessage
			string errorMessage = null;

			ICollection<BeezUpData> returnData = new List<BeezUpData>();

			foreach (string line in data)
			{
				errorMessage = null;
				string[] l = line.Split(";");

				if (l.Length >= 5)
				{
					successParseValueC = int.TryParse(l[3], out valueC);
					successParseValueD = int.TryParse(l[4], out valueD);
					if (successParseValueC && successParseValueD)
					{
						sumCD = valueC + valueD;
						if (sumCD > 100)
							concatAB = string.Concat(l[1], l[2]);
					}
					else if (!successParseValueC || !successParseValueD)
					{
						if (!successParseValueC)
							errorMessage += "Column C connot be convert.";
						if (!successParseValueD)
							errorMessage += "Column D connot be convert.";
					}
				}
				else
					errorMessage = "Line don't have sufficent column : " + (l.Length - 1);

				if (string.IsNullOrEmpty(errorMessage) && string.IsNullOrWhiteSpace(errorMessage))
				{
					if(sumCD > 100)
						returnData.Add(
							new BeezUpData()
							{
								ConcatAB = concatAB,
								LineNumber = cptLine,
								SumCD = sumCD,
								Type = Model.Type.ok
							}
						);
					//Else ignore the line
				}
				else
					returnData.Add(
						new BeezUpData()
						{
							LineNumber = cptLine,
							ErrorMessage = errorMessage,
							Type = Model.Type.error
						}
					);
				cptLine++;
			}
			return returnData;
		}
	}
}
