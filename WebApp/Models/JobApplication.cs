using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    /// <summary>
    /// Job application class
    /// </summary>
    public class JobApplication
    {
        /// <summary>
        /// id of the application
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// id of the job offer connected to the job application
        /// </summary>
        public int OfferId { get; set; }
        /// <summary>
        /// object of class JobOffer, for which the application is
        /// </summary>
        public JobOffer Offer { get; set; }
        /// <summary>
        /// First name of the applicant
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        /// <summary>
        /// Last name of the applicant
        /// </summary>
        [Required]
        public string LastName { get; set; }
        /// <summary>
        /// Phone number of the applicant
        /// </summary>
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Email address of the applicant
        /// </summary>
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        /// <summary>
        /// If the applicant toggled the contact agreement
        /// </summary>
        [Required]
        public bool ContactAgreement { get; set; }
        /// <summary>
        /// URL of the CV of the applicant
        /// </summary>
        public string CvUrl { get; set; }
    }
}
