using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using GoProject.Nodes;
using GoProject.Sample.Core;
using GoProject.Sample.Models;

namespace GoProject.Sample.Controllers
{
    public class GoApiController : ApiController
    {
        private static string FilePath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GoDiagram.json");

        [HttpPost]
        public IHttpActionResult SaveDiagram([FromBody]Diagram diagram)
        {
            // Save json text on local drives
            var json = JsonConvert.SerializeObject(diagram, Formatting.Indented);
            File.WriteAllText(FilePath, json);
            //
            // Store on Sql Server Database
            if (diagram == null) return InternalServerError(new ArgumentNullException(nameof(diagram)));

            diagram.Name = diagram.Name ?? "TestDiagramName"; // Very important param

            Connections.GoProjectDb.SqlConn.StoreOnDb(diagram, 0);

            return Ok(FilePath);
        }

        public IHttpActionResult GetDiagram()
        {
            var json = File.ReadAllText(FilePath, System.Text.Encoding.UTF8);
            var diagram = JsonConvert.DeserializeObject<Diagram>(json);


            return Ok(diagram);
        }

        public IHttpActionResult GetDiagram(string id)
        {
            //var diagram = Connections.GoProjectDb.SqlConn.LoadFromDb(id);
            var diagram = Connections.CAS.SqlConn.LoadFromCasDb(id);


            return Ok(diagram);
        }

        public IHttpActionResult GetPaletteNodes()
        {
            var diagram = new Diagram
            {
                //TreeNodes = GoHelper.PaletteTreeNodes()
                NodeDataArray = Connections.GoProjectDb.SqlConn.GetPaletteNodesByUserRole(0).ToList()
            };

            return Ok(diagram);
        }

        public IHttpActionResult GetCustomPaletteNodes()
        {
            var parentNode = new ExpenseCenterNode();
            ((IGroupNode) parentNode.Nodes[0]).Nodes = new ObservableCollection<Node>()
            {
                new MaterialNode(),
                new SemiFinishMaterialNode(),
                new EndProductNode(),
                new WorkStationNode()
            };

            var diagram = new Diagram
            {
                TreeNodes = new List<Node>()
                {
                    parentNode
                }
            };
            

            return Ok(diagram);
        }

        public IHttpActionResult GetLocalization()
        {
            var jsonResources = GoProject.Properties.Localization
                .ResourceManager.GetResourceSet(CultureInfo.DefaultThreadCurrentCulture, true, true) 
                .Cast<DictionaryEntry>()
                .ToDictionary(x => x.Key.ToString(),
                         x => x.Value.ToString());
            

            return Ok(jsonResources);
        }
    }
}
