package Mod5;

import Mod5.Security.SecurityType;
import Mod5.CustomLogging;
import java.util.logging.Level;
import java.util.HashMap;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;

public class OrderManagementSystem 
{
	    private static final String ORDER_ID_FILE_PATH = "C:\\CommonDatabases\\orderId.txt";
	    private static OrderManagementSystem instance;
	    private HashMap<Integer, Order> orders;

	    private OrderManagementSystem() 
	    {
	        this.orders = new HashMap<>();
	    }
	    
	    public HashMap<Integer, Order> getOrders() 
	    {
	        return orders;
	    }
	    
	    public static OrderManagementSystem getInstance() 
	    {
	        if (instance == null) 
	        {
	            instance = new OrderManagementSystem();
	        }
	        return instance;
	    }

	    public int getNextOrderId() 
	    {
	        int orderId = 1;

	        try 
	        {
	            File orderIdFile = new File(ORDER_ID_FILE_PATH);
	            if (orderIdFile.exists())
	            {
	                FileReader fileReader = new FileReader(orderIdFile);
	                char[] buffer = new char[(int) orderIdFile.length()];
	                fileReader.read(buffer);
	                fileReader.close();
	                orderId = Integer.parseInt(new String(buffer)) + 1;
	            }

	            FileWriter fileWriter = new FileWriter(orderIdFile);
	            fileWriter.write(String.valueOf(orderId));
	            fileWriter.close();
	            
	        } 
	        catch (IOException e) 
	        {
	            CustomLogging.log(Level.SEVERE, "Error reading or writing order ID file: " + e.getMessage());
	        }

	        return orderId;
	    }

	    public void enterOrder(String symbol, SecurityType securityType) 
	    {
	        int orderId = getNextOrderId();
	        Order order = new Order(orderId, 0, securityType, 0, symbol);
	        orders.put(orderId, order);

	        CustomLogging.log(Level.INFO, "Order entered: " + orderId + ", " + symbol + ", " + securityType);
	    }

    public void validateOrder(Order order) 
    {
        try
        {
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
    
    public Order(int orderId, int traderId, Security.SecurityType security, int quantity, String orderType) 
    {
        this.orderId = orderId;
        this.traderId = traderId;
        this.security = security;
        this.quantity = quantity;
        this.orderType = orderType;
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

class Trader 
{
    private int traderId;
    private String name;
    private String email;

}



