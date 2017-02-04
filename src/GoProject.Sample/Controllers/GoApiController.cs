using System;
using System.Web.Http;

namespace GoProject.Sample.Controllers
{
    public class GoApiController : ApiController
    {
        private static Diagram Buffer { get; set; }

        [HttpPost]
        public IHttpActionResult SaveDiagram([FromBody]Diagram diagram)
        {
            Buffer = diagram;

            return Ok(diagram);
        }

        public IHttpActionResult GetDiagram()
        {
            return Ok(Buffer);
        }
    }
}
