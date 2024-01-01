using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamination.DataAccess
{
   public class Students
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Contact { get; set; }

        public string CvFileName { get; set; }

        public string PictureFileName { get; set; }

        public int GroupsId { get; set; }

        public Groups Groups { get; set; }
        public ICollection<ExamResults> ExamResults { get; set; } = new HashSet<ExamResults>();


    }

}
