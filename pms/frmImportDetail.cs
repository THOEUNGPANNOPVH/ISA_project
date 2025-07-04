using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pms
{
    public partial class frmImportDetail : Form
    {

        A6Y3 d = new A6Y3();
        SqlCommand com;
        SqlDataAdapter dap;
        DataTable dt;
        IDataReader dr;
        decimal Total = 0;
        public frmImportDetail()
        {
            InitializeComponent();
        }

        private void frmImportDetail_Load(object sender, EventArgs e)
        {
            d.Connection();
            dap = new SqlDataAdapter("SELECT *FROM fnGetAllStaff()", d.con);
            dt = new DataTable();
            dap.Fill(dt);
            cboID.DataSource = dt;
            cboID.DisplayMember = "staffID";
            cboID.ValueMember = "FullName";
            cboID.Text = null;

            dap = new SqlDataAdapter("SELECT *FROM  fnGetAllSupplier()", d.con);
            dt = new DataTable();
            dap.Fill(dt);
            cboSup.DataSource = dt;
            cboSup.DisplayMember = "supplier";
            cboSup.ValueMember = "supID";
            cboSup.Text = null;

            lsv.Clear();
            lsv.View = View.Details;
            lsv.Columns.Add("Product Code", 150);
            lsv.Columns.Add("Product Name", 200);
            lsv.Columns.Add("Quantity", 150);
            lsv.Columns.Add("Price", 120);
            lsv.Columns.Add("Amount", 150);
        }

        private void cboID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            textName.Text = cboID.SelectedValue.ToString();
        }

        private void cboSup_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtSupID.Text = cboSup.SelectedValue.ToString();
        }

        private void txtPCode_Leave(object sender, EventArgs e)
        {
            SqlCommand com = new SqlCommand("spGetProductByID", d.con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@pc", txtPCode.Text);

            SqlDataReader dr = com.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    txtPName.Text = dr[0].ToString();
                }
            }
            else
            {
                txtPName.Text = null;
            }
            dr.Dispose();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            decimal amount, s;
            ListViewItem lv = null;
            foreach (ListViewItem item in lsv.Items)
            {
                if (item.Text.Equals(txtPCode.Text, StringComparison.Ordinal))
                {
                    lv = item;
                    break;
                }
            }
            if (lv != null)
            {
                var qty = int.Parse(lv.SubItems[2].Text) + int.Parse(txtQty.Text);
                lv.SubItems[2].Text = qty.ToString();

                Total = Total - decimal.Parse(lv.SubItems[4].Text, NumberStyles.Currency);
                var price = decimal.Parse(txtPrice.Text, NumberStyles.Currency);
                amount = qty * price;

                lv.SubItems[4].Text = string.Format("{0:c}", amount);
                Total = Total + amount;
            }
            else
            {
                ListViewItem item;
                string[] arr = new string[5];

                // Fill array with item data
                arr[0] = txtPCode.Text;
                arr[1] = txtPName.Text;
                arr[2] = txtQty.Text;

                // Parse price and quantity to calculate total
                s = decimal.Parse(txtPrice.Text);
                arr[3] = string.Format("{0:c}", s);

                amount = decimal.Parse(txtQty.Text) * s;
                arr[4] = string.Format("{0:c}", amount);
                // Create ListViewItem and add to ListView
                item = new ListViewItem(arr);
                lsv.Items.Add(item);
                // Update running total
                Total = Total + amount;
            }
            // Update total display and clear input fields
            txtTotal.Text = string.Format("{0:c}", Total);
            txtPCode.Text = null;
            txtPName.Text = null;
            txtQty.Text = null;
            txtPrice.Text = null;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult re;
            foreach (ListViewItem item in lsv.Items)
            {
                if (item.Selected)
                {
                    re = MessageBox.Show("Do you want to remove this item?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (re == DialogResult.Yes)
                    {
                        var a = Decimal.Parse(item.SubItems[4].Text, NumberStyles.Currency);
                        Total -= a;
                        lsv.Items.Remove(item);
                        txtTotal.Text = string.Format("{0:c}", Total);
                        break;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtMaster = new DataTable();

            dtMaster.Columns.Add("ImpDate", typeof(string));
            dtMaster.Columns.Add("staffID", typeof(int));
            dtMaster.Columns.Add("FullName", typeof(string));
            dtMaster.Columns.Add("supID", typeof(int));
            dtMaster.Columns.Add("Supplier", typeof(string));
            dtMaster.Columns.Add("Total", typeof(float));

            string impDate = dtp.Value.ToString("yyyy-MM-dd HH:mm:ss");
            dtMaster.Rows.Add(DateTime.Parse(impDate), cboID.Text,
            textName.Text, txtSupID.Text, cboSup.Text, Total);


            DataTable dtDetail = new DataTable();
            dtDetail.Columns.Add(" ProCode ", typeof(string));
            dtDetail.Columns.Add(" ProName", typeof(string));
            dtDetail.Columns.Add("Qty", typeof(int));
            dtDetail.Columns.Add("Price", typeof(float));
            dtDetail.Columns.Add("Amount", typeof(float));
            foreach (ListViewItem Item in lsv.Items)
            {
                string pc = Item.Text;
                string pn = Item.SubItems[1].Text;
                int q = int.Parse(Item.SubItems[2].Text);
                float p = float.Parse(Item.SubItems[3].Text, NumberStyles.Currency);
                float a = float.Parse(Item.SubItems[4].Text, NumberStyles.Currency);
                dtDetail.Rows.Add(pc, pn, q, p, a);
            }
            // for insert to server 
            SqlCommand com = new SqlCommand("spSetImport", d.con);
            com.CommandType = CommandType.StoredProcedure;
            SqlParameter par1 = new SqlParameter();
            par1.ParameterName = "@IM";
            par1.SqlDbType = SqlDbType.Structured;
            par1.Value = dtMaster;
            com.Parameters.Add(par1);

            SqlParameter par2 = new SqlParameter();
            par2.ParameterName = "@ID";
            par2.SqlDbType = SqlDbType.Structured;
            par2.Value = dtDetail;
            com.Parameters.Add(par2);
            com.ExecuteNonQuery();
        }
    }
}
