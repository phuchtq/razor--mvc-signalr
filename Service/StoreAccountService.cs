using Repository;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IStoreAccountService
    {
        Task<StoreAccount> Authenticate(string email, string password);
    }

    public class StoreAccountService : IStoreAccountService
    {
        private readonly StoreAccountRepository _repo;
        public StoreAccountService() 
        {
            _repo ??= new StoreAccountRepository();
        }

        public async Task<StoreAccount> Authenticate(string email, string password) => await _repo.Authenticate(email, password);
    }
}
