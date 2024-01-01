using Microsoft.Extensions.Logging;
using OnlineExamination.DataAccess;
using OnlineExamination.DataAccess.UnitOfWork;
using OnlineExamination.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamination.BLL.servicees
{
    public class AccountService : IAccountService
    {
        IUnitOfWork _unitOfWork;
        ILogger<StudentService> _logger;

        public AccountService(IUnitOfWork unitOfWork, ILogger<StudentService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public bool AddTeacher(UsersViewModel vm)
        {
            try
            {
                Users obj = new Users
                {
                    Name = vm.Name,
                    UserName = vm.UserName,
                    Password = vm.Password,
                    Role = (int)EnumRoles.teacher
                };
              _unitOfWork.GenericRepository<Users>().AddAsync(obj);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return false;
            }
            return true;
        }

        public PagedResult<UsersViewModel> GetAllTeachers(int PageNumber, int PageSize)
        {
            var model = new UsersViewModel();
            try
            {
                int ExcludeRecords = (PageSize * PageNumber) - PageSize;
                List<UsersViewModel> detailList = new List<UsersViewModel>();
                var modelList = _unitOfWork.GenericRepository<Users>().GetAll().Where
                   (x => x.Role == (int)EnumRoles.teacher).Skip(ExcludeRecords)
                   .Take(PageSize).ToList();
             //   var totalCount = _unitOfWork.GenericRepository<Users>().GetAll().ToList();
                detailList = ListInfo(modelList);
                if (detailList != null)
                {
                    model.UserList = detailList;
                    model.TotalCount = _unitOfWork.GenericRepository<Users>().GetAll().Count(
                        x => x.Role == (int)EnumRoles.teacher);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            var result = new PagedResult<UsersViewModel>
            {
                Data = model.UserList,
                TotalItems = model.TotalCount,
                PageNumber = PageNumber,
                PageSize = PageSize
            };
            return result;
        }

        private List<UsersViewModel> ListInfo(List<Users> modelList)
        {
            return modelList.Select(o => new UsersViewModel(o)).ToList();
        }

        public LoginViewModel Login(LoginViewModel vm)
        {
           if(vm.Role == (int)EnumRoles.Admin || vm.Role == (int)EnumRoles.teacher)
            {
                var user = _unitOfWork.GenericRepository<Users>().GetAll().
                    FirstOrDefault(a => a.UserName == vm.UserName.Trim() &&
                    a.Password == vm.pasword.Trim() && a.Role == vm.Role);
                if(user != null)
                {
                    vm.Id = user.Id;
                    return vm;
                }

            }
           else
            {
                var student=_unitOfWork.GenericRepository<Students>().GetAll().
                    FirstOrDefault(a => a.UserName == vm.UserName.Trim() &&
                    a.Password == vm.pasword.Trim());
                if(student != null)
                {
                    vm.Id = student.Id;
                }
                return vm;

            }
            return null;
        }
    }
}
