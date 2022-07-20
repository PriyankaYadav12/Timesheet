using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTimeSheetManagement.Concrete;
using WebTimeSheetManagement.Filters;
using WebTimeSheetManagement.Interface;
using WebTimeSheetManagement.Models;
using Newtonsoft.Json;


namespace WebTimeSheetManagement.Controllers
{
    [ValidateUserSession]
    public class MonthlyTimeSheetController : Controller
    {
        IProject _IProject;
        ITimeSheet _ITimeSheet;
        IUsers _IUsers;
        public MonthlyTimeSheetController()
        {
            _IProject = new ProjectConcrete();
            _ITimeSheet = new TimeSheetConcrete();
            _IUsers = new UsersConcrete();
        }
        // GET: MonthlyTimeSheet
        public ActionResult Index()
        {
            string _intregistrationid = Convert.ToString(Session["RegistrationID"]);
            //var listofProjects = _IProject.GetListofProjects();
            var listofProjects = _IProject.GetListofProjectsByRegistrationID(Convert.ToInt32(_intregistrationid));
            string json = JsonConvert.SerializeObject(listofProjects);
            //ViewBag.ProjectNameDropDown = Json(listofProjects, JsonRequestBehavior.AllowGet);
            ViewBag.ProjectNameDropDown = json;
            return View();
        }
        public JsonResult ListofProjects()
        {
            try
            {
                string _intregistrationid = Convert.ToString(Session["RegistrationID"]);
                //var listofProjects = _IProject.GetListofProjects();
                var listofProjects = _IProject.GetListofProjectsByRegistrationID(Convert.ToInt32(_intregistrationid));
                return Json(listofProjects, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
