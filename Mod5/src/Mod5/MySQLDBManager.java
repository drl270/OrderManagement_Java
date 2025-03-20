package Mod5;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.HashMap;
import java.util.ArrayList;
import java.util.List;

import javax.swing.table.DefaultTableModel;

public class MySQLDBManager 
{
    private static final String SERVER = "192.168.1.33";
    private static final String PORT = "3306";
    private static final String DB_NAME = "mydb";
    private static final String TABLE_NAME = "ordermanagement";
    private static final String USERNAME = "user";
    private static final String PASSWORD = "Schmaltz1";

    public boolean testDBConnection() 
    {
        try 
        {
            Connection connection = DriverManager.getConnection("jdbc:mysql://" + SERVER + ":" + PORT + "/" + DB_NAME, USERNAME, PASSWORD);
            System.out.println("DB Connection successful");
            connection.close();
            return true;
        } 
        catch (SQLException e)
        {
            System.out.println("DB Connection failed: " + e.getMessage());
            return false;
        }
    }
    
    public void writeOrdersToDB(HashMap<Integer, Order> orders) 
    {
        try (Connection connection = DriverManager.getConnection("jdbc:mysql://" + SERVER + ":" + PORT + "/" + DB_NAME, USERNAME, PASSWORD)) 
        {
            String query = "INSERT INTO " + TABLE_NAME + " (orderid, securitytype, symbol) VALUES (?, ?, ?)";
            PreparedStatement statement = connection.prepareStatement(query);

            for (Order order : orders.values()) 
            {
                statement.setInt(1, order.getOrderId());
                statement.setString(2, order.getSecurity().toString());
                statement.setString(3, order.getOrderType());
                statement.executeUpdate();
            }
        } 
        catch (SQLException e) 
        {
            System.out.println("Error writing orders to DB: " + e.getMessage());
        }
    }
    
    // Returns all 'Order' objects 
    public void readOrdersFromDB(DefaultTableModel ordersTableModel) 
    {
        try (Connection connection = DriverManager.getConnection("jdbc:mysql://" + SERVER + ":" + PORT + "/" + DB_NAME, USERNAME, PASSWORD)) 
        {
            String query = "SELECT orderid, securitytype, symbol FROM " + TABLE_NAME;
            Statement statement = connection.createStatement();
            ResultSet resultSet = statement.executeQuery(query);

            while (resultSet.next()) 
            {
                Object[] row = new Object[] 
                {
                    resultSet.getInt("orderid"),
                    resultSet.getString("securitytype"),
                    resultSet.getString("symbol")
                };
                ordersTableModel.addRow(row);
            }
        } 
        catch (SQLException e) 
        {
            System.out.println("Error reading orders from DB: " + e.getMessage());
        }
    }
    
   // Returns 'Order' object by ID
    public Order getOrderById(int orderId) 
    {
        Order order = null;
        try (Connection connection = DriverManager.getConnection("jdbc:mysql://" + SERVER + ":" + PORT + "/" + DB_NAME, USERNAME, PASSWORD)) {
            String query = "SELECT securitytype, symbol FROM " + TABLE_NAME + " WHERE orderid = ?";
            PreparedStatement statement = connection.prepareStatement(query);
            statement.setInt(1, orderId);
            ResultSet resultSet = statement.executeQuery();

            if (resultSet.next()) 
            {
                String securityTypeString = resultSet.getString("securitytype");
                Security.SecurityType securityType = Security.SecurityType.valueOf(securityTypeString); 
                String orderType = resultSet.getString("symbol");

                order = new Order(orderId, 0, securityType, 0, orderType);
            }
        } 
        catch (SQLException e)
        {
            System.out.println("Error retrieving order from DB: " + e.getMessage());
        }
        return order;
    }
    
    
    // Returns list of 'Order' objects by symbol
    public List<Order> getOrdersBySymbol(String symbol) 
    {
        List<Order> orders = new ArrayList<>();
        try (Connection connection = DriverManager.getConnection("jdbc:mysql://" + SERVER + ":" + PORT + "/" + DB_NAME, USERNAME, PASSWORD)) 
        {
            String query = "SELECT orderid, securitytype FROM " + TABLE_NAME + " WHERE symbol = ?";
            PreparedStatement statement = connection.prepareStatement(query);
            statement.setString(1, symbol);
            ResultSet resultSet = statement.executeQuery();

            while (resultSet.next())
            {
                int orderId = resultSet.getInt("orderid");
                String securityTypeString = resultSet.getString("securitytype");
                Security.SecurityType securityType = Security.SecurityType.valueOf(securityTypeString);
                String orderType = symbol; 

                Order order = new Order(orderId, 0, securityType, 0, orderType);
                orders.add(order);
            }
        } 
        catch (SQLException e)
        {
            System.out.println("Error retrieving orders by symbol: " + e.getMessage());
        }
        return orders;
    }
}