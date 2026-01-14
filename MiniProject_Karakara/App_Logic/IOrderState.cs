namespace MiniProject_Karakara
{
    // Design Pattern 7: State Pattern
    // Allows an object to alter its behavior when its internal state changes.
    // The object will appear to change its class.
    public interface IOrderState
    {
        void ConfirmOrder(OrderContext context);
        void CancelOrder(OrderContext context);
        void ShipOrder(OrderContext context);
        string GetStatus();
    }
}
