using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
namespace WebApp.ViewModels
{
    public class JobFormViewModel
    {
        public JobOffer JobOffer { get; set; }
        public JobApplication JobApplication { get; set; }
    }
}
