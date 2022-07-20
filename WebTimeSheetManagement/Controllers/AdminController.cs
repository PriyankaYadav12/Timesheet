using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTimeSheetManagement.Concrete;
using WebTimeSheetManagement.Filters;
using WebTimeSheetManagement.Helpers;
using WebTimeSheetManagement.Interface;
using WebTimeSheetManagement.Models;

namespace WebTimeSheetManagement.Controllers
{
    [ValidateAdminSession]
    public class AdminController : Controller
    {
        private ITimeSheet _ITimeSheet;
        private IExpense _IExpense;
        private IRoles _IRoles;
        private IAssignRoles _IAssignRoles;
        //private ICacheManager _ICacheManager;
        //private IUsers _IUsers;

        public AdminController()
        {
            _ITimeSheet = new TimeSheetConcrete();
            _IExpense = new ExpenseConcrete();
            _IRoles = new RolesConcrete();
            _IAssignRoles = new AssignRolesConcrete();

        }
        // GET: Admin
        [HttpGet]
        public ActionResult Dashboard()
        {
            try
            {
                var timesheetResult = _ITimeSheet.GetTimeSheetsCountByAdminID(Convert.ToString(Session["AdminUser"]));

                if (timesheetResult != null)
                {
                    ViewBag.SubmittedTimesheetCount = timesheetResult.SubmittedCount;
                    ViewBag.ApprovedTimesheetCount = timesheetResult.ApprovedCount;
                    ViewBag.RejectedTimesheetCount = timesheetResult.RejectedCount;
                }
                else
                {
                    ViewBag.SubmittedTimesheetCount = 0;
                    ViewBag.ApprovedTimesheetCount = 0;
                    ViewBag.RejectedTimesheetCount = 0;
                }


                var expenseResult = _IExpense.GetExpenseAuditCountByAdminID(Convert.ToString(Session["AdminUser"]));

                if (expenseResult != null)
                {
                    ViewBag.SubmittedExpenseCount = expenseResult.SubmittedCount;
                    ViewBag.ApprovedExpenseCount = expenseResult.ApprovedCount;
                    ViewBag.RejectedExpenseCount = expenseResult.RejectedCount;
                }
                else
                {
                    ViewBag.SubmittedExpenseCount = 0;
                    ViewBag.ApprovedExpenseCount = 0;
                    ViewBag.RejectedExpenseCount = 0;
                }

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult AssignRoles()
        {
            try
            {
                AssignRolesModel assignRolesModel = new AssignRolesModel();
                int _registrationID = Convert.ToInt32(Session["RegistrationID"]);
                string _strtechnology = Convert.ToString(Session["Technology"]);
                assignRolesModel.ListofAdmins = _IAssignRoles.ListoSingleAdmin(_registrationID);
                assignRolesModel.ListofUser = _IAssignRoles.GetListofUnAssignedUsersBasedOnTechnology(_registrationID, _strtechnology);
                return View(assignRolesModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult AssignRoles(AssignRolesModel objassign)
        {
            
            try
            {
                int _registrationID = 0;
                string _strtechnology = string.Empty;
                if (objassign.ListofUser == null)
                {
                    TempData["MessageErrorRoles"] = "There are no Users to Assign Roles";
                    //objassign.ListofAdmins = _IAssignRoles.ListofAdmins();
                    //objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsers();
                    _registrationID = Convert.ToInt32(Session["RegistrationID"]);
                    _strtechnology = Convert.ToString(Session["Technology"]);
                    objassign.ListofAdmins = _IAssignRoles.ListoSingleAdmin(_registrationID);
                    objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsersBasedOnTechnology(_registrationID, _strtechnology);
                    return View(objassign);
                }


                var SelectedCount = (from User in objassign.ListofUser
                                     where User.selectedUsers == true
                                     select User).Count();

                if (SelectedCount == 0)
                {
                    TempData["MessageErrorRoles"] = "You have not Selected any User to Assign Roles";
                    //objassign.ListofAdmins = _IAssignRoles.ListofAdmins();
                    //objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsers();
                    _registrationID = Convert.ToInt32(Session["RegistrationID"]);
                    _strtechnology = Convert.ToString(Session["Technology"]);
                    objassign.ListofAdmins = _IAssignRoles.ListoSingleAdmin(_registrationID);
                    objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsersBasedOnTechnology(_registrationID, _strtechnology);
                    return View(objassign);
                }

                if (ModelState.IsValid)
                {
                    //objassign.CreatedBy = Convert.ToInt32(Session["SuperAdmin"]);
                    objassign.CreatedBy = Convert.ToInt32(Session["RegistrationID"]);
                    _IAssignRoles.SaveAssignedRoles(objassign);
                    TempData["MessageRoles"] = "Roles Assigned Successfully!";
                }

                objassign = new AssignRolesModel();
                //objassign.ListofAdmins = _IAssignRoles.ListofAdmins();
                //objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsers();
                _registrationID = Convert.ToInt32(Session["RegistrationID"]);
                _strtechnology = Convert.ToString(Session["Technology"]);
                objassign.ListofAdmins = _IAssignRoles.ListoSingleAdmin(_registrationID);
                objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsersBasedOnTechnology(_registrationID, _strtechnology);

                return RedirectToAction("AssignRoles");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}