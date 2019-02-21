using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeConsumer.Models
{
    public class Employee
    {
        public int ID { get; set; }
        [Required]
        public string FirstName { set; get; }
        public string LastName { set; get; }
    }
}
