using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamination.ViewModels
{
    public class AttendExamViewModel
    {
        public int StudentId { get; set; }
        public string ExamName { get; set; }
        public List<QnAsViewModel> QnAs { get; set; }
        public string message { get; set; }

    }
}
