package Mod5;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

public class MySQLDBManager {
    private static final String SERVER = "192.168.1.33";
    private static final String PORT = "3306";
    private static final String DB_NAME = "mydb";
    private static final String TABLE_NAME = "ordermanagement";
    private static final String USERNAME = "user";
    private static final String PASSWORD = "Schmaltz1";

    public boolean testDBConnection() {
        try {
            Connection connection = DriverManager.getConnection("jdbc:mysql://" + SERVER + ":" + PORT + "/" + DB_NAME, USERNAME, PASSWORD);
            System.out.println("DB Connection successful");
            connection.close();
            return true;
        } catch (SQLException e) {
            System.out.println("DB Connection failed: " + e.getMessage());
            return false;
        }
    }
}