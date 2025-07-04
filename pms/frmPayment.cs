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
using static System.Net.WebRequestMethods;

namespace pms
{
    public partial class frmPayment : Form
    {
        A6Y3 d = new A6Y3();
        SqlCommand com;
        SqlDataAdapter dap;
        DataTable dt;
        IDataReader dr;
        decimal Total = 0;
        decimal r, t, ds;
        public frmPayment()
        {
            InitializeComponent();
        }

        private void frmPayment_Load(object sender, EventArgs e)
        {
            d.Connection();
            dap = new SqlDataAdapter("SELECT *FROM fnGetAllStaff()", d.con);
            dt = new DataTable();
            dap.Fill(dt);
            cboStaffID.DataSource = dt;
            cboStaffID.DisplayMember = "staffID";
            cboStaffID.ValueMember = "FullName";
            cboStaffID.Text = null;

            dap = new SqlDataAdapter("SELECT * FROM fnGetOrderCode()", d.con);
            dt = new DataTable();
            dap.Fill(dt);
            cboOrderCode.DataSource = null;
            cboOrderCode.Items.Clear();
            cboOrderCode.DataSource = dt;
            cboOrderCode.DisplayMember = "OrdCode";
            cboOrderCode.ValueMember = "OrdCode";
            cboOrderCode.Text = null;
        }

        private void txtDeposit_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtTotal.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out decimal t) &&
     decimal.TryParse(txtDeposit.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out decimal ds))
            {
                decimal r = t - ds;
                // Ensure remain is not negative (optional, adjust based on your needs)
                r = r < 0 ? 0 : r;
                txtRemain.Text = r.ToString("C", CultureInfo.CurrentCulture);
            }
            else
            {
                txtRemain.Text = "Invalid input";
            }
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
             com = new SqlCommand("spPayment", d.con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@PD", dtp.Value.ToString());
            com.Parameters.AddWithValue("@SI", cboStaffID.Text);
            com.Parameters.AddWithValue("@FN", txtStaffName.Text);
            com.Parameters.AddWithValue("@IC", int.Parse(cboOrderCode.SelectedValue.ToString()));

            if (txtDeposit.Enabled == true && txtDeposit.ReadOnly == false)
            {
                com.Parameters.AddWithValue("@Dep", decimal.Parse(txtDeposit.Text, NumberStyles.Currency, CultureInfo.CurrentCulture));
            }
            else
            {
                com.Parameters.AddWithValue("@Dep", float.Parse(txtRemain.Text, NumberStyles.Currency, CultureInfo.CurrentCulture));
            }

            com.Parameters.AddWithValue("@A", decimal.Parse(txtTotal.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat));

            com.ExecuteNonQuery();
            MessageBox.Show("Pay..");
        }

        private void cboStaffID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtStaffName.Text = cboStaffID.SelectedValue.ToString();
        }

        private void cboOrderCode_SelectionChangeCommitted(object sender, EventArgs e)
        {
            com = new SqlCommand("spGetPayment", d.con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@oc", cboOrderCode.SelectedValue.ToString());

            var dr = com.ExecuteReader();

            try
            {
                if (dr.Read())
                {
                    txtTotal.Text = string.Format("{0:c}", Decimal.Parse(dr[0].ToString()));
                    txtDeposit.Text = dr[1]?.ToString() ?? string.Empty;

                    if (string.IsNullOrEmpty(txtDeposit.Text))
                    {
                        txtRemain.Text = txtTotal.Text;
                    }
                    else
                    {
                        t = decimal.Parse(txtTotal.Text, NumberStyles.Currency, CultureInfo.CurrentCulture);
                        ds = decimal.Parse(txtDeposit.Text, NumberStyles.Currency, CultureInfo.CurrentCulture);
                        r = t - ds;
                        txtRemain.Text = string.Format("{0:c}", r); // Simplified parsing
                        txtDeposit.ReadOnly = true;
                    }
                }
            }
            finally
            {
                dr.Dispose(); // Corrected spelling and placed in finally block
            }
        }
    }
}
