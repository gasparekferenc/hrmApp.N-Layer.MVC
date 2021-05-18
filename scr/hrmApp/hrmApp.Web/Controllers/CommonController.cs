using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using hrmApp.Core.Constants;
using hrmApp.Core.Models;
using hrmApp.Core.Services;
using hrmApp.Web.Constants;

namespace hrmApp.Web.Controllers
{
    [Authorize(Policy = PolicyNames.EmployeeHandler)]
    public class CommonController : Controller
    {
        #region CommonController

        private readonly IApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailService _mailService;

        public CommonController(
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
        #endregion

        #region GET:  Common/SetActiveProject

        // GET: Common/SetActiveProject -----------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> SetActiveProject(int id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            int currentProjectId = id;
            var user = await _userManager.GetUserAsync(User);
            user.CurrentProjectId = currentProjectId;

            try
            {
                await _applicationUserService.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                Log.Information($"Common/SetActiveProject - UpdateAsync(user) id=({id}). {ex}");
                throw;
            }

            HttpContext.Session.SetInt32(SessionKeys.ProjectIdSessionKey, currentProjectId);

            Log.Information($"SetActiveProject id=({id}).");

            return RedirectToAction(nameof(EmployeeController.Index), "Employee");
        }
        #endregion
    }
}