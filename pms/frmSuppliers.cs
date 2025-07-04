using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace pms
{
    public partial class frmSuppliers : Form
    {
        A6Y3 d = new A6Y3();
        SqlCommand com;
        string store_ID = "";
        private BindingSource bindingSource = new BindingSource();
        private DateTime lastUpdate = DateTime.MinValue;
        private readonly TimeSpan debounceInterval = TimeSpan.FromSeconds(1);
        public frmSuppliers()
        {
            d.Connection();
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            try
            {
                dgvSupplier.DataSource = null;

                using (var com = new SqlCommand("spGetAllSupplier", d.con))
                {
                    com.CommandType = CommandType.StoredProcedure;

                    // SqlDependency setup
                    var dep = new SqlDependency(com);
                    dep.OnChange += OnChange;

                    using (var dap = new SqlDataAdapter(com))
                    {
                        DataTable dt = new DataTable();
                        dap.Fill(dt);
                        bindingSource.DataSource = dt;
                        dgvSupplier.DataSource = bindingSource;

                        dgvSupplier.DefaultCellStyle.Font = new Font("Khmer OS System", 12);
                        dgvSupplier.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                if (d.con.State != ConnectionState.Open) d.con.Open();

                using (var com = new SqlCommand("spAddSupplier", d.con)) // Use insert SP
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Supplier", txtSupName.Text.Trim());
                    com.Parameters.AddWithValue("@SupAdd", txtSupAddress.Text.Trim());
                    com.Parameters.AddWithValue("@SupCon", txtSupContact.Text.Trim());
                    com.ExecuteNonQuery();
                    //MessageBox.Show("Supplier added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    LoadData();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Insert Error: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                d.con.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(store_ID) || !int.TryParse(store_ID, out int id))
            {
                MessageBox.Show("Please select a valid Supplier to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidateInputs()) return;

            try
            {
                if (d.con.State != ConnectionState.Open) d.con.Open();
                using (var com = new SqlCommand("spUpdateSupplier", d.con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@supID", id);
                    com.Parameters.AddWithValue("@Supplier", txtSupName.Text);
                    com.Parameters.AddWithValue("@SupAdd", txtSupAddress.Text);
                    com.Parameters.AddWithValue("@SupCon", txtSupContact.Text);
                    com.ExecuteNonQuery();
                    //MessageBox.Show("Supplier updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtSupName.Text) || string.IsNullOrWhiteSpace(txtSupAddress.Text) || string.IsNullOrWhiteSpace(txtSupContact.Text))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void ClearInputs()
        {
            store_ID = "";
            txtSupName.Text = "";
            txtSupAddress.Text = "";
            txtSupContact.Text = "";
            dgvSupplier.ClearSelection();
            dgvSupplier.CurrentCell = null;
            txtSupName.Focus();
        }

        private void dgvSupplier_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i < 0) return; // Header clicked

            DataGridViewRow row = dgvSupplier.Rows[i];

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

            store_ID = row.Cells[0].Value?.ToString() ?? "";
            txtSupName.Text = row.Cells[1].Value?.ToString() ?? "";
            txtSupAddress.Text = row.Cells[2].Value?.ToString() ?? "";
            txtSupContact.Text = row.Cells[3].Value?.ToString() ?? "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(store_ID))
            {
                MessageBox.Show("Please select a supplier to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (d.con.State != ConnectionState.Open) d.con.Open();

                using (var com = new SqlCommand("spDeleteSupplier", d.con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@supID", int.Parse(store_ID));
                    com.ExecuteNonQuery();
                    //MessageBox.Show("Supplier deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    LoadData();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Delete Error: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                d.con.Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                bindingSource.Filter = $"Supplier LIKE '%{txtSearch.Text.Trim()}%' OR SupCon LIKE '%{txtSearch.Text.Trim()}%'";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmSuppliers_Load(object sender, EventArgs e)
        {

        }
    }
}
