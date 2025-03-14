using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service
{
    public interface IMedicineInformationService
    {
        Task<List<MedicineInformation>> GetAll();
        Task<List<MedicineInformation>> GetMedicineInformationsPagination(int pageNumber);
        Task<(List<MedicineInformation> data, int records)> GetMedicineInformationsByFilter(string activeIngredients, string expiration, string warningsAndPrecautions, int pageNumber);
        Task<MedicineInformation> Get(string id);
        Task<string> CreateMedicineInformation(MedicineInformation medicine);
        Task<string> UpdateMedicineInformation(MedicineInformation medicine);
        Task RemoveMedicineInformation(MedicineInformation medicine);
    }

	public class MedicineInformationService : IMedicineInformationService
    {
        private readonly MedicineInformationRepository _repo;

        private const string pattern = @"\b[A-Z0-9][A-Za-z0-9]*\b";
        private const string invalidIngredientsErrMsg = "Active Ingredients is invalid.";
        private const string successMsg = "Success";

        public MedicineInformationService()
        {
            _repo ??= new MedicineInformationRepository();
        }

		public async Task<List<MedicineInformation>> GetAll() => await _repo.GetAllAsync();

        public async Task<(List<MedicineInformation> data, int records)> GetMedicineInformationsByFilter(string activeIngredients, string expiration, string warningsAndPrecautions, int pageNumber)
        {
            var tmpStorage = await _repo.GetAll();

            if (!activeIngredients.IsNullOrEmpty())
            {
                tmpStorage = tmpStorage.Where(x => x.ActiveIngredients.ToLower().Contains(activeIngredients.ToLower().Trim())).ToList();
            }

            if (!expiration.IsNullOrEmpty())
            {
                tmpStorage = tmpStorage.Where(x => x.ExpirationDate.ToLower().Contains(expiration.ToLower().Trim())).ToList();
            }

            if (!warningsAndPrecautions.IsNullOrEmpty())
            {
                tmpStorage = tmpStorage.Where(x => x.WarningsAndPrecautions.ToLower().Contains(warningsAndPrecautions.ToLower().Trim())).ToList();
            }

            //var res = tmpStorage.Skip((pageNumber - 1) * 3).Take(3).ToList();
            return (tmpStorage.Skip((pageNumber - 1) * 3).Take(3).ToList(), tmpStorage.Count);
        }

        public async Task<string> CreateMedicineInformation(MedicineInformation medicine)
        {
            if (!IsIngredientValid(medicine.ActiveIngredients))
            {
                return invalidIngredientsErrMsg;   
            }

            medicine.MedicineID = $"{(await _repo.GetAllAsync()).Count + 1}";

            await _repo.CreateAsync(medicine);

            return successMsg;
        }

        public async Task<string> UpdateMedicineInformation(MedicineInformation medicine)
        {
            if (!IsIngredientValid(medicine.ActiveIngredients))
            {
                return invalidIngredientsErrMsg;
            }

            await _repo.UpdateAsync(medicine);

            return successMsg;
        }

        public async Task<List<MedicineInformation>> GetMedicineInformationsPagination(int pageNumber) => pageNumber < 1 ? null : await _repo.GetMidicinesPagination(pageNumber);

        public async Task<MedicineInformation> Get(string id) => await _repo.GetByIdAsync(id);

        public async Task RemoveMedicineInformation(MedicineInformation medicine) => await _repo.RemoveAsync(medicine);



        private bool IsIngredientValid(string src)
        {
            if (src.Contains("#") || src.Contains("@") || src.Contains("&") || src.Contains("(") || src.Contains(")"))
            {
                return false;
            }

            string spacePattern = @"\s+";

            string[] words = Regex.Split(src.Trim(), spacePattern);

            foreach (string word in words)
            {
                if (!Regex.IsMatch(word, pattern))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
