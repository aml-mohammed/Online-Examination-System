using Microsoft.AspNetCore.Mvc;
using OnlineExamination.BLL.servicees;
using OnlineExamination.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamnation.Web.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IGroupService _groupService;

        public GroupsController(IStudentService studentService, IGroupService groupService)
        {
            _studentService = studentService;
            _groupService = groupService;
        }

        public IActionResult Index(int PageNumber = 1, int pageSize = 10)
        {
            return View(_groupService.GetAllGroups(PageNumber, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(GroupViewModel groupViewModel)
        {
            if (ModelState.IsValid)
            {
                await _groupService.AddGroupsAsync(groupViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(groupViewModel);

        }
        public IActionResult Details(string groupId)
        {
            var model = _groupService.GetById(Convert.ToInt32(groupId));
            model.studentCheckList = _studentService.GetAllStudents().Select(
                a => new StudentCheckBoxListViewModel()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Selected = a.GroupsId == Convert.ToInt32(groupId)
                }).ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Details(GroupViewModel groupViewModel)
        {
            bool result = _studentService.SetGroupIdToStudents(groupViewModel);
            if (result)
                return RedirectToAction("Details",new {groupId=groupViewModel.Id});
            return View(groupViewModel);
            
        }
    }
}
