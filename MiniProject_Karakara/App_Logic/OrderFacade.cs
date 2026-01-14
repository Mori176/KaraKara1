using System;
using System.Data;
using System.Transactions;

namespace MiniProject_Karakara
{
    // Design Pattern 4: Facade
    // Provides a simplified interface to a complex subsystem (ordering process).
    // The Client (CartPage) just calls "PlaceOrder" and doesn't know about 
    // transactions, stock checking, or tax calculations.
    public class OrderFacade
    {
        private CartRepository _cartRepo = new CartRepository();
        private ProductRepository _productRepo = new ProductRepository();
        private OrderRepository _orderRepo = new OrderRepository();
        private PricingStrategy _pricing = new PricingStrategy();

        public bool PlaceOrder(int userId, out string orderNumber, out string error)
        {
            orderNumber = GenerateOrderNumber();
            error = string.Empty;

            try
            {
                // 1. Get Cart
                DataTable cartItems = _cartRepo.GetCartItems(userId);
                if (cartItems.Rows.Count == 0)
                {
                    error = "Cart is empty.";
                    return false;
                }

                decimal subTotal = 0;
                foreach (DataRow row in cartItems.Rows)
                {
                    subTotal += Convert.ToDecimal(row["TotalItemPrice"]);
                }

                decimal tax = _pricing.CalculateTax(subTotal);
                decimal grandTotal = _pricing.CalculateTotal(subTotal);

                // Design Pattern 6: Chain of Responsibility usage
                CartIntegrityHandler integrity = new CartIntegrityHandler();
                StockHandler stock = new StockHandler();
                integrity.SetNext(stock);

                if (!integrity.Validate(userId, cartItems, out error))
                {
                    return false;
                }

                // Use TransactionScope to ensure atomicity across Repositories
                using (TransactionScope scope = new TransactionScope())
                {
                    // Deduct Stock (Validation already passed, but we deduct here)
                    // Note: In a real high-concurrency app, we might check again or lock.
                    foreach (DataRow row in cartItems.Rows)
                    {
                        int productId = Convert.ToInt32(row["ProductId"]);
                        int qty = Convert.ToInt32(row["Quantity"]);
                        _productRepo.DeductStock(productId, qty);
                    }

                    // 3. Create Order
                    _orderRepo.CreateOrder(userId, orderNumber, DateTime.Now, subTotal, tax, grandTotal, cartItems);

                    // 4. Clear Cart
                    _cartRepo.ClearCart(userId);

                    scope.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        private string GenerateOrderNumber()
        {
            return "ORD" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
    }
}
