using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.Utilities.Exceptions;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Persistence.DAL;
using FinalProjectFb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProjectFb.Web.Controllers
{
	public class JobController : Controller
	{
		private readonly IJobService _service;
		private readonly ICompanyRepository _company;
        private readonly AppDbContext _context;

        public JobController(IJobService service, ICompanyRepository company,AppDbContext context)
		{
			_service = service;
			_company = company;
            _context = context;
        }
		
		public async Task<IActionResult> Index(int id, int page = 1, int take = 5)
		{
			PaginateVM<Job> vm = await _service.GetAllAsync(id, page, take);
			vm.CompanyId = id;
			if (vm.Items == null) return NotFound();
			return View(vm);
		}

        //public async Task<IActionResult> AllJob()
        //{

        //              var result = await _service.AllJobAsync();

        //              if (result != null)
        //              {
        //                  return View(result);
        //              }
        //              else
        //              {
        //                  return NotFound();
        //              }



        //      }

        public async Task<IActionResult> AllJob(int? categoryId,int? order,int rangeValue = 50, int priceFrom = 0, int priceTo = 100, int page = 1, int take = 5)
        {
            IQueryable<Job> query = _context.Jobs.Include(j => j.Images).Include(j=>j.Category).Include(j=>j.Company).ThenInclude(c=>c.CompanyCities).ThenInclude(cc=>cc.City).AsQueryable();
            IQueryable<Category> querya = _context.Categories.AsQueryable();

            switch (order)
            {
                case 1:
                    query = query.OrderBy(j => j.Name);
                    break;
                case 2:
                    query = query.OrderByDescending(j => j.Salary);
                    break;
                case 3:
                    query = query.OrderByDescending(j => j.Id);
                    break;
            }
			if (categoryId!=null)
			{
				query =  query.Where(c => c.CategoryId == categoryId);
			}

            rangeValue = Math.Clamp(rangeValue, 0, 10000);
            priceFrom = Math.Clamp(priceFrom, 0, 10000);
            priceTo = Math.Clamp(priceTo, 0, 10000);

            var allJobResult = await _service.AllJobAsync(page,take);

            if (allJobResult != null)
            {
                var model = new AllJobVM
                {
                    Jobs = await query.ToListAsync(),
                    //Categories = allJobResult.Categories,
					Categories=await querya.ToListAsync(),
                    Companies = allJobResult.Companies,
                    RangeValue = rangeValue,
                    PriceFrom = priceFrom,
                    PriceTo = priceTo
                };

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }


		public async Task<IActionResult> CategoryJobs(int id, int page = 1, int take = 5)
		{
			PaginateVM<Job> job = await _service.GetCategoryId(id, page, take);
            job.CategoryId = id;
            if (job.Items == null) return NotFound();
            return View(job);
        }
        public async Task<IActionResult> Detail(int id)
		{
			return View(await _service.DetailAsync(id));
		}
		public async Task<IActionResult> Update(int id)
		{
			return View(await _service.UpdatedAsync(id));
		}
		[HttpPost]
		public async Task<IActionResult> Update(int id, UpdateJobVM updateVm)
		{
			if (await _service.UpdateAsync(updateVm, ModelState, id)) return RedirectToAction(nameof(Index));
			return View(await _service.UpdatedAsync(id));
		}


		//public async Task<IActionResult> Create(int companyId)
		//{
		//	CreateJobVM createJobVM = new CreateJobVM();
		//	createJobVM.CompanyId = companyId;
		//	createJobVM = await _service.CreatedAsync(createJobVM);
		//	return View(createJobVM);
		//}

		//[HttpPost]
		//public async Task<IActionResult> Create(CreateJobVM createJobVM)
		//{
		//	if (User.Identity.IsAuthenticated)
		//	{

		//		string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


		//		Company company = await _company.GetByExpressionAsync(x => x.AppUserId == userId, isDeleted: false);


		//		createJobVM.CompanyId = company?.Id;
		//	}

		//	if (await _service.CreateAsync(createJobVM, ModelState))
		//		return RedirectToAction("Index", "Home", new { id = createJobVM.CompanyId });

		//	return View(await _service.CreatedAsync(createJobVM));
		//}


		public async Task<IActionResult> Create(int companyid)
		{
			CreateJobVM vm = new CreateJobVM
			{
				CompanyId = companyid,
			};
			
			vm = await _service.CreatedAsync(vm);
			return View(vm);
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreateJobVM vm)
		{
			if (await _service.Create(vm, ModelState))
				return RedirectToAction(nameof(Index), new { id = vm.CompanyId });
			return View(await _service.CreatedAsync(vm));
		}

		//public async Task<IActionResult> Update(int id)
		//{
		//	return View(await _service.UpdatedAsync(new UpdateJobVM(), id));
		//}
		//[HttpPost]
		//public async Task<IActionResult> Update(int id, UpdateJobVM updateVm)
		//{
		//	if (await _service.UpdateAsync(updateVm, ModelState, id)) return RedirectToAction(nameof(Index));
		//	return View(await _service.UpdatedAsync(updateVm, id));
		//}
		//public async Task<IActionResult> Delete(int id)
		//{
		//	await _service.DeleteAsync(id);
		//	return RedirectToAction("Index","Home");
		//}
		//public async Task<IActionResult> SoftDelete(int id)
		//{
		//	await _service.SoftDeleteAsync(id);
		//	return RedirectToAction(nameof(Index));
		//}
		//public async Task<IActionResult> ReverseDelete(int id)
		//{
		//	await _service.ReverseDeleteAsync(id);
		//	return RedirectToAction("Index", "Home");
		//}
		//public async Task<IActionResult> Sumbit(int id)
		//{
		//	await _service.Submit(id);
		//	return RedirectToAction("Index", "Home");
		//}


	}
}
