using System;
using System.ComponentModel.DataAnnotations;


namespace EmployeeApi.Models
{
    public class Employee
    {
        public int ID { get; set; }
        [Required]
        public string FirstName { set; get; }
        public string LastName { set; get; }
    }
}
