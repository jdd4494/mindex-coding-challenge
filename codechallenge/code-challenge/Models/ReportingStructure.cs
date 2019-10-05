using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class ReportingStructure
    {
        [Key] // Prob not needed since id is already assumed to be primary key
        public int Id { get; set; }
        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }
    }
}
