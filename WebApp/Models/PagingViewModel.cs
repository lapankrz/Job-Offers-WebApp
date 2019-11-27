using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class PagingViewModel
    {
        public IEnumerable<JobOffer> Offers { get; set; }
        public int TotalPage { get; set; }
    }
}
