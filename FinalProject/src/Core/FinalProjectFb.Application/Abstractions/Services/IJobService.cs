using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.Abstractions.Services
{
    public interface IJobService
    {
        Task<AllJobVM> AllJobAsync();
        //Task<CreateJobVM> CreatedAsyc(CreateJobVM vm);
        //Task<bool> CreateAsync(CreateJobVM createJobVM, ModelStateDictionary ms);
        Task<bool> UpdateAsync(UpdateJobVM JobVm, ModelStateDictionary modelState, int id);
		Task<UpdateJobVM> UpdatedAsync(int id);
		Task DeleteAsync(int id);
        Task<PaginateVM<Job>> GetCategoryId(int id, int page = 1, int take = 10);
        Task<Job> GetJob(string job);
        Task<List<Job>> GetJobs(string job);
        Task ReverseDeleteAsync(int id);
		Task Submit(int id);
		Task SoftDeleteAsync(int id);
		Task<JobDetailVM> DetailAsync(int id);
        Task<bool> Create(CreateJobVM createJobVM, ModelStateDictionary modelstate);
        Task<CreateJobVM> CreatedAsync(CreateJobVM vm);
        Task<PaginateVM<Job>> GetAllAsync(int id, int page = 1, int take = 5);
		//Task<JobItemVM> SortingAsync(int key = 1, int page = 1, int id=1);
	}
}
