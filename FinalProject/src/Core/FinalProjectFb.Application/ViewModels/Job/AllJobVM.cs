using FinalProjectFb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.ViewModels
{
    public class AllJobVM
    {
        public List<Job> Jobs{ get; set; }
        public List<Category> Categories { get; set; }
		public List<CompanyCity> CompanyCities { get; set; }

		public List<Company> Companies { get; set; }
        public int RangeValue { get; set; }
        public int PriceFrom { get; set; }
        public int PriceTo { get; set; }
        

    }
}
