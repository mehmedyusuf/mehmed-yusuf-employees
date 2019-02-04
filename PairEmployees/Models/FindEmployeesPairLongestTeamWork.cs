using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PairEmployees.Models
{
    public class FindEmployeesPairLongestTeamWork
    {
        public string filePath = Directory.GetCurrentDirectory() + "\\Employees.txt";
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<EmployeesPair> Pairs { get; set; } = new List<EmployeesPair>();

        public FindEmployeesPairLongestTeamWork()
        {
            this.ReadFromFile();
        }
        private void ReadFromFile()
        {
            try
            {
                string line;
                if (File.Exists(filePath))
                {
                    using (StreamReader file = new StreamReader(filePath))
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            string[] lineItems = line.Split(new string[] { ", " }, StringSplitOptions.None);
                            if (lineItems != null && lineItems.Length == 4)
                            {
                                var emp = new Employee(lineItems[0], lineItems[1], lineItems[2], lineItems[3]);
                                this.Employees.Add(emp);
                            }
                        }
                        if (Employees != null && Employees.Count > 0)
                        {
                            this.CalculateTeamWorkPeriodBetweenEmployees();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("File not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Incorrect file data.");
            }
        }

        private void CalculateTeamWorkPeriodBetweenEmployees()
        {
            foreach (var emp1 in Employees)
            {
                foreach (var emp2 in Employees)
                {
                    if (emp1.EmpId != emp2.EmpId && emp1.ProjectId == emp2.ProjectId && emp1.DateFrom < emp2.DateTo && emp2.DateFrom < emp1.DateTo)
                    {
                        EmployeesPair pair = null;

                        if (emp1.DateFrom >= emp2.DateFrom && emp1.DateTo >= emp2.DateTo)
                        {
                            pair = new EmployeesPair(emp1.ProjectId, emp1.EmpId, emp2.EmpId, (emp2.DateTo - emp1.DateFrom).Days);
                        }
                        else if (emp1.DateFrom >= emp2.DateFrom && emp1.DateTo <= emp2.DateTo)
                        {
                            pair = new EmployeesPair(emp1.ProjectId, emp1.EmpId, emp2.EmpId, (emp1.DateTo - emp1.DateFrom).Days);
                        }
                        else if (emp1.DateFrom <= emp2.DateFrom && emp1.DateTo <= emp2.DateTo)
                        {
                            pair = new EmployeesPair(emp1.ProjectId, emp1.EmpId, emp2.EmpId, (emp1.DateTo - emp2.DateFrom).Days);
                        }
                        else if (emp1.DateFrom <= emp2.DateFrom && emp1.DateTo >= emp2.DateTo)
                        {
                            pair = new EmployeesPair(emp1.ProjectId, emp1.EmpId, emp2.EmpId, (emp2.DateTo - emp2.DateFrom).Days);
                        }

                        if (pair != null && !Pairs.Any(x => ((x.employeeOne == pair.employeeOne && x.employeeTwo == pair.employeeTwo)
                                                    || (x.employeeOne == pair.employeeTwo && x.employeeTwo == pair.employeeOne)
                                                    && x.projectId == pair.projectId && x.teamDays == pair.teamDays)))
                        {
                            var find = Pairs.Where(x => ((x.employeeOne == pair.employeeOne && x.employeeTwo == pair.employeeTwo)
                                                    || (x.employeeOne == pair.employeeTwo && x.employeeTwo == pair.employeeOne))).FirstOrDefault();
                            if (find != null)
                            {
                                var index = Pairs.IndexOf(find);
                                Pairs[index].teamDays += pair.teamDays;
                            }
                            else
                            {
                                Pairs.Add(pair);
                            }

                        }
                    }
                }
            }

            if(Pairs != null && Pairs.Count > 0)
            {
                this.OrderPairsByWorkDays();
            }
        }

        private void OrderPairsByWorkDays()
        {
            var orderedPairs = Pairs.OrderByDescending(x => x.teamDays).ToList();

            if (orderedPairs != null && orderedPairs.Count > 2)
            {
                if (orderedPairs[0].teamDays == orderedPairs[1].teamDays)
                {
                    var pairsWithEqualWorkDays = Pairs.Where(x => x.teamDays == orderedPairs[0].teamDays).ToList();
                    this.PrintPairsWithLongestWorkPeriod(pairsWithEqualWorkDays);
                }
                else
                {
                    this.PrintPairsWithLongestWorkPeriod( new List<EmployeesPair> { orderedPairs[0] });
                }
            }
        }

        private void PrintPairsWithLongestWorkPeriod(List<EmployeesPair> pairs)
        {
            Console.WriteLine("Employees pair with the longest time as a team on joint projects.");
            Console.WriteLine(new string('-', 65));
            foreach (var pair in pairs)
            {
                Console.WriteLine($"Employee 1: {pair.employeeOne}, Employee 2: {pair.employeeTwo}, work days in a team: {pair.teamDays}.");
            }
            Console.ReadKey();
        }
    }
}
