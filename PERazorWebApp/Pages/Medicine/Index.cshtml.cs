using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Models;
using Service;

namespace PERazorWebApp.Pages.Medicine
{
    [Authorize(Roles = "2,3")]
    public class IndexModel : PageModel
    {
        private readonly IMedicineInformationService _medicineService;
        private readonly IManufacturerService _manufacturerService;

        private const int pageSize = 3;


        public IndexModel(IMedicineInformationService medicineInformationService, IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
            _medicineService = medicineInformationService;
        }

        public IList<MedicineInformation> MedicineInformation { get;set; } = default!;

        [BindProperty]
        public int PageNumber { get; set; } = 1;

        [BindProperty]
        public int TotalPages { get; set; }

        public string? ActiveIngredients { get; set; }
        public string? WarningsAndPrecautions { get; set; }
        public string? ExpirationDate { get; set; }

        public async Task OnGetAsync(int? selectedPage, string? activeIngredients, string? warningsAndPrecautions, string? expirationDate)
        {
            if (selectedPage != null)
            {
                PageNumber = selectedPage.Value;
            }

            if (!activeIngredients.IsNullOrEmpty() || !warningsAndPrecautions.IsNullOrEmpty() || !expirationDate.IsNullOrEmpty())
            {
                var (medicines, records) = await _medicineService.GetMedicineInformationsByFilter(activeIngredients, expirationDate, warningsAndPrecautions, PageNumber);

                ActiveIngredients = activeIngredients;
                WarningsAndPrecautions = warningsAndPrecautions;
                ExpirationDate = expirationDate;

                TotalPages = (int)Math.Ceiling(records / (double)pageSize);
                MedicineInformation = medicines;
                return;
            }

            TotalPages = (int)Math.Ceiling((await _medicineService.GetAll()).Count / (double)pageSize);
            MedicineInformation = await _medicineService.GetMedicineInformationsPagination(PageNumber);
        }
    }
}
