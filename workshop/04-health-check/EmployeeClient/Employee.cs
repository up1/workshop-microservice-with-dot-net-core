using System.Runtime.Serialization;

namespace EmployeeClient
{
    [DataContract(Name = "employee")]
    public class Employee
    {
        [DataMember(Name = "id")]
        public int ID { get; set; }
        [DataMember(Name = "firstName")]
        public string FirstName { set; get; }
        [DataMember(Name = "lastName")]
        public string LastName { set; get; }
    }
}