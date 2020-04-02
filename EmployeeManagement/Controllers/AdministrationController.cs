using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    //this is using claim base policy 
    [Authorize(Policy = "AdminRolePolicy")] //=>for role base poliyc [Authorize(Role="Admin")] 
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AdministrationController> logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<AdministrationController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult ListUsers()
        {

            var users = userManager.Users;

            return View(users);
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = viewModel.RoleName
                };
                var result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {Id} was not found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }


        }


        [HttpPost]
        [Authorize(Policy= "DelectRolePolicy")]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {Id} was not found";
                return View("NotFound");
            }
            else
            {
                try
                {

                    var result = await roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View("ListRoles");
                }
                catch (DbUpdateException ex)
                {

                    logger.LogError($"Error deleting Role: ", ex);
                    ViewBag.ErrorTitle = $"{role.Name} role is in use";

                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role. If you want to delete this role, please remove the users first";

                    return View("Error");


                }
            }


        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;

            return View(roles);
        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")] //this is used to protect the editrole from user using the querystring manually to access this action method
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {id} cannot be found";
                return View("NotFound");
            }
            var viewModel = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    viewModel.Users.Add(user.UserName);
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")] 
        public async Task<IActionResult> EditRole(EditRoleViewModel viewModel)
        {
            var role = await roleManager.FindByIdAsync(viewModel.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {viewModel.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = viewModel.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }


            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
                return View("NotFound");
            }
            ViewBag.roleName = role.Name;

            var viewModelList = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                viewModelList.Add(userRoleViewModel);

            }

            return View(viewModelList);
        }


        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> viewModel, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < viewModel.Count; i++)
            {
                var user = await userManager.FindByIdAsync(viewModel[i].UserId);

                IdentityResult result = null;
                if (viewModel[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);

                }
                else if (!viewModel[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {

                    result = await userManager.RemoveFromRoleAsync(user, role.Name);


                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < viewModel.Count - 1)
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { Id = roleId });
                    }
                }
            }


            return RedirectToAction("EditRole", new { Id = roleId });
        }


        [HttpGet]
       
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id {id} was not found";
                return View("NotFound");
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var editUser = new EditUserViewModel
            {
                Id = user.Id,
                City = user.City,
                Email = user.Email,
                UserName = user.UserName,
                Claims = userClaims.Select(c => $"{ c.Type } : {c.Value}").ToList(),
                Roles = userRoles
            };

            return View(editUser);


        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id {model.Id} was not found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.City = model.City;
                user.UserName = model.UserName;
                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }

                return View(model);
            }


        }


        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id {userId} was not found";
                return View("Error");
            }
            ViewBag.UserName = user.UserName;

            var roles = roleManager.Roles;
            var model = new List<UserRolesViewModel>();
            foreach (var role in roles)
            {
                var rv = new UserRolesViewModel();
                rv.RoleId = role.Id;
                rv.RoleName = role.Name;
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    rv.IsSelected = true;
                }
                else
                {
                    rv.IsSelected = false;
                }
                model.Add(rv);
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id {userId} was not found";
                return View("Error");
            }
            ViewBag.UserName = user.UserName;
            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user from existing roles");
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user, model.Where(r => r.IsSelected).Select(r => r.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add user to existing roles");
                return View(model);

            }

            return RedirectToAction("EditUser", new { id = userId });


        }


        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} was not found";
                return View("NotFound");
            }

            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userId,
            };

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                //if the user has the claim, set Isselecte property to true, so the checkbox 
                //next to the clim is cheked on the UI
                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
                {
                    userClaim.IsSelected = true;

                }
                model.Claims.Add(userClaim);

            }
           

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel viewModel)
        {
            var user = await userManager.FindByIdAsync(viewModel.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {viewModel.UserId} was not found";
                return View("NotFound");
            }

            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(viewModel);
            }


            result = await userManager.AddClaimsAsync(user, viewModel.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Unable to update claims");
                return View(viewModel);

            }

            return RedirectToAction("EditUser", new { id = viewModel.UserId });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
