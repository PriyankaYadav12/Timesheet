using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTimeSheetManagement.Interface;
using WebTimeSheetManagement.Models;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Data;

namespace WebTimeSheetManagement.Concrete
{
    public class AssignRolesConcrete : IAssignRoles
    {
        public List<AdminModel> ListofAdmins()
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var result = con.Query<AdminModel>("Usp_GetListofAdmins", null, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
                    result.Insert(0, new AdminModel { Name = "----Select----", RegistrationID = "" });
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public List<UserModel> ListofUser()
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var result = con.Query<UserModel>("Usp_GetListofUsers", null, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public int UpdateAssigntoAdmin(string AssignToAdminID, string UserID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@AssignTo", AssignToAdminID);
                    param.Add("@RegistrationID", UserID);
                    var result = con.Execute("Usp_UpdateAssignToUser", param, null, 0, System.Data.CommandType.StoredProcedure);
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public IQueryable<UserModel> ShowallRoles(string sortColumn, string sortColumnDir, string Search)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from AssignedRoles in _context.AssignedRoles
                                       join registration in _context.Registration on AssignedRoles.RegistrationID equals registration.RegistrationID
                                       join AssignedRolesAdmin in _context.Registration on AssignedRoles.AssignToAdmin equals AssignedRolesAdmin.RegistrationID
                                       select new UserModel
                                       {
                                           Name = registration.Name,
                                           AssignToAdmin = string.IsNullOrEmpty(AssignedRolesAdmin.Name) ? "*Not Assigned*" : AssignedRolesAdmin.Name.ToUpper(),
                                           RegistrationID = registration.RegistrationID

                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.Name == Search);
            }

            return IQueryabletimesheet;

        }

        public bool RemovefromUserRole(string RegistrationID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@RegistrationID", RegistrationID);
                    var result = con.Execute("Usp_UpdateUserRole", param, null, 0, System.Data.CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public List<UserModel> GetListofUnAssignedUsers()
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                   
                    var result = con.Query<UserModel>("Usp_GetListofUnAssignedUsers", null, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public bool SaveAssignedRoles(AssignRolesModel AssignRolesModel)
        {
            bool result = false;
            using (var _context = new DatabaseContext())
            {
                try
                {
                    for (int i = 0; i < AssignRolesModel.ListofUser.Count(); i++)
                    {
                        if (AssignRolesModel.ListofUser[i].selectedUsers)
                        {
                            AssignedRoles AssignedRoles = new AssignedRoles
                            {
                                AssignedRolesID = 0,
                                AssignToAdmin = AssignRolesModel.RegistrationID,
                                CreatedOn = DateTime.Now,
                                CreatedBy = AssignRolesModel.CreatedBy,
                                Status = "A",
                                RegistrationID = AssignRolesModel.ListofUser[i].RegistrationID
                            };

                            _context.AssignedRoles.Add(AssignedRoles);
                            _context.SaveChanges();
                        }
                    }

                    result = true;
                }
                catch (Exception)
                {
                    throw;
                }

                return result;
            }
        }

        public bool CheckIsUserAssignedRole(int RegistrationID)
        {
            var IsassignCount = 0;
            using (var _context = new DatabaseContext())
            {
                IsassignCount = (from assignUser in _context.AssignedRoles
                                 where assignUser.RegistrationID == RegistrationID
                                 select assignUser).Count();
            }

            if (IsassignCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<AdminModel> ListoSingleAdmin(int RegistrationID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@RegistrationID", RegistrationID);
                    var result = con.Query<AdminModel>("Usp_GetListofAdminsByRegistrationID", param, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
                    //result.Insert(0, new AdminModel { Name = "----Select----", RegistrationID = "" });
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public List<UserModel> GetListofUnAssignedUsersBasedOnTechnology(int RegistrationID, string Technology)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@RegistrationID", RegistrationID);
                    param.Add("@Technology", Technology);
                    var result = con.Query<UserModel>("Usp_GetListofUnAssignedUsersBasedOnTechnology", param, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public List<SelectListItem> popdropdown()
        {
            string constr = ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ConnectionString;
            List<SelectListItem> assignPro = new List<SelectListItem>();
            using (SqlConnection con1 = new SqlConnection(constr))
            {
                string query1 = " SELECT ProjectName, ProjectID FROM ProjectMaster";
                using (SqlCommand cmd = new SqlCommand(query1))
                {
                    cmd.Connection = con1;
                    con1.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            assignPro.Add(new SelectListItem
                            {
                                Text = sdr["ProjectName"].ToString(),
                                Value = sdr["ProjectID"].ToString()

                            });
                        }
                    }
                    con1.Close();
                }
            }
            return assignPro;
        }

        public bool insertmapping(AssignRolesModel AssignRolesModel)
        {
            List<SelectListItem> selectedItems = AssignRolesModel.dropdown.Where(p => AssignRolesModel.ProjectID.Contains(int.Parse(p.Value))).ToList();
            string constr = ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ConnectionString;
            List<SelectListItem> assignPro = new List<SelectListItem>();
            using (SqlConnection con1 = new SqlConnection(constr))
            {
                for (int i = 0; i < AssignRolesModel.ListofUser.Count(); i++)
                {
                    if (AssignRolesModel.ListofUser[i].selectedUsers)
                    {
                        foreach (var ite in selectedItems)
                        {

                            SqlCommand cmd = new SqlCommand("insert into userprojectmapping values('" + AssignRolesModel.ListofUser[i].RegistrationID + "','" + Convert.ToInt32(ite.Value) + "')", con1);
                            con1.Open();
                            SqlDataReader sdr = cmd.ExecuteReader();
                            con1.Close();

                        }
                    }
                }
                return true;
            }
        }

        public List<UserModel> GetListofAssignedUsersBasedOnTechnology(int RegistrationID, string Technology)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@RegistrationID", RegistrationID);
                    param.Add("@Technology", Technology);
                    var result = con.Query<UserModel>("Usp_GetListofAssignedUsersBasedOnTechnology", param, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }



        //Get Checked Value from database
        public List<AssignRolesModel> PopChkBox2(AssignRolesModel obj)
        {
            string constr = ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ConnectionString;
            List<AssignRolesModel> Chkedvalue = new List<AssignRolesModel>();
            using (SqlConnection con1 = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Select PM.ProjectID,IsNull(UPM.ProjectID,0) as UserMappingProjectId,ProjectName,ISNULL(RegistrationID,'') as RegistationID from ProjectMaster PM left outer join UserProjectMapping UPM on PM.ProjectID = UPM.ProjectID and RegistrationID =" + Convert.ToInt32(obj.DropDownId) + "", con1))
                {
                    cmd.Connection = con1;
                    con1.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow rows in dt.Rows)
                    {
                        int ss = Convert.ToInt32(rows["RegistationID"].ToString());
                        if (ss != 0)
                        {
                            Chkedvalue.Add(new AssignRolesModel
                            {
                                ChkName = rows["ProjectName"].ToString(),
                                ChkId = Convert.ToInt32(rows["ProjectID"].ToString()),
                                ChkSelected = true

                            });
                        }
                        else
                        {
                            Chkedvalue.Add(new AssignRolesModel
                            {
                                ChkName = rows["ProjectName"].ToString(),
                                ChkId = Convert.ToInt32(rows["ProjectID"].ToString()),
                                ChkSelected = false
                            });
                        }

                        con1.Close();
                    }
                }
            }
            return Chkedvalue;
        }




        public bool Delete(AssignRolesModel AssignRolesModel)
        {
            //List<SelectListItem> selectedItems = AssignRolesModel.dropdown.Where(p => AssignRolesModel.ProjectID.Contains(int.Parse(p.Value))).ToList();
            string constr = ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ConnectionString;
            //List<SelectListItem> y = new List<SelectListItem>();
            using (SqlConnection con1 = new SqlConnection(constr))
            {
                //for (int i = 0; i < AssignRolesModel.ListofUser.Count(); i++)
                //{
                //    if (AssignRolesModel.ListofUser[i].selectedUsers)
                //    {
                //        foreach (var ite in selectedItems)
                //        {

                SqlCommand cmd = new SqlCommand("delete from userprojectmapping where RegistrationID = " + AssignRolesModel.DropDownId + "", con1);
                con1.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                con1.Close();

                //        }
                //    }
                //}
                return true;
            }
        }


        public bool Insert(AssignRolesModel AssignRolesModel)
        {
            //List<SelectListItem> selectedItems = AssignRolesModel.dropdown.Where(p => AssignRolesModel.ProjectID.Contains(int.Parse(p.Value))).ToList();
            string constr = ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ConnectionString;
            // List<SelectListItem> y = new List<SelectListItem>();
            using (SqlConnection con1 = new SqlConnection(constr))
            {

                for (int i = 0; i < AssignRolesModel.ChkListProj.Count(); i++)
                {
                    if (AssignRolesModel.ChkListProj[i].ChkSelected)
                    {
                        SqlCommand cmd = new SqlCommand("insert into userprojectmapping values('" + AssignRolesModel.DropDownId + "','" + Convert.ToInt32(AssignRolesModel.ChkListProj[i].ChkId) + "')", con1);
                        con1.Open();
                        SqlDataReader sdr = cmd.ExecuteReader();
                        con1.Close();
                    }
                }
                return true;
            }
        }

        public List<AssignRolesModel> PopChkBox()
        {
            List<AssignRolesModel> y = new List<AssignRolesModel>();
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ConnectionString;            
                    using (SqlConnection con1 = new SqlConnection(constr))
                    {
                        string query1 = " SELECT ProjectName, ProjectID FROM ProjectMaster";
                        using (SqlCommand cmd = new SqlCommand(query1))
                        {
                            cmd.Connection = con1;
                            con1.Open();
                            using (SqlDataReader sdr = cmd.ExecuteReader())
                            {
                                while (sdr.Read())
                                {
                                    y.Add(new AssignRolesModel
                                    {
                                        ChkName = Convert.ToString(sdr["ProjectName"].ToString()),
                                        ChkId = Convert.ToInt32(sdr["ProjectID"].ToString()),
                                        ChkSelected = false
                                    });
                                }
                            }
                            con1.Close();
                        }
                    }
            }
            catch (Exception)
            {

                throw;
            }
            
            return y;
        }

        public List<UserModel> GetListofUnAssignedProjectUserBasedOnTechnology(int RegistrationID, string Technology)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@RegistrationID", RegistrationID);
                    param.Add("@Technology", Technology);
                    var result = con.Query<UserModel>("Usp_GetListofUnProjectUsersBasedOnTechnology", param, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
