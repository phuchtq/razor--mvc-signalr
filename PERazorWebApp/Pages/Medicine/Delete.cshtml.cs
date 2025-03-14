using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PERazorWebApp.Hubs;
using Repository.Models;
using Service;

namespace PERazorWebApp.Pages.Medicine
{
    [Authorize(Roles = "2")]
    public class DeleteModel : PageModel
    {
        private readonly IMedicineInformationService _service;
        private readonly IHubContext<ChatHub> _hubContext;

        public DeleteModel(IMedicineInformationService service, IHubContext<ChatHub> hubContext)
        {
           _service = service;
            _hubContext = hubContext;
        }

        [BindProperty]
        public MedicineInformation MedicineInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineinformation = await _service.Get(id);

            if (medicineinformation == null)
            {
                return NotFound();
            }
            else
            {
                MedicineInformation = medicineinformation;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineinformation = await _service.Get(id);
            if (medicineinformation != null)
            {
                MedicineInformation = medicineinformation;
                await _service.RemoveMedicineInformation(medicineinformation);
                await _hubContext.Clients.All.SendAsync("RemoveMedicine", id);
            }

            return RedirectToPage("./Index");
        }
    }
}
