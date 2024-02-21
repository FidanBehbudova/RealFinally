﻿using FinalProjectFb.Application.Abstractions.Repositories;
using FinalProjectFb.Application.Abstractions.Services;
using FinalProjectFb.Application.Utilities.Exceptions;
using FinalProjectFb.Application.ViewModels;
using FinalProjectFb.Domain.Entities;
using FinalProjectFb.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Persistence.Implementations.Services
{
	public class CvService:ICvService
	{
		private readonly ICvRepository _repository;
		private readonly IHttpContextAccessor _accessor;
		private readonly IUserService _user;

		public CvService(ICvRepository repository,IHttpContextAccessor accessor,IUserService user) 
        {
			_repository = repository;
			_accessor = accessor;
			_user = user;
		}
        //public async Task<PaginateVM<Cv>> GetAllAsync(int id, int page = 1, int take = 10)
        //{
        //    if (page < 1 || take < 1) throw new Exception("Bad request");

        //    // Filtreleme ekleyin
        //    ICollection<Cv> cvs = await _repository
        //        .GetPagination(
        //            filter: x => x.JobId == id, // Bu satırı ekleyin
        //            skip: (page - 1) * take,
        //            take: take,
        //            orderExpression: x => x.Id,
        //            IsDescending: true
        //        )
        //        .ToListAsync();

        //    // Diğer kodlar...
        //}
        public async Task ReverseDeleteAsync(int id)
        {
            if (id < 1) throw new WrongRequestException("");
            Cv existed = await _repository.GetByIdAsync(id, isDeleted: true);
            if (existed == null) throw new NotFoundException("Not Found");
            _repository.ReverseDelete(existed);
            await _repository.SaveChangesAsync();
        }

        public async Task<Cv> GetJobAsync(int id)
        {
            if (id < 1) throw new WrongRequestException("");
            Cv cv= await _repository.GetByIdAsync(id,includes: nameof(Job));            
            if (cv == null) throw new NotFoundException("");
            return cv;
           
            
        }
        public async Task SoftDeleteAsync(int id)
        {
            if (id < 1) throw new WrongRequestException("");
            Cv existed = await _repository.GetByIdAsync(id, isDeleted: false);
            if (existed == null) throw new NotFoundException("");
            _repository.SoftDelete(existed);
            await _repository.SaveChangesAsync();
        }
        public async Task<PaginateVM<Cv>> GetAllAsync(int id, int page = 1, int take = 10)
        {
            if (page < 1 || take < 1) throw new Exception("Bad request");
            ICollection<Cv> cvs = await _repository.GetPagination(skip: (page - 1) * take, take: take, orderExpression: x => x.Id, IsDescending: true).Where(x=>x.JobId==id).ToListAsync();
            if (cvs == null) throw new Exception("Not Found");
            int count = await _repository.GetAll().CountAsync();
            if (count < 0) throw new Exception("Not Found");
            double totalpage = Math.Ceiling((double)count / take);
            PaginateVM<Cv> vm = new PaginateVM<Cv>
            {
                Items = cvs,
                CurrentPage = page,
                TotalPage = totalpage
            };
            return vm;
        }
        public async Task<PaginateVM<Cv>> GetUserCvs(string userid,int id, int page = 1, int take = 10)
        {
			if (page < 1 || take < 1) throw new Exception("Bad request");
			ICollection<Cv> cvs = await _repository.GetPagination(skip: (page - 1) * take, take: take, orderExpression: x => x.Id, IsDescending: true).Where(x => x.JobId == id).ToListAsync();
            cvs=await _repository.GetAllWhere(c=>c.AppUserId==userid).ToListAsync();
			if (cvs == null) throw new Exception("Not Found");
			int count = await _repository.GetAll().CountAsync();
			if (count < 0) throw new Exception("Not Found");
			double totalpage = Math.Ceiling((double)count / take);
			PaginateVM<Cv> vm = new PaginateVM<Cv>
			{
				Items = cvs,
				CurrentPage = page,
				TotalPage = totalpage
			};
			return vm;
		}
        public async Task<bool> CreateAsync(CreateCvVM vm, ModelStateDictionary modelstate)
		{

			if (!modelstate.IsValid) return false;			

			string username = "";
			if (_accessor.HttpContext.User.Identity != null)
			{
				username = _accessor.HttpContext.User.Identity.Name;
			}
			AppUser User = await _user.GetUser(username);
      

            if (vm != null)
            {
                
                if (vm.Name != null && vm.FatherName != null && vm.Surname != null
                    && vm.FinnishCode != null && vm.Address != null && vm.Birthday != null
                    && vm.PhoneNumber != null && vm.JobId > 0)
                {
                    
                    Cv cv = new Cv
                    {
                        IsDeleted = null,
                        AppUserId = User.Id,
                        CreatedBy = User.UserName,
                        CreatedAt = DateTime.UtcNow,
                        Name = vm.Name,
                        FatherName = vm.FatherName,
                        Surname = vm.Surname,
                        FinnishCode = vm.FinnishCode,
                        Address = vm.Address,
                        Birthday = vm.Birthday,
                        PhoneNumber = vm.PhoneNumber,
						EmailAddress = vm.EmailAddress,
                        JobId = vm.JobId
                    };

                   
                    await _repository.AddAsync(cv);
                    await _repository.SaveChangesAsync();
                    return true;
                }
            }

            return false;
		}

		public async  Task<CvDetailVM> DetailAsync(int id)
		{
			if (id < 1) throw new WrongRequestException("");

			Cv cv = await _repository.GetByIdAsync(id);


			CvItemVM CvItemVM = new CvItemVM
			{
                CvId=cv.Id,
				Name = cv.Name,
				Surname = cv.Surname,
				FatherName = cv.FatherName,
				FinnishCode = cv.FinnishCode,
				Address = cv.Address,
				EmailAddress = cv.EmailAddress,
				JobId = cv.JobId,
				Birthday = cv.Birthday,
				PhoneNumber = cv.PhoneNumber
			};
				   
			if (cv is null) throw new NotFoundException("");

			CvDetailVM detailVM = new CvDetailVM
			{

				Cv = CvItemVM
			};
			return detailVM;
		}
		
	}

	
	
}
