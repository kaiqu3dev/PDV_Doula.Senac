namespace RR_DoulaEFuroHumanizado
{
    partial class TelaDeServicos
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
            btnTelaServical_Buscar = new Button();
            label1 = new Label();
            dgvTelaServical_Comanda = new DataGridView();
            label2 = new Label();
            cbbTelaServical_Buscar = new ComboBox();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvTelaServical_Comanda).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnTelaServical_Buscar
            // 
            btnTelaServical_Buscar.Location = new Point(250, 65);
            btnTelaServical_Buscar.Name = "btnTelaServical_Buscar";
            btnTelaServical_Buscar.Size = new Size(75, 23);
            btnTelaServical_Buscar.TabIndex = 0;
            btnTelaServical_Buscar.Text = "Buscar";
            btnTelaServical_Buscar.UseVisualStyleBackColor = true;
            btnTelaServical_Buscar.Click += btnTelaServical_Buscar_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Ink Free", 9.75F, FontStyle.Bold | FontStyle.Italic);
            label1.Location = new Point(351, 9);
            label1.Name = "label1";
            label1.Size = new Size(194, 16);
            label1.TabIndex = 1;
            label1.Text = "RR Doula e Furo Humanizado";
            // 
            // dgvTelaServical_Comanda
            // 
            dgvTelaServical_Comanda.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTelaServical_Comanda.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTelaServical_Comanda.Location = new Point(0, 122);
            dgvTelaServical_Comanda.Name = "dgvTelaServical_Comanda";
            dgvTelaServical_Comanda.Size = new Size(887, 477);
            dgvTelaServical_Comanda.TabIndex = 2;
            dgvTelaServical_Comanda.CellValueChanged += dgvTelaServical_Comanda_CellValueChanged;
            dgvTelaServical_Comanda.CurrentCellDirtyStateChanged += dgvTelaServical_Comanda_CurrentCellDirtyStateChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 38);
            label2.Name = "label2";
            label2.Size = new Size(78, 15);
            label2.TabIndex = 3;
            label2.Text = "Dia/Mês/Ano";
            // 
            // cbbTelaServical_Buscar
            // 
            cbbTelaServical_Buscar.FormattingEnabled = true;
            cbbTelaServical_Buscar.Items.AddRange(new object[] { "Hoje", "Semana", "Mês", "Ano" });
            cbbTelaServical_Buscar.Location = new Point(12, 66);
            cbbTelaServical_Buscar.Name = "cbbTelaServical_Buscar";
            cbbTelaServical_Buscar.Size = new Size(232, 23);
            cbbTelaServical_Buscar.TabIndex = 4;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(label2);
            panel1.Controls.Add(cbbTelaServical_Buscar);
            panel1.Controls.Add(btnTelaServical_Buscar);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(887, 116);
            panel1.TabIndex = 5;
            // 
            // TelaDeServicos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(887, 599);
            Controls.Add(panel1);
            Controls.Add(dgvTelaServical_Comanda);
            Name = "TelaDeServicos";
            Text = "TelaDeServicos";
            ((System.ComponentModel.ISupportInitialize)dgvTelaServical_Comanda).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btnTelaServical_Buscar;
        private Label label1;
        private DataGridView dgvTelaServical_Comanda;
        private Label label2;
        private ComboBox cbbTelaServical_Buscar;
        private Panel panel1;
    }
}