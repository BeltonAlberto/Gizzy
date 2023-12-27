using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
namespace gizzy;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;
    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View(_context.Categories.ToList());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (await _context.Categories.AnyAsync(c => c.Name == category.Name))
        {
            ModelState.AddModelError("Name", "Category already registered");
        }
        else if (category.Name == null)
        {
            ModelState.AddModelError("Name", "Name cannot be empty");
        }
        else if (ContainsNumbers(category.Name))
        {
            ModelState.AddModelError("Name", "Cannot contain numbers");
        }

        // Ensure the first letter of the Name is capitalized
        category.Name = CapitalizeFirstLetter(category.Name);

        if (ModelState.IsValid)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            TempData["success"] = $"{category.Name} Added Successfully!";
            return RedirectToAction("Index");

        }
        return View();

    }

    private bool ContainsNumbers(string input)
    {
        return input.Any(char.IsDigit);
    }

    private string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        return char.ToUpper(input[0]) + input.Substring(1);
    }

    public async Task<IActionResult> Edit(Guid Id)
    {
        var item = await _context.Categories.FindAsync(Id);
        if (item == null)
        {
            return View("NotFoundView");
        }
        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Category category)
    {
        var existingCategory = await _context.Categories
        .FirstOrDefaultAsync(c => c.Name == category.Name && c.Id != category.Id);

        if (existingCategory != null)
        {
            ModelState.AddModelError("Name", "Category with this name already exists");
        }
        else if (category.Name == null)
        {
            ModelState.AddModelError("Name", "Name cannot be empty");
        }
        else if (ContainsNumbers(category.Name))
        {
            ModelState.AddModelError("Name", "Cannot contain numbers");
        }

        // Ensure the first letter of the Name is capitalized
        category.Name = CapitalizeFirstLetter(category.Name);

        if (ModelState.IsValid)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            TempData["success"] = $"{category.Name} Updated Successfully!";
            return RedirectToAction("Index");
        }
        return View();
    }

    public async Task<IActionResult> Delete(Guid Id)
    {

        var item = await _context.Categories.FindAsync(Id);
        if (item == null)
        {
            return View("NotFoundView");
        }
        return View(item);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Category category)
    {
        if (category == null)
        {
            return View("NotFoundView");
        }
        _context.Categories.Remove(category);
        TempData["success"] = $"Excluded Successfully!";
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public IActionResult NotFoundView()
    {
        return View();
    }

}

