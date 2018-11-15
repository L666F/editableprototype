using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EditablePrototype.Models;
using EditablePrototype.Models.Data;
using EditablePrototype.Models.VMs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace EditablePrototype.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext context;

        public UsersController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View(new RegisterVM());
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterVM vm)
        {
            vm.Message = null;
            if (!ModelState.IsValid)
                return View(vm);
            else
            {
                //USERNAME UNIQUE
                if (context.Users.Where(m => m.Username == vm.Username).Count() > 0)
                {
                    vm.Message = "The username is already in use.";
                }
                    

                //PASSWORD REQUIREMENTS
                if (!vm.Password.Any(char.IsLower))
                    vm.Message = "The password should contain at least 1 lowercase letter.";
                if (!vm.Password.Any(char.IsUpper))
                    vm.Message = "The password should contain at least 1 uppercase letter.";
                if (!vm.Password.Any(char.IsNumber))
                    vm.Message = "The password should contain at least 1 number.";
                if (!vm.Password.Any(char.IsSymbol))
                    vm.Message = "The password should contain at least 1 symbol.";

                if (vm.Message != null)
                    return View(vm);

                //SAVE USER

                var user = new User()
                {
                    Username = vm.Username,
                    PasswordHash = SecurePasswordHasher.Hash(vm.Password),
                    CanEditAboutUs = false,
                    CanEditContacts = false,
                    CanEditHome = false
                };

                if (context.Users.Count() == 0)
                    user.IsAdmin = true;

                await context.AddAsync(user);

                await context.SaveChangesAsync();

                return RedirectToAction("Login", "Users");
            }
        }

        public ActionResult Login()
        {
            if (HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            else
                return View(new LoginVM());
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginVM vm)
        {
            vm.Message = null;

            if (!ModelState.IsValid)
                return View(vm);
            else
            {
                var user = context.Users.SingleOrDefault(m => m.Username == vm.Username && SecurePasswordHasher.Verify(vm.Password, m.PasswordHash));
                if(user == null)
                {
                    vm.Message = "Wrong username and/or password.";
                    return View(vm);
                }

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, vm.Username)
                    };
                ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(principal);

                return Redirect("/");
            }
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Users");
        }
    }
}