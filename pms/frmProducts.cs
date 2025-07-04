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
    public partial class frmProducts : Form
    {
        string store_ID = "";
        private BindingSource bindingSource = new BindingSource();
        private DateTime lastUpdate = DateTime.MinValue;
        private readonly TimeSpan debounceInterval = TimeSpan.FromSeconds(1);
        A6Y3 d = new A6Y3();
        SqlCommand com;
        int row_selected = -1; // used to store the selected row index

        public frmProducts()
        {
            d.Connection();
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            try
            {
                dgvProduct.DataSource = null;
                using (var com = new SqlCommand("spGetAllProducts", d.con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    var dep = new SqlDependency(com);
                    dep.OnChange += OnChange;

                    using (var dap = new SqlDataAdapter(com))
                    {
                        DataTable dt = new DataTable();
                        dap.Fill(dt);
                        bindingSource.DataSource = dt;
                        dgvProduct.DataSource = bindingSource;
                        dgvProduct.DefaultCellStyle.Font = new Font("Khmer OS System", 12);
                        dgvProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (DateTime.Now - lastUpdate < debounceInterval)
                return;

            lastUpdate = DateTime.Now;
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(LoadData));
            else
                LoadData();
        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i < 0) return;

            DataGridViewRow row = dgvProduct.Rows[i];
            if (row == null || row.Cells.Cast<DataGridViewCell>().All(c => c.Value == null || string.IsNullOrWhiteSpace(c.Value.ToString())))
            {
                MessageBox.Show("This row is empty.");
                return;
            }

            store_ID = row.Cells[0].Value?.ToString() ?? "";
            txtProName.Text = row.Cells[1].Value?.ToString() ?? "";
            txtQtypro.Text = row.Cells[2].Value?.ToString() ?? "";
            txtUPrice.Text = row.Cells[3].Value?.ToString() ?? "";
            txtSPrice.Text = row.Cells[4].Value?.ToString() ?? "";
        }
        private void ClearInputs()
        {
            store_ID = "";
            txtProID.Clear();
            txtProName.Clear();
            txtQtypro.Clear();
            txtUPrice.Clear();
            txtSPrice.Clear();
            dgvProduct.ClearSelection();
            dgvProduct.CurrentCell = null;
            txtProID.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                com = new SqlCommand("spInsertProduct", d.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ProCode", txtProID.Text);
                com.Parameters.AddWithValue("@ProName", txtProName.Text);
                com.Parameters.AddWithValue("@Qty", int.Parse(txtQtypro.Text));
                com.Parameters.AddWithValue("@UPIS", decimal.Parse(txtUPrice.Text));
                com.Parameters.AddWithValue("@SUP", decimal.Parse(txtSPrice.Text));
                com.ExecuteNonQuery();
                //MessageBox.Show("Product added successfully.");
                ClearInputs();
                LoadData();
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void clean_Click(object sender, EventArgs e)
        {
            txtProID.Clear();
            txtProName.Clear();
            txtQtypro.Clear();
            txtUPrice.Clear();
            txtSPrice.Clear();
            dgvProduct.ClearSelection();
            dgvProduct.CurrentCell = null;
            txtProID.Enabled = true;
            txtProID.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(store_ID))
            {
                MessageBox.Show("Please select a product to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (d.con.State != ConnectionState.Open) d.con.Open();
                using (var com = new SqlCommand("spUpdateProduct", d.con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ProCode", store_ID);
                    com.Parameters.AddWithValue("@ProName", txtProName.Text);
                    com.Parameters.AddWithValue("@Qty", int.Parse(txtQtypro.Text));
                    com.Parameters.AddWithValue("@UPIS", decimal.Parse(txtUPrice.Text, NumberStyles.Currency));
                    com.Parameters.AddWithValue("@SUP", decimal.Parse(txtSPrice.Text, NumberStyles.Currency));
                    com.ExecuteNonQuery();
                    //MessageBox.Show("Product updated successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                d.con.Close();
                ClearInputs();
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(store_ID))
            {
                MessageBox.Show("Please select a product to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                com = new SqlCommand("spDeleteProduct", d.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ProCode", store_ID);
                com.ExecuteNonQuery();
                LoadData();
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                bindingSource.Filter = $"ProName LIKE '%{txtSearch.Text.Trim()}%' OR Convert(ProCode, 'System.String') LIKE '%{txtSearch.Text.Trim()}%'";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmProducts_Load(object sender, EventArgs e)
        {

        }
    }
}
    

