using OnlineExamination.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamination.ViewModels
{
    public class QnAsViewModel
    {
        public QnAsViewModel()
        {
                
        }
        public QnAsViewModel(QnAs model)
        {
            Id = model.Id;
            ExamsId = model.ExamsId;
            Qustion = model.Qustion ?? "";
            Option1 = model.Option1 ?? "";
            Option2 = model.Option2 ?? "";
            Option3 = model.Option3 ?? "";
            Option4 = model.Option4 ?? "";
            Answer = model.Answer;


        }
        public QnAs ConvertViewModel(QnAsViewModel vm)
        {
            return new QnAs
            {
                Id = vm.Id,
                ExamsId = vm.ExamsId,
                Qustion = vm.Qustion ?? "",
                Option1 = vm.Option1 ?? "",
                Option2 = vm.Option2 ?? "",
                Option3 = vm.Option3 ?? "",
                Option4 = vm.Option4 ?? "",
                Answer = vm.Answer,
            };
        }
        public int Id { get; set; }
        [Required]
        [Display(Name = " Exam Name")]
        public int ExamsId { get; set; }
        [Required]
        [Display(Name = " Question")]
        public string Qustion { get; set; }
        [Required]
        [Display(Name = " Answer")]
        public int Answer { get; set; }
        [Required]
        [Display(Name = " option1")] 
        public string Option1 { get; set; }
        [Required]
        [Display(Name = " option2")]
        public string Option2 { get; set; }
        [Required]
        [Display(Name = " option3")]
        public string Option3 { get; set; }
        [Required]
        [Display(Name = " option4")]
        public string Option4 { get; set; }
        public List<QnAsViewModel> QnAsList { get; set; }
        public IEnumerable<Exams> ExamList { get; set; }
        public int Totalcount { get; set; }
        public int SelectedAnswer { get; set; }

    }
}
