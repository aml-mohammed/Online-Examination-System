using Microsoft.AspNetCore.Mvc;
using OnlineExamination.BLL.servicees;
using OnlineExamination.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamnation.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        public IActionResult LogIn()
        {
            LoginViewModel sessionObj = HttpContext.Session.Get<LoginViewModel>("loginVm");
            if (sessionObj == null)
                return View();
            else
            {
                return RedirectUser(sessionObj);
            }
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Set<LoginViewModel>("loginVm",null);
            return RedirectToAction("Login");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogIn(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                LoginViewModel loginVm = _accountService.Login(loginViewModel);
                if (loginVm != null)
                {
                    HttpContext.Session.Set<LoginViewModel>("loginVm", loginVm);
                    return RedirectUser(loginVm);
                }
            }
            
                return View(loginViewModel);

        }
        public IActionResult RedirectUser(LoginViewModel loginVm)
        {
            if(loginVm.Role==(int)EnumRoles.Admin)
            {
                return RedirectToAction("Index","Users");
            }
            else if(loginVm.Role == (int)EnumRoles.teacher)
            {

                return RedirectToAction("Index", "Exams");
            }
            return RedirectToAction("Index","Students");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
