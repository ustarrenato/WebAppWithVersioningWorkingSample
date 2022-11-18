using Microsoft.AspNetCore.Mvc;

namespace WebAppWithVersioning
{
    [ApiController]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    [Route("v{version:ApiVersion}/Names")]
    public class NamesController : Controller
    {
        [HttpGet]
        [MapToApiVersion("3.0")]
        [Route("All")]
        public IActionResult GetNamesV1() => Ok(new List<string>{"a", "b", "c"});

        [HttpGet]
        [MapToApiVersion("2.0")]
        [Route("All")]
        public IActionResult GetNamesV2() => Ok(new List<string> { "a", "b", "c", "d" });
    }
}
