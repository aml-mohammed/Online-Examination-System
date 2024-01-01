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
    public class ExamService : IExamService
    {

        IUnitOfWork _unitOfWork;
        ILogger<StudentService> _logger;

        public ExamService(IUnitOfWork unitOfWork, ILogger<StudentService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ExamViewModel> AddExamAsync(ExamViewModel examVm)
        {
            try
            {
                Exams objExam = examVm.ConvertViewModel(examVm);
                await _unitOfWork.GenericRepository<Exams>().AddAsync(objExam);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                return null;
            }
            return examVm;
        }

        public PagedResult<ExamViewModel> GetAll(int PageNumber, int PageSize)
        {
            var model = new ExamViewModel();
            try
            {
                int ExcludeRecords = (PageSize * PageNumber) - PageSize;
                List<ExamViewModel> detailList = new List<ExamViewModel>();
                var modelList = _unitOfWork.GenericRepository<Exams>().GetAll().Skip
                    (ExcludeRecords).Take(PageSize).ToList();
                var totalCount = _unitOfWork.GenericRepository<Exams>().GetAll().ToList();
                detailList = GroupListInfo(modelList);
                if (detailList != null)
                {
                    model.ExamList = detailList;
                    model.TotalCount = totalCount.Count();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            var result = new PagedResult<ExamViewModel>
            {
                Data = model.ExamList,
                TotalItems = model.TotalCount,
                PageNumber = PageNumber,
                PageSize = PageSize

            };
            return result;

        }

        private List<ExamViewModel> GroupListInfo(List<Exams> modelList)
        {
            return modelList.Select(o => new ExamViewModel(o)).ToList();
        }

        public IEnumerable<Exams> GetAllExams()
        {
            try
            {
                var exam = _unitOfWork.GenericRepository<Exams>().GetAll();
                return exam;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
            return Enumerable.Empty<Exams>();
        }
    }
}
