using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


using System.Threading.Tasks;

namespace HrSystem.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountsService _userService;

        public AccountsController(IAccountsService userService)
        {
            _userService = userService;
        }
        [Authorize(Policy = AuthConstants.Permissions.Show)]
        public async Task<IActionResult> Index()
        {

            var model = await _userService.GetAllUsersAsync();
            return View(model);
        }
        [Authorize(Policy = AuthConstants.Permissions.Add)]
        public async Task<IActionResult> Create()
        {
            var model = await _userService.GetCreateUserViewModelAsync();
            return View(model);
        }

        [Authorize(Policy = AuthConstants.Permissions.Add)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateUserAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                }
            }

            model.AvailableRoles = (await _userService.GetCreateUserViewModelAsync()).AvailableRoles;
            return View(model);
        }
        [Authorize(Policy = AuthConstants.Permissions.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var model = await _userService.GetEditUserViewModelAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [Authorize(Policy = AuthConstants.Permissions.Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Password) && string.IsNullOrEmpty(model.ConfirmPassword))
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }
            if (ModelState.IsValid)
            {

                var result = await _userService.UpdateUserAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                }
            }

            model.AvailableRoles = (await _userService.GetCreateUserViewModelAsync()).AvailableRoles;
            return View(model);
        }
        [Authorize(Policy = AuthConstants.Permissions.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var model = await _userService.GetDeleteUserViewModelAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [Authorize(Policy = AuthConstants.Permissions.Delete)]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(DeleteUserViewModel model)
        {
            var result = await _userService.DeleteUserAsync(model.Id);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                Console.WriteLine(User.Identity.Name);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(LoginViewModel model)
        {
            await _userService.Signout();
            try
            {
                var result = await _userService.Signin(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return View("Login", model);
        }


        public async Task<IActionResult> Logout()
        {
            this._userService.Signout();
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> UserProfile()
        {
            var user = await _userService.getUserData(User); 
            return View(user); 
        }

        public async Task<IActionResult> ResetPassword()
        {
            var currentUser = await _userService.getUserData(User);

            if (currentUser == null)
            {
                return NotFound("User not found");
            }

            var model = new ResetUserPasswordViewModel
            {
                UserId = currentUser.Id 
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetUserPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = await _userService.getUserData(User);

            if (currentUser == null)
            {
                return NotFound("User not found");
            }

            if (model.NewPassword != model.ConfirmNewPassword)
            {
                ModelState.AddModelError(string.Empty, "New password and confirmation do not match.");
                return View(model);
            }

            var result = await _userService.ResetPasswordAsync(new ResetUserPasswordViewModel
            {
                UserId = currentUser.Id,
                OldPassword = model.OldPassword,
                NewPassword = model.NewPassword
            });

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                Console.WriteLine(error.Description);
            }

            return View(model);
        }





    }

    public class AccountController: Controller 
    {
        public IActionResult Login()
        {
            return RedirectToAction("Login", "Accounts");
        }
    }
}
