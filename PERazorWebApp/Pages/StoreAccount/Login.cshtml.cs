using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System.Security.Claims;

namespace PERazorWebApp.Pages.StoreAccount
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginModel : PageModel
    {
        private readonly IStoreAccountService _service;
        
        public LoginModel(IStoreAccountService service)
        {
            _service = service;
        }


        [BindProperty]
        public LoginRequest Request { get; set; }

        [TempData]
        public string? ErrMsg { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var account = await _service.Authenticate(Request.Email, Request.Password);


            if (account != null)
            {
                if (account.Role != 2 && account.Role != 3)
                {
                    ErrMsg = "You do not have permission to do this function!";
                    TempData["ErrMsg"] = ErrMsg;
                    return Page();
                }

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, account.EmailAddress),
                new Claim(ClaimTypes.Role, account.Role.ToString()),

            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                Response.Cookies.Append("Email", account.EmailAddress);
                Response.Cookies.Append("Role", account.Role.ToString());

                return RedirectToPage("/Medicine/Index");
            }

            ModelState.AddModelError("", "Incorrect email or password");
            return Page();
        }


    }
}
