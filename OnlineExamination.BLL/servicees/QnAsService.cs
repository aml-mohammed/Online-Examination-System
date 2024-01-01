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
    public class QnAsService : IQnAsService
    {
        IUnitOfWork _unitOfWork;
        ILogger<StudentService> _logger;

        public QnAsService(IUnitOfWork unitOfWork, ILogger<StudentService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<QnAsViewModel> AddQnAsAsync(QnAsViewModel QnAsVm)
        {
            try
            {
                QnAs objGroup = QnAsVm.ConvertViewModel(QnAsVm);
                await _unitOfWork.GenericRepository<QnAs>().AddAsync(objGroup);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                return null;
            }
            return QnAsVm;
        }

        public PagedResult<QnAsViewModel> GetAll(int PageNumber, int PageSize)
        {
            var model = new QnAsViewModel();
            try
            {
                int ExcludeRecords = (PageSize * PageNumber) - PageSize;
                List<QnAsViewModel> detailList = new List<QnAsViewModel>();
                var modelList = _unitOfWork.GenericRepository<QnAs>().GetAll().Skip
                    (ExcludeRecords).Take(PageSize).ToList();
                var totalCount = _unitOfWork.GenericRepository<QnAs>().GetAll().ToList();
                detailList = QnAsListInfo(modelList);
                if (detailList != null)
                {
                    model.QnAsList = detailList;
                    model.Totalcount = totalCount.Count();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            var result = new PagedResult<QnAsViewModel>
            {
                Data = model.QnAsList,
                TotalItems = model.Totalcount,
                PageNumber = PageNumber,
                PageSize = PageSize

            };
            return result;
        }

        private List<QnAsViewModel> QnAsListInfo(List<QnAs> modelList)
        {
            return modelList.Select(o => new QnAsViewModel(o)).ToList();
        }

        public IEnumerable<QnAsViewModel> GetAllQnAsByExam(int examid)
        {
            try
            {
                var QnAs = _unitOfWork.GenericRepository<QnAs>().GetAll().Where(x=>x.ExamsId==examid);
                return QnAsListInfo(QnAs.ToList());
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
            return Enumerable.Empty<QnAsViewModel>();
        }

        public bool IsExamAteended(int ExamId, int StudentId)
        {
            try
            {
                var qnaRecors = _unitOfWork.GenericRepository<ExamResults>().GetAll().
                    FirstOrDefault(x => x.ExamsId == ExamId && x.StudentsId == StudentId);
                return qnaRecors == null ? false : true;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
            return false;
        }
    }
}
