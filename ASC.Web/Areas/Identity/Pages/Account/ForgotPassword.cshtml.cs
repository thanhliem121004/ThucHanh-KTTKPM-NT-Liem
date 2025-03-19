using ASC.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities; // Thêm namespace này để sử dụng WebEncoders
using System.ComponentModel.DataAnnotations;
using System.Text; // Thêm namespace này để sử dụng Encoding
using System.Threading.Tasks;

namespace ASC.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // Generate the password reset token
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                // Include userId, code, and email in the callback URL
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new
                    {
                        userId = user.Id,
                        code = encodedCode,
                        email = Input.Email // Thêm email vào URL
                    },
                    protocol: Request.Scheme);

                // Log for debugging
                Console.WriteLine($"Token: {code}");
                Console.WriteLine($"Encoded Token: {encodedCode}");
                Console.WriteLine($"Callback URL: {callbackUrl}");

                // Send the email with the reset link
                await _emailSender.SendEmailAsync(Input.Email, "Reset Password",
                    $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}