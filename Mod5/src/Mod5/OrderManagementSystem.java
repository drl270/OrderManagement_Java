package Mod5;

import Mod5.Security.SecurityType;
import Mod5.CustomLogging;
import java.util.logging.Level;

public class OrderManagementSystem 
{
    public void enterOrder(String symbol, SecurityType securityType) 
    {
        Security security = new Security(symbol, securityType);
        CustomLogging.log(Level.INFO, "Order entered: " + symbol + ", " + securityType);
    }

    public void validateOrder(Order order) 
    {
        try
        {
            // Add logic 
            CustomLogging.log(Level.FINE, "Order validated: " + order.getOrderId());
        } 
        catch (Exception e) 
        {
            CustomLogging.log(Level.SEVERE, "Error validating order: " + e.getMessage());
        }
    }
}

class Order 
{
    private int orderId;
    private int traderId;
    private Security.SecurityType security;
    private int quantity;
    private String orderType;
    
    public int getOrderId() 
    {
        return orderId;
    }

}


class Trader 
{
    private int traderId;
    private String name;
    private String email;

}



