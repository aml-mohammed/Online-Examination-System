using OnlineExamination.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamination.ViewModels
{
   public class UsersViewModel
    {
        public UsersViewModel()
        {

        }
        public UsersViewModel(Users Model)
        {
            Id = Model.Id;
            Name = Model.Name;
            UserName = Model.UserName;
            Password = Model.Password;
            Role = Model.Role;

        }
        public Users ConvertViewModel(UsersViewModel vm)
        {
            return new Users
            {
                Id = vm.Id,
                Name = vm.Name,
                UserName = vm.UserName,
                Password = vm.Password,
                Role = vm.Role
            };
        }
        public int Id { get; set; }
        [Required]
        [Display(Name="Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public int Role { get; set; }
        public List<UsersViewModel> UserList { get; set; }
        public int TotalCount { get; set; }
    }
}
