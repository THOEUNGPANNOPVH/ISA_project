using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pms
{
    public partial class Main : Form
    {
        frmStaffs staffs = new frmStaffs();
        frmSuppliers suppliers = new frmSuppliers();
        frmProducts products = new frmProducts();
        frmOrderDetail detail = new frmOrderDetail();
        frmImportDetail Import = new frmImportDetail();
    frmPayment pay = new frmPayment();
        frmCustomer customer = new frmCustomer();
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            staffs.ShowDialog();
            this.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            suppliers.ShowDialog(); 
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            products.ShowDialog();
            this.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            detail.ShowDialog();
            this.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Import.ShowDialog();
            this.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            pay.ShowDialog();
            this.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            customer.ShowDialog();
            this.Show();
        }
    }
}
