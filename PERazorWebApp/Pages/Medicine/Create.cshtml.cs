using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Models;
using Service;

namespace PERazorWebApp.Pages.Medicine
{
    [Authorize(Roles = "2")]
    public class CreateModel : PageModel
    {
        private readonly IMedicineInformationService _medicineService;
        private readonly IManufacturerService _manufacturerService;
        public CreateModel(IMedicineInformationService medicineInformationService, IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
            _medicineService = medicineInformationService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["ManufacturerID"] = new SelectList(await _manufacturerService.GetAll(), "ManufacturerID", "ManufacturerID");
            return Page();
        }

        [BindProperty]
        public MedicineInformation MedicineInformation { get; set; } = default!;

        [BindProperty]
        public string? ErrMsg { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string expectedRes = "Success";
            string resMsg = await _medicineService.CreateMedicineInformation(MedicineInformation);
            if (resMsg != expectedRes)
            {
                ErrMsg = resMsg;
                TempData["ErrMsg"] = ErrMsg;

                ViewData["ManufacturerID"] = new SelectList(await _manufacturerService.GetAll(), "ManufacturerID", "ManufacturerID");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
