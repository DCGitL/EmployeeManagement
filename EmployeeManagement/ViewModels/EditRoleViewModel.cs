using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        [Display(Name ="Role ID")]
        public string Id { get; set; }
        [Required(ErrorMessage ="Role Name is required")]
        public string  RoleName { get; set; }

        public List<String> Users { get; set; }
    }
}
