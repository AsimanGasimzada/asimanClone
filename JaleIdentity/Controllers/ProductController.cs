using JaleIdentity.Data;
using JaleIdentity.Dtos.ProductDtos;
using JaleIdentity.Extensions;
using JaleIdentity.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JaleIdentity.Controllers;

public class ProductController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.Include(x => x.Category).ToListAsync();
        return View(products);
    }


    public async Task<IActionResult> Create()
    {
        var categories = await _context.Categories.ToListAsync();

        ViewBag.Categories = categories;
        return View();

    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductPostDto dto)
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;


        //if (!ModelState.IsValid)
        //    return View();
        if (!dto.Image.ValidateType("image"))
        {
            ModelState.AddModelError("Image", "Xahis olunur image daxil edin");
            return View();
        }


        if (!dto.Image.ValidateSize(1))
        {
            ModelState.AddModelError("Image", "Sekilin olcusu 1 mb dan artiq ola bilmez");
            return View();
        }






        return Content(_webHostEnvironment.ContentRootPath);
    }



}
