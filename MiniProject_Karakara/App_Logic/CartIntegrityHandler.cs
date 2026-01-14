using System.Data;

namespace MiniProject_Karakara
{
    // Concrete Handler 1: Cart Integrity
    public class CartIntegrityHandler : OrderValidationHandler
    {
        public override bool Validate(int userId, DataTable cartItems, out string error)
        {
            error = string.Empty;

            if (cartItems == null || cartItems.Rows.Count == 0)
            {
                error = "Cart is empty.";
                return false;
            }

            // Could add more checks here (e.g., price validity)

            if (_nextHandler != null)
            {
                return _nextHandler.Validate(userId, cartItems, out error);
            }

            return true;
        }
    }
}
