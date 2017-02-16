using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace GoProject.Sample.Controllers
{
    public class GoApiController : ApiController
    {
        private static string FilePath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GoDiagram.json");

        [HttpPost]
        public IHttpActionResult SaveDiagram([FromBody]Diagram diagram)
        {
            Task.Run(async () =>
            {
                var json = await JsonConvert.SerializeObjectAsync(diagram, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            });

            return Ok(FilePath);
        }

        public IHttpActionResult GetDiagram()
        {
            var json = File.ReadAllText(FilePath, System.Text.Encoding.UTF8);
            var diagram = JsonConvert.DeserializeObject<Diagram>(json);

            //var diagram = new Diagram();
            //diagram.NodeDataArray = Node.PaletteNodes();

            return Ok(diagram);
        }
    }
}
