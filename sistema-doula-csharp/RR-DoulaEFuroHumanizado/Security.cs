using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using RR_DoulaEFuroHumanizado;

public static class AuthPopup
{
    public static bool PedirSenhaAdmin()
    {
        bool autorizado = false;

        // O 'using' garante que o formulário será removido da memória ao fechar
        using (Form popup = new Form())
        {
            popup.Width = 300;
            popup.Height = 170;
            popup.Text = "Confirmação de Acesso";
            popup.StartPosition = FormStartPosition.CenterScreen;
            popup.FormBorderStyle = FormBorderStyle.FixedDialog;
            popup.MaximizeBox = false;
            popup.MinimizeBox = false;

            Label lbl = new Label() { Text = "Digite sua senha (ADM/SUBADM):", Left = 20, Top = 20, Width = 250 };
            TextBox txtSenha = new TextBox() { Left = 20, Top = 50, Width = 240, PasswordChar = '*' };
            Button btnOk = new Button() { Text = "Confirmar", Left = 20, Top = 85, Width = 100 };
            Button btnCancelar = new Button() { Text = "Cancelar", Left = 140, Top = 85, Width = 100 };

            // Atalhos de teclado (Enter confirma, Esc cancela)
            popup.AcceptButton = btnOk;
            popup.CancelButton = btnCancelar;

            int tentativas = 0;

            btnOk.Click += (s, e) =>
            {
                // Evita consulta no banco se o campo estiver vazio
                if (string.IsNullOrWhiteSpace(txtSenha.Text))
                {
                    MessageBox.Show("Por favor, digite a senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                    {
                        conn.Open();

                        string sql = @"
                            SELECT COUNT(*) 
                            FROM usuarios 
                            WHERE Senha = @Senha 
                            AND TipoUsuario IN ('ADM', 'SUBADM')
                            AND Status = 'ATIVO'";

                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@Senha", txtSenha.Text);
                            autorizado = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao conectar no banco de dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!autorizado)
                {
                    tentativas++;

                    if (tentativas >= 3)
                    {
                        MessageBox.Show("Acesso bloqueado. Número máximo de tentativas atingido.", "Acesso Negado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        popup.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Senha incorreta ou sem permissão! ({tentativas}/3)", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSenha.Clear(); // Limpa a senha errada
                        txtSenha.Focus(); // Coloca o cursor de volta na caixa de texto
                    }
                    return;
                }

                popup.Close();
            };

            btnCancelar.Click += (s, e) =>
            {
                autorizado = false;
                popup.Close();
            };

            // Foca na caixa de texto automaticamente quando o popup abre
            popup.Shown += (s, e) => txtSenha.Focus();

            popup.Controls.Add(lbl);
            popup.Controls.Add(txtSenha);
            popup.Controls.Add(btnOk);
            popup.Controls.Add(btnCancelar);

            popup.ShowDialog();
        }

        return autorizado;
    }
}