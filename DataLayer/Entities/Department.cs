using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Remote(action: "ValidateDepartmentName", controller:"Departments", AdditionalFields="Id")]
        public string DepartmentName { get; set; }

        public ICollection<Employee> ?Employees { get; set; }
    }

}
