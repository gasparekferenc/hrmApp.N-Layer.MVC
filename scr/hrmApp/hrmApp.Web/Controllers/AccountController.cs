using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Serilog;
using hrmApp.Core.Models;
using hrmApp.Core.Services;
using hrmApp.Web.Constants;
using hrmApp.Web.Extensions;
using hrmApp.Web.ViewModels.AccountViewModels;

namespace hrmApp.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailService _mailService;

        public AccountController(
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
        public string ErrorMessage { get; set; }

        #region GET:  Account/Login

        // GET: Account/Login ----------------------------------------------------------------------------
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        #endregion

        #region POST: Account/Login
        // POST: Account/Login ----------------------------------------------------------------------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                // TODO: Átállítani a lockoutOnFailure-t true-ra!!!
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    Log.Information($"User ({model.Email}) logged in ({DateTime.Now}).");

                    var user = await _userManager.FindByEmailAsync(model.Email);

                    var currentProjectId = user.CurrentProjectId;

                    if (currentProjectId == null)
                    {
                        Log.Information($"User ({model.Email}) FIRST login ({DateTime.Now}).");
                        currentProjectId = 0;
                    }

                    if (currentProjectId == 0)
                    {
                        var userId = user.Id;
                        var projectsOfUser = await _applicationUserService.GetProjectsOfUserAsync(userId);
                        if (projectsOfUser.Count() > 0)
                        {
                            var currentProject = projectsOfUser.LastOrDefault();
                            currentProjectId = currentProject.Id;

                            user.CurrentProjectId = currentProjectId;

                            try
                            {
                                await _applicationUserService.UpdateAsync(user);
                            }
                            catch (Exception ex)
                            {
                                Log.Information($"Account/Login - UpdateAsync(user) currentProjectId=({currentProjectId}). {ex}");
                                throw;
                            }
                        }
                    }

                    HttpContext.Session.SetInt32(SessionKeys.ProjectIdSessionKey, (int)currentProjectId);

                    return RedirectToLocal(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    Log.Information($"Locked User {model.Email} attempt ({DateTime.Now}).");
                    Log.Warning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Érvénytelen bejelentkezési kísérlet({model.Email}/{DateTime.Now}).");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region GET:  Account/Lockout

        // GET: Account/Lockout ----------------------------------------------------------------------
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }
        #endregion

        #region GET:  Account/SignedOut

        // GET: Account/SignedOut ----------------------------------------------------------------------
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignedOut()
        {
            return View();
        }
        #endregion

        #region POST: Account/Logout

        // POST: Account/Logout ----------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            Log.Information("Logout...");
            await _signInManager.SignOutAsync();
            Log.Information("User logged out.");
            // return RedirectToAction(nameof(HomeController.Index), "Home");
            return RedirectToAction(nameof(SignedOut));
        }
        #endregion

        #region GET:  Account/ConfirmEmail

        // GET: Account/ConfirmEmail?userId=<>&code=<> ------------------------------------------------
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                //TODO: Hova menjünk, ha nem OK a link
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                Log.Information($"Unable to load user with ID '{userId}'.");
                //throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var viewModel = new ConfirmEmailViewModel
            {
                UserId = userId,
                Email = user.Email,
                Code = code
            };
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return View(nameof(ConfirmEmail), viewModel);
            }
            Log.Information($"Access denied - ConfirmEmail: userID={userId}, Email={user.Email}, Code={code}");
            return View(nameof(AccessDenied));
        }
        #endregion

        #region POST: Account/ConfirmEmail

        // POST: Account/ConfirmEmail?userId=<>&code=<> ------------------------------------------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmail(string userId, string code, ConfirmEmailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            Log.Information($"userId: {userId}");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                Log.Information($"SetPassword Unable to load user with ID '{userId}'.");
                //TODO: Hova menjünk, ha valami nem OK?
                return RedirectToAction(nameof(HomeController.Index), "Home");
                // return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var resultAddPassword = await _userManager.AddPasswordAsync(user, viewModel.Password);
            if (resultAddPassword.Succeeded)
            {
                // bejegyezzük a ApplicationUser-hez a hozzájárulásokat.
                user.GDPRConfirmed = true;
                user.TermOfUseConfirmed = true;
                user.DateOfPoliciesConfirm = DateTime.Now;
                await _applicationUserService.UpdateAsync(user);
                // Bejelentkeztessük a felhasználót? ...

                // return RedirectToAction(nameof(Index), "Home");
                return RedirectToAction(nameof(Login));
            }
            // Ha nme sikerült beállítani...
            AddErrors(resultAddPassword);
            return View(viewModel);
        }

        #endregion

        #region GET: Account/ForgotPassword

        // GET: Account/ForgotPassword ------------------------------------------------------------
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        #endregion

        #region POST: Account/ForgotPassword

        // POST: Account/ForgotPassword -----------------------------------------------------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _mailService.SendResetPasswordAsync(model.Email, model.Email, callbackUrl);

                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region GET:  ForgotPasswordConfirmation

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        #endregion

        #region GET:  ResetPassword

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                Log.Warning("Reset password attempt without code!");
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        #endregion

        #region POST: ResetPassword


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        #endregion

        #region GET:  ResetPasswordConfirmation


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #endregion

        #region GET:  AccessDenied


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                // Ha nincs returnUrl, akkor a bejelentkezés után
                // ide kerül a User.
                // Home/Index vagy Employee/Index vagy külön DashBoard?
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
