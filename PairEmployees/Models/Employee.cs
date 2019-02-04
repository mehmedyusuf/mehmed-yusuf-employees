using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairEmployees.Models
{
    public class Employee
    {
        DateTime dateToConversion;
        public int EmpId { get; private set; }
        public int ProjectId { get; private set; }
        public DateTime DateFrom { get; private set; }
        public DateTime DateTo { get; private set; }

        public Employee(string empId, string projectId, string dateFrom, string dateTo)
        {
            if (!DateTime.TryParse(dateTo, out dateToConversion))
                dateToConversion = DateTime.Now;

            this.EmpId = Convert.ToInt32(empId);
            this.ProjectId = Convert.ToInt32(projectId);
            this.DateFrom = Convert.ToDateTime(dateFrom);
            this.DateTo = dateToConversion;
        }
    }
}
