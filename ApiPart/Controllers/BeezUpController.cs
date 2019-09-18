using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using static Tools.Writer;

namespace ApiPart.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BeezUpController : ControllerBase
	{
		/// <summary>
		/// Get api to filter csv file
		/// </summary>
		/// <param name="csvUri"></param>
		/// <returns></returns>
		[HttpGet("filter")]
		public async Task<IActionResult> Get(string csvUri, int format)
		{
			TypeOfResult type;
			Enum.TryParse(format.ToString(), out type);

			byte[] result = Program.DoFilter(csvUri, type);

			switch (type)
			{
				case TypeOfResult.JSON:
					return File(result, "application/json");
				case TypeOfResult.XML:
					return File(result, "application/xml");
				default:
					return File(result, "application/json");
			}
		}

		/// <summary>
		/// Post api to filter csv file
		/// </summary>
		/// <param name="csvUri"></param>
		/// <returns></returns>
		[HttpPost("filter")]
		public async Task<IActionResult> Post([FromBody] FilterRequestObject request)
		{
			if (request == null)
				return Forbid();

			TypeOfResult type;
			Enum.TryParse(request.ResponseType.ToString(), out type);

			byte[] result = Program.DoFilter(request.csvUri, type);

			switch(type){
				case TypeOfResult.JSON:
					return File(result, "application/json");
				case TypeOfResult.XML:
					return File(result, "application/xml");
				default:
					return File(result, "application/json");
			}
		}
	}
}
