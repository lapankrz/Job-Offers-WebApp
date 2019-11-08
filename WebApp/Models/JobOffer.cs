using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class JobOffer
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public Company Company { get; set; }
        public string Location { get; set; }
        public int SalaryFrom { get; set; }
        public int SalaryTo { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ValidUntil { get; set; }
        public List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    }
}
