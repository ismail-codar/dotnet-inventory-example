using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiasInventory.Data;
using DiasInventory.Models;

namespace DiasInventory.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _db;
        public DeleteModel(AppDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public Category Category { get; set; }
        public void OnGet(int id)
        {
            Category = _db.Categories.Find(id);
        }
        public async Task<IActionResult> OnPost()
        {
            if (Category != null)
            {
                _db.Categories.Remove(Category);
                await _db.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
