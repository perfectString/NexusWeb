// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable


using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nexus.Models;
using static Nexus.Common.ValidationConstants;

namespace Nexus.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Profile> signInManager;
        private readonly UserManager<Profile> userManager;
        private readonly IUserStore<Profile> userStore;
        private readonly IUserEmailStore<Profile> emailStore;

        public RegisterModel(
            UserManager<Profile> userManager,
            IUserStore<Profile> userStore,
            SignInManager<Profile> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.userStore = userStore;
            this.emailStore = GetEmailStore();
            this.signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }


        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(15, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Range(AgeMinValue, AgeMaxValue)]
            public int Age { get; set; }

            [Required]
            [MinLength(CityMinLen)]
            [MaxLength(CityMaxLen)]
            public string City { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = "/Profile/Edit")
        {
            ReturnUrl = returnUrl;
            
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = "/Profile/Edit")
        {
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.DisplayName = Input.Email;
                user.City = Input.City;
                user.Age = Input.Age;

                var result = await userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    
                    if (userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private Profile CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Profile>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Profile)}'. " +
                    $"Ensure that '{nameof(Profile)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<Profile> GetEmailStore()
        {
            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Profile>)userStore;
        }
    }
}
