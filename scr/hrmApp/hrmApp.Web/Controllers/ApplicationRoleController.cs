using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Serilog;
using hrmApp.Core.Constants;
using hrmApp.Web.Constants;
using hrmApp.Web.ViewModels;
using Microsoft.AspNetCore.Http;

namespace hrmApp.Web.Controllers
{
    [Authorize(Policy = PolicyNames.RoleHandler)]
    public class ApplicationRoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public ApplicationRoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        #region GET: Index()
        // GET: ApplicationRole --------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            List<RoleClaimsViewModel> viewModel = new List<RoleClaimsViewModel>();
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var identityRole in roles)
            {
                viewModel.Add(await AssembleViewModel(identityRole));
            }

            return View(viewModel);
        }
        #endregion

        #region GET: ApplicationRole/Create
        // GET: ApplicationRole/Create -----------------------------------------------
        public IActionResult Create()
        {
            RoleViewModel viewModel = new RoleViewModel();
            return View(viewModel);
        }
        #endregion

        #region POST: ApplicationRole/Create
        // POST: ApplicationRole/Create ------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var identityRole = new IdentityRole { Name = viewModel.RoleName };
                var roleResult = await _roleManager.CreateAsync(identityRole);

                if (roleResult.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, $"A Szerepkört ({viewModel.RoleName}) nem sikerült elmenteni.");
                return View(viewModel);
            }
            ModelState.AddModelError(string.Empty, $"A Szerepkört ({viewModel.RoleName}) nem sikerült elmenteni...");
            return View(viewModel);
        }
        #endregion

        #region GET: ApplicationRole/Edit/5
        // GET: ApplicationRole/Edit/5 ------------------------------------------------
        public async Task<IActionResult> Edit(string id)
        {
            if (String.IsNullOrEmpty(id)) { return BadRequest(); }

            IdentityRole identityRole = await _roleManager.FindByIdAsync(id);

            if (identityRole == null) { return NotFound(); }

            RoleViewModel viewModel = new RoleViewModel();
            viewModel.Id = identityRole.Id;
            viewModel.RoleName = identityRole.Name;

            return View(viewModel);
        }
        #endregion

        #region POST: ApplicationRole/Edit/5
        // POST: ApplicationRole/Edit/5 ------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, RoleViewModel viewModel)
        {
            if (String.IsNullOrEmpty(id) || (id != viewModel.Id)) { return BadRequest(); }

            if (ModelState.IsValid)
            {
                IdentityRole identityRole = await _roleManager.FindByIdAsync(id);
                if (identityRole != null)
                {
                    identityRole.Name = viewModel.RoleName;
                    IdentityResult roleResult = await _roleManager.UpdateAsync(identityRole);
                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            // Model.State invalid
            ModelState.AddModelError(string.Empty, "Nem megfelelő név.");
            return View();
        }
        #endregion

        #region GET: ApplicationRole/Delete/5
        // GET: ApplicationRole/Delete/5 -----------------------------------------------
        public async Task<IActionResult> Delete(string id)
        {
            if (String.IsNullOrEmpty(id)) { return BadRequest(); }

            IdentityRole identityRole = await _roleManager.FindByIdAsync(id);

            if (identityRole == null) { return NotFound(); }

            RoleViewModel viewModel = new RoleViewModel
            {
                Id = id,
                RoleName = identityRole.Name
            };

            return View(viewModel);
        }
        #endregion

        #region POST: ApplicationRole/Delete/5
        // POST: ApplicationRole/Delete/5 ----------------------------------------------
        //[HttpPost, ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, RoleViewModel viewModel)
        {

            if (String.IsNullOrEmpty(id) || (id != viewModel.Id)) { return BadRequest(); }

            IdentityRole identityRole = await _roleManager.FindByIdAsync(id);

            if (identityRole != null)
            {
                try
                {
                    IdentityResult roleResult = await _roleManager.DeleteAsync(identityRole);

                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    //TODO Display error message
                    Log.Information($"DeleteRole error: {ex}");
                    return View(id);
                }
            }
            //TODO: Display error message
            return View(viewModel);
        }
        #endregion

        #region GET: ApplicationRole/ManageClaim/5
        // GET: ApplicationRole/ManageClaim/5 -----------------------------------------------
        public async Task<ActionResult> ManageClaim(string id)
        {
            if (String.IsNullOrEmpty(id)) { return BadRequest(); }

            IdentityRole identityRole = await _roleManager.FindByIdAsync(id);

            if (identityRole == null) { return NotFound(); }

            var viewModel = await AssembleViewModel(identityRole);

            return View(viewModel);
        }
        #endregion

        #region POST: ApplicationRole/ManageClaim/5
        // POST: ApplicationRole/ManageClaim/5 -----------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageClaim(string id, RoleClaimsViewModel viewModel)
        {
            if (String.IsNullOrEmpty(id) || (id != viewModel.Id)) { return BadRequest(); }

            if (ModelState.IsValid)
            {
                IdentityRole identityRole = await _roleManager.FindByIdAsync(id);
                if (identityRole != null)
                {
                    await UpdateRoleClaims(identityRole, viewModel);
                }
                return RedirectToAction(nameof(Index));
            }
            // Model.State invalid
            return View();
        }
        #endregion

        #region Helpers

        // ---------------------------- helper methods ----------------------------

        private async Task UpdateRoleClaims(IdentityRole identityRole, RoleClaimsViewModel viewModel)
        {
            //Get claims associated with the role
            IList<Claim> roleClaimList = await _roleManager.GetClaimsAsync(identityRole);
            //Extract the claim type
            List<string> roleClaimTypeList = new List<string>();

            foreach (var roleClaim in roleClaimList)
            {
                roleClaimTypeList.Add(roleClaim.Type);
            }

            foreach (var roleClaim in viewModel.RoleClaims)
            {
                //create a new claim with the claim name
                Claim claim = new Claim(roleClaim.ClaimName, "");
                //get the associated claim from the role's claim list
                Claim associatedClaim = roleClaimList
                                            .Where(x => x.Type == roleClaim.ClaimName)
                                            .FirstOrDefault();

                if (roleClaim.HasClaim && !roleClaimTypeList.Contains(roleClaim.ClaimName))
                {
                    IdentityResult claimResult = await _roleManager.AddClaimAsync(identityRole, claim);
                    if (!claimResult.Succeeded)
                    {
                        //TODO log details and display some sort of error

                        Log.Information($"ManageClaim(POST).UpdateRoleClaims.AddClaimAsync " +
                                        "RoleName: {identityRole.Name}" +
                                        "ClaimType: {claim.Type}" +
                                        " - NOT Succeeded!");
                    }
                }
                else if (!roleClaim.HasClaim && roleClaimTypeList.Contains(roleClaim.ClaimName))
                {
                    IdentityResult claimResult = await _roleManager.RemoveClaimAsync(identityRole, associatedClaim);
                    if (!claimResult.Succeeded)
                    {
                        // log details and display some sort of error
                        Log.Information($"ManageClaim(POST).UpdateRoleClaims.RemoveClaimAsync " +
                                        "RoleName: {identityRole.Name}" +
                                        "ClaimType: {claim.Type}" +
                                        " - NOT Succeeded!");
                    }
                }
            }
        }

        private async Task<RoleClaimsViewModel> AssembleViewModel(IdentityRole identityRole)
        {
            //Get claims associated with the role
            IList<Claim> roleClaimList = await _roleManager.GetClaimsAsync(identityRole);
            List<string> roleClaimTypeList = new List<string>();
            List<ClaimsViewModel> roleClaims = new List<ClaimsViewModel>();

            foreach (var roleClaim in roleClaimList)
            {
                roleClaimTypeList.Add(roleClaim.Type);
            }

            foreach (var claimName in ClaimNames.ClaimName)
            {
                roleClaims.Add(new ClaimsViewModel()
                {
                    ClaimName = claimName,
                    HasClaim = roleClaimTypeList.Contains(claimName)
                });
            }

            RoleClaimsViewModel viewModel = new RoleClaimsViewModel
            {
                Id = identityRole.Id,
                RoleName = identityRole.Name,
                RoleClaims = roleClaims
            };

            return viewModel;
        }
        #endregion
    }
}
