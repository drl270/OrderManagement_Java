package Mod5;

import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.HashMap;

public class GUI {
    private JTextField txtSymbol;
    private JComboBox<Security.SecurityType> securityTypeComboBox;
    private JTable ordersTable;
    private DefaultTableModel ordersTableModel;
    private JTextField orderIdTextField;

    public GUI() {
        JFrame frame = new JFrame("Order Entry");       
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setSize(1600, 800);
        JPanel panel = new JPanel();
        panel.setLayout(new GridBagLayout());
        
        GridBagConstraints gbc = new GridBagConstraints(); // Create gbc once
        gbc.insets = new Insets(0, 5, 0, 5); // Set insets once
        gbc.weightx = 0.0; // Prevent horizontal stretching
        gbc.fill = GridBagConstraints.NONE; // Prevent component stretching
        gbc.anchor = GridBagConstraints.CENTER; // Center the component

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
        enterOrderButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                String symbol = txtSymbol.getText();
                Security.SecurityType securityType = (Security.SecurityType) securityTypeComboBox.getSelectedItem();

                OrderManagementSystem orderManagementSystem = OrderManagementSystem.getInstance();
                orderManagementSystem.enterOrder(symbol, securityType);
            }
        });

        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 2; // Moved to the same row
        gbc.gridy = 1; // Moved to the same row
        panel.add(enterOrderButton, gbc);

        JButton testDBConnectionButton = new JButton("Test DB Connection");
        testDBConnectionButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                MySQLDBManager dbManager = new MySQLDBManager();
                dbManager.testDBConnection();
            }
        });

        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 3; // Moved to the same row
        gbc.gridy = 1; // Moved to the same row
        panel.add(testDBConnectionButton, gbc);

        JButton displayOrdersButton = new JButton("Display Orders in Memory");
        displayOrdersButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                OrderManagementSystem orderManagementSystem = OrderManagementSystem.getInstance();
                displayOrders(orderManagementSystem.getOrders());
            }
        });

        JButton writeOrdersToDBButton = new JButton("Write to DB");
        writeOrdersToDBButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                OrderManagementSystem orderManagementSystem = OrderManagementSystem.getInstance();
                MySQLDBManager dbManager = new MySQLDBManager();
                dbManager.writeOrdersToDB(orderManagementSystem.getOrders());
            }
        });

        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 4; // Moved to the same row
        gbc.gridy = 1; // Moved to the same row
        panel.add(writeOrdersToDBButton, gbc);

        JButton readOrdersFromDBButton = new JButton("Read From DB");
        readOrdersFromDBButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                ordersTableModel.setRowCount(0);
                MySQLDBManager dbManager = new MySQLDBManager();
                dbManager.readOrdersFromDB(ordersTableModel);
            }
        });

        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 5; // Moved to the same row
        gbc.gridy = 1; // Moved to the same row
        panel.add(readOrdersFromDBButton, gbc);

        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 6; // Moved to the same row
        gbc.gridy = 1; // Moved to the same row
        panel.add(displayOrdersButton, gbc);

        // Added components for Get Order from DB
        JLabel orderIdLabel = new JLabel("Order ID:");
        orderIdTextField = new JTextField(10);
        JButton getOrderButton = new JButton("Get Order from DB");

        gbc.anchor = GridBagConstraints.LINE_END;
        gbc.gridx = 7; // Moved to the same row
        gbc.gridy = 1; // Moved to the same row
        panel.add(orderIdLabel, gbc);

        gbc.anchor = GridBagConstraints.LINE_START;
        gbc.gridx = 8; // Moved to the same row
        gbc.gridy = 1; // Moved to the same row
        panel.add(orderIdTextField, gbc);

        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 9; // Moved to the same row
        gbc.gridy = 1; // Moved to the same row
        panel.add(getOrderButton, gbc);

        // ActionListener for Get Order from DB button
        getOrderButton.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                String orderIdStr = orderIdTextField.getText();
                try {
                    int orderId = Integer.parseInt(orderIdStr);
                    MySQLDBManager dbManager = new MySQLDBManager();
                    Order order = dbManager.getOrderById(orderId);

                    if (order != null) {
                        ordersTableModel.setRowCount(0);
                        Object[] row = new Object[]{
                                order.getOrderId(),
                                order.getOrderType(),
                                order.getSecurity().toString(),
                                order.getQuantity(),
                                order.getOrderType()
                        };
                        ordersTableModel.addRow(row);
                    } else {
                        JOptionPane.showMessageDialog(frame, "Order not found.", "Error", JOptionPane.ERROR_MESSAGE);
                    }
                } catch (NumberFormatException ex) {
                    JOptionPane.showMessageDialog(frame, "Invalid Order ID. Please enter an integer.", "Error", JOptionPane.ERROR_MESSAGE);
                }
            }
        });

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
        gbc.gridwidth = 10; // span all the columns
        gbc.fill = GridBagConstraints.BOTH;
        gbc.weightx = 1;
        gbc.weighty = 1;
        JScrollPane tableScrollPane = new JScrollPane(ordersTable);
        panel.add(tableScrollPane, gbc);

        frame.getContentPane().add(panel, BorderLayout.CENTER);

        //frame.pack();
        frame.setVisible(true);
    }

    private void displayOrders(HashMap<Integer, Order> orders) {
        ordersTableModel.setRowCount(0);
        for (Order order : orders.values()) {
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

    public static void main(String[] args) {

    	SwingUtilities.invokeLater(new Runnable() {

    	public void run() {

    	new GUI();

    	}

    	});

    	}

    	}