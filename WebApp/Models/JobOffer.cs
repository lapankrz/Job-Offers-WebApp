using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class JobOffer
    {
        public int Id { get; set; }
        [Required]
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public Company Company { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public int SalaryFrom { get; set; }
        [Required]
        [SalaryToIsBigger("SalaryFrom")]
        public int SalaryTo { get; set; }
        public DateTime CreationDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [IsDateAfter("CreationDate")]
        [Required]
        public DateTime ValidUntil { get; set; }
        public List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    }
}
