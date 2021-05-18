#region using   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Serilog;
using hrmApp.Core.Constants;
using hrmApp.Core.Models;
using hrmApp.Core.Services;
using hrmApp.Web.DTO;
using hrmApp.Web.Extensions;
using hrmApp.Web.ViewModels;
#endregion

namespace hrmApp.Web.Controllers
{
    [Authorize(Policy = PolicyNames.UserHandler)]
    public class ApplicationUserController : Controller
    {
        #region ApplicationUserController

        private readonly IApplicationUserService _applicationUserService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public ApplicationUserController(
            IApplicationUserService applicationUserService,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IMailService mailService,
            IMapper mapper
                                        )
        {
            _applicationUserService = applicationUserService;
            _roleManager = roleManager;
            _userManager = userManager;
            _mailService = mailService;
            _mapper = mapper;
        }
        #endregion

        #region GET: ApplicationUser/Index

        // GET: ApplicationUser/Index --------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var ViewModel = new List<UserViewModel>();

            var applicationUsers = await _applicationUserService.GetAllAsync();

            // This not work!!! EF problem...?
            // var applicationUsers = await _userManager.Users
            //                                 .AsNoTracking()
            //                                 .ToListAsync();

            foreach (var applicationUser in applicationUsers)
            {
                ViewModel.Add(await AssembleUserViewModel(applicationUser));
            }
            return View(ViewModel);
        }
        #endregion

        #region GET: ApplicationUser/Details/5

        // GET: ApplicationUser/Details/5 ----------------------------------------------
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            var applicationUserDTO = _mapper.Map<ApplicationUserDTO>(applicationUser);
            return View(applicationUserDTO);
        }
        #endregion

        #region GET: ApplicationUser/Create

        // GET: ApplicationUser/Create -----------------------------------------------
        public IActionResult Create()
        {

            UserViewModel viewModel = new UserViewModel();
            viewModel.LockoutEnabled = true;    // kizárjuk, ameddig nem jelez vissza
            viewModel.UserRoles = _roleManager.Roles.Select(role => new UserRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                HasRole = false         // alapból nem adunk semmilyen jogosultságot

            }).ToList();

            return View(viewModel);
        }
        #endregion

        #region POST: ApplicationUser/Create

        // POST: ApplicationUser/Create ------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                ApplicationUserDTO applicationUserDTO = new ApplicationUserDTO()
                {
                    UserName = viewModel.UserName,
                    ForeName = viewModel.ForeName,
                    SurName = viewModel.SurName,
                    Email = viewModel.Email,
                    PhoneNumber = viewModel.PhoneNumber,
                    LockoutEnabled = true,          //viewModel.LockoutEnabled
                };

                var applicationUser = _mapper.Map<ApplicationUser>(applicationUserDTO);
                // Password nélkül hozzuk létre, de lockolva!
                IdentityResult userResult = await _userManager.CreateAsync(applicationUser);

                if (userResult.Succeeded)
                {
                    // TODO: jogosultság beállításai 
                    foreach (UserRoleViewModel role in viewModel.UserRoles)
                    {
                        if (role.HasRole)
                        {
                            IdentityRole identityRole = await _roleManager.FindByIdAsync(role.Id);

                            if (identityRole != null)
                            {
                                IdentityResult roleResult = await _userManager.AddToRoleAsync(applicationUser, role.RoleName);

                                if (!roleResult.Succeeded)
                                {
                                    Log.Information($"Role settings faild => " +
                                                        "ApplicationUser.Id: {applicationUser.Id})" +
                                                        "Role.Id: {role.Id}");
                                }
                            }
                            else
                            {
                                Log.Information($"Role not exist => Role.Id: {role.Id}");
                            }
                        }
                    }

                    // Send Confirmation e-mail
                    var applicationUserId = applicationUser.Id;
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                    var callbackUrl = Url.EmailConfirmationLink(applicationUser.Id, code, Request.Scheme);

                    try
                    {                        
                        await _mailService.SendEmailConfirmationAsync(applicationUser.Email, applicationUser.UserName, callbackUrl).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Log.Information($"Hiba az Email kiküldésénél: {ex}");
                        //throw;
                    }

                    Log.Information($"Email elküldve - mailto: {applicationUser.Email}, callbackUrl: {callbackUrl}");
                    // TODO: Toast??
                    return RedirectToAction(nameof(Index));
                }
                // !userResult.Succeeded
                Log.Information($"ApplicationUser can't create.");
                ModelState.AddModelError(string.Empty, "A Felhasználót nem lehet létrehozni." +
                                                        "\nKattints a [Vissza] gombra.");

                return View(viewModel);
            }
            // !ModelState.IsValid
            ModelState.AddModelError(string.Empty, "Hiba az adatokban");

            return View(viewModel);
        }
        #endregion

        #region GET: ApplicationUser/Edit/5

        // GET: ApplicationUser/Edit/5 ------------------------------------------------
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) { return BadRequest(); }

            var applicationUser = await _applicationUserService.GetByIdAysnc(id);

            // same error like above in Index action...
            // ar applicationUser = await _userManager.FindByIdAsync(id);
            // .AsNoTracking()
            // .SingleOrDefaultAsync(u => u.Id == id);

            if (applicationUser == null) { return NotFound(); }

            var applicationUserDTO = _mapper.Map<ApplicationUserDTO>(applicationUser);

            // rendezi kell az osztályokat!
            var viewModel = await AssembleUserViewModel(applicationUser);

            return View(viewModel);
        }
        #endregion

        #region POST: ApplicationUser/Edit/5

        // POST: ApplicationUser/Edit/5 ------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserViewModel viewModel)
        {
            if (id != viewModel.Id) { return BadRequest(); }

            if (ModelState.IsValid)
            {
                var applicationUser = _mapper.Map<ApplicationUser>(viewModel);
                applicationUser = _mapper.Map<ApplicationUser>(viewModel);

                try
                {
                    var userResult = await _applicationUserService.UpdateAsync(applicationUser);

                    if (userResult != null)
                    {
                        foreach (UserRoleViewModel role in viewModel.UserRoles)
                        {
                            if (role.HasRole)
                            {
                                if (await _roleManager.RoleExistsAsync(role.RoleName))
                                {
                                    IdentityResult roleResult = await _userManager.AddToRoleAsync(applicationUser, role.RoleName);

                                    if (!roleResult.Succeeded && roleResult.Errors.First().Code != "UserAlreadyInRole")
                                    {
                                        Log.Information($"Add role failed => " +
                                                            "ApplicationUser.Id: {applicationUser.Id})" +
                                                            "Role.Id: {role.Id}");
                                    }
                                }
                                else
                                {
                                    Log.Information($"Role not exist => Role.Id: {role.Id}");
                                }
                            }
                            else
                            {
                                if (await _roleManager.RoleExistsAsync(role.RoleName))
                                {
                                    IdentityResult roleResult = await _userManager.RemoveFromRoleAsync(applicationUser, role.RoleName);

                                    if (!roleResult.Succeeded && roleResult.Errors.First().Code != "UserNotInRole")
                                    {
                                        Log.Information($"Removed role failed => " +
                                                            "ApplicationUser.Id: {applicationUser.Id})" +
                                                            "Role.Id: {role.Id}");
                                    }
                                }
                                else
                                {
                                    Log.Information($"Role not exist => Role.Id: {role.Id}");
                                }
                            }
                        }
                    }
                }

                // Database concurrency
                catch (DbUpdateConcurrencyException ex)
                {
                    Log.Information("Concurrency catching.", ex);

                    // Database Conflict or Deleted record
                    // Back to Edit page with error message
                    return View(viewModel);
                }
                // update success
                return RedirectToAction(nameof(Index));
            }
            // Model.State invalid
            return View(viewModel);
        }
        #endregion

        #region GET: ApplicationUser/Delete/5


        // GET: ApplicationUser/Delete/5 -----------------------------------------------
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) { return BadRequest(); }

            var applicationUser = await _applicationUserService.GetWithAssignmentsByIdAsync(id);

            if (applicationUser == null) { return NotFound(); }

            var applicationUserDTO = _mapper.Map<ApplicationUserDTO>(applicationUser);
            return View(applicationUserDTO);
        }
        #endregion

        #region POST: ApplicationUser/Delete/5

        // POST: ApplicationUser/Delete/5 ----------------------------------------------
        //[HttpPost, ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, ApplicationUserDTO applicationUserDTO)
        {
            if (id != applicationUserDTO.Id) { return BadRequest(); }

            var applicationUser = await _applicationUserService.GetWithAssignmentsByIdAsync(id);

            if (applicationUser == null)
            {
                //return NotFound();
                ModelState.AddModelError(string.Empty,
                                "A Felhasználót  egy másik felhasználó törölte." +
                                "\nKattints a [Vissza] gombra.");
                return View(applicationUserDTO);
            }

            if (applicationUser.Assignments.Any())
            {
                Log.Information("There is related entity for this ApplicationUser.");
                ModelState.AddModelError(string.Empty,
                                "A Felhasználó nem törölhető! " +
                                "A Felhasználóra hivatkozásokat tartalmaz az adatbázis." +
                                "\nKattints a [Vissza] gombra.");
                // Back to Delete page with error message
                return View(_mapper.Map<ApplicationUserDTO>(applicationUser));
            }

            try
            {
                await _applicationUserService.RemoveAsync(applicationUser);

                // A jogokat is törölni kell!!!
                // Itt a DB cascade miatt törli a jogokat is - OK :-)
                //await _userManager.DeleteAsync(applicationUser);
            }
            // Database constraint voliation
            catch (DbUpdateException ex)
            {
                Log.Information("Database constraint voliation.", ex);

                ModelState.Clear();
                ModelState.AddModelError(string.Empty,
                                "A Felhasználó nem törölhető! " +
                                "A Felhasználóra hivatkozásokat tartalmaz az adatbázis." +
                                "\nKattints a [Vissza] gombra.");

                // Back to Delete page with error message
                return View(applicationUserDTO);
            }
            // update success
            return RedirectToAction(nameof(Index));

        }
        #endregion

        // ---------------------------- helper methods ----------------------------

        #region helper methods: AssembleUserViewModel(ApplicationUser applicationUser) 
        private async Task<UserViewModel> AssembleUserViewModel(ApplicationUser applicationUser)
        {

            var userRoles = new List<UserRoleViewModel>();

            var applicationRoles = await _roleManager.Roles.ToListAsync();

            var userRoleNames = await _userManager.GetRolesAsync(applicationUser);

            foreach (var applicationRole in applicationRoles)
            {
                userRoles.Add(new UserRoleViewModel
                {
                    Id = applicationRole.Id,
                    RoleName = applicationRole.Name,
                    HasRole = userRoleNames.Contains(applicationRole.Name)
                });
            }

            UserViewModel user = new UserViewModel
            {
                Id = applicationUser.Id,
                UserName = applicationUser.UserName,
                SurName = applicationUser.SurName,
                ForeName = applicationUser.ForeName,
                Email = applicationUser.Email,
                PhoneNumber = applicationUser.PhoneNumber,
                Description = applicationUser.Description,
                LockoutEnabled = applicationUser.LockoutEnabled,
                EmailConfirmed = applicationUser.EmailConfirmed,
                SecurityStamp = applicationUser.SecurityStamp,
                ConcurrencyStamp = applicationUser.ConcurrencyStamp,
                RowVersion = applicationUser.RowVersion,

                UserRoles = userRoles
            };
            return user;
        }
        #endregion

        #region helper methods: ConcurrencyHandler(DbUpdateConcurrencyException ex, ref ApplicationUserDTO applicationUserDTO)
        private void ConcurrencyHandler(DbUpdateConcurrencyException ex,
                                       ref ApplicationUserDTO applicationUserDTO)
        {
            var exceptionEntry = ex.Entries.Single();
            var clientValues = (ApplicationUser)exceptionEntry.Entity;
            var databaseEntry = exceptionEntry.GetDatabaseValues();

            ModelState.Clear();

            if (databaseEntry == null)
            {
                ModelState.AddModelError(string.Empty,
                                "A módosításokat nem lehet menteni." +
                                "A Felhasználót egy másik felhasználó törölte." +
                                "\nKattints a [Vissza] gombra.");
            }
            else
            {
                var databaseValues = (ApplicationUser)databaseEntry.ToObject();

                applicationUserDTO.RowVersion = (byte[])databaseValues.RowVersion;
                ModelState.Remove("RowVersion");

                ModelState.AddModelError(string.Empty,
                                "A szerkesztés alatt a Felhasználó adatait " +
                                "egy másik felhasználó megváltoztatta." +
                                "\nPirossal az adatbázisban jelenleg tárolt," +
                                " eltérő adatok láthatóak." +
                                "\nA [Mentés] gombra kattintva az adatbázis" +
                                " adatai felülíródnak." +
                                "\nA [Vissza] gombra kattintva módosításaid" +
                                " elvesznek.");

                if (databaseValues.SurName != clientValues.SurName)
                {
                    ModelState.AddModelError(nameof(databaseValues.SurName),
                        $"Adatbázisban: {databaseValues.SurName}");
                }
                if (databaseValues.ForeName != clientValues.ForeName)
                {
                    ModelState.AddModelError(nameof(databaseValues.ForeName),
                        $"Adatbázisban: {databaseValues.ForeName}");
                }
                if (databaseValues.PhoneNumber != clientValues.PhoneNumber)
                {
                    ModelState.AddModelError(nameof(databaseValues.PhoneNumber),
                        $"Adatbázisban: {databaseValues.PhoneNumber}");
                }
                if (databaseValues.Email != clientValues.Email)
                {
                    ModelState.AddModelError(nameof(databaseValues.Email),
                        $"Adatbázisban: {databaseValues.Email}");
                }
                if (databaseValues.Description != clientValues.Description)
                {
                    ModelState.AddModelError(nameof(databaseValues.Description),
                        $"Adatbázisban: {databaseValues.Description}");
                }
            }
        }
        #endregion        
    }
}