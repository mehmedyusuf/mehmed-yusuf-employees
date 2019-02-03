using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairEmployees.Models
{
    class Employee
    {
        public int EmpId { get; private set; }
        public int ProjectId { get; private set; }
        public DateTime DateFrom { get; private set; }
        public DateTime DateTo { get; private set; }
        public double Duration { get; private set; }

        public Employee(string empId, string projectId, string dateFrom, string dateTo)
        {
            this.EmpId = Convert.ToInt32(empId);
            this.ProjectId = Convert.ToInt32(projectId);
            this.DateFrom = Convert.ToDateTime(dateFrom);
            this.DateTo = (dateTo.ToLower() == "null" ? DateTime.Now.Date : Convert.ToDateTime(dateTo));
            this.Duration = (this.DateTo - this.DateFrom).TotalDays;
        }
    }
}
