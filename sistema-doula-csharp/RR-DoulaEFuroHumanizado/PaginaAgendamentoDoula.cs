using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RR_DoulaEFuroHumanizado
{
    public partial class PaginaAgendamentoDoula : Form
    {
        private string emailDoUsuario;
        private long idDoCliente;

        public int? IdReagendamento = null;   // Id do item em agendamento_servicos
        public int? AgendamentoIdPai = null;  // Id do Agendamento pai

        private Agendamento agendamentoExistente;
        private bool modoReagendamento = false;

        private DateTime dataSelecionada = DateTime.MinValue;

        private List<string> horariosSelecionadosDoula = new List<string>();
        private Dictionary<string, int> quantidadeFuroPorHorario = new Dictionary<string, int>();

        private readonly ToolTip toolTip = new ToolTip();

        private int capacidadeMaximaFuro = 3;

        private List<string> horariosDoula = new List<string>
        {
            "08:00", "09:00", "10:00", "11:00", "12:00", "18:00"
        };

        private List<string> horariosFuro = new List<string>
        {
            "14:00", "15:00", "16:00", "17:00"
        };

        private string nomeDoCliente;
        private string emailDoCliente;
        private string telefoneDoCliente;
        private string cpfDoCliente;
        private long _idClienteSelecionado;

        private const string EmailSistema = "projetodoulaefuro01@gmail.com";
        private const string SenhaAppEmail = "qvxmylkwzrgqtiee";
        private const string NomeRemetente = "Sistema Doula";

        public PaginaAgendamentoDoula(long idCliente, string email)
        {
            InitializeComponent();

            idDoCliente = idCliente;
            emailDoUsuario = email;
            dataSelecionada = DateTime.MinValue;
        }

        public PaginaAgendamentoDoula(Agendamento agendamento, string email)
        {
            InitializeComponent();

            emailDoUsuario = email;
            agendamentoExistente = agendamento;
            modoReagendamento = true;

            IdReagendamento = agendamento.ItemServicoId;
            AgendamentoIdPai = agendamento.Id;
            dataSelecionada = agendamento.Data;
        }

        public PaginaAgendamentoDoula(long idCliente)
        {
            InitializeComponent();
            _idClienteSelecionado = idCliente;
        }

        public PaginaAgendamentoDoula(string email)
        {
            InitializeComponent();
            emailDoUsuario = email;
            dataSelecionada = DateTime.MinValue;
        }

        public PaginaAgendamentoDoula(string emailFuncionario, string nomeCliente, string emailCliente, string telefoneCliente, string cpfCliente)
        {
            InitializeComponent();

            emailDoUsuario = emailFuncionario;
            dataSelecionada = DateTime.MinValue;

            nomeDoCliente = nomeCliente;
            emailDoCliente = emailCliente;
            telefoneDoCliente = telefoneCliente;
            cpfDoCliente = cpfCliente;
        }

        private void PaginaAgendamentoDoula_Load(object sender, EventArgs e)
        {
            monthCalendar1.MinDate = DateTime.Today;

            cbbQuantidadeFuro_Pessoas.Items.Clear();
            cbbQuantidadeFuro_Pessoas.Items.AddRange(new object[] { "1", "2", "3" });
            cbbQuantidadeFuro_Pessoas.SelectedIndex = 0;

            cbbQuantidadeFuro_Pessoas.SelectedIndexChanged += (s, ev) =>
            {
                int novaQuantidade = int.Parse(cbbQuantidadeFuro_Pessoas.SelectedItem.ToString());

                var horariosSelecionados = quantidadeFuroPorHorario.Keys.ToList();
                foreach (var h in horariosSelecionados)
                    quantidadeFuroPorHorario[h] = novaQuantidade;

                AtualizarValorTotal();
            };

            pnlAgendamentoDoula_Horarios.WrapContents = true;
            pnlAgendamentoDoula_Horarios.AutoScroll = true;

            pnlAgendamentoFuro_Horarios.WrapContents = true;
            pnlAgendamentoFuro_Horarios.AutoScroll = true;

            ccbAgendaDoula_ConsultaPreNatal.CheckedChanged += ccbAgendaDoula_CheckedChanged;
            ccbAgendaDoula_AcompanhamentoParto.CheckedChanged += ccbAgendaDoula_CheckedChanged;
            ccbAgendaDoula_PosParto.CheckedChanged += ccbAgendaDoula_CheckedChanged;
            ccbAgendaDoula_Amamentacao.CheckedChanged += ccbAgendaDoula_CheckedChanged;

            ccbAgendaFuro_Titanio.CheckedChanged += ccbAgendaDoula_CheckedChanged;
            ccbAgendaFuro_Aco.CheckedChanged += ccbAgendaDoula_CheckedChanged;
            ccbAgendaFuro_Ouro.CheckedChanged += ccbAgendaDoula_CheckedChanged;
            ccbAgendaFuro_Prata.CheckedChanged += ccbAgendaDoula_CheckedChanged;

            lblAgendaDoula_ValorTotal.ForeColor = Color.DarkGreen;
            lblAgendaDoula_ValorTotal.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            if (modoReagendamento && agendamentoExistente != null)
            {
                monthCalendar1.SetDate(agendamentoExistente.Data);

                ccbAgendaDoula_ConsultaPreNatal.Enabled = false;
                ccbAgendaDoula_AcompanhamentoParto.Enabled = false;
                ccbAgendaDoula_PosParto.Enabled = false;
                ccbAgendaDoula_Amamentacao.Enabled = false;

                ccbAgendaFuro_Titanio.Enabled = false;
                ccbAgendaFuro_Aco.Enabled = false;
                ccbAgendaFuro_Ouro.Enabled = false;
                ccbAgendaFuro_Prata.Enabled = false;

                if (agendamentoExistente.TipoItem == "Doula")
                {
                    string servico = (agendamentoExistente.ServicoItem ?? "").Trim().ToLower();

                    ccbAgendaDoula_ConsultaPreNatal.Checked = false;
                    ccbAgendaDoula_AcompanhamentoParto.Checked = false;
                    ccbAgendaDoula_PosParto.Checked = false;
                    ccbAgendaDoula_Amamentacao.Checked = false;

                    if (servico.Contains("consulta"))
                        ccbAgendaDoula_ConsultaPreNatal.Checked = true;
                    else if (servico.Contains("parto"))
                        ccbAgendaDoula_AcompanhamentoParto.Checked = true;
                    else if (servico.Contains("pós") || servico.Contains("pos"))
                        ccbAgendaDoula_PosParto.Checked = true;
                    else if (servico.Contains("amamentação") || servico.Contains("amamentacao"))
                        ccbAgendaDoula_Amamentacao.Checked = true;

                    horariosSelecionadosDoula.Clear();
                    horariosSelecionadosDoula.AddRange(agendamentoExistente.Horarios ?? new List<string>());
                }
                else
                {
                    if ((agendamentoExistente.ServicoItem ?? "").Contains("Titânio") || (agendamentoExistente.ServicoItem ?? "").Contains("Titanio"))
                        ccbAgendaFuro_Titanio.Checked = true;

                    if ((agendamentoExistente.ServicoItem ?? "").Contains("Aço") || (agendamentoExistente.ServicoItem ?? "").Contains("Aco"))
                        ccbAgendaFuro_Aco.Checked = true;

                    if ((agendamentoExistente.ServicoItem ?? "").Contains("Ouro"))
                        ccbAgendaFuro_Ouro.Checked = true;

                    if ((agendamentoExistente.ServicoItem ?? "").Contains("Prata"))
                        ccbAgendaFuro_Prata.Checked = true;

                    quantidadeFuroPorHorario.Clear();
                    foreach (var h in agendamentoExistente.Horarios ?? new List<string>())
                    {
                        quantidadeFuroPorHorario[h] = agendamentoExistente.QuantidadePessoas <= 0
                            ? 1
                            : agendamentoExistente.QuantidadePessoas;
                    }

                    string qtd = agendamentoExistente.QuantidadePessoas.ToString();
                    if (cbbQuantidadeFuro_Pessoas.Items.Contains(qtd))
                        cbbQuantidadeFuro_Pessoas.SelectedItem = qtd;
                }
            }

            CarregarHorarios();
            AtualizarValorTotal();
        }

        private void ccbAgendaDoula_CheckedChanged(object sender, EventArgs e)
        {
            AtualizarValorTotal();
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (e.Start.Date < DateTime.Today)
            {
                MessageBox.Show("Não é possível escolher datas passadas!");
                dataSelecionada = DateTime.MinValue;
                monthCalendar1.SetDate(DateTime.Today);
                return;
            }

            if (e.Start.DayOfWeek == DayOfWeek.Sunday)
            {
                MessageBox.Show("Não atendemos aos domingos!");
                dataSelecionada = DateTime.MinValue;
                monthCalendar1.SetDate(DateTime.Today);
                return;
            }

            dataSelecionada = e.Start.Date;

            horariosSelecionadosDoula.Clear();
            quantidadeFuroPorHorario.Clear();

            CarregarHorarios();
            AtualizarValorTotal();
        }

        private void CarregarHorarios()
        {
            pnlAgendamentoDoula_Horarios.Controls.Clear();
            pnlAgendamentoFuro_Horarios.Controls.Clear();

            foreach (var hora in horariosDoula)
                pnlAgendamentoDoula_Horarios.Controls.Add(CriarBotaoHorario(hora, true));

            foreach (var hora in horariosFuro)
                pnlAgendamentoFuro_Horarios.Controls.Add(CriarBotaoHorario(hora, false));
        }

        private Button CriarBotaoHorario(string hora, bool isDoula)
        {
            Button btn = new Button
            {
                Width = 55,
                Height = 32,
                Margin = new Padding(3),
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Text = hora
            };

            if (dataSelecionada == DateTime.MinValue)
            {
                btn.Enabled = false;
                btn.BackColor = Color.LightGray;
                toolTip.SetToolTip(btn, "Escolha uma data primeiro");
                return btn;
            }

            DateTime horaBotao = DateTime.Parse(hora);
            DateTime dataHoraBotao = dataSelecionada.Date
                .AddHours(horaBotao.Hour)
                .AddMinutes(horaBotao.Minute);

            if (dataSelecionada.Date == DateTime.Today && dataHoraBotao <= DateTime.Now)
            {
                btn.Enabled = false;
                btn.BackColor = Color.LightGray;
                toolTip.SetToolTip(btn, "Horário já passou");
                return btn;
            }

            if (isDoula)
            {
                bool ocupado = ExisteAgendamentoDoulaNoHorario(dataSelecionada, hora);

                if (modoReagendamento && agendamentoExistente != null &&
                    agendamentoExistente.TipoItem == "Doula" &&
                    agendamentoExistente.Data.Date == dataSelecionada.Date &&
                    agendamentoExistente.Horarios != null &&
                    agendamentoExistente.Horarios.Contains(hora))
                {
                    ocupado = false;
                }

                if (ocupado)
                {
                    btn.Enabled = false;
                    btn.BackColor = Color.LightGray;
                    toolTip.SetToolTip(btn, "Horário ocupado para doula");
                    return btn;
                }

                toolTip.SetToolTip(btn, "Disponível para doula (1 pessoa)");

                if (horariosSelecionadosDoula.Contains(hora))
                    btn.BackColor = Color.LightPink;

                btn.Click += (s, e) =>
                {
                    if (!horariosSelecionadosDoula.Contains(hora))
                    {
                        horariosSelecionadosDoula.Add(hora);
                        btn.BackColor = Color.LightPink;
                    }
                    else
                    {
                        horariosSelecionadosDoula.Remove(hora);
                        btn.BackColor = SystemColors.Control;
                    }

                    AtualizarValorTotal();
                };
            }
            else
            {
                int vagasOcupadas = ObterQuantidadeFuroNoHorario(dataSelecionada, hora);

                if (modoReagendamento && agendamentoExistente != null &&
                    agendamentoExistente.TipoItem == "Furo" &&
                    agendamentoExistente.Data.Date == dataSelecionada.Date &&
                    agendamentoExistente.Horarios != null &&
                    agendamentoExistente.Horarios.Contains(hora))
                {
                    vagasOcupadas -= Math.Max(1, agendamentoExistente.QuantidadePessoas);
                    if (vagasOcupadas < 0)
                        vagasOcupadas = 0;
                }

                int disponivel = capacidadeMaximaFuro - vagasOcupadas;
                toolTip.SetToolTip(btn, $"Vagas disponíveis: {disponivel}/{capacidadeMaximaFuro}");

                if (disponivel <= 0)
                {
                    btn.Enabled = false;
                    btn.BackColor = Color.LightGray;
                    return btn;
                }

                if (quantidadeFuroPorHorario.ContainsKey(hora))
                    btn.BackColor = Color.LightGreen;

                btn.Click += (s, e) =>
                {
                    int quantidade = int.Parse(cbbQuantidadeFuro_Pessoas.SelectedItem.ToString());

                    if (!quantidadeFuroPorHorario.ContainsKey(hora))
                    {
                        if (quantidade > disponivel)
                        {
                            MessageBox.Show($"Só restam {disponivel} vaga(s) nesse horário!");
                            return;
                        }

                        quantidadeFuroPorHorario[hora] = quantidade;
                        btn.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        quantidadeFuroPorHorario.Remove(hora);
                        btn.BackColor = SystemColors.Control;
                    }

                    AtualizarValorTotal();
                };
            }

            return btn;
        }

        private bool ExisteAgendamentoDoulaNoHorario(DateTime data, string horario)
        {
            using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
            {
                conn.Open();

                string sql = @"
SELECT COUNT(*)
FROM agendamento_servicos
WHERE Data = @Data
  AND Horario = @Horario
  AND Tipo = 'Doula'
  AND Status = 'Ativo'";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Data", data.Date);
                    cmd.Parameters.AddWithValue("@Horario", horario);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        private int ObterQuantidadeFuroNoHorario(DateTime data, string horario)
        {
            using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
            {
                conn.Open();

                string sql = @"
SELECT COUNT(*)
FROM agendamento_servicos
WHERE Data = @Data
  AND Horario = @Horario
  AND Tipo = 'Furo'
  AND Status = 'Ativo'";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Data", data.Date);
                    cmd.Parameters.AddWithValue("@Horario", horario);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private decimal CalcularValorServicos()
        {
            decimal totalDoula = 0;
            if (ccbAgendaDoula_ConsultaPreNatal.Checked) totalDoula += 100;
            if (ccbAgendaDoula_AcompanhamentoParto.Checked) totalDoula += 300;
            if (ccbAgendaDoula_PosParto.Checked) totalDoula += 150;
            if (ccbAgendaDoula_Amamentacao.Checked) totalDoula += 120;

            if (horariosSelecionadosDoula != null && horariosSelecionadosDoula.Count > 0)
            {
                totalDoula *= horariosSelecionadosDoula.Count;
            }

            decimal totalFuro = 0;
            if (ccbAgendaFuro_Titanio.Checked) totalFuro += 80;
            if (ccbAgendaFuro_Aco.Checked) totalFuro += 60;
            if (ccbAgendaFuro_Ouro.Checked) totalFuro += 150;
            if (ccbAgendaFuro_Prata.Checked) totalFuro += 100;

            return totalDoula + totalFuro;
        }

        private void AtualizarValorTotal()
        {
            lblAgendaDoula_ValorTotal.Text = $"Total: R$ {CalcularValorServicos():N2}";
        }

        private List<string> ObterServicosSelecionados()
        {
            List<string> servicos = new List<string>();

            if (ccbAgendaDoula_ConsultaPreNatal.Checked) servicos.Add("Consulta pré-natal");
            if (ccbAgendaDoula_AcompanhamentoParto.Checked) servicos.Add("Acompanhamento parto");
            if (ccbAgendaDoula_PosParto.Checked) servicos.Add("Pós-parto");
            if (ccbAgendaDoula_Amamentacao.Checked) servicos.Add("Amamentação");

            if (ccbAgendaFuro_Titanio.Checked) servicos.Add("Titânio");
            if (ccbAgendaFuro_Aco.Checked) servicos.Add("Aço");
            if (ccbAgendaFuro_Ouro.Checked) servicos.Add("Ouro");
            if (ccbAgendaFuro_Prata.Checked) servicos.Add("Prata");

            return servicos;
        }

        private EmailService CriarEmailService()
        {
            return new EmailService(
                "smtp.gmail.com",
                587,
                EmailSistema,
                SenhaAppEmail,
                NomeRemetente
            );
        }

        private int SalvarAgendamentoCabecalho(
    string email,
    DateTime data,
    decimal total,
    int quantidadePessoasFuro,
    string nomeCompanheiro,
    string nomeBebe,
    string localParto,
    DateTime dpp,
    string equipeMedica)
        {
            using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
            {
                conn.Open();

                if (!modoReagendamento)
                {
                    string queryUpdateCliente = @"
                UPDATE clientes SET 
                    NomeCompanheiro = @NomeCompanheiro, 
                    NomeBebe = @NomeBebe, 
                    LocalParto = @LocalParto, 
                    DPP = @DPP, 
                    EquipeMedica = @EquipeMedica
                WHERE Id = @ClienteId";

                    using (MySqlCommand cmdCliente = new MySqlCommand(queryUpdateCliente, conn))
                    {
                        cmdCliente.Parameters.AddWithValue("@ClienteId", idDoCliente);

                        cmdCliente.Parameters.AddWithValue("@NomeCompanheiro", string.IsNullOrWhiteSpace(nomeCompanheiro) ? (object)DBNull.Value : nomeCompanheiro);
                        cmdCliente.Parameters.AddWithValue("@NomeBebe", string.IsNullOrWhiteSpace(nomeBebe) ? (object)DBNull.Value : nomeBebe);
                        cmdCliente.Parameters.AddWithValue("@LocalParto", string.IsNullOrWhiteSpace(localParto) ? (object)DBNull.Value : localParto);
                        cmdCliente.Parameters.AddWithValue("@DPP", dpp == DateTime.MinValue ? (object)DBNull.Value : dpp.Date);
                        cmdCliente.Parameters.AddWithValue("@EquipeMedica", string.IsNullOrWhiteSpace(equipeMedica) ? (object)DBNull.Value : equipeMedica);

                        cmdCliente.ExecuteNonQuery();
                    }
                }

                string primeiroHorario = horariosSelecionadosDoula.FirstOrDefault() ?? quantidadeFuroPorHorario.Keys.FirstOrDefault() ?? "00:00";

                string queryAgendamento = @"
            INSERT INTO agendamentos
            (ClienteId, DataAgendamento, HoraAgendamento, QuantidadePessoas, ValorTotal, StatusPagamento, StatusServico)
            VALUES
            (@ClienteId, @DataAgendamento, @HoraAgendamento, @QuantidadePessoas, @ValorTotal, 'PENDENTE', 'AGENDADO');
            SELECT LAST_INSERT_ID();";

                using (MySqlCommand cmdAgendamento = new MySqlCommand(queryAgendamento, conn))
                {
                    cmdAgendamento.Parameters.AddWithValue("@ClienteId", idDoCliente);
                    cmdAgendamento.Parameters.AddWithValue("@DataAgendamento", data.Date);
                    cmdAgendamento.Parameters.AddWithValue("@HoraAgendamento", primeiroHorario);
                    cmdAgendamento.Parameters.AddWithValue("@QuantidadePessoas", quantidadePessoasFuro == 0 ? 1 : quantidadePessoasFuro);
                    cmdAgendamento.Parameters.AddWithValue("@ValorTotal", total);

                    return Convert.ToInt32(cmdAgendamento.ExecuteScalar());
                }
            }
        }

        private void InserirItemServico(MySqlConnection conn, int agendamentoId, DateTime data, string tipo, string horario, string servico, decimal valor)
        {
            string sql = @"
INSERT INTO agendamento_servicos
(AgendamentoId, Data, Tipo, Horario, Servico, Status, Valor)
VALUES
(@AgendamentoId, @Data, @Tipo, @Horario, @Servico, @Status, @Valor)";

            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@AgendamentoId", agendamentoId);
                cmd.Parameters.AddWithValue("@Data", data.Date);
                cmd.Parameters.AddWithValue("@Tipo", tipo);
                cmd.Parameters.AddWithValue("@Horario", horario);
                cmd.Parameters.AddWithValue("@Servico", servico);
                cmd.Parameters.AddWithValue("@Status", "Ativo");
                cmd.Parameters.AddWithValue("@Valor", valor);
                cmd.ExecuteNonQuery();
            }
        }

        private void SalvarItensAgendamento(int agendamentoId)
        {
            using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
            {
                conn.Open();

                foreach (string horario in horariosSelecionadosDoula)
                {
                    if (ccbAgendaDoula_ConsultaPreNatal.Checked)
                        InserirItemServico(conn, agendamentoId, dataSelecionada, "Doula", horario, "Consulta pré-natal", 100);

                    if (ccbAgendaDoula_AcompanhamentoParto.Checked)
                        InserirItemServico(conn, agendamentoId, dataSelecionada, "Doula", horario, "Acompanhamento parto", 300);

                    if (ccbAgendaDoula_PosParto.Checked)
                        InserirItemServico(conn, agendamentoId, dataSelecionada, "Doula", horario, "Pós-parto", 150);

                    if (ccbAgendaDoula_Amamentacao.Checked)
                        InserirItemServico(conn, agendamentoId, dataSelecionada, "Doula", horario, "Amamentação", 120);
                }

                foreach (var item in quantidadeFuroPorHorario)
                {
                    string horario = item.Key;
                    int quantidade = item.Value;

                    if (ccbAgendaFuro_Titanio.Checked)
                        for (int i = 0; i < quantidade; i++)
                            InserirItemServico(conn, agendamentoId, dataSelecionada, "Furo", horario, "Titânio", 80);

                    if (ccbAgendaFuro_Aco.Checked)
                        for (int i = 0; i < quantidade; i++)
                            InserirItemServico(conn, agendamentoId, dataSelecionada, "Furo", horario, "Aço", 60);

                    if (ccbAgendaFuro_Ouro.Checked)
                        for (int i = 0; i < quantidade; i++)
                            InserirItemServico(conn, agendamentoId, dataSelecionada, "Furo", horario, "Ouro", 150);

                    if (ccbAgendaFuro_Prata.Checked)
                        for (int i = 0; i < quantidade; i++)
                            InserirItemServico(conn, agendamentoId, dataSelecionada, "Furo", horario, "Prata", 100);
                }
            }
        }

        private void AtualizarResumoAgendamentoPai(int agendamentoId)
        {
            using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
            {
                conn.Open();

                string sqlUpdate = @"
            UPDATE agendamentos 
            SET ValorTotal = IFNULL((SELECT SUM(Valor) FROM agendamento_servicos WHERE AgendamentoId = @Id AND Status = 'ATIVO'), 0),
                StatusServico = IF((SELECT COUNT(*) FROM agendamento_servicos WHERE AgendamentoId = @Id AND Status = 'ATIVO') > 0, 'REMARCADO', 'CANCELADO')
            WHERE Id = @Id";

                using (MySqlCommand cmd = new MySqlCommand(sqlUpdate, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", agendamentoId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ResetarFlagsNotificacao(int agendamentoId)
        {
            using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
            {
                conn.Open();

                string sql = "DELETE FROM notificacoes WHERE AgendamentoId = @Id AND Status = 'PENDENTE'";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", agendamentoId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void EnviarConfirmacaoDeAgendamento(
            int idAgendamento,
            string emailCliente,
            DateTime data,
            IEnumerable<string> horarios,
            IEnumerable<string> servicos,
            decimal total,
            bool reagendamento)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(emailCliente))
                    return;

                EmailService emailService = CriarEmailService();

                string assunto = reagendamento
                    ? "Seu agendamento foi reagendado com sucesso"
                    : "Confirmação do seu agendamento";

                string corpo = $@"Olá!

{(reagendamento ? "Seu agendamento foi reagendado com sucesso." : "Seu agendamento foi reservado! Para finalizá-lo, confirme o pagamento abaixo.")}

Número do agendamento: {idAgendamento}
Data: {data:dd/MM/yyyy}
Horário(s): {string.Join(", ", horarios.Distinct())}
Serviço(s): {string.Join(", ", servicos.Distinct())}
Valor total: R$ {total:N2}

{(reagendamento ? "" : $"👉 CLIQUE NO LINK ABAIXO PARA CONFIRMAR SEU PAGAMENTO:\nhttp://localhost:8080/pagar?id={idAgendamento}\n\n")}

Obrigada por agendar conosco!
Sistema Doula";
                emailService.EnviarEmail(emailCliente, assunto, corpo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Agendamento salvo, mas houve erro ao enviar o e-mail:\n" + ex.Message);
            }
        }

        private void EnviarEmailAvisoProprietario(
            int idAgendamento,
            string emailCliente,
            DateTime data,
            IEnumerable<string> horarios,
            IEnumerable<string> servicos,
            decimal total)
        {
            try
            {
                EmailService emailService = CriarEmailService();

                string emailProprietario = EmailSistema;

                string assunto = $"🚨 NOVO AGENDAMENTO: {emailCliente} - {data:dd/MM/yyyy}";

                string corpo = $@"Olá, equipe!

Um novo pagamento foi aprovado e o agendamento foi confirmado no sistema.

👤 DADOS DO CLIENTE:
E-mail: {emailCliente}
(Acesse o painel para ver o telefone e as informações médicas)

📅 RESUMO DO ATENDIMENTO:
Número do Agendamento: {idAgendamento}
Data: {data:dd/MM/yyyy}
Horário(s): {string.Join(", ", horarios.Distinct())}
Serviço(s): {string.Join(", ", servicos.Distinct())}
Valor pago: R$ {total:N2}

Bom trabalho!
Sistema Doula Automático";

                emailService.EnviarEmail(emailProprietario, assunto, corpo);
            }
            catch
            {
            }
        }

        private void EnviarEmailAgendamentoConcluido(
            int idAgendamento,
            string emailCliente,
            DateTime data,
            IEnumerable<string> horarios,
            IEnumerable<string> servicos,
            decimal total)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(emailCliente))
                    return;

                EmailService emailService = CriarEmailService();

                string assunto = "Pagamento Confirmado! Seu agendamento está garantido ✅";

                string corpo = $@"Olá!

Recebemos o seu pagamento e o seu agendamento está 100% confirmado em nosso sistema! 🎉

Resumo do seu agendamento:
Número: {idAgendamento}
Data: {data:dd/MM/yyyy}
Horário(s): {string.Join(", ", horarios.Distinct())}
Serviço(s): {string.Join(", ", servicos.Distinct())}
Valor pago: R$ {total:N2}

Qualquer dúvida, estamos à disposição.
Nos vemos em breve!

Com carinho,
Equipe Doula e Furo Humanizado";

                emailService.EnviarEmail(emailCliente, assunto, corpo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pagamento confirmado no sistema, mas houve um erro ao enviar o e-mail de recibo para o cliente:\n" + ex.Message);
            }
        }

        private bool MostrarResumoConfirmacao(
            List<string> servicos,
            string nomeCompanheiro,
            string nomeBebe,
            string localParto,
            DateTime dpp,
            string equipeMedica)
        {
            string resumo = $"Data: {dataSelecionada:dd/MM/yyyy}\n\n";

            if (horariosSelecionadosDoula.Count > 0)
            {
                resumo += "Horários Doula:\n";
                foreach (var h in horariosSelecionadosDoula)
                    resumo += $"- {h} (1 pessoa)\n";
                resumo += "\n";
            }

            if (quantidadeFuroPorHorario.Count > 0)
            {
                resumo += "Furo:\n";
                foreach (var h in quantidadeFuroPorHorario)
                    resumo += $"- {h.Key} ({h.Value} pessoa(s))\n";
                resumo += "\n";
            }

            resumo += "Serviços:\n";
            foreach (var s in servicos)
                resumo += $"- {s}\n";

            if (horariosSelecionadosDoula.Count > 0)
            {
                resumo += "\nInformações adicionais:\n";
                resumo += $"- Companheiro: {nomeCompanheiro}\n";
                resumo += $"- Bebê: {nomeBebe}\n";
                resumo += $"- Local do parto: {localParto}\n";
                resumo += $"- DPP: {(dpp == DateTime.MinValue ? "-" : dpp.ToString("dd/MM/yyyy"))}\n";
                resumo += $"- Equipe médica: {equipeMedica}\n";
            }

            resumo += $"\nTotal: R$ {CalcularValorServicos():N2}";

            var resultado = MessageBox.Show(
                resumo + "\n\nDeseja confirmar?",
                "Resumo do Agendamento",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information
            );

            return resultado == DialogResult.Yes;
        }

        private void btnDoula_Agendar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataSelecionada == DateTime.MinValue)
                {
                    MessageBox.Show("Escolha uma data!");
                    return;
                }

                if (dataSelecionada.Date < DateTime.Today)
                {
                    MessageBox.Show("Não é possível agendar em datas passadas!");
                    return;
                }

                if (dataSelecionada.DayOfWeek == DayOfWeek.Sunday)
                {
                    MessageBox.Show("Não atendemos aos domingos!");
                    return;
                }

                bool temHorarioDoula = horariosSelecionadosDoula.Count > 0;
                bool temHorarioFuro = quantidadeFuroPorHorario.Count > 0;

                if (!temHorarioDoula && !temHorarioFuro)
                {
                    MessageBox.Show("Escolha pelo menos um horário!");
                    return;
                }

                List<string> servicos = ObterServicosSelecionados();

                if (modoReagendamento && agendamentoExistente != null && servicos.Count == 0)
                {
                    if (!string.IsNullOrWhiteSpace(agendamentoExistente.ServicoItem))
                        servicos.Add(agendamentoExistente.ServicoItem);
                }

                if (servicos.Count == 0)
                {
                    MessageBox.Show("Selecione pelo menos um serviço!");
                    return;
                }

                bool servicoDoulaSelecionado =
                    ccbAgendaDoula_ConsultaPreNatal.Checked ||
                    ccbAgendaDoula_AcompanhamentoParto.Checked ||
                    ccbAgendaDoula_PosParto.Checked ||
                    ccbAgendaDoula_Amamentacao.Checked;

                bool servicoFuroSelecionado =
                    ccbAgendaFuro_Titanio.Checked ||
                    ccbAgendaFuro_Aco.Checked ||
                    ccbAgendaFuro_Ouro.Checked ||
                    ccbAgendaFuro_Prata.Checked;

                // --- INÍCIO DA VALIDAÇÃO BIDIRECIONAL CRUZADA ---
                if (temHorarioDoula && !servicoDoulaSelecionado)
                {
                    MessageBox.Show("Você escolheu um horário na agenda da Doula, mas não marcou qual serviço ela fará!");
                    return;
                }
                if (servicoDoulaSelecionado && !temHorarioDoula)
                {
                    MessageBox.Show("Você marcou um serviço de Doula, mas não escolheu nenhum horário para ela!");
                    return;
                }

                if (temHorarioFuro && !servicoFuroSelecionado)
                {
                    MessageBox.Show("Você escolheu um horário para o Furo, mas não escolheu a joia!");
                    return;
                }
                if (servicoFuroSelecionado && !temHorarioFuro)
                {
                    MessageBox.Show("Você marcou uma joia de Furo, mas não escolheu nenhum horário para o Furo!");
                    return;
                }
                // --- FIM DA VALIDAÇÃO BIDIRECIONAL ---

                // TRAVA DE SEGURANÇA: VERIFICAÇÃO DE VAGAS NO BANCO DE DADOS
                if (temHorarioFuro && !modoReagendamento)
                {
                    using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                    {
                        conn.Open();
                        foreach (var item in quantidadeFuroPorHorario)
                        {
                            string horarioVerificado = item.Key;
                            int qtdDesejada = item.Value;

                            string sqlVagas = "SELECT COUNT(*) FROM agendamento_servicos WHERE Tipo='Furo' AND Data=@data AND Horario=@horario AND Status<>'CANCELADO'";
                            using (MySqlCommand cmdVagas = new MySqlCommand(sqlVagas, conn))
                            {
                                cmdVagas.Parameters.AddWithValue("@data", dataSelecionada.Date);
                                cmdVagas.Parameters.AddWithValue("@horario", horarioVerificado);

                                int vagasOcupadas = Convert.ToInt32(cmdVagas.ExecuteScalar());
                                int vagasLivres = 3 - vagasOcupadas;

                                if (qtdDesejada > vagasLivres)
                                {
                                    MessageBox.Show($"Capacidade excedida! O horário das {horarioVerificado} possui apenas {vagasLivres} vaga(s) disponível(is), mas você está tentando agendar {qtdDesejada} pessoa(s).", "Falta de Vagas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return; // Encerra tudo antes de cobrar
                                }
                            }
                        }
                    }
                }

                int quantidadePessoasFuro = quantidadeFuroPorHorario.Values.Sum();

                string nomeCompanheiro = "";
                string nomeBebe = "";
                string localParto = "";
                DateTime dpp = DateTime.MinValue;
                string equipeMedica = "";

                if (!modoReagendamento && temHorarioDoula)
                {
                    MessageBox.Show("A quantidade de pessoas se refere somente ao furo. A doula atende apenas 1 pessoa por horário.");
                    MessageBox.Show("A doula oferece suporte emocional, físico e informativo, mas não realiza procedimentos médicos.");

                    nomeCompanheiro = PerguntaObrigatoria("Nome do companheiro:");
                    if (nomeCompanheiro == null) return;

                    nomeBebe = PerguntaObrigatoria("Nome do bebê:");
                    if (nomeBebe == null) return;

                    localParto = PerguntaObrigatoria("Local do parto:");
                    if (localParto == null) return;

                    while (true)
                    {
                        string input = Prompt("Data provável do parto (dd/mm/aaaa):");

                        if (string.IsNullOrWhiteSpace(input))
                        {
                            var sair = MessageBox.Show("Deseja cancelar o preenchimento?", "Cancelar", MessageBoxButtons.YesNo);
                            if (sair == DialogResult.Yes) return;
                            continue;
                        }

                        if (DateTime.TryParse(input, out dpp) && dpp >= DateTime.Today)
                            break;

                        MessageBox.Show("Digite uma data válida e futura!");
                    }

                    equipeMedica = PerguntaObrigatoriaLivre("Equipe médica / obstetra:");
                    if (equipeMedica == null) return;
                }

                if (!MostrarResumoConfirmacao(servicos, nomeCompanheiro, nomeBebe, localParto, dpp, equipeMedica))
                    return;


                // MATEMÁTICA INTELIGENTE DO VALOR TOTAL (Por Joias/Serviços)
                decimal totalDoula = 0;
                if (ccbAgendaDoula_ConsultaPreNatal.Checked) totalDoula += 100;
                if (ccbAgendaDoula_AcompanhamentoParto.Checked) totalDoula += 300;
                if (ccbAgendaDoula_PosParto.Checked) totalDoula += 150;
                if (ccbAgendaDoula_Amamentacao.Checked) totalDoula += 120;
                totalDoula *= horariosSelecionadosDoula.Count;

                decimal totalFuroReal = 0;
                List<string> servicosFuroMarcados = new List<string>();
                if (ccbAgendaFuro_Titanio.Checked) servicosFuroMarcados.Add("Titânio");
                if (ccbAgendaFuro_Aco.Checked) servicosFuroMarcados.Add("Aço");
                if (ccbAgendaFuro_Ouro.Checked) servicosFuroMarcados.Add("Ouro");
                if (ccbAgendaFuro_Prata.Checked) servicosFuroMarcados.Add("Prata");

                if (quantidadePessoasFuro > 0 && servicosFuroMarcados.Count > 0)
                {
                    // REGRA : Tem mais pessoas do que opções marcadas (Ex: 3 pessoas e 2 brincos)
                    if (quantidadePessoasFuro > servicosFuroMarcados.Count)
                    {
                        int totalDistribuidos = 0;
                        foreach (string servico in servicosFuroMarcados)
                        {
                            string resposta = Microsoft.VisualBasic.Interaction.InputBox(
                                $"Temos {quantidadePessoasFuro} pessoas agendadas.\nQuantas escolheram a joia de {servico}?",
                                "Distribuição de Joias",
                                "1"
                            );

                            if (int.TryParse(resposta, out int qtd) && qtd > 0)
                            {
                                totalDistribuidos += qtd;

                                if (servico == "Titânio") totalFuroReal += 80 * qtd;
                                else if (servico == "Aço") totalFuroReal += 60 * qtd;
                                else if (servico == "Ouro") totalFuroReal += 150 * qtd;
                                else if (servico == "Prata") totalFuroReal += 100 * qtd;
                            }
                        }

                        // Trava de segurança da soma
                        if (totalDistribuidos != quantidadePessoasFuro)
                        {
                            MessageBox.Show($"A conta não bateu! Você agendou {quantidadePessoasFuro} pessoas, mas informou {totalDistribuidos} joias. Cancele e tente novamente.", "Erro de Soma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    // REGRA : 1 pessoa com 2 brincos, ou 2 pessoas com 2 brincos.
                    else
                    {
                        foreach (string servico in servicosFuroMarcados)
                        {
                            if (servico == "Titânio") totalFuroReal += 80;
                            else if (servico == "Aço") totalFuroReal += 60;
                            else if (servico == "Ouro") totalFuroReal += 150;
                            else if (servico == "Prata") totalFuroReal += 100;
                        }
                    }
                }

                decimal total = totalDoula + totalFuroReal;

                // lógica de reagendamento caso já exista um valor antigo
                if (modoReagendamento && agendamentoExistente != null && total <= 0)
                {
                    total = agendamentoExistente.ValorTotal;
                }

                // LÓGICA DE REAGENDAMENTO
                if (modoReagendamento && IdReagendamento.HasValue && AgendamentoIdPai.HasValue && agendamentoExistente != null)
                {
                    using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                    {
                        conn.Open();

                        if (agendamentoExistente.TipoItem == "Doula")
                        {
                            string novoHorario = horariosSelecionadosDoula.FirstOrDefault();

                            if (string.IsNullOrWhiteSpace(novoHorario))
                            {
                                MessageBox.Show("Selecione o novo horário da doula!");
                                return;
                            }

                            string sql = @"
                UPDATE agendamento_servicos
                SET Data = @Data, Horario = @Horario, Status = 'Ativo'
                WHERE Id = @Id";

                            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@Data", dataSelecionada.Date);
                                cmd.Parameters.AddWithValue("@Horario", novoHorario);
                                cmd.Parameters.AddWithValue("@Id", IdReagendamento.Value);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string novoHorario = quantidadeFuroPorHorario.Keys.FirstOrDefault();
                            int novaQuantidade = quantidadeFuroPorHorario.Values.FirstOrDefault();

                            if (string.IsNullOrWhiteSpace(novoHorario))
                            {
                                MessageBox.Show("Selecione o novo horário do furo!");
                                return;
                            }

                            string servicoAtual = agendamentoExistente.ServicoItem;
                            string horarioAntigo = agendamentoExistente.Horarios?.FirstOrDefault();
                            int quantidadeAntiga = agendamentoExistente.QuantidadePessoas;

                            if (string.IsNullOrWhiteSpace(servicoAtual) || string.IsNullOrWhiteSpace(horarioAntigo))
                            {
                                MessageBox.Show("Não foi possível identificar o item original do furo.");
                                return;
                            }

                            List<int> idsParaAtualizar = new List<int>();

                            string sqlBuscarIds = @"
                SELECT Id FROM agendamento_servicos
                WHERE AgendamentoId = @AgendamentoId AND Tipo = 'Furo' AND Servico = @Servico
                  AND Data = @DataAntiga AND Horario = @HorarioAntigo AND Status = 'Ativo'
                LIMIT @Quantidade";

                            using (MySqlCommand cmdBuscar = new MySqlCommand(sqlBuscarIds, conn))
                            {
                                cmdBuscar.Parameters.AddWithValue("@AgendamentoId", AgendamentoIdPai.Value);
                                cmdBuscar.Parameters.AddWithValue("@Servico", servicoAtual);
                                cmdBuscar.Parameters.AddWithValue("@DataAntiga", agendamentoExistente.Data.Date);
                                cmdBuscar.Parameters.AddWithValue("@HorarioAntigo", horarioAntigo);
                                cmdBuscar.Parameters.AddWithValue("@Quantidade", quantidadeAntiga);

                                using (MySqlDataReader reader = cmdBuscar.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        idsParaAtualizar.Add(Convert.ToInt32(reader["Id"]));
                                    }
                                }
                            }

                            foreach (int idItem in idsParaAtualizar)
                            {
                                string sqlUpdate = "UPDATE agendamento_servicos SET Data = @NovaData, Horario = @NovoHorario WHERE Id = @Id";
                                using (MySqlCommand cmdUpdate = new MySqlCommand(sqlUpdate, conn))
                                {
                                    cmdUpdate.Parameters.AddWithValue("@NovaData", dataSelecionada.Date);
                                    cmdUpdate.Parameters.AddWithValue("@NovoHorario", novoHorario);
                                    cmdUpdate.Parameters.AddWithValue("@Id", idItem);
                                    cmdUpdate.ExecuteNonQuery();
                                }
                            }

                            if (novaQuantidade != quantidadeAntiga)
                            {
                                MessageBox.Show("No reagendamento do furo, a quantidade original foi mantida para evitar inconsistências.");
                            }
                        }
                    }

                    AtualizarResumoAgendamentoPai(AgendamentoIdPai.Value);
                    ResetarFlagsNotificacao(AgendamentoIdPai.Value);

                    string emailDestinoReagendamento = emailDoUsuario;

                    EnviarConfirmacaoDeAgendamento(
                        AgendamentoIdPai.Value,
                        emailDestinoReagendamento,
                        dataSelecionada,
                        horariosSelecionadosDoula.Concat(quantidadeFuroPorHorario.Keys),
                        servicos,
                        total,
                        true
                    );

                    MessageBox.Show("Reagendamento realizado com sucesso! 🔄");
                    LimparFormulario();
                    Close();
                    return;
                }
                // LÓGICA DE AGENDAMENTO NOVO E SIMULAÇÃO DE PAGAMENTO
                else
                {
                    TelaDePagamento telaPagamento = new TelaDePagamento(
                        dataSelecionada, string.Join(",", horariosSelecionadosDoula), string.Join(", ", servicos),
                        total, quantidadePessoasFuro, 0, nomeCompanheiro, nomeBebe, localParto, dpp, equipeMedica
                    );

                    if (telaPagamento.ShowDialog() == DialogResult.OK)
                    {
                        string formaEscolhida = telaPagamento.FormaEscolhida;

                        int idAgendamento = SalvarAgendamentoCabecalho(
                            emailDoUsuario, dataSelecionada, total, quantidadePessoasFuro,
                            nomeCompanheiro, nomeBebe, localParto, dpp, equipeMedica
                        );

                        if (idAgendamento > 0)
                        {
                            SalvarItensAgendamento(idAgendamento);
                            AtualizarResumoAgendamentoPai(idAgendamento);

                            string linkPagamento = $"http://localhost:8080/pagar?id={idAgendamento}";
                            string emailDestinoFinal = emailDoUsuario;

                            EnviarConfirmacaoDeAgendamento(idAgendamento, emailDestinoFinal, dataSelecionada, horariosSelecionadosDoula, servicos, total, false);

                            AguardandoPagamento telaEspera = new AguardandoPagamento(idAgendamento);
                            telaEspera.ShowDialog();

                            if (telaEspera.Sucesso)
                            {
                                MessageBox.Show($"O cliente clicou no link e o pagamento via {formaEscolhida} foi confirmado no sistema! ✅", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                EnviarEmailAgendamentoConcluido(idAgendamento, emailDestinoFinal, dataSelecionada, horariosSelecionadosDoula.Concat(quantidadeFuroPorHorario.Keys), servicos, total);
                                EnviarEmailAvisoProprietario(idAgendamento, emailDestinoFinal, dataSelecionada, horariosSelecionadosDoula.Concat(quantidadeFuroPorHorario.Keys), servicos, total);
                            }
                            else
                            {
                                MessageBox.Show("A tela de espera foi cancelada. O agendamento continuará PENDENTE no painel.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }

                LimparFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro inesperado:\n" + ex.Message);
            }
        }

        private void LimparFormulario()
        {
            horariosSelecionadosDoula.Clear();
            quantidadeFuroPorHorario.Clear();

            ccbAgendaDoula_ConsultaPreNatal.Checked = false;
            ccbAgendaDoula_AcompanhamentoParto.Checked = false;
            ccbAgendaDoula_PosParto.Checked = false;
            ccbAgendaDoula_Amamentacao.Checked = false;

            ccbAgendaFuro_Titanio.Checked = false;
            ccbAgendaFuro_Aco.Checked = false;
            ccbAgendaFuro_Ouro.Checked = false;
            ccbAgendaFuro_Prata.Checked = false;

            cbbQuantidadeFuro_Pessoas.SelectedIndex = 0;

            dataSelecionada = DateTime.MinValue;
            monthCalendar1.SetDate(DateTime.Today);

            lblAgendaDoula_ValorTotal.Text = "Total: R$ 0,00";

            pnlAgendamentoDoula_Horarios.Controls.Clear();
            pnlAgendamentoFuro_Horarios.Controls.Clear();

            CarregarHorarios();
        }

        private string Prompt(string texto)
        {
            return Microsoft.VisualBasic.Interaction.InputBox(texto, "Informação", "");
        }

        private string PerguntaObrigatoria(string pergunta)
        {
            while (true)
            {
                string resposta = Prompt(pergunta);

                if (string.IsNullOrWhiteSpace(resposta))
                {
                    var opcao = MessageBox.Show(
                        "O campo está vazio.\n\nDeseja cancelar o preenchimento?",
                        "Campo obrigatório",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (opcao == DialogResult.Yes)
                        return null;

                    continue;
                }

                if (resposta.Any(char.IsDigit))
                {
                    MessageBox.Show("Não é permitido usar números!");
                    continue;
                }

                return resposta;
            }
        }

        private string PerguntaObrigatoriaLivre(string pergunta)
        {
            while (true)
            {
                string resposta = Prompt(pergunta);

                if (string.IsNullOrWhiteSpace(resposta))
                {
                    var opcao = MessageBox.Show(
                        "O campo está vazio.\n\nDeseja cancelar o preenchimento?",
                        "Campo obrigatório",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (opcao == DialogResult.Yes)
                        return null;

                    continue;
                }

                return resposta;
            }
        }

        private void btnDoula_Limpar_Click(object sender, EventArgs e)
        {
            var confirmar = MessageBox.Show(
                "Deseja realmente limpar todo o agendamento?",
                "Limpar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmar == DialogResult.Yes)
                LimparFormulario();
        }

        private void btnDoula_Voltar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDoula_Historico_Click(object sender, EventArgs e)
        {
            TelaHistoricoDeAgendamento historico = new TelaHistoricoDeAgendamento(emailDoUsuario);
            Hide();
            historico.ShowDialog();
            Show();
        }
    }

    public class Agendamento
    {
        public int Id { get; set; }
        public int ItemServicoId { get; set; }

        public DateTime Data { get; set; }
        public List<string> Horarios { get; set; }
        public List<string> Servicos { get; set; }
        public decimal ValorTotal { get; set; }
        public int QuantidadePessoas { get; set; }

        public string NomeCompanheiro { get; set; }
        public string NomeBebe { get; set; }
        public string LocalParto { get; set; }
        public DateTime DPP { get; set; }
        public string EquipeMedica { get; set; }

        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }

        public string Status { get; set; }

        public string TipoItem { get; set; }
        public string ServicoItem { get; set; }
    }
}