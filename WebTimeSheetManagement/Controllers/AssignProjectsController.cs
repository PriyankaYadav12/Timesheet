using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTimeSheetManagement.Concrete;
using WebTimeSheetManagement.Interface;
using WebTimeSheetManagement.Models;


namespace WebTimeSheetManagement.Controllers
{
    public class AssignProjectsController : Controller
    {
        // GET: AssignProjects
        private ITimeSheet _ITimeSheet;
        private IExpense _IExpense;
        private IRoles _IRoles;
        private IAssignRoles _IAssignRoles;
        //private ICacheManager _ICacheManager;
        //private IUsers _IUsers;

        public AssignProjectsController()
        {
            _ITimeSheet = new TimeSheetConcrete();
            _IExpense = new ExpenseConcrete();
            _IRoles = new RolesConcrete();
            _IAssignRoles = new AssignRolesConcrete();

        }

        [HttpGet]
        public ActionResult AssignProjects()
        {
            try
            {

                AssignRolesModel assignRolesModel = new AssignRolesModel();
                int _registrationID = Convert.ToInt32(Session["RegistrationID"]);
                string _strtechnology = Convert.ToString(Session["Technology"]);
                assignRolesModel.dropdown = _IAssignRoles.popdropdown();
                assignRolesModel.ListofAdmins = _IAssignRoles.ListoSingleAdmin(_registrationID);
                //assignRolesModel.ListofUser = _IAssignRoles.GetListofUnAssignedUsersBasedOnTechnology(_registrationID, _strtechnology);
                 assignRolesModel.ListofUser = _IAssignRoles.GetListofUnAssignedProjectUserBasedOnTechnology(_registrationID, _strtechnology);
                return View(assignRolesModel);

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public ActionResult AssignProjects(AssignRolesModel objassign)
        {

            try
            {
                int _registrationID = 0;
                string _strtechnology = string.Empty;
                if (objassign.ListofUser == null)
                {
                    
                    TempData["MessageErrorRoles"] = "There are no Users to Assign Project";
                    objassign.ListofAdmins = _IAssignRoles.ListofAdmins();
                    objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsers();
                    _registrationID = Convert.ToInt32(Session["RegistrationID"]);
                    _strtechnology = Convert.ToString(Session["Technology"]);
                    objassign.ListofAdmins = _IAssignRoles.ListoSingleAdmin(_registrationID);
                    objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsersBasedOnTechnology(_registrationID, _strtechnology);
                    objassign.dropdown = _IAssignRoles.popdropdown();
                    return View(objassign);
                }


                var SelectedCount = (from User in objassign.ListofUser
                                     where User.selectedUsers == true
                                     select User).Count();

                if (SelectedCount == 0)
                {
                    
                    TempData["MessageErrorRoles"] = "You have not Selected any Project to Assign User";
                    objassign.ListofAdmins = _IAssignRoles.ListofAdmins();
                    objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsers();
                    _registrationID = Convert.ToInt32(Session["RegistrationID"]);
                    _strtechnology = Convert.ToString(Session["Technology"]);
                    objassign.ListofAdmins = _IAssignRoles.ListoSingleAdmin(_registrationID);
                    objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsersBasedOnTechnology(_registrationID, _strtechnology);
                    objassign.dropdown = _IAssignRoles.popdropdown();
                    return View(objassign);
                }

                if (ModelState.IsValid)
                {
                    objassign.RegistrationID = Convert.ToInt32(Session["RegistrationID"]);
                    objassign.CreatedBy = Convert.ToInt32(Session["SuperAdmin"]);
                    objassign.CreatedBy = Convert.ToInt32(Session["RegistrationID"]);
                    //_IAssignRoles.SaveAssignedRoles(objassign);
                    objassign.dropdown = _IAssignRoles.popdropdown();
                    _IAssignRoles.insertmapping(objassign);
                    TempData["MessageRoles"] = "Project Assigned Successfully!";
                }

                objassign = new AssignRolesModel();
                objassign.ListofAdmins = _IAssignRoles.ListofAdmins();
                objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsers();
                _registrationID = Convert.ToInt32(Session["RegistrationID"]);
                _strtechnology = Convert.ToString(Session["Technology"]);
                objassign.ListofAdmins = _IAssignRoles.ListoSingleAdmin(_registrationID);
                objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsersBasedOnTechnology(_registrationID, _strtechnology);

                return RedirectToAction("AssignProjects");
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
    
