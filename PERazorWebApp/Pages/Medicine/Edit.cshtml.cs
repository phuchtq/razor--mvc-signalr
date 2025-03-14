using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Service;

namespace PERazorWebApp.Pages.Medicine
{
    [Authorize(Roles = "2")]
    public class EditModel : PageModel
    {
        private readonly IMedicineInformationService _medicineService;
        private readonly IManufacturerService _manufacturerService;

        public EditModel(IMedicineInformationService medicineService, IManufacturerService manufacturerService)
        {
            _medicineService = medicineService;
            _manufacturerService = manufacturerService;
        }

        [BindProperty]
        public MedicineInformation MedicineInformation { get; set; } = default!;


        [BindProperty]
        public string? ErrMsg { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineinformation =  await _medicineService.Get(id);
            if (medicineinformation == null)
            {
                return NotFound();
            }
            MedicineInformation = medicineinformation;
           ViewData["ManufacturerID"] = new SelectList(await _manufacturerService.GetAll(), "ManufacturerID", "ManufacturerID");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                string expectedRes = "Success";
                string resMsg = await _medicineService.CreateMedicineInformation(MedicineInformation);
                if (resMsg != expectedRes)
                {
                    ErrMsg = resMsg;
                    TempData["ErrMsg"] = ErrMsg;

                    ViewData["ManufacturerID"] = new SelectList(await _manufacturerService.GetAll(), "ManufacturerID", "ManufacturerID");
                    return Page();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MedicineInformationExists(MedicineInformation.MedicineID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> MedicineInformationExists(string id) => await _medicineService.Get(id) != null;
    }
}
