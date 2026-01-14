using System;

namespace MiniProject_Karakara
{
    // Context Class
    public class OrderContext
    {
        private IOrderState _currentState;
        public int OrderID { get; private set; }
        private OrderRepository _repo = new OrderRepository();

        public OrderContext(int orderId, string currentStatus)
        {
            OrderID = orderId;
            // Initialize state based on string from DB
            switch (currentStatus)
            {
                case "Pending":
                case "New":
                    _currentState = new NewOrderState();
                    break;
                case "Confirmed":
                    _currentState = new ConfirmedState();
                    break;
                case "Cancelled":
                    _currentState = new CancelledState();
                    break;
                default:
                    _currentState = new NewOrderState(); // Default
                    break;
            }
        }

        public void SetState(IOrderState state)
        {
            _currentState = state;
            // Persist the state change to DB
            _repo.UpdateOrderStatus(OrderID, _currentState.GetStatus());
        }

        public void Confirm()
        {
            _currentState.ConfirmOrder(this);
        }

        public void Cancel()
        {
            _currentState.CancelOrder(this);
        }

        public string GetStatus()
        {
            return _currentState.GetStatus();
        }
    }
}
