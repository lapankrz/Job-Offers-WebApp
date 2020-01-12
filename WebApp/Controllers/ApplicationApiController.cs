using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.EntityFramework;
using WebApp.Models;

namespace WebApp.Controllers
{
    /// <summary>
    /// api controller to get a list of job applications from the database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationApiController : ControllerBase
    {
        /// <summary>
        /// DataContext of the app
        /// </summary>
        public DataContext context;
        /// <summary>
        /// Constructor for the controller
        /// </summary>
        /// <param name="dataContext">DataContext of the app</param>
        /// <returns></returns>
        public ApplicationApiController(DataContext dataContext)
        {
            context = dataContext;
        }
        //ajax offer details
        /// <summary>
        /// Gets a list of job applications from the database
        /// </summary>
        /// <param name="id">id of the job offer for which the applications must be obtained</param>
        /// <returns>list of job applications for the job offer</returns>
        [HttpPost("{id}")]
        public IEnumerable<JobApplication> GetJobApplications(int id)
        {
            var applications = context.JobApplications.Where(a => a.OfferId == id).ToList();
            return applications;
        }
    }
}