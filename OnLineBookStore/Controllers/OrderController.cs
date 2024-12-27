using Microsoft.AspNetCore.Mvc;
using OnLineBookStore.Data;
using OnLineBookStore.Models;

namespace OnLineBookStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly OnLineBookStoreContext _context;
        private readonly Cart _cart;

        public OrderController(OnLineBookStoreContext context,Cart cart) 
        {
            _context = context;
            _cart = cart;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CheckOut()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CheckOut(Order order)
        {
            var cartItems= _cart.GetAllCartItems();
            _cart.CartItems= cartItems;
            if (_cart.CartItems.Count == 0)
            {
                ModelState.AddModelError(key: "", errorMessage: "Cart is Empty Please Add Book First.");
            }
            if (ModelState.IsValid)
            {
                CreateOrder(order);
                _cart.ClearCart();
                return View(viewName: "CheckOutComplete",order);
            }
            return View(order);
        }


        public IActionResult CheckOutComplete(Order order)
        {
            return View(order);
        }
        public void CreateOrder(Order order)
        {
            order.OrderPlaced=DateTime.Now;
            var cartItems= _cart.CartItems;

            foreach (var item in cartItems)
            {
                var orderitem = new OrderItem()
                {
                    Quantity = item.Quantity,
                    BookId = item.Book.Id,
                    OrderId = order.Id,
                    Price = item.Book.Price * item.Quantity

                };
                order.OrderItems.Add(orderitem);
                order.OrderTotal += orderitem.Price;
            
            }
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
    }
}
