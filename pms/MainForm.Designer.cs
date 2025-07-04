namespace pms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Product = new System.Windows.Forms.Button();
            this.Payment = new System.Windows.Forms.Button();
            this.OrderDetail = new System.Windows.Forms.Button();
            this.ImportDetail = new System.Windows.Forms.Button();
            this.Customer = new System.Windows.Forms.Button();
            this.Supplire = new System.Windows.Forms.Button();
            this.Staffs = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.PaleVioletRed;
            this.textBox1.Font = new System.Drawing.Font("Monotype Corsiva", 48F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Blue;
            this.textBox1.Location = new System.Drawing.Point(0, -4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(1324, 111);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "Management System";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Product
            // 
            this.Product.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 22F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Product.Image = global::pms.Properties.Resources.add_product;
            this.Product.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Product.Location = new System.Drawing.Point(472, 123);
            this.Product.Name = "Product";
            this.Product.Size = new System.Drawing.Size(356, 293);
            this.Product.TabIndex = 11;
            this.Product.Text = "Products";
            this.Product.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Product.UseVisualStyleBackColor = true;
            this.Product.Click += new System.EventHandler(this.Product_Click);
            // 
            // Payment
            // 
            this.Payment.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 22F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Payment.Image = global::pms.Properties.Resources.cashless_payment;
            this.Payment.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Payment.Location = new System.Drawing.Point(905, 744);
            this.Payment.Name = "Payment";
            this.Payment.Size = new System.Drawing.Size(360, 275);
            this.Payment.TabIndex = 10;
            this.Payment.Text = "Payment";
            this.Payment.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Payment.UseVisualStyleBackColor = true;
            this.Payment.Click += new System.EventHandler(this.Payment_Click);
            // 
            // OrderDetail
            // 
            this.OrderDetail.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 22F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrderDetail.Image = global::pms.Properties.Resources.order;
            this.OrderDetail.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.OrderDetail.Location = new System.Drawing.Point(472, 744);
            this.OrderDetail.Name = "OrderDetail";
            this.OrderDetail.Size = new System.Drawing.Size(356, 275);
            this.OrderDetail.TabIndex = 9;
            this.OrderDetail.Text = "OrderDetail";
            this.OrderDetail.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.OrderDetail.UseVisualStyleBackColor = true;
            this.OrderDetail.Click += new System.EventHandler(this.OrderDetail_Click);
            // 
            // ImportDetail
            // 
            this.ImportDetail.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 22F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImportDetail.Image = global::pms.Properties.Resources.import;
            this.ImportDetail.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ImportDetail.Location = new System.Drawing.Point(33, 435);
            this.ImportDetail.Name = "ImportDetail";
            this.ImportDetail.Size = new System.Drawing.Size(1232, 282);
            this.ImportDetail.TabIndex = 8;
            this.ImportDetail.Text = "ImportDetail";
            this.ImportDetail.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ImportDetail.UseVisualStyleBackColor = true;
            this.ImportDetail.Click += new System.EventHandler(this.ImportDetail_Click);
            // 
            // Customer
            // 
            this.Customer.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 22F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Customer.Image = global::pms.Properties.Resources.rating;
            this.Customer.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Customer.Location = new System.Drawing.Point(905, 123);
            this.Customer.Name = "Customer";
            this.Customer.Size = new System.Drawing.Size(360, 293);
            this.Customer.TabIndex = 7;
            this.Customer.Text = "Customer";
            this.Customer.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Customer.UseVisualStyleBackColor = true;
            this.Customer.Click += new System.EventHandler(this.Customer_Click);
            // 
            // Supplire
            // 
            this.Supplire.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 22F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Supplire.Image = global::pms.Properties.Resources.supplier;
            this.Supplire.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Supplire.Location = new System.Drawing.Point(33, 744);
            this.Supplire.Name = "Supplire";
            this.Supplire.Size = new System.Drawing.Size(367, 275);
            this.Supplire.TabIndex = 6;
            this.Supplire.Text = "Supplirer";
            this.Supplire.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Supplire.UseVisualStyleBackColor = true;
            this.Supplire.Click += new System.EventHandler(this.Supplire_Click);
            // 
            // Staffs
            // 
            this.Staffs.Font = new System.Drawing.Font("Yu Gothic UI Semibold", 22F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Staffs.Image = global::pms.Properties.Resources.people;
            this.Staffs.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Staffs.Location = new System.Drawing.Point(33, 123);
            this.Staffs.Name = "Staffs";
            this.Staffs.Size = new System.Drawing.Size(367, 293);
            this.Staffs.TabIndex = 5;
            this.Staffs.Text = "Staffs";
            this.Staffs.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Staffs.UseVisualStyleBackColor = true;
            this.Staffs.Click += new System.EventHandler(this.Staffs_Click);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.PaleVioletRed;
            this.textBox2.Font = new System.Drawing.Font("Monotype Corsiva", 48F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.Color.Blue;
            this.textBox2.Location = new System.Drawing.Point(0, 1035);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(1324, 111);
            this.textBox2.TabIndex = 12;
            this.textBox2.Text = "Build by THOEUNG PANNO";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ClientSize = new System.Drawing.Size(1326, 1165);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.Product);
            this.Controls.Add(this.Payment);
            this.Controls.Add(this.OrderDetail);
            this.Controls.Add(this.ImportDetail);
            this.Controls.Add(this.Customer);
            this.Controls.Add(this.Supplire);
            this.Controls.Add(this.Staffs);
            this.Controls.Add(this.textBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Staffs;
        private System.Windows.Forms.Button Supplire;
        private System.Windows.Forms.Button Customer;
        private System.Windows.Forms.Button ImportDetail;
        private System.Windows.Forms.Button OrderDetail;
        private System.Windows.Forms.Button Payment;
        private System.Windows.Forms.Button Product;
        private System.Windows.Forms.TextBox textBox2;
    }
}