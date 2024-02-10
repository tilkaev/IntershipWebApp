using IntershipWebApp.Data;
using IntershipWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace IntershipWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDBContext _context;

        public HomeController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
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


        public IActionResult List()
        {
            List<_Task> tasks = _context.Tasks
                .Include(s => s.Status)
                .ToList();

            return View(tasks);
        }

        private List<SelectListItem> GetStatuses()
        {
            var lstStatuses = new List<SelectListItem>();

            List<Status> Tasks = _context.Statuses.ToList();

            lstStatuses = Tasks.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "Select Status"
            };

            lstStatuses.Insert(0, defItem);
            return lstStatuses;
        }


        private List<SelectListItem> GetStatuses(int statusId = 1)
        {
            List<SelectListItem> lstStatuses = _context.Statuses
                .Where(s => s.Id == statusId)
                .Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "Select Status"
            };

            lstStatuses.Insert(0, defItem);
            return lstStatuses;
        }

        [HttpGet]
        public IActionResult Create() 
        {
            _Task task = new _Task();
            ViewBag.StatusId = GetStatuses();
            return View(task);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(_Task task)
        {
            _context.Tasks.AddAsync(task);
            _context.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }



    }
}
