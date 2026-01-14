using System.Data;

namespace MiniProject_Karakara
{
    // Design Pattern 6: Chain of Responsibility
    // Defines a handler interface and maintains a reference to the next handler.
    public abstract class OrderValidationHandler
    {
        protected OrderValidationHandler _nextHandler;

        public void SetNext(OrderValidationHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public abstract bool Validate(int userId, DataTable cartItems, out string error);
    }
}
