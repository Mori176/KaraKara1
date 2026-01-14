using System;

namespace MiniProject_Karakara
{
    // Concrete State: New (Pending)
    public class NewOrderState : IOrderState
    {
        public void ConfirmOrder(OrderContext context)
        {
            // Transition to Confirmed
            context.SetState(new ConfirmedState());
        }

        public void CancelOrder(OrderContext context)
        {
            // Transition to Cancelled
            // Logic to restock items could go here if needed.
            context.SetState(new CancelledState());
        }

        public void ShipOrder(OrderContext context)
        {
             // Invalid transition for this simple example, or go to Shipped
             throw new InvalidOperationException("Cannot ship a new order without confirmation.");
        }

        public string GetStatus()
        {
            return "Pending"; // Matches DB definition
        }
    }

    // Concrete State: Confirmed
    public class ConfirmedState : IOrderState
    {
        public void ConfirmOrder(OrderContext context)
        {
            // Already confirmed
        }

        public void CancelOrder(OrderContext context)
        {
            // Transition to Cancelled
            // Need to Restock items ideally.
             context.SetState(new CancelledState());
        }

        public void ShipOrder(OrderContext context)
        {
            // context.SetState(new ShippedState());
        }

        public string GetStatus()
        {
            return "Confirmed";
        }
    }

    // Concrete State: Cancelled
    public class CancelledState : IOrderState
    {
        public void ConfirmOrder(OrderContext context)
        {
             throw new InvalidOperationException("Cannot confirm a cancelled order.");
        }

        public void CancelOrder(OrderContext context)
        {
            // Already cancelled
        }

        public void ShipOrder(OrderContext context)
        {
             throw new InvalidOperationException("Cannot ship a cancelled order.");
        }

        public string GetStatus()
        {
            return "Cancelled";
        }
    }
}
