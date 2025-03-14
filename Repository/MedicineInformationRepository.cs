using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class MedicineInformationRepository : GenericRepository<MedicineInformation>
    {
        private const int pageSize = 3;
        public MedicineInformationRepository() { }
		public async Task<List<MedicineInformation>> GetMidicinesPagination(int pageNumber)
			=> await _context.MedicineInformations.Include(x => x.Manufacturer)
												  .Skip((pageNumber - 1) * pageSize)
													.Take(pageSize)
													.ToListAsync();

		public async Task<List<MedicineInformation>> GetAll()
			=> await _context.MedicineInformations.Include(x => x.Manufacturer)
													.ToListAsync();
	}
}
