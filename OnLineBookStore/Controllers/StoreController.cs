using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnLineBookStore.Data;

namespace OnLineBookStore.Controllers
{
    [AllowAnonymous]
    public class StoreController : Controller
    {
        private readonly OnLineBookStoreContext _context;

        public StoreController(OnLineBookStoreContext context) 
        {
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
    }
}
