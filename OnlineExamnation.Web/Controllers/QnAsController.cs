using Microsoft.AspNetCore.Mvc;
using OnlineExamination.BLL.servicees;
using OnlineExamination.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamnation.Web.Controllers
{
    public class QnAsController : Controller

    {
        private readonly IExamService _examService;
        private readonly IQnAsService _qnAService;

        public QnAsController(IExamService examService, IQnAsService qnAService)
        {
            _examService = examService;
            _qnAService = qnAService;
        }

        public IActionResult Index(int PageNumber = 1, int pageSize = 10)
        {
            return View(_qnAService.GetAll(PageNumber, pageSize));
        }
        public IActionResult Create()
        {
            var model = new QnAsViewModel();
            model.ExamList = _examService.GetAllExams();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(QnAsViewModel qnAViewModel)
        {
            if (ModelState.IsValid)
            {
                await _qnAService.AddQnAsAsync(qnAViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(qnAViewModel);

        }
    }
}
