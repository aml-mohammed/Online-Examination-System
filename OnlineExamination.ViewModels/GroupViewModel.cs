using OnlineExamination.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamination.ViewModels
{
 public class GroupViewModel
    {
        public GroupViewModel()
        {

        }
        public GroupViewModel(Groups model)
        {
            Id = model.Id;
            Name = model.Name ?? "";
            Description = model.Description ?? "";
            UsersId = model.UsersId;
        }
        public Groups convertGroupViewModel(GroupViewModel vm)
        {
            return new Groups
            {
                Id = vm.Id,
                Name = vm.Name ?? "",
                Description = vm.Description ?? "",
                UsersId = vm.UsersId

            };
        }
        public int Id { get; set; }
        [Required]
        [Display(Name = " Group Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = " Description")]
        public string Description { get; set; }
        public int UsersId { get; set; }
        public List<GroupViewModel> GroupList { get; set; }
        public int TotalCount { get; set; }
        public List<StudentCheckBoxListViewModel> studentCheckList { get; set; }

    }
}
