using OnlineExamination.DataAccess;
using OnlineExamination.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamination.BLL.servicees
{
   public interface IGroupService
    {
        PagedResult<GroupViewModel> GetAllGroups(int PageNumber,int PageSize);
        Task<GroupViewModel> AddGroupsAsync(GroupViewModel groubVm);
        IEnumerable<Groups> GetAllGroups();
        GroupViewModel GetById(int groupId);
    }
}
