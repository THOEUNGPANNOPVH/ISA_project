using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

namespace pms
{
    public partial class frmOrderDetail : Form
    {
        A6Y3 d = new A6Y3();
        SqlCommand com;
        SqlDataAdapter dap;
        DataTable dt;
        IDataReader dr;
        decimal Total = 0;
        public frmOrderDetail()
        {
            InitializeComponent();
        }

        private void frmOrderDetail_Load(object sender, EventArgs e)
        {
            d.Connection();
            dap = new SqlDataAdapter("SELECT *FROM fnGetAllStaff()", d.con);
            dt = new DataTable();
            dap.Fill(dt);
            cboStaffID.DataSource = dt;
            cboStaffID.DisplayMember = "staffID";
            cboStaffID.ValueMember = "FullName";
            cboStaffID.Text = null;

            dap = new SqlDataAdapter("SELECT *FROM  fnGetAllCus()", d.con);
            dt = new DataTable();
            dap.Fill(dt);
            cboCusID.DataSource = dt;
            cboCusID.DisplayMember = "cusID";
            cboCusID.ValueMember = "CusName";
            cboCusID.Text = null;

            lsv.Clear();
            lsv.View = View.Details;
            lsv.Columns.Add("Product Code", 150);
            lsv.Columns.Add("Product Name", 200);
            lsv.Columns.Add("Quantity", 150);
            lsv.Columns.Add("Price", 120);
            lsv.Columns.Add("Amount", 150);
        }

        private void cboStaffID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtStaffName.Text = cboStaffID.SelectedValue.ToString();
        }

        private void cboCusID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtCusName.Text = cboCusID.SelectedValue.ToString();
        }

        private void txtProCode_Leave(object sender, EventArgs e)
        {
            SqlCommand com = new SqlCommand("spGetProductByID", d.con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@pc", txtProCode.Text);

            var dr = com.ExecuteReader();
            if (dr.Read())
            {
                txtProName.Text = dr[0].ToString();
                txtPrice.Text = string.Format("{0:c}", decimal.Parse(dr[1].ToString()));
            }
            dr.Dispose();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Decimal amount, s;
            ListViewItem lv = null;
            foreach (ListViewItem item in lsv.Items)
            {
                if (item.Text.Equals(txtProCode.Text, StringComparison.Ordinal))
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
                Total = amount + Total;
            }
            else
            {
                ListViewItem item;
                string[] arr = new string[5];
                arr[0] = txtProCode.Text;
                arr[1] = txtProName.Text;
                arr[2] = txtQty.Text;
                s = decimal.Parse(txtPrice.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat);
                arr[3] = string.Format("{0:c}", s);
                amount = decimal.Parse(txtQty.Text) * decimal.Parse(txtPrice.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat);
                arr[4] = string.Format("{0:c}", amount);
                item = new ListViewItem(arr);
                lsv.Items.Add(item);
                Total = Total + amount;
            }
            txtTotal.Text = string.Format("{0:c}", Total);
            txtProCode.Text = null;
            txtProName.Text = null;
            txtQty.Text = null;
            txtPrice.Text = null;
            txtQty.Text = null;

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
            dtMaster.Columns.Add("OrdDate", typeof(DateTime));
            dtMaster.Columns.Add("staffID", typeof(int));
            dtMaster.Columns.Add("FullName", typeof(string));
            dtMaster.Columns.Add("cusID", typeof(int));
            dtMaster.Columns.Add("cusName", typeof(string));
            dtMaster.Columns.Add("Total", typeof(float));


            string impDateStr = dtp.Value.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime impDate = DateTime.ParseExact(impDateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            dtMaster.Rows.Add(impDate,
                              Convert.ToInt32(cboStaffID.Text),
                              txtStaffName.Text,
                              Convert.ToInt32(cboCusID.Text),
                              txtCusName.Text, Total);

            DataTable dtDetail = new DataTable();
            dtDetail.Columns.Add("ProCode", typeof(string));
            dtDetail.Columns.Add("ProName", typeof(string));
            dtDetail.Columns.Add("Qty", typeof(int));
            dtDetail.Columns.Add("Price", typeof(float));
            dtDetail.Columns.Add("Amount", typeof(float));

            // Fill dtDetail from ListView
            foreach (ListViewItem item in lsv.Items)
            {
                string pc = item.Text;
                string pn = item.SubItems[1].Text;
                int q = int.Parse(item.SubItems[2].Text);
                float p = float.Parse(item.SubItems[3].Text, NumberStyles.Currency);
                float a = float.Parse(item.SubItems[4].Text, NumberStyles.Currency);

                dtDetail.Rows.Add(pc, pn, q, p, a);
            }

            SqlCommand com = new SqlCommand("spSetOrder", d.con);
            com.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = new SqlParameter();
            par1.ParameterName = "@OM";
            par1.SqlDbType = SqlDbType.Structured;
            par1.Value = dtMaster;
            com.Parameters.Add(par1);
            SqlParameter par2 = new SqlParameter();
            com.ExecuteNonQuery();
            MessageBox.Show("Saved.......!");
        }

        //private void Report_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        using (SqlCommand com = new SqlCommand("spGetNST", d.con))
        //        {
        //            com.CommandType = CommandType.StoredProcedure;

        //            // Output parameters
        //            com.Parameters.Add("@N", SqlDbType.Int).Direction = ParameterDirection.Output;
        //            com.Parameters.Add("@S", SqlDbType.Int).Direction = ParameterDirection.Output;
        //            com.Parameters.Add("@T", SqlDbType.Money).Direction = ParameterDirection.Output;

        //            // Make sure connection is open
        //            if (d.con.State != ConnectionState.Open)
        //                d.con.Open();

        //            com.ExecuteNonQuery();

        //            // Read output values
        //            int productCount = (int)com.Parameters["@N"].Value;
        //            int totalQty = (int)com.Parameters["@S"].Value;
        //            decimal totalAmount = (decimal)com.Parameters["@T"].Value;

        //            MessageBox.Show($"Product Count: {productCount}\nTotal Quantity: {totalQty}\nTotal Amount: {totalAmount:C}",
        //                            "Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        MessageBox.Show("Error: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void textBox1_TextChanged(object sender, EventArgs e)
        //{

        //}
    }
}
