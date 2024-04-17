using ASP_MVC.Data;
using ASP_MVC.Data.Models;
using ASP_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace ASP_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly MydbContext _context;

        public HomeController(ILogger<HomeController> logger, MydbContext mydbContext)
        {
            _logger = logger;
            _context = mydbContext;
            _context.Statuses.ToList();
            _context.Staff.ToList();
            _context.Problems.ToList();
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Application()
        {

            


            if (User.IsInRole("Диспетчер"))
            {
                ViewData["Apllications"] = _context.Applications.ToList();
            }
            else
            {
                ViewData["Apllications"] = _context.Applications.Where(x => x.IdStaffNavigation.Fullname == User.Identity.Name).ToList();
            }

            return View();
        }

        [Authorize]
        public IActionResult CreateApplication() 
        {
            ViewData["Problems"] = _context.Problems.ToList();
            ViewData["Staff"] = _context.Staff.Where(x => x.Role == 1).ToList();
            return View();
        }

        [Authorize]
        public IActionResult Statistic()
        {
            _context.Problems.ToList();
            _context.Statuses.ToList();

            int countApp = _context.Applications.Where(x => x.Status == 3).Count();

            float avgTime = (float)_context.Applications.Where(x => x.TimeWork != 0).Sum(x => x.TimeWork) / (float)_context.Applications.Where(x => x.TimeWork != 0).Count();

            var result = _context.Applications
            .GroupBy(x => x.IdProblem)
            .Select(g => new ProblemModel
            {
                IdProblem = (int)g.Key,
                ProblemName = _context.Problems.FirstOrDefault(x => x.IdProblem == g.Key).NameProblem,
                ApplicationCount = g.Count()
            }).ToList();


            

            ViewData["Count"] = countApp;
            ViewData["Avg"] = avgTime;
            ViewData["Results"] = result;

            return View();
        }

        [Authorize] 
        public IActionResult CreateApp(string fullname, int type, int staff, string equipment, int time)
        {

            int id = _context.Applications.OrderByDescending(e => e.IdApplication).FirstOrDefault().IdApplication;

            _context.Add(new Application
            {
                IdApplication = id + 1,
                Status = 1,
                DateAddition = DateOnly.FromDateTime(DateTime.Today),
                FullnameClient = fullname,
                Comment = "",
                WorkStatus = "В ожидании",
                IdStaff = staff,
                IdProblem = type,
                NameEquipment = equipment,
                Cost = 0,
                DateEnd = DateOnly.FromDateTime(DateTime.Today.AddDays(time)),
                TimeWork = 0
            });

            _context.SaveChanges();

            _context.Statuses.ToList();
            ViewData["Apllications"] = _context.Applications.ToList();
            return RedirectToAction("Application", "Home");
        }

        [Authorize]
        public IActionResult CurrentApplicationEx(int applicationId) 
        {
            ViewData["CurrentApplication"] = (ASP_MVC.Data.Models.Application)_context.Applications.SingleOrDefault(app => app.IdApplication == applicationId);

            return View();
        }

        [Authorize]
        public IActionResult CurrentApplication(int applicationId)
        {

            _context.Statuses.ToList();
            ViewData["SatusList"] = _context.Statuses.ToList();

            ViewData["Problems"] = _context.Problems.ToList();
            ViewData["Staff"] = _context.Staff.Where(x => x.Role == 1).ToList();

            // Idstatus StatusName

            ViewData["CurrentApplication"] = (ASP_MVC.Data.Models.Application)_context.Applications.SingleOrDefault(app => app.IdApplication == applicationId);

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdateApplication(int id, string fullname, int status, int problem, int staff, string equipment, DateTime dateAddition, DateTime dateEnd)
        {

            Application app = _context.Applications.FirstOrDefault(x =>  x.IdApplication == id);

            if (app == null)
            {
                _context.Statuses.ToList();
                ViewData["Apllications"] = _context.Applications.ToList();
                return RedirectToAction("Application");
            }

            app.FullnameClient = fullname;
            app.Status = status;
            app.IdProblem = problem;
            app.IdStaff = staff;
            app.NameEquipment = equipment;
            app.DateAddition = DateOnly.FromDateTime(dateAddition);
            app.DateEnd = DateOnly.FromDateTime(dateEnd);

            _context.SaveChanges();

            _context.Statuses.ToList();
            ViewData["Apllications"] = _context.Applications.ToList();

            return RedirectToAction("Application");
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdateApplicationEx(int id, string comment, int status, int cost, int timework) 
        {

            _context.Problems.ToList();
            _context.Statuses.ToList();

            Application application = _context.Applications.FirstOrDefault(x => x.IdApplication == id);

            if (application == null)
            {
                ViewData["Apllications"] = _context.Applications.Where(x => x.IdStaffNavigation.Fullname == User.Identity.Name).ToList();
                return RedirectToAction("Application");
            }


            application.Comment = comment;
            application.Cost = cost;
            if (status == 1) application.WorkStatus = "В ожидании";
            else if (status == 2) application.WorkStatus = "В работе";
            else application.WorkStatus = "Выполнено";
            application.TimeWork = timework;

            _context.SaveChanges();

            ViewData["Apllications"] = _context.Applications.Where(x => x.IdStaffNavigation.Fullname == User.Identity.Name).ToList();
            return RedirectToAction("Application");
        }


        [Authorize]
        public IActionResult SearchApplication(string searchString, string type)
        {
            List<Application> applications;

            _context.Statuses.ToList();
            _context.Staff.ToList();

            if (User.IsInRole("Диспетчер"))
            {
                applications = _context.Applications.ToList();
            }
            else
            {
                applications = _context.Applications.Where(x => x.IdStaffNavigation.Fullname == User.Identity.Name).ToList();
            }

            if (!string.IsNullOrEmpty(searchString)) 
            {
                applications = applications.Where(app => app.FullnameClient.Contains(searchString)).ToList();
            }

            if (type == "1")
            {
                applications = applications.Where(x => x.Status == 1).ToList();
            }
            if (type == "2") 
            {
                applications = applications.Where(x => x.Status == 2).ToList();
            }
            if (type == "3")
            {
                applications = applications.Where(x => x.Status == 3).ToList();
            }

            ViewData["Apllications"] = applications;

            return View("Application");
        }

        [Authorize]
        public async Task<IActionResult> DeleteApplication(int applicationId) 
        {
            var recordToDelete = await _context.Applications.FirstOrDefaultAsync(x => x.IdApplication == applicationId);

            if (recordToDelete != null) 
            { 
                _context.Applications.Remove(recordToDelete);

                await _context.SaveChangesAsync();
            }

            _context.Statuses.ToList();
            ViewData["Apllications"] = _context.Applications.ToList();

            return View("Application");
        }

        [Authorize]
        public async Task<IActionResult> EditApplication()
        {
            _context.Statuses.ToList();
            ViewData["Apllications"] = _context.Applications.ToList();
            return View("Application");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
