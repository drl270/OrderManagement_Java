package Mod5;

import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.HashMap;

public class GUI 
{
    private JTextField txtSymbol;
    private JComboBox<Security.SecurityType> securityTypeComboBox;
    private JTable ordersTable;
    private DefaultTableModel ordersTableModel;

    public GUI() 
    {
        JFrame frame = new JFrame("Order Entry");
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        JPanel panel = new JPanel();
        panel.setLayout(new GridBagLayout());

        JLabel symbolLabel = new JLabel("Symbol:");
        txtSymbol = new JTextField(10);

        GridBagConstraints gbc = new GridBagConstraints();
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
        gbc.gridx = 1;
        gbc.gridy = 2;
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
        gbc.gridx = 1;
        gbc.gridy = 3;
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

        gbc.anchor = GridBagConstraints.CENTER;
        gbc.gridx = 1;
        gbc.gridy = 4;
        panel.add(displayOrdersButton, gbc);

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
        gbc.gridwidth = 2;
        gbc.fill = GridBagConstraints.BOTH;
        gbc.weightx = 1;
        gbc.weighty = 1;
        JScrollPane tableScrollPane = new JScrollPane(ordersTable);
        panel.add(tableScrollPane, gbc);

        frame.getContentPane().add(panel, BorderLayout.CENTER);

        frame.pack();
        frame.setVisible(true);
    }

    private void displayOrders(HashMap<Integer, Order> orders) 
    {
        ordersTableModel.setRowCount(0);
        for (Order order : orders.values())
        {
            Object[] row = new Object[] {
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