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
using System.Xml.Linq;


namespace pms
{
   
    public partial class frmCustomer : Form
    {
        A6Y3 d = new A6Y3 ();
        SqlCommand com;
        string store_cusID = "";

        public frmCustomer()
        {
            d.Connection();
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            try
            {
                dgvCustomer.DataSource = null;
                com = new SqlCommand("spGetAllCustomers", d.con);
                com.CommandType = CommandType.StoredProcedure;

                SqlDependency dep = new SqlDependency(com);
                dep.OnChange += new OnChangeEventHandler(OnChange);

                SqlDataAdapter dap = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                dap.Fill(dt);

                dgvCustomer.DataSource = dt;
                dgvCustomer.DefaultCellStyle.Font = new Font("Khmer OS System", 12);
                dgvCustomer.Columns["cusID"].Width = 60;
                dgvCustomer.Columns["CusName"].Width = 200;
                dgvCustomer.Columns["CusContact"].Width = 200;
                dgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public void OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(() => LoadData()));
            }
            else
            {
                LoadData();
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                com = new SqlCommand("spInsertCustomer", d.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CusName", txtCusName.Text);
                com.Parameters.AddWithValue("@CusContact", txtCusContact.Text);
                com.ExecuteNonQuery();
                LoadData();
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                txtCusName.Clear();
                txtCusContact.Clear();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(store_cusID))
                {
                    MessageBox.Show("Please select a customer to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                com = new SqlCommand("spDeleteCustomer", d.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@cusID", int.Parse(store_cusID));
                com.ExecuteNonQuery();
                LoadData();
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i < 0) return; // Header clicked

            DataGridViewRow row = dgvCustomer.Rows[i];

            // Check if the entire row is empty/null
            bool isRowEmpty = true;
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.Value != null && cell.Value != DBNull.Value && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                {
                    isRowEmpty = false;
                    break;
                }
            }
            if (isRowEmpty)
            {
                MessageBox.Show("This row is empty.");
                return;
            }

            store_cusID = row.Cells[0].Value?.ToString() ?? "";
            txtCusName.Text = row.Cells[1].Value?.ToString() ?? "";
            txtCusContact.Text = row.Cells[2].Value?.ToString() ?? "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if a customer is selected
                if (string.IsNullOrEmpty(store_cusID))
                {
                    MessageBox.Show("Please select a customer to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate input fields
                if (string.IsNullOrWhiteSpace(txtCusName.Text) || string.IsNullOrWhiteSpace(txtCusContact.Text))
                {
                    MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Optional: Validate contact number format
                // if (!Regex.IsMatch(txtContact.Text, @"^\d{10}$"))
                // {
                //     MessageBox.Show("Please enter a valid 10-digit contact number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //     return;
                // }

                // Prepare update command
                com = new SqlCommand("spUpdateCustomer", d.con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@cusID", int.Parse(store_cusID));
                com.Parameters.AddWithValue("@CusName", txtCusName.Text.Trim());
                com.Parameters.AddWithValue("@CusContact", txtCusContact.Text.Trim());

                com.ExecuteNonQuery();

                //MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh data grid
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Clear inputs
                txtCusName.Text = "";
                txtCusContact.Text = "";
                store_cusID = "";
            }
        }
        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            try
            {
                (dgvCustomer.DataSource as DataTable).DefaultView.RowFilter = string.Format("CusName LIKE '%{0}%' OR CusContact LIKE '%{0}%'", txtSearch.Text.Trim());

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmCustomer_Load(object sender, EventArgs e)
        {
            //--
        }
    }
}
