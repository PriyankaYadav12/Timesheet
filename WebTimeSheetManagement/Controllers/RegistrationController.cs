using EventApplicationCore.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTimeSheetManagement.Concrete;
using WebTimeSheetManagement.Filters;
using WebTimeSheetManagement.Interface;
using WebTimeSheetManagement.Models;

namespace WebTimeSheetManagement.Controllers
{
    [ValidateSuperAdminSession]
    public class RegistrationController : Controller
    {
        private IRegistration _IRegistration;
        private IRoles _IRoles;
        public RegistrationController()
        {
            _IRegistration = new RegistrationConcrete();
            _IRoles = new RolesConcrete();
        }

        
        // GET: Registration/Create
        public ActionResult Registration()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Microsoft", Value = "Microsoft", Selected = true });

            items.Add(new SelectListItem { Text = "Oracle", Value = "Oracle" });

            items.Add(new SelectListItem { Text = "Application Support", Value = "Application Support", Selected = true });

            items.Add(new SelectListItem { Text = "MiddleWare", Value = "MiddleWare" });

            ViewBag.TechnologyName = items;

            return View(new Registration());
        }

        // POST: Registration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(Registration registration)
        {
            try
            {
                var isUsernameExists = _IRegistration.CheckUserNameExists(registration.Username);

                if (isUsernameExists)
                {
                    ModelState.AddModelError("", errorMessage: "Username Already Used try unique one!");
                }
                else
                {
                    registration.CreatedOn = DateTime.Now;
                    registration.RoleID = _IRoles.getRolesofUserbyRolename("Users");
                    registration.Password = EncryptionLibrary.EncryptText(registration.Password);
                    registration.ConfirmPassword = EncryptionLibrary.EncryptText(registration.ConfirmPassword);
                    registration.Technology = Request.Form["TechnologyName"].ToString();
                    if (_IRegistration.AddUser(registration) > 0)
                    {
                        TempData["MessageRegistration"] = "Data Saved Successfully!";
                        return RedirectToAction("Registration");
                    }
                    else
                    {
                        return View(registration);
                    }
                }
                return RedirectToAction("Registration");
            }
            catch
            {
                return View(registration);
            }
        }

        public JsonResult CheckUserNameExists(string Username)
        {
            try
            {
                var isUsernameExists = false;

                if (Username != null)
                {
                    isUsernameExists = _IRegistration.CheckUserNameExists(Username);
                }

                if (isUsernameExists)
                {
                    return Json(data: true);
                }
                else
                {
                    return Json(data: false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
