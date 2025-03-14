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
    public class StoreAccountRepository : GenericRepository<StoreAccount>
    {
        public StoreAccountRepository() { }

        public async Task<StoreAccount> Authenticate(string email, string password) => await _context.StoreAccounts.FirstOrDefaultAsync(x => x.EmailAddress == email && x.StoreAccountPassword == password);


    }
}
