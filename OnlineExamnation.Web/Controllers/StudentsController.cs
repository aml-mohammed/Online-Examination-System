using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using OnlineExamination.BLL.servicees;
using OnlineExamination.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamnation.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IExamService _examService;
        private readonly IQnAsService _qnAService;

        public StudentsController(IStudentService studentService, IExamService examService, IQnAsService qnAService)
        {
            _studentService = studentService;
            _examService = examService;
            _qnAService = qnAService;
        }

        public IActionResult Index(int PageNumber = 1, int pageSize = 10)
        {
            return View(_studentService.GetAll(PageNumber,pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(StudentsViewModel studentViewModel)
        {
            //if (ModelState.IsValid)
            //{
                await _studentService.AddAsync(studentViewModel);
                return RedirectToAction(nameof(Index));
            //}
          //  return View(studentViewModel);

        }
        public IActionResult AttendExam()
        {
            var model = new AttendExamViewModel();
            LoginViewModel sessionObj = HttpContext.Session.Get<LoginViewModel>("loginVm");
            if (sessionObj != null)
            {
                model.StudentId = Convert.ToInt32(sessionObj.Id);
                model.QnAs = new List<QnAsViewModel>();
                var todayExam = _examService.GetAllExams().Where(
                    a=>a.StartDate==DateTime.Today.Date).FirstOrDefault();
                if (todayExam == null)
                {
                    model.message = "No Exam Scheduale today";
                }
                else
                {
                    if (!_qnAService.IsExamAteended(todayExam.Id, model.StudentId))
                    {
                        model.QnAs = _qnAService.GetAllQnAsByExam(todayExam.Id).ToList();
                        model.ExamName = todayExam.Title;
                        model.message = "";
                    }
                    else
                    
                        model.message = "you have already attened this exam";
                }
                return View(model);

            }
            return RedirectToAction("Login","Account");


        }
        [HttpPost]
        public IActionResult AttendExam(AttendExamViewModel attendExamViewModel)
        {
            bool result = _studentService.SetExamResult(attendExamViewModel);
            return RedirectToAction("AttendExam");
        }
        public IActionResult Result(string studenId)
        {
            var model = _studentService.GetExamResults(Convert.ToInt32(studenId));
            return View(model);
        }
        public IActionResult ViewResult()
        {
            LoginViewModel seesionObj = HttpContext.Session.Get<LoginViewModel>("loginVm");
            if (seesionObj != null)
            {
                var model = _studentService.GetExamResults(Convert.ToInt32(seesionObj.Id));
            }
            return RedirectToAction("Login","Account");
        }
        public IActionResult Profile()
        {
            LoginViewModel seesionObj = HttpContext.Session.Get<LoginViewModel>("loginVm");
            if (seesionObj != null)
            {

                var model = _studentService.GetStudentDetails(Convert.ToInt32(seesionObj.Id));
                if (model.PictureFileName != null)
                {
                    model.PictureFileName = ConfigurationManager.GetFilePath() + model.PictureFileName;
                }
                model.CvFileName = ConfigurationManager.GetFilePath() + model.CvFileName;
                return View(model);
            }
            return RedirectToAction("Login", "Account");


        }
        public IActionResult Profile([FromForm]StudentsViewModel studentViewModel)
        {
            if(studentViewModel.PictureFile != null)
            {
                studentViewModel.PictureFileName = SaveStudentFile(studentViewModel.PictureFile);
            }
            if(studentViewModel.CVFile != null)
            {
                studentViewModel.CvFileName = SaveStudentFile(studentViewModel.CVFile);
            }
            _studentService.UpdateAsync(studentViewModel);
            return RedirectToAction("Profile");
        }

        private string SaveStudentFile(IFormFile pictureFile)
        {
            if(pictureFile==null)
            {
                return string.Empty;
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/file");
            return SaveFile(path, pictureFile);
        }

        private string SaveFile(string path, IFormFile pictureFile)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filename = Guid.NewGuid().ToString() + "." + pictureFile.FileName.Split('.')
                [pictureFile.FileName.Split('.').Length - 1];
            path = Path.Combine(path, filename);
            using (Stream stream=new FileStream(path, FileMode.Create))
            {
                pictureFile.CopyTo(stream);
            }
            return filename;


        }
    }
}
