using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
		public async Task<IActionResult> Get(string csvUri)
		{
			//return json file
			return File(Program.DoFilter(csvUri), "application/json");
		}

		/// <summary>
		/// Post api to filter csv file
		/// </summary>
		/// <param name="csvUri"></param>
		/// <returns></returns>
		[HttpPost("filter")]
		public async Task<IActionResult> Post([FromBody] string csvUri)
		{
			//return json file
			return File(Program.DoFilter(csvUri), "application/json"); ;
		}
	}
}
