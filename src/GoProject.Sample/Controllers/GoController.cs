using System;
using System.Web.Mvc;

namespace GoProject.Sample.Controllers
{
    public class GoController : Controller
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

        public ActionResult BpmnEditorSample()
        {
            ViewBag.Title = "BpmnEditorSample";
            return View();
        }

        public ActionResult PipesSample()
        {
            ViewBag.Title = "PipesSample";
            return View();
        }


    }
}