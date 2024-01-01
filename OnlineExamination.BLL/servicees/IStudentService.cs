using OnlineExamination.DataAccess;
using OnlineExamination.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamination.BLL.servicees
{
  public interface IStudentService
    {
        PagedResult<StudentsViewModel> GetAll(int PageNumber, int PageSize);
        Task<StudentsViewModel> AddAsync(StudentsViewModel vm);
        IEnumerable<Students> GetAllStudents();
        bool SetGroupIdToStudents(GroupViewModel vm);
        bool SetExamResult(AttendExamViewModel vm);
        IEnumerable<ResultViewModel> GetExamResults(int StudentId);
        StudentsViewModel GetStudentDetails(int StudentId);
        Task<StudentsViewModel> UpdateAsync(StudentsViewModel vm);



    }
}
