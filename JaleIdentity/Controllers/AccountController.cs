using JaleIdentity.Dtos.AppUserDtos;
using JaleIdentity.Enums;
using JaleIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace JaleIdentity.Controllers;

public class AccountController : Controller
{

    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;


    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public IActionResult Register()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return View();

        AppUser user = new()
        {
            UserName = dto.UserName,
            Email = dto.Email,
            Fullname = dto.Fullname
        };


        var result = await _userManager.CreateAsync(user, dto.Password);


        if (result.Succeeded)
        {

            await _userManager.AddToRoleAsync(user, nameof(IdentityRoles.Member));
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Content("Ok");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View();

    }



    public IActionResult Login()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        if (!ModelState.IsValid)
            return View();


        var user = await _userManager.FindByEmailAsync(dto.Email);


        if (user is null)
        {
            ModelState.AddModelError("", "Email ve ya sifre yanlisdir");
            return View();
        }


        var result = await _signInManager.PasswordSignInAsync(user, dto.Password, false, true);


        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Email ve ya sifre yanlisdir");
            return View();
        }

        var roles = await _userManager.GetRolesAsync(user);


        //if (roles[0] == IdentityRoles.Admin.ToString())
        //{
        //    return RedirectToAction("adminpanel");
        //}


        return Content(user.Fullname +  roles[0] + "  Salam meloyku");

    }


    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Login");
    }




    //public async Task<IActionResult> CreateRoles()
    //{
    //    foreach (var role in Enum.GetNames(typeof(IdentityRoles)))
    //    {
    //        IdentityRole identityRole = new() { Name = role.ToString() };

    //        await _roleManager.CreateAsync(identityRole);
    //    }


    //    return RedirectToAction("Login");
    //}
}
