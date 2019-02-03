using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairEmployees.Models
{
    class EmployeesPair
    {
        public int projectId { get; set; }
        public int employeeOne { get; set; }
        public int employeeTwo { get; set; }
        public int teamDays { get; set; }

        public EmployeesPair(int projectId, int empOne, int empTwo, int teamDays)
        {
            this.projectId = projectId;
            this.employeeOne = empOne;
            this.employeeTwo = empTwo;
            this.teamDays = teamDays;
        }
    }
}
