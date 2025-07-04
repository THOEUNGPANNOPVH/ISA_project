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
    public partial class MainForm : Form
    {
        frmPayment pay = new frmPayment();
        frmStaffs staffs = new frmStaffs();
        frmOrderDetail Order = new frmOrderDetail();
        frmImportDetail Import = new frmImportDetail();
        frmProducts Products= new frmProducts();
        frmSuppliers Sup = new frmSuppliers();
       frmCustomer Cus = new frmCustomer();
        public MainForm()
        {
            InitializeComponent();
        }

        private void Payment_Click(object sender, EventArgs e)
        {
            this.Hide();
            pay.ShowDialog();
            this.Show();
        }

        private void Staffs_Click(object sender, EventArgs e)
        {
            this.Hide();
            staffs.ShowDialog();
            this.Show();
        }

        private void OrderDetail_Click(object sender, EventArgs e)
        {
            this.Hide();
            Order.ShowDialog();
            this.Show();
        }

        private void ImportDetail_Click(object sender, EventArgs e)
        {
            this.Hide();
            Import.ShowDialog();
            this.Show();
        }

        private void Product_Click(object sender, EventArgs e)
        {
            this.Hide();
            Products.ShowDialog();
            this.Show();
        }

        

        private void Supplire_Click(object sender, EventArgs e)
        {
            this.Hide();
            Sup.ShowDialog();
            this.Show();
        }

        private void Customer_Click(object sender, EventArgs e)
        {
            this.Hide();
            Cus.ShowDialog();
            this.Show();
        }
    }
}
