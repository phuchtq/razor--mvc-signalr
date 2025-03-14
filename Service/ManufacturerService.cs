using Repository;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IManufacturerService
    {
        Task<List<Manufacturer>> GetAll();
    }

    public class ManufacturerService : IManufacturerService
    {
        private readonly ManufacturerRepository _repo;

        public ManufacturerService()
        {
            _repo ??= new ManufacturerRepository();
        }

        public async Task<List<Manufacturer>> GetAll() => await _repo.GetAllAsync();
    }
}
