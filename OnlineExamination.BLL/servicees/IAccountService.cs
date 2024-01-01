using OnlineExamination.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamination.BLL.servicees
{
    public interface IAccountService
    {
        LoginViewModel Login(LoginViewModel vm);
        bool AddTeacher(UsersViewModel vm);
        PagedResult<UsersViewModel> GetAllTeachers(int PageNumber, int PageSize);
    }
}
