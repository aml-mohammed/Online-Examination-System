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
    public class StudentService : IStudentService
    {
        IUnitOfWork _unitOfWork;
        ILogger<StudentService> _logger;

        public StudentService(IUnitOfWork unitOfWork, ILogger<StudentService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<StudentsViewModel> AddAsync(StudentsViewModel vm)
        {
            try
            {
                Students obj = vm.ConvertViewModel(vm);
                await _unitOfWork.GenericRepository<Students>().AddAsync(obj);
            }
            catch (Exception ex)
            {
                return null;
            }
            return vm;
        }
        private List<StudentsViewModel> GroupListInfo(List<Students> modellist)
        {
            return modellist.Select(o=>new StudentsViewModel(o)).ToList();
        }
        public PagedResult<StudentsViewModel> GetAll(int PageNumber, int PageSize)
        {
            var model = new StudentsViewModel();
            try
            {
                int ExcludeRecords = (PageSize * PageNumber) - PageSize;
                List<StudentsViewModel> detailList = new List<StudentsViewModel>();
                var modelList = _unitOfWork.GenericRepository<Students>().GetAll().Skip
                    (ExcludeRecords).Take(PageSize).ToList();
                var totalCount = _unitOfWork.GenericRepository<Students>().GetAll().ToList();
                detailList = GroupListInfo(modelList);
                if (detailList != null)
                {
                    model.StudentList = detailList;
                    model.TotalCount = totalCount.Count();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            var result = new PagedResult<StudentsViewModel>
            {
                Data = model.StudentList,
                TotalItems = model.TotalCount,
                PageNumber = PageNumber,
                PageSize = PageSize
            };
            return result;
        }

        public IEnumerable<Students> GetAllStudents()
        {
            //try
            //{
                var students = _unitOfWork.GenericRepository<Students>().GetAll();
                return students;
         //   }
            //catch (Exception ex)
            //{

            //    _logger.LogError(ex.Message);

            //}
           
            // return Enumerable.Empty<Students>();
        }

        public IEnumerable<ResultViewModel> GetExamResults(int StudentId)
        {
            try
            {
                var examResults = _unitOfWork.GenericRepository<ExamResults>().GetAll().
                    Where(e => e.StudentsId == StudentId);
                var students = _unitOfWork.GenericRepository<Students>().GetAll();
                var exams = _unitOfWork.GenericRepository<Exams>().GetAll();
                var qnas = _unitOfWork.GenericRepository<QnAs>().GetAll();
                var requiredData = examResults.Join(students, er => er.StudentsId, s => s.Id,
                    (er, st) => new { er, st }).Join(exams, erj => erj.er.ExamsId, ex => ex.Id,
                    (erj, ex) => new { erj, ex }).Join(qnas, exj => exj.erj.er.QnAsId, q => q.Id,
                    (exj, q) => new ResultViewModel()
                    {
                        StudentId=StudentId,
                        ExamName=exj.ex.Title,
                        TotalQuestion=examResults.Count(a=>a.StudentsId==StudentId 
                        && a.ExamsId==exj.ex.Id),
                        CorrectAnswer=examResults.Count(a => a.StudentsId == StudentId && a.ExamsId == exj.ex.Id
                        && a.Answer==q.Answer),
                        WrongAnswer= examResults.Count(a => a.StudentsId == StudentId && a.ExamsId == exj.ex.Id
                        && a.Answer != q.Answer)



                    });
                return requiredData;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
            return Enumerable.Empty<ResultViewModel>();
        }

        public StudentsViewModel GetStudentDetails(int StudentId)
        {
            try
            {
                var student = _unitOfWork.GenericRepository<Students>().GetById(StudentId);
                return student != null ? new StudentsViewModel(student) : null;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public bool SetExamResult(AttendExamViewModel vm)
        {
            try
            {
                foreach (var item in vm.QnAs)
                {
                    ExamResults examResult = new ExamResults();
                    examResult.StudentsId = vm.StudentId;
                    examResult.QnAsId = item.Id;
                    examResult.ExamsId = item.ExamsId;
                    examResult.Answer = item.SelectedAnswer;
                    _unitOfWork.GenericRepository<ExamResults>().AddAsync(examResult);
                }
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return false;
        }

        public bool SetGroupIdToStudents(GroupViewModel vm)
        {
            try
            {
                foreach (var item in vm.studentCheckList)
                {
                    var student = _unitOfWork.GenericRepository<Students>().GetById(item.Id);
                    if (item.Selected)
                    {
                        student.GroupsId = vm.Id;
                        _unitOfWork.GenericRepository<Students>().Update(student);
                    }
                    else
                    {
                        if (student.GroupsId == vm.Id)
                        {
                            student.GroupsId = 0;
                        }
                    }
                    _unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
            return false;
        }

        public async Task<StudentsViewModel> UpdateAsync(StudentsViewModel vm)
        {
            try
            {
                Students obj = _unitOfWork.GenericRepository<Students>().GetById(vm.Id);
                obj.Name = vm.Name;
                obj.UserName = vm.UserName;
                obj.PictureFileName = vm.PictureFileName != null ?
                    vm.PictureFileName : obj.PictureFileName;
                obj.CvFileName = vm.CvFileName != null ?
                    vm.CvFileName : obj.CvFileName;
                obj.Contact = vm.Contact;
                await _unitOfWork.GenericRepository<Students>().UpdateAsync(obj);


            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }

            return vm;
           
        }
    }
}
