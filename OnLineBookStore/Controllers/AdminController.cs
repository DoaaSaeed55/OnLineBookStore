using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnLineBookStore.Models;
using OnLineBookStore.ViewModel;

namespace OnLineBookStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult ListAllRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public IActionResult AddRole()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleViewModel model)
        {
            if (ModelState.IsValid) 
            {
                IdentityRole identityRole = new()
                {
                    Name=model.RoleName
                };
                var resule=await _roleManager.CreateAsync(identityRole);
                if (resule.Succeeded)
                {
                    return RedirectToAction(actionName:"ListAllRoles");
                }

                foreach (var error in resule.Errors)
                {
                    ModelState.AddModelError(key:"", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role=await _roleManager.FindByIdAsync(id);
            if(role == null)
            {
                ViewData[index: "Error Message"] = $"No role with id '{id}' Was found";
                return View(viewName:"Error");
            }
            EditRoleViewModel model= new EditRoleViewModel()
            {
                Id=role.Id,
                RoleName=role.Name
            };
            foreach(var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewData[index: "Error Message"] = $"No role with id '{model.Id}' Was found";
                return View(viewName: "Error");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction(actionName: "ListAllRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            ViewData[index: "roleId"] = id;    
            ViewData[index: "roleName"] = role.Name;
            if (role == null)
            {
                ViewData[index: "ErrorMessage"] = $"No role with id '{id}' Was found";
                return View(viewName: "Error");
            }
            var model = new List<UserRoleViewModel>();
            foreach (var user in _userManager.Users)
            {
                UserRoleViewModel roleViewModel = new()
                {
                    Id=user.Id,
                    Name=user.UserName
                };
                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    roleViewModel.IsSelected = true;
                }
                else
                {
                    roleViewModel.IsSelected = false;
                }
                model.Add(roleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model,string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
           
            if (role == null)
            {
                ViewData[index: "ErrorMessage"] = $"No role with id '{id}' Was found";
                return View(viewName: "Error");
            }
           
            for(int i=0;i<model.Count;i++)
            {
               var user=await _userManager.FindByIdAsync(model[i].Id);
                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if(!(model[i].IsSelected) && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                
            }
            return RedirectToAction(actionName: "EditRole", new {Id=id});
        }




    }
}
