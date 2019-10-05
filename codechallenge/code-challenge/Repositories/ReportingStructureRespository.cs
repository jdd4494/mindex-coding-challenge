using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class ReportingStructureRespository : IReportingStructureRepository
    {
        private readonly ReportingStructureContext _reportingStructureContext;
        private readonly ILogger<IReportingStructureRepository> _logger;

        public ReportingStructureRespository(ILogger<IReportingStructureRepository> logger, ReportingStructureContext reportingStructureContext)
        {
            _reportingStructureContext = reportingStructureContext;
            _logger = logger;
        }

        public ReportingStructure Add(ReportingStructure reportingStructure)
        {
            Console.WriteLine("[Repo] Adding: " + reportingStructure);
            reportingStructure.Employee.EmployeeId = Guid.NewGuid().ToString();
            _reportingStructureContext.ReportingStructures.Add(reportingStructure);
            return reportingStructure;
        }

        public ReportingStructure GetById(string id)
        {
            ReportingStructure report = _reportingStructureContext.ReportingStructures
                .Where(e => e.Employee.EmployeeId == id)
                .Include(e => e.Employee)
                .Include(e => e.Employee.DirectReports)
                .FirstOrDefault();

            return report;
        }

        public Task SaveAsync()
        {
            return _reportingStructureContext.SaveChangesAsync();
        }

        public ReportingStructure Remove(ReportingStructure reportingStructure)
        {
            return _reportingStructureContext.Remove(reportingStructure).Entity;
        }
    }
}
