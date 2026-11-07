using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace likicoffee
{
    public partial class Form1 : Form
    {
        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        // CALCULATE ORDER
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            string name = txtCustomerName.Text.Trim();

            if (name == "")
            {
                MessageBox.Show("Please enter customer name.");
                return;
            }

            int quantity;

            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity < 1)
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }

            double price;

            if (rbTea.Checked)
            {
                price = 1;
            }
            else if (rbCoffee.Checked)
            {
                price = 2;
            }
            else if (rbJuice.Checked)
            {
                price = 3;
            }
            else
            {
                MessageBox.Show("Please select a drink.");
                return;
            }

            string drink = "";

            if (rbTea.Checked)
                drink = "Tea";
            else if (rbCoffee.Checked)
                drink = "Coffee";
            else if (rbJuice.Checked)
                drink = "Juice";

            lstOrder.Items.Clear();

            bool hasPrize = false;

            for (int i = 1; i <= quantity; i++)
            {
                int luckyNumber = random.Next(1, 51);

                if (luckyNumber == 7 || luckyNumber == 17)
                {
                    hasPrize = true;
                }

                lstOrder.Items.Add(
                    i + ") " + name + " - " + drink +
                    " - Lucky: " + luckyNumber +
                    " - $" + price.ToString("0.00")
                );
            }

            double total = price * quantity;

            if (chkStudent.Checked)
            {
                total = total - (total * 0.10);
            }

            lblTotalPrice.Text = "Total Price: $" + total.ToString("0.00");

            if (hasPrize)
            {
                lblLuckyMessage.Text = "You got a Free Cookie!";
            }
            else
            {
                lblLuckyMessage.Text = "No prize this time.";
            }
        }

        // SAVE TO FILE
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lstOrder.Items.Count == 0)
            {
                MessageBox.Show("No order to save.");
                return;
            }

            using (StreamWriter writer = File.AppendText("orders.txt"))
            {
                foreach (var item in lstOrder.Items)
                {
                    writer.WriteLine(item.ToString());
                }

                writer.WriteLine(lblTotalPrice.Text);
                writer.WriteLine(lblLuckyMessage.Text);
                writer.WriteLine("--------------------");
            }

            MessageBox.Show("Order saved.");
        }

        // LOAD FROM FILE
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!File.Exists("orders.txt"))
            {
                MessageBox.Show("No file found.");
                return;
            }

            lstOrder.Items.Clear();

            using (StreamReader reader = File.OpenText("orders.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    lstOrder.Items.Add(line);
                }
            }
        }

        // CLEAR FORM
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCustomerName.Clear();
            txtQuantity.Clear();

            lstOrder.Items.Clear();

            rbTea.Checked = false;
            rbCoffee.Checked = false;
            rbJuice.Checked = false;

            chkStudent.Checked = false;

            lblTotalPrice.Text = "Total Price: $0.00";
            lblLuckyMessage.Text = "";

            txtCustomerName.Focus();
        }
        

        

        
    }
}