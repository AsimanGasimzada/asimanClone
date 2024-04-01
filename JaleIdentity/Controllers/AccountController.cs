using JaleIdentity.Dtos.AppUserDtos;
using JaleIdentity.Enums;
using JaleIdentity.Models;
using JaleIdentity.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JaleIdentity.Controllers;

public class AccountController : Controller
{

    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly RoleManager<IdentityRole> _roleManager;


    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _emailSender = emailSender;
    }



    public class ResetPasswordDto
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
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


        if (!result.Succeeded)
        {

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }

        await _userManager.AddToRoleAsync(user, nameof(IdentityRoles.Member));

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        string? url = Url.Action("ConfirmEmail", "Account", new {userId=user.Id ,token=token},HttpContext.Request.Scheme);

        _emailSender.SendEmail(user.Email, "Hesab dogrulama", url);

        return Content("Zehmet olmasa emaili tesdiq edin.");

    }


    public async Task<IActionResult> sendToken()
    {


        var user = await _userManager.FindByIdAsync("c206deb7-0ec9-4b03-a543-6f0213517f18");

        var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);



        string? url = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = resetPasswordToken }, HttpContext.Request.Scheme);

        _emailSender.SendEmail(user.Email, "Reset Password", url);

        return Content("Ok");
    }

    [HttpGet]
    public async Task<IActionResult> ResetPassword(string userId,string token)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return NotFound();

        return View();

    }


    [HttpPost]
    public async Task<IActionResult> ResetPassword(string userId, string token,ResetPasswordDto dto)
    {
        if (!ModelState.IsValid)
            return View();

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return NotFound();

        var result = await _userManager.ResetPasswordAsync(user, token, dto.Password);


        if (!result.Succeeded)
            return BadRequest();


        await _signInManager.SignInAsync(user, false);


        return Content("Ok");
    }

    public async Task<IActionResult> ConfirmEmail(string token, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);


        if (user is null)
            return NotFound();



        var result = await _userManager.ConfirmEmailAsync(user, token);


        if (!result.Succeeded)
            return BadRequest();



        await _signInManager.SignInAsync(user, false);


        return Content("Hesabiniz ugurla aktiv oldu");
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


        return Content(user.Fullname + roles[0] + "  Salam meloyku");

    }


    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Login");
    }



    public IActionResult SendEmail()
    {
        _emailSender.SendEmail("asimanjg@code.edu.az", "salam", "Eloyku");


        return Content("Ok");

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
