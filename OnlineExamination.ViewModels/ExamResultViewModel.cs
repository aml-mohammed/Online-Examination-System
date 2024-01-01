using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamination.ViewModels
{
  public class ExamResultViewModel
    {
        public int id { get; set; }

        public int StudentsId { get; set; }
        public int ExamsId { get; set; }
        public int QnAsId { get; set; }
        public int Answer { get; set; }
    }
}
