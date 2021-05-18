using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using hrmApp.Core.Models;
using hrmApp.Core.Services;
using hrmApp.Web.ViewModels.ManageViewModels;

namespace hrmApp.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {

        private readonly IApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailService _mailService;

        public ManageController(
            IApplicationUserService applicationUserService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMailService mailService)
        {
            _applicationUserService = applicationUserService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Manage/ChangeProfilData -------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> ChangeProfilData()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Log.Information($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var ViewModel = new ProfilDataViewModel
            {
                Username = user.UserName,
                SurName = user.SurName,
                ForeName = user.ForeName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email

                // IsEmailConfirmed = user.EmailConfirmed,
                // StatusMessage = StatusMessage
            };

            return View(ViewModel);
        }

        // POST: Manage/ChangeProfilData -------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeProfilData(ProfilDataViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Log.Information($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            // Email címet nem engedünk megváltoztatni!
            // var email = user.Email;
            // if (viewModel.Email != email)
            // {
            //     var setEmailResult = await _userManager.SetEmailAsync(user, viewModel.Email);
            //     if (!setEmailResult.Succeeded)
            //     {
            //         throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
            //     }
            // }

            user.SurName = viewModel.SurName;
            user.ForeName = viewModel.ForeName;
            user.PhoneNumber = viewModel.PhoneNumber;

            var result = await _applicationUserService.UpdateAsync(user);
            if (result == null)
            {
                Log.Information($"Unexpected error occurred update user '{user.Id}'.");
                throw new ApplicationException($"Unexpected error occurred update user '{user.Id}'.");
            }

            StatusMessage = "A felhasználói profiladatok aktualizálása megtörtént.";

            return RedirectToAction(nameof(HomeController.Index), "/Home");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (hasPassword)
            {
                var viewModel = new ChangePasswordViewModel { StatusMessage = StatusMessage };
                return View(viewModel);
            }
            
            return RedirectToAction(nameof(AccountController.AccessDenied), nameof(AccountController));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Log.Information($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, viewModel.OldPassword, viewModel.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(viewModel);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            Log.Information("User changed their password successfully.");
            StatusMessage = "A jelszó megváltoztatása megtörtént.";

            return RedirectToAction(nameof(HomeController.Index), "/Home");
        }


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}
