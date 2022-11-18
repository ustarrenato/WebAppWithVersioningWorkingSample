using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebAppWithVersioning
{
    [ApiController]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    [Route("v{version:apiVersion}/Names")]
    public class NamesController : Controller
    {
        [HttpGet("All")]
        [MapToApiVersion("3.0")]
        public IActionResult GetNamesV1() => Ok(new List<string>{"a", "b", "c"});

        [HttpGet("All")]
        [MapToApiVersion("2.0")]
        public IActionResult GetNamesV2() => Ok(new List<string> { "a", "b", "c", "d" });
    }
}
