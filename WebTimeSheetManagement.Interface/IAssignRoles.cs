using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebTimeSheetManagement.Models;

namespace WebTimeSheetManagement.Interface
{
    public interface IAssignRoles
    {
        List<AdminModel> ListofAdmins();
        List<UserModel> ListofUser();
        int UpdateAssigntoAdmin(string AssignToAdminID, string UserID);
        IQueryable<UserModel> ShowallRoles(string sortColumn, string sortColumnDir, string Search);
        bool RemovefromUserRole(string RegistrationID);
        List<UserModel> GetListofUnAssignedUsers();
        bool SaveAssignedRoles(AssignRolesModel AssignRolesModel);
        bool CheckIsUserAssignedRole(int RegistrationID);

        List<AdminModel> ListoSingleAdmin(int RegistrationID);

        List<UserModel> GetListofUnAssignedUsersBasedOnTechnology(int RegistrationID,string Technology);
        List<UserModel> GetListofAssignedUsersBasedOnTechnology(int RegistrationID, string Technology);
        List<SelectListItem> popdropdown();

        bool insertmapping(AssignRolesModel AssignRolesModel);
        bool Delete(AssignRolesModel AssignRolesModel);
        bool Insert(AssignRolesModel AssignRolesModel);
        List<AssignRolesModel> PopChkBox2(AssignRolesModel obj);
        List<AssignRolesModel> PopChkBox();
        //List<AssignRolesModel> PopChkBox1(AssignRolesModel obj);

        List<UserModel> GetListofUnAssignedProjectUserBasedOnTechnology(int RegistrationID, string Technology);

    }
}
