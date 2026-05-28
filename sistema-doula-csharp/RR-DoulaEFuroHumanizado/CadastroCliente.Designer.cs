namespace RR_DoulaEFuroHumanizado
{
    partial class CadastroCliente
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
            components = new System.ComponentModel.Container();
            btnCadastroCliente_Cadastrar = new Button();
            cbbCadastroCliente_eCivil = new ComboBox();
            label1 = new Label();
            Comfirmaçao_Cadastro_Cliente = new ErrorProvider(components);
            txtCadastroCliente_Nome = new TextBox();
            mskCadastroCliente_Telefone = new MaskedTextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            txtCadastroCliente_Idade = new TextBox();
            txtCadastroCliente_Bairro = new TextBox();
            txtCadastroCliente_Email = new TextBox();
            cbbCadastroCliente_Nacionalidade = new ComboBox();
            cbbCadastroCliente_Sexualidade = new ComboBox();
            mskCadastroCliente_CEP = new MaskedTextBox();
            btnCadastroCliente_Voltar = new Button();
            btnCadastroCliente_LimparTudo = new Button();
            label12 = new Label();
            label10 = new Label();
            mskCadastroCliente_CPF = new MaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)Comfirmaçao_Cadastro_Cliente).BeginInit();
            SuspendLayout();
            // 
            // btnCadastroCliente_Cadastrar
            // 
            btnCadastroCliente_Cadastrar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnCadastroCliente_Cadastrar.Location = new Point(260, 388);
            btnCadastroCliente_Cadastrar.Name = "btnCadastroCliente_Cadastrar";
            btnCadastroCliente_Cadastrar.Size = new Size(75, 23);
            btnCadastroCliente_Cadastrar.TabIndex = 0;
            btnCadastroCliente_Cadastrar.Text = "Cadastrar";
            btnCadastroCliente_Cadastrar.UseVisualStyleBackColor = true;
            btnCadastroCliente_Cadastrar.Click += btnCadastroCliente_Cadastrar_Click;
            // 
            // cbbCadastroCliente_eCivil
            // 
            cbbCadastroCliente_eCivil.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            cbbCadastroCliente_eCivil.FormattingEnabled = true;
            cbbCadastroCliente_eCivil.Items.AddRange(new object[] { "Solteiro", "Casado", "Divorciado", "Viuvo" });
            cbbCadastroCliente_eCivil.Location = new Point(25, 162);
            cbbCadastroCliente_eCivil.Name = "cbbCadastroCliente_eCivil";
            cbbCadastroCliente_eCivil.Size = new Size(158, 25);
            cbbCadastroCliente_eCivil.TabIndex = 1;
            cbbCadastroCliente_eCivil.Validating += cbbCadastroCliente_eCivil_Validating;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label1.Location = new Point(25, 64);
            label1.Name = "label1";
            label1.Size = new Size(45, 17);
            label1.TabIndex = 2;
            label1.Text = "Nome";
            // 
            // Comfirmaçao_Cadastro_Cliente
            // 
            Comfirmaçao_Cadastro_Cliente.ContainerControl = this;
            // 
            // txtCadastroCliente_Nome
            // 
            txtCadastroCliente_Nome.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            txtCadastroCliente_Nome.Location = new Point(25, 91);
            txtCadastroCliente_Nome.Name = "txtCadastroCliente_Nome";
            txtCadastroCliente_Nome.Size = new Size(290, 25);
            txtCadastroCliente_Nome.TabIndex = 3;
            txtCadastroCliente_Nome.Validating += txtCadastroCliente_Nome_Validating;
            // 
            // mskCadastroCliente_Telefone
            // 
            mskCadastroCliente_Telefone.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            mskCadastroCliente_Telefone.Location = new Point(416, 91);
            mskCadastroCliente_Telefone.Name = "mskCadastroCliente_Telefone";
            mskCadastroCliente_Telefone.Size = new Size(161, 25);
            mskCadastroCliente_Telefone.TabIndex = 4;
            mskCadastroCliente_Telefone.Validating += mskCadastroCliente_Telefone_Validating;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label2.Location = new Point(328, 64);
            label2.Name = "label2";
            label2.Size = new Size(42, 17);
            label2.TabIndex = 5;
            label2.Text = "Idade";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label3.Location = new Point(416, 64);
            label3.Name = "label3";
            label3.Size = new Size(61, 17);
            label3.TabIndex = 6;
            label3.Text = "Telefone";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label4.Location = new Point(25, 134);
            label4.Name = "label4";
            label4.Size = new Size(80, 17);
            label4.TabIndex = 7;
            label4.Text = "Estado Civil";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label5.Location = new Point(245, 218);
            label5.Name = "label5";
            label5.Size = new Size(96, 17);
            label5.TabIndex = 8;
            label5.Text = "Nacionalidade";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label6.Location = new Point(372, 134);
            label6.Name = "label6";
            label6.Size = new Size(82, 17);
            label6.TabIndex = 9;
            label6.Text = "Sexualidade";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label7.Location = new Point(25, 218);
            label7.Name = "label7";
            label7.Size = new Size(45, 17);
            label7.TabIndex = 10;
            label7.Text = "Bairro";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label8.Location = new Point(428, 218);
            label8.Name = "label8";
            label8.Size = new Size(31, 17);
            label8.TabIndex = 11;
            label8.Text = "CEP";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label9.Location = new Point(25, 297);
            label9.Name = "label9";
            label9.Size = new Size(42, 17);
            label9.TabIndex = 12;
            label9.Text = "Email";
            // 
            // txtCadastroCliente_Idade
            // 
            txtCadastroCliente_Idade.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            txtCadastroCliente_Idade.Location = new Point(328, 91);
            txtCadastroCliente_Idade.Name = "txtCadastroCliente_Idade";
            txtCadastroCliente_Idade.Size = new Size(73, 25);
            txtCadastroCliente_Idade.TabIndex = 15;
            txtCadastroCliente_Idade.Validating += txtCadastroCliente_Idade_Validating;
            // 
            // txtCadastroCliente_Bairro
            // 
            txtCadastroCliente_Bairro.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            txtCadastroCliente_Bairro.Location = new Point(25, 245);
            txtCadastroCliente_Bairro.Name = "txtCadastroCliente_Bairro";
            txtCadastroCliente_Bairro.Size = new Size(202, 25);
            txtCadastroCliente_Bairro.TabIndex = 17;
            txtCadastroCliente_Bairro.Validating += txtCadastroCliente_Bairro_Validating;
            // 
            // txtCadastroCliente_Email
            // 
            txtCadastroCliente_Email.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            txtCadastroCliente_Email.Location = new Point(25, 325);
            txtCadastroCliente_Email.Name = "txtCadastroCliente_Email";
            txtCadastroCliente_Email.Size = new Size(552, 25);
            txtCadastroCliente_Email.TabIndex = 19;
            txtCadastroCliente_Email.Validating += txtCadastroCliente_Email_Validating;
            // 
            // cbbCadastroCliente_Nacionalidade
            // 
            cbbCadastroCliente_Nacionalidade.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            cbbCadastroCliente_Nacionalidade.FormattingEnabled = true;
            cbbCadastroCliente_Nacionalidade.Items.AddRange(new object[] { "Brasil", "Argentina", "Cuba", "Espanha", "Uruguai", "Paraguai" });
            cbbCadastroCliente_Nacionalidade.Location = new Point(245, 245);
            cbbCadastroCliente_Nacionalidade.Name = "cbbCadastroCliente_Nacionalidade";
            cbbCadastroCliente_Nacionalidade.Size = new Size(165, 25);
            cbbCadastroCliente_Nacionalidade.TabIndex = 21;
            cbbCadastroCliente_Nacionalidade.Validating += cbbCadastroCliente_Nacionalidade_Validating;
            // 
            // cbbCadastroCliente_Sexualidade
            // 
            cbbCadastroCliente_Sexualidade.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            cbbCadastroCliente_Sexualidade.FormattingEnabled = true;
            cbbCadastroCliente_Sexualidade.Items.AddRange(new object[] { "Masculino", "Feminino", "Lesbica", "Trans", "Sis" });
            cbbCadastroCliente_Sexualidade.Location = new Point(372, 162);
            cbbCadastroCliente_Sexualidade.Name = "cbbCadastroCliente_Sexualidade";
            cbbCadastroCliente_Sexualidade.Size = new Size(205, 25);
            cbbCadastroCliente_Sexualidade.TabIndex = 22;
            cbbCadastroCliente_Sexualidade.Validating += cbbCadastroCliente_Sexualidade_Validating;
            // 
            // mskCadastroCliente_CEP
            // 
            mskCadastroCliente_CEP.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            mskCadastroCliente_CEP.Location = new Point(428, 245);
            mskCadastroCliente_CEP.Name = "mskCadastroCliente_CEP";
            mskCadastroCliente_CEP.Size = new Size(149, 25);
            mskCadastroCliente_CEP.TabIndex = 25;
            mskCadastroCliente_CEP.Validating += mskCadastroCliente_CEP_Validating;
            // 
            // btnCadastroCliente_Voltar
            // 
            btnCadastroCliente_Voltar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnCadastroCliente_Voltar.Location = new Point(25, 454);
            btnCadastroCliente_Voltar.Name = "btnCadastroCliente_Voltar";
            btnCadastroCliente_Voltar.Size = new Size(75, 23);
            btnCadastroCliente_Voltar.TabIndex = 26;
            btnCadastroCliente_Voltar.Text = "Voltar";
            btnCadastroCliente_Voltar.UseVisualStyleBackColor = true;
            btnCadastroCliente_Voltar.Click += btnCadastroCliente_Voltar_Click;
            // 
            // btnCadastroCliente_LimparTudo
            // 
            btnCadastroCliente_LimparTudo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnCadastroCliente_LimparTudo.Location = new Point(483, 450);
            btnCadastroCliente_LimparTudo.Name = "btnCadastroCliente_LimparTudo";
            btnCadastroCliente_LimparTudo.Size = new Size(94, 31);
            btnCadastroCliente_LimparTudo.TabIndex = 27;
            btnCadastroCliente_LimparTudo.Text = "Limpar Tudo";
            btnCadastroCliente_LimparTudo.UseVisualStyleBackColor = true;
            btnCadastroCliente_LimparTudo.Click += btnCadastroCliente_LimparTudo_Click;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Ink Free", 9.75F, FontStyle.Bold | FontStyle.Italic);
            label12.Location = new Point(200, 9);
            label12.Name = "label12";
            label12.Size = new Size(194, 16);
            label12.TabIndex = 28;
            label12.Text = "RR Doula e Furo Humanizado";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label10.Location = new Point(199, 134);
            label10.Name = "label10";
            label10.Size = new Size(31, 17);
            label10.TabIndex = 29;
            label10.Text = "CPF";
            // 
            // mskCadastroCliente_CPF
            // 
            mskCadastroCliente_CPF.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            mskCadastroCliente_CPF.Location = new Point(199, 162);
            mskCadastroCliente_CPF.Mask = "000.000.000-xx";
            mskCadastroCliente_CPF.Name = "mskCadastroCliente_CPF";
            mskCadastroCliente_CPF.Size = new Size(152, 25);
            mskCadastroCliente_CPF.TabIndex = 30;
            mskCadastroCliente_CPF.Validating += mskCadastroCliente_CPF_Validating;
            // 
            // CadastroCliente
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(595, 510);
            Controls.Add(mskCadastroCliente_CPF);
            Controls.Add(label10);
            Controls.Add(label12);
            Controls.Add(btnCadastroCliente_LimparTudo);
            Controls.Add(btnCadastroCliente_Voltar);
            Controls.Add(mskCadastroCliente_CEP);
            Controls.Add(cbbCadastroCliente_Sexualidade);
            Controls.Add(cbbCadastroCliente_Nacionalidade);
            Controls.Add(txtCadastroCliente_Email);
            Controls.Add(txtCadastroCliente_Bairro);
            Controls.Add(txtCadastroCliente_Idade);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(mskCadastroCliente_Telefone);
            Controls.Add(txtCadastroCliente_Nome);
            Controls.Add(label1);
            Controls.Add(cbbCadastroCliente_eCivil);
            Controls.Add(btnCadastroCliente_Cadastrar);
            Name = "CadastroCliente";
            Text = "CadastroCliente";
            ((System.ComponentModel.ISupportInitialize)Comfirmaçao_Cadastro_Cliente).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCadastroCliente_Cadastrar;
        private ComboBox cbbCadastroCliente_eCivil;
        private Label label1;
        private ErrorProvider Comfirmaçao_Cadastro_Cliente;
        private ComboBox cbbCadastroCliente_Sexualidade;
        private ComboBox cbbCadastroCliente_Nacionalidade;
        private TextBox txtCadastroCliente_Email;
        private TextBox txtCadastroCliente_Bairro;
        private TextBox txtCadastroCliente_Idade;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private MaskedTextBox mskCadastroCliente_Telefone;
        private TextBox txtCadastroCliente_Nome;
        private Label label12;
        private Button btnCadastroCliente_LimparTudo;
        private Button btnCadastroCliente_Voltar;
        private MaskedTextBox mskCadastroCliente_CEP;
        private MaskedTextBox mskCadastroCliente_CPF;
        private Label label10;
    }
}