using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;

namespace ASC.Web.Areas.Identity.Pages.Account
{
    public class ExternalLoginConfirmationModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public ExternalLoginConfirmationModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return RedirectToPage("./ExternalLoginFailure");
                }

                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email, EmailConfirmed = true };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    var addClaimResult = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Email, Input.Email));
                    var addIsActiveResult = await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("IsActive", "true"));

                    if (addClaimResult.Succeeded && addIsActiveResult.Succeeded)
                    {
                        // Assign user to Engineer Role
                        var roleResult = await _userManager.AddToRoleAsync(user, "Engineer");

                        if (roleResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                            _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                            return RedirectToPage("/Dashboard/Dashboard", new { Area = "ServiceRequests" });
                        }
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    foreach (var error in addClaimResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    foreach (var error in addIsActiveResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["ReturnUrl"] = returnUrl;
            return Page();
        }
    }
}