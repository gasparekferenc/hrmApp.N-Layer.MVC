using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using hrmApp.Core.Models;
using hrmApp.Core.Services;
using hrmApp.Web.ViewModels.ComponentsViewModels;

namespace hrmApp.Web.Views.Shared.Components.DataSheetProcessStatus
{
    public class DataSheetMemosViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DataSheetMemosViewComponent(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(int employeeId)
        {
            // Hozzáférés ellenőrzés (UserID)?
            var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);

            var message = (string)TempData["CreateMemoMessage"] ?? "";

            var viewModel = new MemoViewModel
            {
                ApplicationUserId = user.Id,
                EmployeeId = employeeId,
                Message = message,
                EntryDate = DateTime.Now,
                IsReminder = false,
                DeadlineDate = DateTime.Now
            };

            return View(viewModel);
        }
    }
}