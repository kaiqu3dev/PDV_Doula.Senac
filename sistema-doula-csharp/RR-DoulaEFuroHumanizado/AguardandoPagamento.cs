using System;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace RR_DoulaEFuroHumanizado
{
    public class AguardandoPagamento : Form
    {
        public bool Sucesso { get; private set; } = false;
        private int _idAgendamento;
        private HttpListener _listener;
        private bool _escutando = true;

        public AguardandoPagamento(int idAgendamento)
        {
            _idAgendamento = idAgendamento;
            this.Text = "Aguardando E-mail...";
            this.Size = new Size(400, 150);
            this.StartPosition = FormStartPosition.CenterParent;

            Label lbl = new Label { Text = "E-mail enviado!\nAguardando o cliente clicar no link do e-mail...", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            this.Controls.Add(lbl);

            this.FormClosing += (s, e) => { _escutando = false; _listener?.Stop(); };
            IniciarServidorLocal();
        }

        private async void IniciarServidorLocal()
        {
            try
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add("http://localhost:8080/pagar/");
                _listener.Start();

                while (_escutando)
                {
                    var context = await _listener.GetContextAsync();
                    if (context.Request.QueryString["id"] == _idAgendamento.ToString())
                    {
                        using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                        {
                            conn.Open();
                            new MySqlCommand($"UPDATE agendamentos SET StatusPagamento = 'PAGO' WHERE Id = {_idAgendamento}", conn).ExecuteNonQuery();
                        }

                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("<html><body style='text-align:center; margin-top:50px; background-color:#d4edda; color:#155724;'><h1>Pagamento Confirmado! ✅</h1><p>Pode fechar esta tela.</p></body></html>");
                        context.Response.ContentLength64 = buffer.Length;
                        await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                        context.Response.OutputStream.Close();

                        Sucesso = true;
                        _escutando = false;
                        this.Invoke((MethodInvoker)delegate { this.Close(); });
                    }
                }
            }
            catch { }
        }
    }
}