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
    public class GroupService : IGroupService
    {
        IUnitOfWork _unitOfWork;
        ILogger<StudentService> _logger;

        public GroupService(IUnitOfWork unitOfWork, ILogger<StudentService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<GroupViewModel> AddGroupsAsync(GroupViewModel groubVm)
        {
            try
            {
                Groups objGroup = groubVm.convertGroupViewModel(groubVm);
                await _unitOfWork.GenericRepository<Groups>().AddAsync(objGroup);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                return null;
            }
            return groubVm;
        }

        public PagedResult<GroupViewModel> GetAllGroups(int PageNumber, int PageSize)
        {
            var model = new GroupViewModel();
            try
            {
                int ExcludeRecords = (PageSize * PageNumber) - PageSize;
                List<GroupViewModel> detailList = new List<GroupViewModel>();
                var modelList = _unitOfWork.GenericRepository<Groups>().GetAll().Skip
                    (ExcludeRecords).Take(PageSize).ToList();
                var totalCount = _unitOfWork.GenericRepository<Groups>().GetAll().ToList();
                detailList = GroupListInfo(modelList);
                if (detailList != null)
                {
                    model.GroupList= detailList;
                    model.TotalCount = totalCount.Count();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            var result = new PagedResult<GroupViewModel>
            {
                Data = model.GroupList,
                TotalItems = model.TotalCount,
                PageNumber=PageNumber,
                PageSize=PageSize

            };
            return result;


        }

        private List<GroupViewModel> GroupListInfo(List<Groups> modelList)
        {
            return modelList.Select(o => new GroupViewModel(o)).ToList();
        }

        public IEnumerable<Groups> GetAllGroups()
        {
            try
            {
                var group = _unitOfWork.GenericRepository<Groups>().GetAll();
                return group;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
            return Enumerable.Empty<Groups>();

        }

        public GroupViewModel GetById(int groupId)
        {
            try
            {
                var group = _unitOfWork.GenericRepository<Groups>().GetById(groupId);
                return new GroupViewModel(group);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}
