using PairEmployees.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairEmployees
{
    class StartUp
    {
        static readonly string filePath = Directory.GetCurrentDirectory() + "\\Employees.txt";

        static void Main(string[] args)
        {
            var tmp = File.Exists(filePath);
            if (File.Exists(filePath))
            {
                string line;
                List<Employee> employees = new List<Employee>();

                using (StreamReader file = new StreamReader(filePath))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] lineItems = line.Split(new string[] { ", " }, StringSplitOptions.None);
                        if (lineItems != null && lineItems.Length == 4)
                        {
                            var emp = new Employee(lineItems[0], lineItems[1], lineItems[2], lineItems[3]);
                            employees.Add(emp);
                        }
                    }
                }

                if (employees != null && employees.Count > 0)
                {

                    //var pairByProject = new Dictionary<int, KeyValuePair<int, int>>();
                    List<EmployeesPair> pairs = new List<EmployeesPair>();
                    foreach (var emp1 in employees)
                    {
                        foreach (var emp2 in employees)
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

                                if (pair != null && !pairs.Any(x => ((x.employeeOne == pair.employeeOne && x.employeeTwo == pair.employeeTwo)
                                                            || (x.employeeOne == pair.employeeTwo && x.employeeTwo == pair.employeeOne)
                                                            && x.projectId == pair.projectId && x.teamDays == pair.teamDays)))
                                {
                                    var find = pairs.Where(x => ((x.employeeOne == pair.employeeOne && x.employeeTwo == pair.employeeTwo)
                                                            || (x.employeeOne == pair.employeeTwo && x.employeeTwo == pair.employeeOne))).FirstOrDefault();
                                    if(find != null)
                                    {
                                        var index = pairs.IndexOf(find);
                                        pairs[index].teamDays += pair.teamDays;
                                    }
                                    else
                                    {
                                        pairs.Add(pair);
                                    }

                                }
                            }
                        }
                    }

                    //var grouped = pairs.OrderByDescending(x => x.teamDays)
                    //                        .GroupBy(x => new { x.employeeOne, x.employeeTwo })
                    //                        .Select(g => new {
                    //                            ProjectId = g.Key,
                    //                            Employees = g.Select(emp => new {
                    //                                emp.employeeOne,
                    //                                emp.employeeTwo,
                    //                                emp.teamDays
                    //                            })
                    //                        });
                    //foreach (var pair in pairs)
                    //{
                    //    foreach (var pairCompare in pairs)
                    //    {
                    //        if(pair.employeeOne == pairCompare.employeeTwo && pair.employeeTwo == pairCompare.employeeOne)
                    //        {

                    //        }
                    //    }
                    //}
                    var result = pairs.OrderByDescending(x => x.teamDays).ToList();

                    if(result != null && result.Count > 2)
                    {
                        Console.WriteLine($"Employee 1: {result[0].employeeOne}, Employee 2: {result[0].employeeTwo}, work days in team: {result[0].teamDays}.");
                    }
                    Console.ReadKey();
                }
            }
        }
    }
}
