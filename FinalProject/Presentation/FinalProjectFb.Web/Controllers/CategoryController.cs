using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectFb.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Detail(int id, int page = 1, int take = 5)
        {
            PaginateVM<Category> vm = await _service.Detail(id, page, take);
            vm.JobId = id;
            if (vm.Items == null) return NotFound();
            return View(vm);
        }
    }
}
