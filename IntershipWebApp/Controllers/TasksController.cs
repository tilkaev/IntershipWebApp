using IntershipWebApp.Data;
using IntershipWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IntershipWebApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly AppDBContext _context;

        public TasksController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
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
                Text = "Выберите статус"
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
                Text = "Статус задачи"
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(_Task task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("List", "Tasks");
        }


        [HttpGet]
        // GET: TasksController/Edit/5
        public IActionResult Edit(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            ViewBag.StatusId = GetStatuses();
            if (task == null)
            {
                return View("TaskNotFound");
            }
            return View(task);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(_Task task)
        {
            _context.Attach(task);
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return RedirectToAction("List", "Tasks");
        }


        [HttpDelete]
        // GET: TasksController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
