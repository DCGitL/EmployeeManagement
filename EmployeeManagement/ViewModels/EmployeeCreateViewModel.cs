using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeCreateViewModel
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+[.])+[a-z]{2,5}$", ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }
        [Required]
        public Dept? Department { get; set; }

        public List<IFormFile> Photos { get; set; }
    }
}
