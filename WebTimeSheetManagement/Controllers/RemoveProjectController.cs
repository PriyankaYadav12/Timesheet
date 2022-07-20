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
    public class RemoveProjectController : Controller
    {
        // GET: RemoveProject
        private ITimeSheet _ITimeSheet;
        private IExpense _IExpense;
        private IRoles _IRoles;
        private IAssignRoles _IAssignRoles;
        //private ICacheManager _ICacheManager;
        //private IUsers _IUsers;

        public RemoveProjectController()
        {
            _ITimeSheet = new TimeSheetConcrete();
            _IExpense = new ExpenseConcrete();
            _IRoles = new RolesConcrete();
            _IAssignRoles = new AssignRolesConcrete();


        }
        [HttpGet]
        public ActionResult RemoveProject()
        {
            try
            {

                AssignRolesModel assignRolesModel1 = new AssignRolesModel();
                int _registrationID = Convert.ToInt32(Session["RegistrationID"]);
                string _strtechnology = Convert.ToString(Session["Technology"]);
                //  assignRolesModel.dropdown = _IAssignRoles.popdropdown();
                // assignRolesModel.ListofAdmins = _IAssignRoles.ListoSingleAdmin(_registrationID);
                assignRolesModel1.ListofUser = _IAssignRoles.GetListofAssignedUsersBasedOnTechnology(_registrationID, _strtechnology);
                List<SelectListItem> Users = new List<SelectListItem>();

                foreach (var item in assignRolesModel1.ListofUser)
                {

                    Users.Add(new SelectListItem { Text = item.Name, Value = item.RegistrationID.ToString() });
                }

                assignRolesModel1.AssignUsers = Users;

                assignRolesModel1.ChkListProj = _IAssignRoles.PopChkBox();
                return View(assignRolesModel1);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]

        public ActionResult RemoveProject(AssignRolesModel objassign)
        {

            try
            {
                int _registrationID = Convert.ToInt32(Session["RegistrationID"]);
                string _strtechnology = Convert.ToString(Session["Technology"]);
                AssignRolesModel assignRolesModel = new AssignRolesModel();


                if (ModelState.IsValid)
                {
                    objassign.RegistrationID = Convert.ToInt32(Session["RegistrationID"]);
                    objassign.CreatedBy = Convert.ToInt32(Session["SuperAdmin"]);
                    objassign.CreatedBy = Convert.ToInt32(Session["RegistrationID"]);

                    // _IAssignRoles.SaveAssignedRoles(objassign);
                    // objassign.dropdown = _IAssignRoles.popdropdown();
                    // _IAssignRoles.insertmapping(objassign);
                    assignRolesModel.ListofUser = _IAssignRoles.GetListofAssignedUsersBasedOnTechnology(_registrationID, _strtechnology);

                    List<SelectListItem> Users = new List<SelectListItem>();

                    foreach (var item in assignRolesModel.ListofUser)
                    {

                        Users.Add(new SelectListItem { Text = item.Name, Value = item.RegistrationID.ToString() });
                    }

                    assignRolesModel.AssignUsers = Users;
                    // TempData["MessageRoles"] = "Modify Assigned Projects!";
                    assignRolesModel.ChkListProj = _IAssignRoles.PopChkBox2(objassign);
                }

                //objassign = new AssignRolesModel();
                //objassign.ListofAdmins = _IAssignRoles.ListofAdmins();
                //objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsers();
                //_registrationID = Convert.ToInt32(Session["RegistrationID"]);
                //_strtechnology = Convert.ToString(Session["Technology"]);
                //objassign.ListofAdmins = _IAssignRoles.ListoSingleAdmin(_registrationID);
                //objassign.ListofUser = _IAssignRoles.GetListofUnAssignedUsersBasedOnTechnology(_registrationID, _strtechnology);

                return View(assignRolesModel);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]

        public ActionResult RemoveProject1(AssignRolesModel objassigny)
        {

            try
            {
                AssignRolesModel assignRolesModel1 = new AssignRolesModel();
                int _registrationID = Convert.ToInt32(Session["RegistrationID"]);
                string _strtechnology = Convert.ToString(Session["Technology"]);
                if (ModelState.IsValid)
                {
                    int count = 0;
                    List<AssignRolesModel> y = new List<AssignRolesModel>();
                    y = objassigny.ChkListProj;
                    foreach (var stoic in y)
                    {
                        if (stoic.ChkSelected)
                        {
                            count++;
                        }
                    }
                    int b = count;

                    objassigny.RegistrationID = Convert.ToInt32(Session["RegistrationID"]);
                    objassigny.CreatedBy = Convert.ToInt32(Session["SuperAdmin"]);
                    objassigny.CreatedBy = Convert.ToInt32(Session["RegistrationID"]);
                    //_IAssignRoles.SaveAssignedRoles(objassign);
                    //objassign.dropdown = _IAssignRoles.popdropdown();
                    //_IAssignRoles.insertmapping(objassign);
                    //TempData["MessageRoles"] = "Roles Assigned Successfully!";
                    assignRolesModel1.ListofUser = _IAssignRoles.GetListofAssignedUsersBasedOnTechnology(_registrationID, _strtechnology);
                    List<SelectListItem> Users = new List<SelectListItem>();

                    foreach (var item in assignRolesModel1.ListofUser)
                    {

                        Users.Add(new SelectListItem { Text = item.Name, Value = item.RegistrationID.ToString() });
                    }

                    assignRolesModel1.AssignUsers = Users;

                    // assignRolesModel1.ChkListProj = _IAssignRoles.PopChkBox1(objassigny);
                    _IAssignRoles.Delete(objassigny);

                    _IAssignRoles.Insert(objassigny);
                    // assignRolesModel.ChkListProj = _IAssignRoles.PopChkBox();

                    //assignRolesModel1.ChkListProj = _IAssignRoles.PopChkBox2(objassigny);

                    TempData["MessageRoles"] = "Projects Edited Successfully !!";



                }
                int NumberOfChkbx, NumberOfSelctChkBx = 0;
                NumberOfChkbx = objassigny.ChkListProj.Count;
                for (int i = 0; i < objassigny.ChkListProj.Count; i++)
                {
                    if (!objassigny.ChkListProj[i].ChkSelected)
                    {
                        NumberOfSelctChkBx++;
                    }

                }
                //if (NumberOfChkbx == NumberOfSelctChkBx)
                //{
                //    string constr = ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ConnectionString;
                //    using (SqlConnection con1 = new SqlConnection(constr))
                //    {


                //        SqlCommand cmd = new SqlCommand("delete from AssignedRoles where  RegistrationID=" + objassigny.DropDownId + "", con1);
                //        con1.Open();
                //        SqlDataReader sdr = cmd.ExecuteReader();
                //        con1.Close();


                //    }
                //}

                return RedirectToAction("RemoveProject", "RemoveProject");

            }
            catch (Exception)
            {
                throw;
            }
        }


        public PartialViewResult GetEmployeeRecordWithProjects(int DropDownId)
        {
            List<AssignRolesModel> CheckedValue = new List<AssignRolesModel>();
            AssignRolesModel obj = new AssignRolesModel();
            obj.DropDownId = DropDownId;//Stored Id in to Dropdown
            CheckedValue = _IAssignRoles.PopChkBox2(obj);
            obj.ChkListProj = _IAssignRoles.PopChkBox2(obj);
            return PartialView("_EmpTestPartial", obj);

        }
    }
}
