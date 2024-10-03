
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DataLayer.Models;
using DataLayer.Entities;
using DataLayer.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using DataLayer.Entities;

namespace HrSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender emailSender;
        
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            this.emailSender = emailSender;

        }


        

        //role claim

        public IActionResult Index()
        {
            emailSender.SendOutlookEmail("abdallah3162@outlook.com", "subject");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
