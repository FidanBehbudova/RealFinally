
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.Utilities.Exceptions;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Persistence.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FinalProjectFb.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _service;
        private readonly AppDbContext _context;
        private readonly IJobService _job;

        public HomeController(IHomeService service,AppDbContext context,IJobService job)
        {
            _service = service;
            _context = context;
            _job = job;
        }

        public async Task<IActionResult> Index(string search)
        {
            try
            {
                IQueryable<Job> queryable = _context.Jobs.Include(j => j.Images).AsQueryable();

                if (!String.IsNullOrEmpty(search))
                {
                    queryable = queryable.Where(j => j.Name.ToLower().Contains(search.ToLower()));
                }

                HomeVM vm = await _service.GetAllAsync();
                return View(vm);
            }
            catch (Exception ex)
            {
               
                return View("Error", new WrongRequestException(""));
            }
        }

        public async Task<IActionResult> Search(string job)
        {
            if (string.IsNullOrWhiteSpace(job) || job.Length < 3)
            {
                TempData["ErrorMessage"] = "Search term must be at least 3 characters long.";
                return RedirectToAction(nameof(Index));
            }
            List<Job> jobs = await _job.GetJobs(job);
            return View(jobs);
        }



    }
}
