namespace Twitter.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Integrations;

    [ApiController, Route("api")]
    public class AnalysisController : ControllerBase
    {
        [HttpGet, Route("analysis")]
        public ActionResult<string> Analysis(string date, string subject)
        {
            var analysis = Persistence.GetAnalysis(date, subject);

            if (analysis == null)
                return NotFound("Analysis not found. Check the date and subject.");

            return Ok(analysis);
        }
    }
}
