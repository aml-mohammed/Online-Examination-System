using OnlineExamination.DataAccess;
using OnlineExamination.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamination.BLL.servicees
{
   public interface IQnAsService
    {
        PagedResult<QnAsViewModel> GetAll(int PageNumber, int PageSize);
        Task<QnAsViewModel> AddQnAsAsync(QnAsViewModel QnAsVm);
        IEnumerable<QnAsViewModel> GetAllQnAsByExam(int examid);
        bool IsExamAteended(int ExamId,int StudentId);
    }
}
