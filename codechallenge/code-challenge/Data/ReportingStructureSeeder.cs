using challenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Data
{
    public class ReportingStructureSeeder
    {
        private ReportingStructureContext _reportingStructureContext;
        private EmployeeContext _employeeContext;
        private CompensationContext _compensationContext;
        private List<Employee> _employees;

        private const String REPORTING_STRUCTURE_SEED_DATA_FILE = "resources/EmployeeSeedData.json";

        public ReportingStructureSeeder(ReportingStructureContext reportingStructureContext, EmployeeContext employeeContext, CompensationContext compensationContext)
        {
            _reportingStructureContext = reportingStructureContext;
            _employeeContext = employeeContext;
            _compensationContext = compensationContext;
            _employees = new List<Employee>();
        }

        public async Task Seed()
        {
            // Load in reporting structure info to reporting structure db
            if (!_reportingStructureContext.ReportingStructures.Any())
            {
                List<ReportingStructure> reportingStructures = LoadReportingStructures();
                _reportingStructureContext.ReportingStructures.AddRange(reportingStructures);
                await _reportingStructureContext.SaveChangesAsync();
            }

            // Load in reporting structure info to employee db
            if (!_employeeContext.Employees.Any())
            {
                _employeeContext.Employees.AddRange(_employees);

                await _employeeContext.SaveChangesAsync();
            }

            // Load compensation data
            if (!_compensationContext.Compensations.Any())
            {
                // Arrange
                var employee = new Employee()                {
                    EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                    Department = "Engineering",
                    FirstName = "John",
                    LastName = "Lennon",
                    Position = "Development Manager",
                };

                var compensation = new Compensation()                {
                    Employee = employee,
                    Salary = 100000,
                    EffectiveDate = new DateTime()
                };

                List<Compensation> compensations = new List<Compensation>();
                compensations.Add(compensation);
                _compensationContext.Compensations.AddRange(compensations);

                await _compensationContext.SaveChangesAsync();
            }
        }

        private List<ReportingStructure> LoadReportingStructures()
        {
            // Reading from the same employee json
            using (FileStream fs = new FileStream(REPORTING_STRUCTURE_SEED_DATA_FILE, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                List<Employee> employees = serializer.Deserialize<List<Employee>>(jr);
                FixUpReferences(employees);

                List<ReportingStructure> reportingStructures = new List<ReportingStructure>();

                for(int i = 0; i < employees.Count; i++)
                {
                    ReportingStructure reportingStructure = new ReportingStructure();
                    reportingStructure.Employee = employees[i];

                    if (employees[i].DirectReports != null)
                    {
                        //Console.WriteLine("Adding employee with num reports: " + employees[i].DirectReports.Count);
                        reportingStructure.NumberOfReports = employees[i].DirectReports.Count;
                    }
                    else
                    {
                        //Console.WriteLine("Adding employee with num reports: " + 0);
                        reportingStructure.NumberOfReports = 0;
                    }

                    // Add report to the list
                    reportingStructures.Add(reportingStructure);
                }

                _employees = employees;
                return reportingStructures;
            }
        }

        private void FixUpReferences(List<Employee> employees)
        {
            var employeeIdRefMap = from employee in employees
                                select new { Id = employee.EmployeeId, EmployeeRef = employee };

            employees.ForEach(employee =>
            {
                
                if (employee.DirectReports != null)
                {
                    var referencedEmployees = new List<Employee>(employee.DirectReports.Count);
                    employee.DirectReports.ForEach(report =>
                    {
                        var referencedEmployee = employeeIdRefMap.First(e => e.Id == report.EmployeeId).EmployeeRef;
                        referencedEmployees.Add(referencedEmployee);
                    });
                    employee.DirectReports = referencedEmployees;
                }
            });
        }
    }
}
