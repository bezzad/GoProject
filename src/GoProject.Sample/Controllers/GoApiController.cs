using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using GoProject.Nodes;
using GoProject.Sample.Models;

namespace GoProject.Sample.Controllers
{
    public class GoApiController : ApiController
    {
        private static string FilePath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GoDiagram.json");

        [HttpPost]
        public IHttpActionResult SaveDiagram([FromBody]Diagram diagram)
        {
            var json = JsonConvert.SerializeObject(diagram, Formatting.Indented);
            File.WriteAllText(FilePath, json);

            return Ok(FilePath);
        }

        public IHttpActionResult GetDiagram()
        {
            var json = File.ReadAllText(FilePath, System.Text.Encoding.UTF8);
            var diagram = JsonConvert.DeserializeObject<Diagram>(json);


            return Ok(diagram);
        }

        public IHttpActionResult GetPaletteNodes()
        {
            var diagram = new Diagram
            {
                NodeDataArray = GoHelper.PaletteNodes()
            };

            return Ok(diagram);
        }

        public IHttpActionResult GetCustomPaletteNodes()
        {
            var diagram = new Diagram
            {
                NodeDataArray = new List<Node>()
                {
                    new MaterialNode(),
                    new SemiFinishMaterialNode(),
                    new WorkStationNode(),
                    new ExpenseCenterNode()
                }
            };

            return Ok(diagram);
        }
    }
}
