using Microsoft.EntityFrameworkCore;
using OnLineBookStore.Data;

namespace OnLineBookStore.Models
{
    public class Cart
    {
        private readonly OnLineBookStoreContext _context;

        public Cart(OnLineBookStoreContext context)
        {
            _context = context;
        }
        public string Id { get; set; }
        public List<CartItem> CartItems { get; set; }
        public static Cart GetCart(IServiceProvider service)
        {
            ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = service.GetService<OnLineBookStoreContext>();
            string cartId = session.GetString(key: "Id") ?? Guid.NewGuid().ToString();
            session.SetString(key: "Id", value: cartId);
            return new Cart(context) { Id = cartId };
        }
        public  List<CartItem> GetAllCartItems()
        {
            return CartItems ??
                (CartItems = _context.CartItems.Where(c => c.CartId == Id)
                .Include(c => c.Book).ToList());
        }

        public int GetCartTotal()
        {
            return _context.CartItems.Where(si => si.CartId == Id)
                                     .Select(si => si.Book.Price * si.Quantity)
                                     .Sum();
        }

        public CartItem GetCartItem(Book book)
        {
            return _context.CartItems.SingleOrDefault(ci =>
            ci.Book.Id == book.Id && ci.CartId == Id);
        }
        public void AddToCart(Book book,int quantity)
        {
            var cartItem=GetCartItem(book);
            if (cartItem == null)
            {
                cartItem = new CartItem()
                {
                    Book = book,
                    Quantity = quantity,
                    CartId = Id
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity+= quantity;
            }
            _context.SaveChanges();
        }

        public int ReduceQuantity(Book book)
        {
            var cartItem = GetCartItem(book);
            var remainingQuantity = 0;
            if (cartItem != null) 
            {
                if (cartItem.Quantity > 1)
                {
                    remainingQuantity =--cartItem.Quantity;
                }
                else
                {
                    _context.CartItems.Remove(cartItem);
                }
            }
            _context.SaveChanges();
            return remainingQuantity;
        }

        public int IncreaseQuantity(Book book)
        {
            var cartItem = GetCartItem(book);
            var remainingQuantity = 0;
            if (cartItem != null)
            {
                if (cartItem.Quantity > 0)
                {
                    remainingQuantity = ++cartItem.Quantity;
                }
                
            }
            _context.SaveChanges();
            return remainingQuantity;
        }

        public void RemoveFromCart(Book book)
        {
            var cartItem = GetCartItem(book);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
            }
            _context.SaveChanges();
        }

        public void ClearCart()
        {
            var cartItems = _context.CartItems.Where(ci => ci.CartId == Id);
            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();
        }

    }
}
