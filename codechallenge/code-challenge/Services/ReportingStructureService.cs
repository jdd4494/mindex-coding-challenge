using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IReportingStructureRepository _reportingStructureRepository;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IReportingStructureRepository reportingStructureRepository)
        {
            _reportingStructureRepository = reportingStructureRepository;
            _logger = logger;
        }

        public ReportingStructure Create(ReportingStructure reportingStructure)
        {
            if(reportingStructure != null)
            {
                _reportingStructureRepository.Add(reportingStructure);
                _reportingStructureRepository.SaveAsync().Wait();
            }

            return reportingStructure;
        }

        public ReportingStructure GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _reportingStructureRepository.GetById(id);
            }

            return null;
        }

    }
}
