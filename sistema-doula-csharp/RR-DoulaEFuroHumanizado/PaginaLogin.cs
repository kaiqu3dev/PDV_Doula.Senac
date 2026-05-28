using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace RR_DoulaEFuroHumanizado
{
    public partial class PaginaLogin : Form
    {
        public string emailDoUsuario;

        public PaginaLogin()
        {
            InitializeComponent();

            if (!EstaEmModoDesign())
            {
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                              ControlStyles.AllPaintingInWmPaint |
                              ControlStyles.UserPaint, true);

                this.UpdateStyles();

                panelTransparente1.BackColor = Color.FromArgb(100, 160, 220, 190);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (EstaEmModoDesign())
                return;

            IntPtr hRgn = CreateRoundRectRgn(
                0, 0,
                panelTransparente1.Width,
                panelTransparente1.Height,
                30, 30);

            panelTransparente1.Region = Region.FromHrgn(hRgn);
            DeleteObject(hRgn);
            ArredondarTextBox();
        }

        private void ArredondarTextBox()
        {
            if (EstaEmModoDesign())
                return;

            IntPtr hRgn;

            hRgn = CreateRoundRectRgn(0, 0, txtE_mail.Width, txtE_mail.Height, 15, 15);
            txtE_mail.Region = Region.FromHrgn(hRgn);
            DeleteObject(hRgn);

            hRgn = CreateRoundRectRgn(0, 0, txtSenha.Width, txtSenha.Height, 15, 15);
            txtSenha.Region = Region.FromHrgn(hRgn);
            DeleteObject(hRgn);
        }

        private bool EstaEmModoDesign()
        {
            return DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        private string VerificarLogin()
        {
            using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
            {
                conn.Open();

                string query = @"

SELECT TipoUsuario
FROM usuarios
WHERE CodigoAcesso = @Codigo
AND Senha = @Senha
AND (Status IS NULL OR Status <> 'Banido')";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", txtE_mail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Senha", txtSenha.Text.Trim());

                    object resultado = cmd.ExecuteScalar();

                    if (resultado != null)
                        return resultado.ToString();

                    return null;
                }
            }
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtE_mail.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Preencha código e senha!");
                return;
            }

            string tipoUsuario = null;
            string codigo = null;
            string nome = null;

            using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
            {
                conn.Open();

                //  VERIFICAÇÃO DA BLACKLIST
                string sqlBlacklist = "SELECT COUNT(*) FROM blacklist WHERE email = @Identificador OR cpf = @Identificador";
                using (MySqlCommand cmdBlacklist = new MySqlCommand(sqlBlacklist, conn))
                {
                    cmdBlacklist.Parameters.AddWithValue("@Identificador", txtE_mail.Text.Trim());
                    int bloqueado = Convert.ToInt32(cmdBlacklist.ExecuteScalar());

                    if (bloqueado > 0)
                    {
                        MessageBox.Show("Acesso negado: Este usuário foi bloqueado do sistema.");
                        return; 
                    }
                }

                //  LOGIN NORMAL
                string sql = "SELECT TipoUsuario, CodigoAcesso, Nome FROM usuarios WHERE CodigoAcesso = @Codigo AND Senha = @Senha";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                { 
                    cmd.Parameters.AddWithValue("@Codigo", txtE_mail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Senha", txtSenha.Text.Trim());

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            tipoUsuario = dr["TipoUsuario"].ToString();
                            codigo = dr["CodigoAcesso"].ToString();
                            nome = dr["Nome"].ToString();
                        }
                    }
                }
            }

            if (tipoUsuario != null)
            {
                MessageBox.Show($"Bem-vindo(a), {nome}!");
                this.Hide();

                PainelAdmin tela = new PainelAdmin(codigo);
                tela.ShowDialog();

                this.Show();
                txtSenha.Clear(); 
            }
            else
            {
                MessageBox.Show("Login inválido. Verifique suas credenciais.");
                txtSenha.Clear();
                txtSenha.Focus();
            }
        }

        private void txtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Se pressionar Enter (código ASCII 13), clica no botão Entrar
            if (e.KeyChar == (char)13)
            {
                btnEntrar_Click(btnEntrar, e);
            }
        }
    }
}
