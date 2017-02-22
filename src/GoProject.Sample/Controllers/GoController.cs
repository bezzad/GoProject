using System;
using System.Web.Mvc;
using Dapper;
using GoProject.Sample.Core;

namespace GoProject.Sample.Controllers
{
    public class GoController : BaseController
    {
        
        public ActionResult PageFlowSample()
        {
            ViewBag.Title = "PageFlowSample";
            return View();
        }
        
        public ActionResult PlanogramSample()
        {
            ViewBag.Title = "PlanogramSample";
            return View();
        }

        public ActionResult DoubleTreeSample()
        {
            ViewBag.Title = "DoubleTreeSample";
            return View();
        }


        public ActionResult PipesSample()
        {
            ViewBag.Title = "PipesSample";
            return View();
        }

        public ActionResult BpmnEditorSample()
        {
            ViewBag.Title = "BpmnEditorSample";
            return View();
        }

        [HttpPost]
        public ActionResult BpmnEditorSample(string id)
        {
            ViewBag.Title = "BpmnEditorSample";
            var model = Connections.GoProjectDb.SqlConn.LoadFromDb(id);
            return View(model);
        }

        public ActionResult GoDiagramsTablePartial()
        {
            var model = Connections.GoProjectDb.SqlConn.Query<Diagram>("Select * From Diagrams");

            return PartialView("_GoDiagramsTablePartial", model);
        }



    }
}