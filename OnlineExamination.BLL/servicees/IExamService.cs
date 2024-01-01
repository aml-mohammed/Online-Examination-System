using OnlineExamination.DataAccess;
using OnlineExamination.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamination.BLL.servicees
{
    public interface IExamService
    {
        PagedResult<ExamViewModel> GetAll(int PageNumber, int PageSize);
        Task<ExamViewModel> AddExamAsync(ExamViewModel examVm);
        IEnumerable<Exams> GetAllExams();
    }
}
