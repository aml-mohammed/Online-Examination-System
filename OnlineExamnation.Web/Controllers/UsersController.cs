using Microsoft.AspNetCore.Mvc;
using OnlineExamination.BLL.servicees;
using OnlineExamination.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamnation.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAccountService _accountService;

        public UsersController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Index(int PageNumber=1,int pageSize=10)
        {
            return View(_accountService.GetAllTeachers(PageNumber,pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
     
        [ValidateAntiForgeryToken]
        public  IActionResult Create(UsersViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
               _accountService.AddTeacher(userViewModel);
                return RedirectToAction("Index");
            }
            return View(userViewModel);
            
        }
    }
}
