using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using EmployeeApi.Models;

namespace EmployeeApi.Repositories
{
    public class EmployeeRepository
    {
        public List<Employee> Employees;

        public EmployeeRepository()
        {
            fakeData();
        }

        private void fakeData()
        {
            Employees = new Faker<Employee>()
                .RuleFor(e => e.ID, f => int.Parse(f.Random.Replace("#####")))
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .Generate(10).ToList();
        }
    }
}
