using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamination.DataAccess
{
  public  class ExamResults
    {
        public int id { get; set; }
        public int StudentsId { get; set; }
        public Students Students { get; set; }
        public int ExamsId { get; set; }
        public Exams Exams { get; set; }
        public int QnAsId { get; set; }
        public QnAs QnAs { get; set; }
        public int Answer { get; set; }
    }
}
