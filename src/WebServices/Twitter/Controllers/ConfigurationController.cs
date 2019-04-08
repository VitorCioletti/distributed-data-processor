namespace Twitter.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    [ApiController, Route("api")]
    public class ConfigurationController : ControllerBase
    {
        [HttpGet, Route("version")]
        public ActionResult<string> Version() =>
            Ok($"v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
    }
}
