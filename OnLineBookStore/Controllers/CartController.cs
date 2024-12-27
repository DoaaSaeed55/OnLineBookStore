using Microsoft.AspNetCore.Mvc;
using OnLineBookStore.Data;
using OnLineBookStore.Models;

namespace OnLineBookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly Cart _cart;
        private readonly OnLineBookStoreContext _context;

        public CartController(Cart cart ,OnLineBookStoreContext context) 
        {
            _cart = cart;
            _context = context;
        }
        public IActionResult Index()
        {
            var items=_cart.GetAllCartItems();
            _cart.CartItems = items;
            return View(_cart);
        }

        public IActionResult AddToCart(int id)
        {
            var selectedBook=GetBookById(id);
            if (selectedBook != null) 
            {
                _cart.AddToCart(selectedBook, quantity: 1);
            
            }
            return RedirectToAction(actionName: "Index",controllerName:"Store");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var selectedBook = GetBookById(id);
            if (selectedBook != null)
            {
                _cart.RemoveFromCart(selectedBook);

            }
            return RedirectToAction(actionName: "Index");
        }
        public IActionResult ReduceQuantity(int id)
        {
            var selectedBook = GetBookById(id);
            if (selectedBook != null)
            {
                _cart.ReduceQuantity(selectedBook);

            }
            return RedirectToAction(actionName: "Index");
        }

        public IActionResult IncreaseQuantity(int id)
        {
            var selectedBook = GetBookById(id);
            if (selectedBook != null)
            {
                _cart.IncreaseQuantity(selectedBook);

            }
            return RedirectToAction(actionName: "Index");
        }

        public IActionResult ClearCart()
        {
            _cart.ClearCart();
            return RedirectToAction(actionName: "Index");
        }
        public Book GetBookById(int id)
        {
            return _context.Books.FirstOrDefault(b => b.Id == id);
        }
    }
}
