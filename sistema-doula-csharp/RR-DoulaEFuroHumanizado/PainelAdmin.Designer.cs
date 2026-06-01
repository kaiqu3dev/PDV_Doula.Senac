namespace RR_DoulaEFuroHumanizado
{
    partial class PainelAdmin
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
            dgvPainelAdm_Agendamentos = new DataGridView();
            btnPainelAdmin_Reagendar = new Button();
            btnPainelAdmin_Reembolsar = new Button();
            txtPainelAdm_Nome = new TextBox();
            txtPainelAdm_Email = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            mskPainelAdm_Telefone = new MaskedTextBox();
            mskPainelAdm_CPF = new MaskedTextBox();
            dtpPainelAdm_NovaData = new DateTimePicker();
            cbbPainelAdm_NovoHorario = new ComboBox();
            btnPainelAdmin_Atualizar = new Button();
            btnPainelAdmin_Agendamento = new Button();
            lblPainelAdmin_Logado = new Label();
            btnPainelAdmin_Novo_Agendamento = new Button();
            btnPainelAdmin_Deletar_Usuario = new Button();
            btnPainelAdmin_Sair = new Button();
            btnPainelAdmin_Servicos = new Button();
            btnPainelAdmin_Cadastrar_Funcionarios = new Button();
            btnPainelAdmin_Financas = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvPainelAdm_Agendamentos).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // dgvPainelAdm_Agendamentos
            // 
            dgvPainelAdm_Agendamentos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPainelAdm_Agendamentos.BackgroundColor = Color.FromArgb(232, 243, 237);
            dgvPainelAdm_Agendamentos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPainelAdm_Agendamentos.Location = new Point(0, 127);
            dgvPainelAdm_Agendamentos.Name = "dgvPainelAdm_Agendamentos";
            dgvPainelAdm_Agendamentos.Size = new Size(1350, 448);
            dgvPainelAdm_Agendamentos.TabIndex = 0;
            dgvPainelAdm_Agendamentos.CellClick += dgvPainelAdm_Agendamentos_CellClick;
            dgvPainelAdm_Agendamentos.RowPrePaint += dgvPainelAdm_Agendamentos_RowPrePaint;
            // 
            // btnPainelAdmin_Reagendar
            // 
            btnPainelAdmin_Reagendar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPainelAdmin_Reagendar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPainelAdmin_Reagendar.Location = new Point(0, 45);
            btnPainelAdmin_Reagendar.Name = "btnPainelAdmin_Reagendar";
            btnPainelAdmin_Reagendar.Size = new Size(104, 80);
            btnPainelAdmin_Reagendar.TabIndex = 1;
            btnPainelAdmin_Reagendar.Text = "Reagendar";
            btnPainelAdmin_Reagendar.UseVisualStyleBackColor = true;
            btnPainelAdmin_Reagendar.Click += btnPainelAdmin_Reagendar_Click;
            // 
            // btnPainelAdmin_Reembolsar
            // 
            btnPainelAdmin_Reembolsar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPainelAdmin_Reembolsar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPainelAdmin_Reembolsar.Location = new Point(110, 45);
            btnPainelAdmin_Reembolsar.Name = "btnPainelAdmin_Reembolsar";
            btnPainelAdmin_Reembolsar.Size = new Size(104, 80);
            btnPainelAdmin_Reembolsar.TabIndex = 2;
            btnPainelAdmin_Reembolsar.Text = "Reembolsar";
            btnPainelAdmin_Reembolsar.UseVisualStyleBackColor = true;
            btnPainelAdmin_Reembolsar.Click += btnPainelAdmin_Reembolsar_Click;
            // 
            // txtPainelAdm_Nome
            // 
            txtPainelAdm_Nome.Anchor = AnchorStyles.Left;
            txtPainelAdm_Nome.Location = new Point(2, 90);
            txtPainelAdm_Nome.Name = "txtPainelAdm_Nome";
            txtPainelAdm_Nome.Size = new Size(244, 23);
            txtPainelAdm_Nome.TabIndex = 4;
            // 
            // txtPainelAdm_Email
            // 
            txtPainelAdm_Email.Anchor = AnchorStyles.Left;
            txtPainelAdm_Email.Location = new Point(288, 90);
            txtPainelAdm_Email.Name = "txtPainelAdm_Email";
            txtPainelAdm_Email.Size = new Size(268, 23);
            txtPainelAdm_Email.TabIndex = 5;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left;
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(2, 70);
            label1.Name = "label1";
            label1.Size = new Size(45, 17);
            label1.TabIndex = 8;
            label1.Text = "Nome";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Left;
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label2.Location = new Point(288, 70);
            label2.Name = "label2";
            label2.Size = new Size(47, 17);
            label2.TabIndex = 9;
            label2.Text = "E-mail";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Left;
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label3.Location = new Point(629, 70);
            label3.Name = "label3";
            label3.Size = new Size(61, 17);
            label3.TabIndex = 10;
            label3.Text = "Telefone";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Left;
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            label4.Location = new Point(930, 70);
            label4.Name = "label4";
            label4.Size = new Size(31, 17);
            label4.TabIndex = 11;
            label4.Text = "CPF";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top;
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Ink Free", 27.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label5.Location = new Point(426, 5);
            label5.Name = "label5";
            label5.Size = new Size(499, 46);
            label5.TabIndex = 12;
            label5.Text = "RR Doula e Furo Humanizado";
            // 
            // mskPainelAdm_Telefone
            // 
            mskPainelAdm_Telefone.Anchor = AnchorStyles.Left;
            mskPainelAdm_Telefone.Location = new Point(629, 90);
            mskPainelAdm_Telefone.Name = "mskPainelAdm_Telefone";
            mskPainelAdm_Telefone.Size = new Size(217, 23);
            mskPainelAdm_Telefone.TabIndex = 13;
            // 
            // mskPainelAdm_CPF
            // 
            mskPainelAdm_CPF.Anchor = AnchorStyles.Left;
            mskPainelAdm_CPF.BackColor = SystemColors.InactiveBorder;
            mskPainelAdm_CPF.Location = new Point(930, 90);
            mskPainelAdm_CPF.Name = "mskPainelAdm_CPF";
            mskPainelAdm_CPF.Size = new Size(174, 23);
            mskPainelAdm_CPF.TabIndex = 14;
            // 
            // dtpPainelAdm_NovaData
            // 
            dtpPainelAdm_NovaData.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dtpPainelAdm_NovaData.Format = DateTimePickerFormat.Short;
            dtpPainelAdm_NovaData.Location = new Point(1085, 0);
            dtpPainelAdm_NovaData.Name = "dtpPainelAdm_NovaData";
            dtpPainelAdm_NovaData.Size = new Size(265, 23);
            dtpPainelAdm_NovaData.TabIndex = 15;
            // 
            // cbbPainelAdm_NovoHorario
            // 
            cbbPainelAdm_NovoHorario.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbbPainelAdm_NovoHorario.FormattingEnabled = true;
            cbbPainelAdm_NovoHorario.Location = new Point(863, 0);
            cbbPainelAdm_NovoHorario.Name = "cbbPainelAdm_NovoHorario";
            cbbPainelAdm_NovoHorario.Size = new Size(222, 23);
            cbbPainelAdm_NovoHorario.TabIndex = 16;
            // 
            // btnPainelAdmin_Atualizar
            // 
            btnPainelAdmin_Atualizar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPainelAdmin_Atualizar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPainelAdmin_Atualizar.Location = new Point(1065, 45);
            btnPainelAdmin_Atualizar.Name = "btnPainelAdmin_Atualizar";
            btnPainelAdmin_Atualizar.Size = new Size(144, 80);
            btnPainelAdmin_Atualizar.TabIndex = 18;
            btnPainelAdmin_Atualizar.Text = "Atualizar";
            btnPainelAdmin_Atualizar.UseVisualStyleBackColor = true;
            btnPainelAdmin_Atualizar.Click += btnPainelAdmin_Atualizar_Click;
            // 
            // btnPainelAdmin_Agendamento
            // 
            btnPainelAdmin_Agendamento.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPainelAdmin_Agendamento.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPainelAdmin_Agendamento.Location = new Point(369, 45);
            btnPainelAdmin_Agendamento.Name = "btnPainelAdmin_Agendamento";
            btnPainelAdmin_Agendamento.Size = new Size(132, 80);
            btnPainelAdmin_Agendamento.TabIndex = 19;
            btnPainelAdmin_Agendamento.Text = "Agendamento";
            btnPainelAdmin_Agendamento.UseVisualStyleBackColor = true;
            btnPainelAdmin_Agendamento.Click += btnPainelAdmin_Agendamento_Click;
            // 
            // lblPainelAdmin_Logado
            // 
            lblPainelAdmin_Logado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPainelAdmin_Logado.BackColor = Color.Transparent;
            lblPainelAdmin_Logado.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblPainelAdmin_Logado.ForeColor = Color.Black;
            lblPainelAdmin_Logado.Location = new Point(1148, 9);
            lblPainelAdmin_Logado.Name = "lblPainelAdmin_Logado";
            lblPainelAdmin_Logado.Size = new Size(200, 49);
            lblPainelAdmin_Logado.TabIndex = 20;
            lblPainelAdmin_Logado.Text = "Usuário";
            lblPainelAdmin_Logado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnPainelAdmin_Novo_Agendamento
            // 
            btnPainelAdmin_Novo_Agendamento.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPainelAdmin_Novo_Agendamento.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPainelAdmin_Novo_Agendamento.Location = new Point(219, 45);
            btnPainelAdmin_Novo_Agendamento.Name = "btnPainelAdmin_Novo_Agendamento";
            btnPainelAdmin_Novo_Agendamento.Size = new Size(144, 80);
            btnPainelAdmin_Novo_Agendamento.TabIndex = 21;
            btnPainelAdmin_Novo_Agendamento.Text = "Novo Agendamento";
            btnPainelAdmin_Novo_Agendamento.UseVisualStyleBackColor = true;
            btnPainelAdmin_Novo_Agendamento.Click += btnPainelAdmin_Novo_Agendamento_Click;
            // 
            // btnPainelAdmin_Deletar_Usuario
            // 
            btnPainelAdmin_Deletar_Usuario.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPainelAdmin_Deletar_Usuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPainelAdmin_Deletar_Usuario.Location = new Point(919, 45);
            btnPainelAdmin_Deletar_Usuario.Name = "btnPainelAdmin_Deletar_Usuario";
            btnPainelAdmin_Deletar_Usuario.Size = new Size(140, 80);
            btnPainelAdmin_Deletar_Usuario.TabIndex = 22;
            btnPainelAdmin_Deletar_Usuario.Text = "Delete";
            btnPainelAdmin_Deletar_Usuario.UseVisualStyleBackColor = true;
            btnPainelAdmin_Deletar_Usuario.Click += btnPainelAdmin_Deletar_Usuario_Click;
            // 
            // btnPainelAdmin_Sair
            // 
            btnPainelAdmin_Sair.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPainelAdmin_Sair.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPainelAdmin_Sair.Location = new Point(1215, 45);
            btnPainelAdmin_Sair.Name = "btnPainelAdmin_Sair";
            btnPainelAdmin_Sair.Size = new Size(133, 80);
            btnPainelAdmin_Sair.TabIndex = 23;
            btnPainelAdmin_Sair.Text = "Sair";
            btnPainelAdmin_Sair.UseVisualStyleBackColor = true;
            btnPainelAdmin_Sair.Click += btnPainelAdmin_Sair_Click;
            // 
            // btnPainelAdmin_Servicos
            // 
            btnPainelAdmin_Servicos.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPainelAdmin_Servicos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPainelAdmin_Servicos.Location = new Point(507, 45);
            btnPainelAdmin_Servicos.Name = "btnPainelAdmin_Servicos";
            btnPainelAdmin_Servicos.Size = new Size(132, 80);
            btnPainelAdmin_Servicos.TabIndex = 24;
            btnPainelAdmin_Servicos.Text = "Serviços";
            btnPainelAdmin_Servicos.UseVisualStyleBackColor = true;
            btnPainelAdmin_Servicos.Click += btnPainelAdmin_Servicos_Click;
            // 
            // btnPainelAdmin_Cadastrar_Funcionarios
            // 
            btnPainelAdmin_Cadastrar_Funcionarios.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPainelAdmin_Cadastrar_Funcionarios.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPainelAdmin_Cadastrar_Funcionarios.Location = new Point(646, 45);
            btnPainelAdmin_Cadastrar_Funcionarios.Name = "btnPainelAdmin_Cadastrar_Funcionarios";
            btnPainelAdmin_Cadastrar_Funcionarios.Size = new Size(132, 80);
            btnPainelAdmin_Cadastrar_Funcionarios.TabIndex = 25;
            btnPainelAdmin_Cadastrar_Funcionarios.Text = "Cadastrar Funcionarios";
            btnPainelAdmin_Cadastrar_Funcionarios.UseVisualStyleBackColor = true;
            btnPainelAdmin_Cadastrar_Funcionarios.Click += btnPainelAdmin_Cadastrar_Funcionarios_Click;
            // 
            // btnPainelAdmin_Financas
            // 
            btnPainelAdmin_Financas.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPainelAdmin_Financas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPainelAdmin_Financas.Location = new Point(783, 45);
            btnPainelAdmin_Financas.Name = "btnPainelAdmin_Financas";
            btnPainelAdmin_Financas.Size = new Size(132, 80);
            btnPainelAdmin_Financas.TabIndex = 26;
            btnPainelAdmin_Financas.Text = "Finanças";
            btnPainelAdmin_Financas.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(111, 174, 173);
            panel1.Controls.Add(btnPainelAdmin_Reagendar);
            panel1.Controls.Add(btnPainelAdmin_Sair);
            panel1.Controls.Add(btnPainelAdmin_Novo_Agendamento);
            panel1.Controls.Add(cbbPainelAdm_NovoHorario);
            panel1.Controls.Add(btnPainelAdmin_Deletar_Usuario);
            panel1.Controls.Add(dtpPainelAdm_NovaData);
            panel1.Controls.Add(btnPainelAdmin_Reembolsar);
            panel1.Controls.Add(btnPainelAdmin_Financas);
            panel1.Controls.Add(btnPainelAdmin_Atualizar);
            panel1.Controls.Add(btnPainelAdmin_Agendamento);
            panel1.Controls.Add(btnPainelAdmin_Cadastrar_Funcionarios);
            panel1.Controls.Add(btnPainelAdmin_Servicos);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 575);
            panel1.Name = "panel1";
            panel1.Size = new Size(1350, 154);
            panel1.TabIndex = 27;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(111, 174, 173);
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.Controls.Add(label5);
            panel2.Controls.Add(lblPainelAdmin_Logado);
            panel2.Controls.Add(txtPainelAdm_Nome);
            panel2.Controls.Add(txtPainelAdm_Email);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(mskPainelAdm_CPF);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(mskPainelAdm_Telefone);
            panel2.Controls.Add(label4);
            panel2.Dock = DockStyle.Top;
            panel2.ForeColor = SystemColors.ControlText;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1350, 127);
            panel2.TabIndex = 28;
            // 
            // PainelAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(1350, 729);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Controls.Add(dgvPainelAdm_Agendamentos);
            Name = "PainelAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PainelAdmin";
            Load += PainelAdmin_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPainelAdm_Agendamentos).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvPainelAdm_Agendamentos;
        private Button btnPainelAdmin_Reagendar;
        private Button btnPainelAdmin_Reembolsar;
        private TextBox txtPainelAdm_Nome;
        private TextBox txtPainelAdm_Email;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private MaskedTextBox mskPainelAdm_Telefone;
        private MaskedTextBox mskPainelAdm_CPF;
        private DateTimePicker dtpPainelAdm_NovaData;
        private ComboBox cbbPainelAdm_NovoHorario;
        private Button btnPainelAdmin_Atualizar;
        private Button btnPainelAdmin_Agendamento;
        private Label lblPainelAdmin_Logado;
        private Button btnPainelAdmin_Novo_Agendamento;
        private Button btnPainelAdmin_Deletar_Usuario;
        private Button btnPainelAdmin_Sair;
        private Button btnPainelAdmin_Servicos;
        private Button btnPainelAdmin_Cadastrar_Funcionarios;
        private Button btnPainelAdmin_Financas;
        private Panel panel1;
        private Panel panel2;
    }
}