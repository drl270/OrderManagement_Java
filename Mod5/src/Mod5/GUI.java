package Mod5;

import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.HashMap;
import java.util.List;

public class GUI 
{
    private JTextField txtSymbol;
    private JComboBox<Security.SecurityType> securityTypeComboBox;
    private JTable ordersTable;
    private DefaultTableModel ordersTableModel;
    private JTextField orderIdTextField;
    private JTextField firstNameTextField;
    private JTextField lastNameTextField;
    private JTextField phoneNumberTextField;
    private JTextField emailTextField;
    private JTextField symbolTextField;

    public GUI() 
    {
        JFrame frame = new JFrame("Order Entry");       
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setSize(2400, 800);
        JPanel panel = new JPanel();
        panel.setLayout(new GridBagLayout());
        
        GridBagConstraints gbc = new GridBagConstraints(); 
        gbc.insets = new Insets(0, 5, 0, 5); 
        gbc.weightx = 0.0; 
        gbc.fill = GridBagConstraints.NONE; 
        gbc.anchor = GridBagConstraints.CENTER;

        JLabel symbolLabel = new JLabel("Symbol:");
        txtSymbol = new JTextField(10);

        gbc.anchor = GridBagConstraints.LINE_END;
        gbc.gridx = 0;
        gbc.gridy = 0;
        panel.add(symbolLabel, gbc);

        gbc.anchor = GridBagConstraints.LINE_START;
        gbc.gridx = 1;
        gbc.gridy = 0;
        panel.add(txtSymbol, gbc);

        JLabel securityTypeLabel = new JLabel("Security Type:");
        securityTypeComboBox = new JComboBox<>(Security.SecurityType.values());

        gbc.anchor = GridBagConstraints.LINE_END;
        gbc.gridx = 0;
        gbc.gridy = 1;
        panel.add(securityTypeLabel, gbc);

        gbc.anchor = GridBagConstraints.LINE_START;
        gbc.gridx = 1;
        gbc.gridy = 1;
        panel.add(securityTypeComboBox, gbc);

        JButton enterOrderButton = new JButton("Enter Order");
        enterOrderButton.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent e) 
            {
                String symbol = txtSymbol.getText();
                Security.SecurityType securityType = (Security.SecurityType) securityTypeComboBox.getSelectedItem();

                OrderManagementSystem orderManagementSystem = OrderManagementSystem.getInstance();
                orderManagementSystem.enterOrder(symbol, securityType);
            }
        });

        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 2; 
        gbc.gridy = 1; 
        panel.add(enterOrderButton, gbc);

        JButton testDBConnectionButton = new JButton("Test DB Connection");
        testDBConnectionButton.addActionListener(new ActionListener() 
        {
            public void actionPerformed(ActionEvent e)
            {
                MySQLDBManager dbManager = new MySQLDBManager();
                dbManager.testDBConnection();
            }
        });

        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 3; 
        gbc.gridy = 1; 
        panel.add(testDBConnectionButton, gbc);

        JButton displayOrdersButton = new JButton("Display Orders in Memory");
        displayOrdersButton.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent e) 
            {
                OrderManagementSystem orderManagementSystem = OrderManagementSystem.getInstance();
                displayOrders(orderManagementSystem.getOrders());
            }
        });

        JButton writeOrdersToDBButton = new JButton("Write to DB");
        writeOrdersToDBButton.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent e) 
            {
                OrderManagementSystem orderManagementSystem = OrderManagementSystem.getInstance();
                MySQLDBManager dbManager = new MySQLDBManager();
                dbManager.writeOrdersToDB(orderManagementSystem.getOrders());
            }
        });

        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 4; 
        gbc.gridy = 1; 
        panel.add(writeOrdersToDBButton, gbc);
        
        
     // Get all 'Order' objects from DB 
        JButton readOrdersFromDBButton = new JButton("Read From DB");
        readOrdersFromDBButton.addActionListener(new ActionListener() 
        {
            public void actionPerformed(ActionEvent e)
            {
                ordersTableModel.setRowCount(0);
                MySQLDBManager dbManager = new MySQLDBManager();
                dbManager.readOrdersFromDB(ordersTableModel);
            }
        });              

        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 5; 
        gbc.gridy = 1; 
        panel.add(readOrdersFromDBButton, gbc);

        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 6; 
        gbc.gridy = 1; 
        panel.add(displayOrdersButton, gbc);

        JLabel orderIdLabel = new JLabel("Order ID:");
        orderIdTextField = new JTextField(10);
        JButton getOrderButton = new JButton("Get Order from DB");

        gbc.anchor = GridBagConstraints.LINE_END;
        gbc.gridx = 7; 
        gbc.gridy = 1; 
        panel.add(orderIdLabel, gbc);
        
        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 8; 
        gbc.gridy = 1; 
        panel.add(getOrderButton, gbc);

        gbc.anchor = GridBagConstraints.LINE_START;
        gbc.gridx = 9; 
        gbc.gridy = 1; 
        panel.add(orderIdTextField, gbc);  
        
        JLabel symbolDBLabel = new JLabel("Symbol:");
        symbolTextField = new JTextField(10);
        JButton readSymbolFromDBButton = new JButton("Get Symbol from DB");             
        
        gbc.anchor = GridBagConstraints.LINE_END;
        gbc.gridx = 10; 
        gbc.gridy = 1; 
        panel.add(symbolDBLabel, gbc);
        
        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 11; 
        gbc.gridy = 1; 
        panel.add(readSymbolFromDBButton, gbc);

        gbc.anchor = GridBagConstraints.LINE_START;
        gbc.gridx = 12; 
        gbc.gridy = 1; 
        panel.add(symbolTextField, gbc);  

        
       // Get 'Order' object from DB by order ID
        getOrderButton.addActionListener(new ActionListener()
        {
            public void actionPerformed(ActionEvent e)
            {
                String orderIdStr = orderIdTextField.getText();
                try {
                    int orderId = Integer.parseInt(orderIdStr);
                    MySQLDBManager dbManager = new MySQLDBManager();
                    Order order = dbManager.getOrderById(orderId);

                    if (order != null) 
                    {
                        ordersTableModel.setRowCount(0);
                        Object[] row = new Object[]{
                                order.getOrderId(),
                                order.getOrderType(),
                                order.getSecurity().toString(),
                                order.getQuantity(),
                                order.getOrderType()
                        };
                        ordersTableModel.addRow(row);
                    } 
                    else
                    {
                        JOptionPane.showMessageDialog(frame, "Order not found.", "Error", JOptionPane.ERROR_MESSAGE);
                    }
                } 
                catch (NumberFormatException ex)
                {
                    JOptionPane.showMessageDialog(frame, "Invalid Order ID. Please enter an integer.", "Error", JOptionPane.ERROR_MESSAGE);
                }
            }
        });
        
        
        // Get 'Order' objects from DB by order Symbol
        readSymbolFromDBButton.addActionListener(new ActionListener() 
        {
        	 public void actionPerformed(ActionEvent e)
             {
                 String symbolStr = symbolTextField.getText();
                 try {
                     MySQLDBManager dbManager = new MySQLDBManager();
                     List<Order> symbolList = dbManager.getOrdersBySymbol(symbolStr);
                     
                     ordersTableModel.setRowCount(0);
                     for (Order order : symbolList)
                     {
                    	 Object[] row = new Object[]{
                                 order.getOrderId(),
                                 order.getOrderType(),
                                 order.getSecurity().toString(),
                                 order.getQuantity(),
                                 order.getOrderType()
                         };
                         ordersTableModel.addRow(row);
                     }

                 } 
                 catch (NumberFormatException ex)
                 {
                     JOptionPane.showMessageDialog(frame, "Invalid Symbol.", "Error", JOptionPane.ERROR_MESSAGE);
                 }
             }
        });
        
        frame.getContentPane().add(panel, BorderLayout.CENTER);
        frame.setVisible(true);

        ordersTableModel = new DefaultTableModel();
        ordersTableModel.addColumn("Order ID");
        ordersTableModel.addColumn("Symbol");
        ordersTableModel.addColumn("Security Type");
        ordersTableModel.addColumn("Quantity");
        ordersTableModel.addColumn("Order Type");
        ordersTable = new JTable(ordersTableModel);

        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 0;
        gbc.gridy = 5;
        gbc.gridwidth = 10;
        gbc.fill = GridBagConstraints.BOTH;
        gbc.weightx = 1;
        gbc.weighty = 1;
        JScrollPane tableScrollPane = new JScrollPane(ordersTable);
        panel.add(tableScrollPane, gbc);

        frame.getContentPane().add(panel, BorderLayout.CENTER);
        frame.setVisible(true);
    }

    private void displayOrders(HashMap<Integer, Order> orders)
    {
        ordersTableModel.setRowCount(0);
        for (Order order : orders.values())
        {
            Object[] row = new Object[]
            		{
                    order.getOrderId(),
                    order.getOrderType(),
                    order.getSecurity().toString(),
                    order.getQuantity(),
                    order.getOrderType()
            };
            ordersTableModel.addRow(row);
        }
    }

    public static void main(String[] args) 
    {
    	SwingUtilities.invokeLater(new Runnable() 
    	{
    		public void run()
    		{
    			new GUI();
    		}

    	});

    }

 }