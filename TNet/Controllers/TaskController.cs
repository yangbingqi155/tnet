using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TNet.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        public ActionResult MyTask()
        {
            return View();
        }
    }
}