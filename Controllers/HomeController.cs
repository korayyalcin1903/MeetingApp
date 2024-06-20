using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MeetingApp.Models;
using MeetingApp.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MeetingApp.Data.Entity;
using MeetingApp.Data.Concrete;
using MeetingApp.Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace MeetingApp.Controllers;

public class HomeController : Controller
{
    private readonly MeetingContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public HomeController(MeetingContext context, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View(_context.Meetings.Take(3).ToList());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register(){
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model, IFormFile imageFile)
    {
        var user = await _userManager.FindByEmailAsync(model.Email); // Doğru kullanıcıyı bulmak için UserManager kullanın.

        var extension = "";

        if (imageFile != null)
        {
            var allowExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var maxSize = 600;
            extension = Path.GetExtension(imageFile.FileName);
            Console.WriteLine(imageFile.Length);

            if (!allowExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Geçerli bir resim seçiniz.");
            }
        }

        if (ModelState.IsValid)
        {
            if (user == null && imageFile != null)
            {
                var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/users", randomFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                user = new User
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    UserName = model.Email,
                    Email = model.Email,
                    Image = randomFileName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "E posta kullanılmaktadır");
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login(){
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: true, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    ViewData["Photo"] = "1.jpg";
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Email veya şifre yanlış");
        }

        return View(model);
    }


    public async Task<IActionResult> Logout(){
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login","Home");
    }
}
