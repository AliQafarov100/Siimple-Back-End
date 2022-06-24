using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Siimple_Back_End.DAL;
using Siimple_Back_End.Models;
using Siimple_Back_End.ViewModels;

namespace Siimple_Back_End.Controllers
{
    public class AccountController : Controller
    {
       
        private readonly UserManager<AppUser> _manager;
        private readonly SignInManager<AppUser> _signIn;

        public AccountController(UserManager<AppUser> manager,SignInManager<AppUser> signIn)
        {
            _manager = manager;
            _signIn = signIn;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> Register(RegisterVM register)
        {
            AppUser user = new AppUser
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.Username,
                Email = register.Email
            };
            IdentityResult result = await _manager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> Login(LoginVM login)
        {
            AppUser user =  await _manager.FindByNameAsync(login.Username);

            Microsoft.AspNetCore.Identity.SignInResult result = await _signIn.PasswordSignInAsync(user, login.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Incorrect password or username");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signIn.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit()
        {
            AppUser user = await _manager.FindByNameAsync(User.Identity.Name);

            EditUserVM userVM = new EditUserVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.UserName
            };

            return View(userVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> Edit(EditUserVM edit)
        {
            AppUser existedUser = await _manager.FindByNameAsync(User.Identity.Name);

            EditUserVM editUser = new EditUserVM
            {
                FirstName = existedUser.FirstName,
                LastName = existedUser.LastName,
                Username = existedUser.UserName
            };

            if (!ModelState.IsValid) return NotFound();

            bool result = edit.CurrentPassword != null && edit.NewPassword == null && edit.ConfirmPassword == null;

            if(edit.Email == null || edit.Email != existedUser.Email)
            {
                ModelState.AddModelError("", "Email cannot changed");
                return View(editUser);
            }

            if (result)
            {
                existedUser.FirstName = edit.FirstName;
                existedUser.LastName = edit.LastName;
                existedUser.UserName = edit.Username;

                await _manager.UpdateAsync(existedUser);
                
            }
            else
            {
                existedUser.FirstName = edit.FirstName;
                existedUser.LastName = edit.LastName;
                existedUser.UserName = edit.Username;

                IdentityResult identityResult = await _manager.ChangePasswordAsync(existedUser, edit.CurrentPassword, edit.NewPassword);

                if (!identityResult.Succeeded)
                {
                    foreach(IdentityError error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(editUser);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
