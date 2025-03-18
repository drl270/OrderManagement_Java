package Mod5;

public class Order 
{
    private int orderId;
    private int traderId;
    private Security.SecurityType security;
    private int quantity;
    private String orderType;

    // Constructor with optional default values use primarily to test DB
    public Order(int orderId, Security.SecurityType security, String orderType, Integer traderId, Integer quantity) 
    {
        this.orderId = orderId;
        this.security = security;
        this.orderType = orderType;
        this.traderId = (traderId != null) ? traderId : 0; // Default to 0 if null
        this.quantity = (quantity != null) ? quantity : 0; // Default to 0 if null
    }

    // Overloaded constructor for cases where traderId and quantity are known
    public Order(int orderId, int traderId, Security.SecurityType security, int quantity, String orderType) 
    {
        this(orderId, security, orderType, traderId, quantity);
    }


    public int getOrderId() 
    {
        return orderId;
    }

    public int getTraderId() 
    {
        return traderId;
    }

    public Security.SecurityType getSecurity() 
    {
        return security;
    }

    public int getQuantity()
    {
        return quantity;
    }

    public String getOrderType()
    {
        return orderType;
    }
}