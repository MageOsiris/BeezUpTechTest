using Newtonsoft.Json;
using System;
using System.IO;

namespace Model
{
	public class BeezUpData
	{
		public int LineNumber;
		public Type Type;
		public string ConcatAB;
		public int SumCD;
		public string ErrorMessage;

		public BeezUpData()
		{
		}
	}

	public enum Type
	{
		ok = 0,
		error = 1
	}
}