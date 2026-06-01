namespace RR_DoulaEFuroHumanizado
{
    partial class PaginaLogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaginaLogin));
            pictureBox1 = new PictureBox();
            panelTransparente1 = new PanelTransparente();
            lblE_Senha = new Label();
            btnEntrar = new Button();
            txtSenha = new TextBox();
            txtE_mail = new TextBox();
            lblE_mail = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            pictureBox1.SuspendLayout();
            panelTransparente1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Controls.Add(panelTransparente1);
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(345, 537);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // panelTransparente1
            // 
            panelTransparente1.BackColor = Color.FromArgb(100, 160, 220, 190);
            panelTransparente1.Controls.Add(lblE_Senha);
            panelTransparente1.Controls.Add(btnEntrar);
            panelTransparente1.Controls.Add(txtSenha);
            panelTransparente1.Controls.Add(txtE_mail);
            panelTransparente1.Controls.Add(lblE_mail);
            panelTransparente1.Location = new Point(48, 257);
            panelTransparente1.Name = "panelTransparente1";
            panelTransparente1.Size = new Size(229, 247);
            panelTransparente1.TabIndex = 1;
            // 
            // lblE_Senha
            // 
            lblE_Senha.AutoSize = true;
            lblE_Senha.BackColor = Color.Transparent;
            lblE_Senha.Font = new Font("MV Boli", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblE_Senha.ForeColor = Color.DarkSlateGray;
            lblE_Senha.Location = new Point(17, 73);
            lblE_Senha.Name = "lblE_Senha";
            lblE_Senha.Size = new Size(46, 17);
            lblE_Senha.TabIndex = 8;
            lblE_Senha.Text = "Senha";
            lblE_Senha.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnEntrar
            // 
            btnEntrar.BackColor = Color.MediumSeaGreen;
            btnEntrar.FlatAppearance.BorderSize = 0;
            btnEntrar.FlatStyle = FlatStyle.Flat;
            btnEntrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEntrar.ForeColor = Color.White;
            btnEntrar.Location = new Point(68, 128);
            btnEntrar.Name = "btnEntrar";
            btnEntrar.Size = new Size(68, 27);
            btnEntrar.TabIndex = 6;
            btnEntrar.Text = "Entrar";
            btnEntrar.UseVisualStyleBackColor = false;
            btnEntrar.Click += btnEntrar_Click;
            // 
            // txtSenha
            // 
            txtSenha.BackColor = Color.White;
            txtSenha.BorderStyle = BorderStyle.None;
            txtSenha.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSenha.Location = new Point(14, 93);
            txtSenha.Name = "txtSenha";
            txtSenha.Size = new Size(170, 18);
            txtSenha.TabIndex = 3;
            txtSenha.UseSystemPasswordChar = true;
            txtSenha.KeyPress += txtSenha_KeyPress;
            // 
            // txtE_mail
            // 
            txtE_mail.BackColor = Color.White;
            txtE_mail.BorderStyle = BorderStyle.None;
            txtE_mail.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtE_mail.Location = new Point(14, 37);
            txtE_mail.Name = "txtE_mail";
            txtE_mail.Size = new Size(170, 18);
            txtE_mail.TabIndex = 2;
            // 
            // lblE_mail
            // 
            lblE_mail.AutoSize = true;
            lblE_mail.BackColor = Color.Transparent;
            lblE_mail.Font = new Font("MV Boli", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblE_mail.ForeColor = Color.DarkSlateGray;
            lblE_mail.Location = new Point(17, 16);
            lblE_mail.Name = "lblE_mail";
            lblE_mail.Size = new Size(119, 17);
            lblE_mail.TabIndex = 1;
            lblE_mail.Text = "Código de Acesso";
            lblE_mail.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PaginaLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(345, 537);
            Controls.Add(pictureBox1);
            DoubleBuffered = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PaginaLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            pictureBox1.ResumeLayout(false);
            panelTransparente1.ResumeLayout(false);
            panelTransparente1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private PanelTransparente panelTransparente1;
        private Button btnEntrar;
        private TextBox txtSenha;
        private TextBox txtE_mail;
        private Label lblE_mail;
        private Label lblE_Senha;
    }
}
