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
    public partial class frmStaffs : Form
    {
        private BindingSource bindingSource = new BindingSource();
        A6Y3 d = new A6Y3();
        SqlCommand com;
        SqlDataAdapter dap;
        DataTable dt;

        string fp;
        byte[] photo;
        public frmStaffs()
        {
            d.Connection();
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            try
            {
                dgView.DataSource = null;
                com = new SqlCommand("spGetAllStaff", d.con);
                com.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter dap = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                dap.Fill(dt);

                dgView.DataSource = dt;
                DataGridViewImageColumn img = (DataGridViewImageColumn)dgView.Columns["photo"];
                if (img != null)
                {
                    img.ImageLayout = DataGridViewImageCellLayout.Stretch;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            fd.Title = "Open an image...";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                fp = fd.FileName;
                pictStaff.Image = Image.FromFile(fp);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                var salary = decimal.Parse(txtSal.Text, NumberStyles.Currency);
                com = new SqlCommand("spInsertStaff", d.con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@id", txtID.Text);
                com.Parameters.AddWithValue("@fn", txtName.Text);
                com.Parameters.AddWithValue("@g", rdbF.Checked ? "F" : "M");
                com.Parameters.AddWithValue("@db", dtpDob.Value);
                com.Parameters.AddWithValue("@po", txtPos.Text);
                com.Parameters.AddWithValue("@s", salary);
                com.Parameters.AddWithValue("@sw", 0);

                if (fp != null)
                {
                    photo = File.ReadAllBytes(fp);
                    com.Parameters.AddWithValue("@ph", photo);
                }
                else
                {
                    com.Parameters.AddWithValue("@ph", DBNull.Value);
                }

                if (d.con.State != ConnectionState.Open)
                    d.con.Open();

                com.ExecuteNonQuery();
                //MessageBox.Show("Staff inserted successfully.");
                LoadData();
                fp = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insert Error: " + ex.Message);
            }
            finally
            {
                d.con.Close();
            }
        }

        private void dgView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i < 0 || dgView.Rows.Count == 0) return;

            DataGridViewRow row = dgView.Rows[i];

            txtID.Text = row.Cells[0]?.Value?.ToString() ?? string.Empty;
            txtName.Text = row.Cells[1]?.Value?.ToString() ?? string.Empty;

            if (row.Cells[2]?.Value?.ToString() == "F")
                rdbF.Checked = true;
            else
                rdbM.Checked = true;

            dtpDob.CustomFormat = "yyyy-MM-dd";
            dtpDob.Text = row.Cells[3]?.Value?.ToString();

            txtPos.Text = row.Cells[4]?.Value?.ToString();
            txtSal.Text = string.Format("{0:c}", row.Cells[5]?.Value);

            if (row.Cells[7].Value != DBNull.Value)
            {
                photo = (byte[])row.Cells[7].Value;
                MemoryStream ms = new MemoryStream(photo);
                pictStaff.Image = Image.FromStream(ms);
            }
            else
            {
                pictStaff.Image = null;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var salary = decimal.Parse(txtSal.Text, NumberStyles.Currency);
                com = new SqlCommand("spUpdateStaff", d.con);
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@id", txtID.Text);
                com.Parameters.AddWithValue("@fn", txtName.Text);
                com.Parameters.AddWithValue("@g", rdbF.Checked ? "F" : "M");
                com.Parameters.AddWithValue("@db", dtpDob.Value);
                com.Parameters.AddWithValue("@po", txtPos.Text);
                com.Parameters.AddWithValue("@s", salary);
                com.Parameters.AddWithValue("@sw", 0);

                if (fp != null)
                {
                    photo = File.ReadAllBytes(fp);
                    com.Parameters.AddWithValue("@ph", photo);
                }
                else
                {
                    com.Parameters.
                AddWithValue("@ph", DBNull.Value);
                }

                if (d.con.State != ConnectionState.Open)
                    d.con.Open();

                com.ExecuteNonQuery();
                //MessageBox.Show("Staff updated successfully.");
                LoadData();
                fp = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update Error: " + ex.Message);
            }
            finally
            {
                d.con.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                com = new SqlCommand("spDeleteStaff", d.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@id", txtID.Text);

                if (d.con.State != ConnectionState.Open)
                    d.con.Open();

                com.ExecuteNonQuery();
                //MessageBox.Show("Staff deleted successfully.");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete Error: " + ex.Message);
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
                DataTable dt = dgView.DataSource as DataTable;

                if (dt == null)
                {
                    MessageBox.Show("No data available for searching.", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Escape quotes
                string search = txtSearch.Text.Trim().Replace("'", "''");

                // Apply filter only on string-based columns
                dt.DefaultView.RowFilter = $"Convert(staffID, 'System.String') LIKE '%{search}%' OR FullName LIKE '%{search}%'";
            }
            catch (EvaluateException ex)
            {
                MessageBox.Show("Filter error: " + ex.Message, "Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmStaffs_Load(object sender, EventArgs e)
        {

        }
    }
}
