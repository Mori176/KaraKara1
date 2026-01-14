using System;
using System.Data;

namespace MiniProject_Karakara
{
    // Concrete Handler 2: Stock Check
    public class StockHandler : OrderValidationHandler
    {
        private ProductRepository _productRepo = new ProductRepository();

        public override bool Validate(int userId, DataTable cartItems, out string error)
        {
            error = string.Empty;

            foreach (DataRow row in cartItems.Rows)
            {
                int productId = Convert.ToInt32(row["ProductId"]);
                int qty = Convert.ToInt32(row["Quantity"]);

                int currentStock = _productRepo.GetStock(productId);
                if (currentStock < qty)
                {
                    error = $"Insufficient stock for product: {row["ProductName"]}. Available: {currentStock}";
                    return false;
                }
            }

            if (_nextHandler != null)
            {
                return _nextHandler.Validate(userId, cartItems, out error);
            }

            return true;
        }
    }
}
