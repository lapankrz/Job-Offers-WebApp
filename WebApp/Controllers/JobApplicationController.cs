using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.EntityFramework;
using WebApp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using System.IO;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace WebApp.Controllers
{
    /// <summary>
    /// controller for displaying and modifying job applications
    /// </summary>
    [Authorize]
    [Route("apply")]
    public class JobApplicationController : Controller
    {
        private readonly DataContext context;
        private readonly CloudBlobContainer container;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// constructor for the controller
        /// </summary>
        /// <param name="dataContext">data context for the database</param>
        /// <param name="configuration">configuration</param>
        public JobApplicationController(DataContext dataContext, IConfiguration configuration)
        {
            context = dataContext;
            _configuration = configuration;
            CloudStorageAccount storageAccount = new CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials("lapanstorage",
                                                                        "31P/CvcdO4hXWM6S1EgkArfEGSetAEnr/I12ZZTE0ov34z2bqAADsYHSIOrIdgvAyQil5va+fGYCW3cfjim8iQ=="), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference("applications");
        }
        /// <summary>
        /// Opens a form for applying for a job
        /// </summary>
        /// <param name="id">id of the job offer for which the application will be created</param>
        /// <returns>view with the form</returns>
        [HttpGet]
        public IActionResult Form(int id)
        {
            JobOffer offer = context.JobOffers.Include(o => o.Company).FirstOrDefault(o => o.Id == id);
            return View(offer);
        }
        /// <summary>
        /// Opens a page with details about a job application
        /// </summary>
        /// <param name="id">id of the job offer for which the applications is</param>
        /// <returns>view with the details</returns>
        [HttpGet]
        [Route("details")]
        public IActionResult Details(int id)
        {
            JobApplication application = context.JobApplications.FirstOrDefault(o => o.Id == id);
            JobOffer offer = context.JobOffers.Include(o => o.Company).FirstOrDefault(o => o.Id == application.OfferId);
            ViewBag.offer = offer;
            return View(application);
        }
        /// <summary>
        /// Validates the application form and saves it
        /// </summary>
        /// <param name="application">job application to be saved, obtained from the form</param>
        /// <param name="CV">CV file uploaded by the user</param>
        /// <returns>details view if the form was validated successfully or the form view to change the incorrect data</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveForm([FromForm] JobApplication application, IFormFile CV)
        {
            var offer = context.JobOffers.Include(o => o.Company).Include(o => o.JobApplications).FirstOrDefault(o => o.Id == application.OfferId);
            string type = "";
            if (CV != null)
            {
                type = CV.ContentType.ToLower();
            }
            if (CV == null || CV.Length == 0 || (type != "application/pdf" && type != "application/msword" && type != "application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
            {
                ModelState.AddModelError("cvMessage", "Upload your CV in one of these formats: PDF, DOC, DOCX");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    string name = application.FirstName + " " + application.LastName + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    using (StreamReader streamReader = new StreamReader(CV.OpenReadStream()))
                    {
                        var blob = container.GetBlockBlobReference(name);
                        blob.Properties.ContentType = CV.ContentType;
                        await blob.UploadFromStreamAsync(streamReader.BaseStream);
                        application.CvUrl = blob.Uri.AbsoluteUri;
                    }
                    SendEmail(name);
                    offer.JobApplications.Add(application);
                    context.JobApplications.Add(application);
                    await context.SaveChangesAsync();
                    return View("~/Views/JobOffer/Details.cshtml", offer);
                }
            }
            return View("~/Views/JobApplication/Form.cshtml", offer);
        }

        private async void SendEmail(string name)
        {
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("dx@example.com", "SendGrid WebApp"));

            msg.AddTo(new EmailAddress("k.lapan@student.mini.pw.edu.pl", "Krzysztof Łapan"));

            msg.SetSubject("New CV was sent to Azure Blob Storage!");

            msg.AddContent(MimeType.Text, "Hello!\nNew CV named '" + name + "' was registered.");

            var apiKey = _configuration.GetSection("SENDGRID_API_KEY").Value;
            var client = new SendGridClient(apiKey);
            await client.SendEmailAsync(msg);
        }
    }
}