using JaleIdentity.Data;
using JaleIdentity.Dtos.ProductDtos;
using JaleIdentity.Extensions;
using JaleIdentity.Models;
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
        var products = await _context.Products.Where(x => !x.IsDeleted).Include(x => x.Category).ToListAsync();
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


        if (!ModelState.IsValid)
            return View();


        var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == dto.CategoryId);

        if (!isExistCategory)
        {
            ModelState.AddModelError("CategoryId", "Bele category movcud deyil");
            return View();
        }



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

        string filename = Guid.NewGuid() + dto.Image.FileName;
        var path = Path.Combine(_webHostEnvironment.WebRootPath, "img", filename);


        using (FileStream stream = new(path, FileMode.Create))
        {
            await dto.Image.CopyToAsync(stream);
        }

        Product product = new()
        {
            Name = dto.Name,
            Price = dto.Price,
            ImagePath = filename,
            CategoryId = dto.CategoryId,
        };


        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();






        return RedirectToAction("Index");
    }



    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return NotFound();


        var path = Path.Combine(_webHostEnvironment.WebRootPath, "img", product.ImagePath);


        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }

        product.IsDeleted = true;
        _context.Products.Update(product);

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");


    }


    public async Task<IActionResult> Update(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return NotFound();


        ProductPutDto dto = new()
        {
            Name = product.Name,
            Price = product.Price,
            CategoryId = product.CategoryId,
            ImagePath = product.ImagePath,
        };

        var categories = await _context.Categories.ToListAsync();

        ViewBag.Categories = categories;

        return View(dto);
    }


    [HttpPost]
    public async Task<IActionResult> Update(int id, ProductPutDto dto)
    {
        var categories = await _context.Categories.ToListAsync();

        ViewBag.Categories = categories;
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var existProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (existProduct is null)
            return NotFound();




        var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == dto.CategoryId);

        if (!isExistCategory)
        {
            ModelState.AddModelError("CategoryId", "Bele category movcud deyil");
            return View();
        }



        if (dto.Image is not null)
        {


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




            string filename = Guid.NewGuid() + dto.Image.FileName;
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "img", filename);


            using (FileStream stream = new(path, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }



            var removedImage = Path.Combine(_webHostEnvironment.WebRootPath, "img", existProduct.ImagePath);


            if (System.IO.File.Exists(removedImage))
            {
                System.IO.File.Delete(removedImage);
            }



            existProduct.ImagePath = filename;




        }


        existProduct.Name=dto.Name;
        existProduct.Price=dto.Price;
        existProduct.CategoryId = dto.CategoryId;


        _context.Products.Update(existProduct);
        await _context.SaveChangesAsync();





        return RedirectToAction("Index");
    }


}
