using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace RR_DoulaEFuroHumanizado
{
    public partial class PainelAdmin : Form
    {
        public string codigoUsuario;
        private string nomeUsuario;
        private string tipoUsuario;

        DataTable tabelaAgendamentos = new DataTable();
        bool atualizandoPainel = false;

        public PainelAdmin(string codigo)
        {
            InitializeComponent();

            codigoUsuario = codigo;
            dgvPainelAdm_Agendamentos.AutoGenerateColumns = true;

            txtPainelAdm_Nome.TextChanged += Filtro_TextChanged;
            txtPainelAdm_Email.TextChanged += Filtro_TextChanged;
            mskPainelAdm_Telefone.TextChanged += Filtro_TextChanged;
            mskPainelAdm_CPF.TextChanged += Filtro_TextChanged;
        }

        private void PainelAdmin_Load(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                {
                    conn.Open();

                    // Ajustado para coincidir com o banco (TipoUsuario, CodigoAcesso)
                    string sql = @"
                        SELECT TipoUsuario, CodigoAcesso, Nome
                        FROM usuarios
                        WHERE CodigoAcesso = @codigo";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@codigo", codigoUsuario);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tipoUsuario = reader["TipoUsuario"].ToString();
                            nomeUsuario = reader["Nome"].ToString();
                            string codigo = reader["CodigoAcesso"].ToString();

                            lblPainelAdmin_Logado.Text = $"{tipoUsuario}\n{nomeUsuario}";
                        }
                        else
                        {
                            lblPainelAdmin_Logado.Text = "👤 Usuário não encontrado";
                        }
                    }
                }

                ConfigurarPermissoes();
                CarregarHorariosAdmin();
                CarregarAgendamentos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar painel: " + ex.Message);
            }
        }

        private void ConfigurarPermissoes()
        {

        }

        private void CarregarAgendamentos()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                {
                    conn.Open();

                    string sql = @"
    SELECT 
        A.Id AS agendamentoid, 
        MAX(S.Id) AS itemservicoid, 
        C.Nome, 
        C.Email, 
        C.Telefone, 
        C.CPF, 
        IFNULL(C.Status,'ATIVO') AS statususuario, 
        IFNULL(S.Tipo, 'Sem Agendamento') AS Tipo, 
        IFNULL(S.Servico, 'Apenas Cadastro') AS Servico, 
        S.Data, 
        IFNULL(S.Horario, '--') AS Horario, 
        COUNT(S.Id) AS QuantidadePessoas, 
        IFNULL(SUM(S.Valor), 0) AS Valor, 
        IFNULL(S.Status, '--') AS Status,
        IF(S.Status = 'CANCELADO' OR S.Status IS NULL, '--', IFNULL(S.Comparecimento, 'PENDENTE')) AS Comparecimento 
    FROM clientes C 
    LEFT JOIN agendamentos A ON C.Id = A.ClienteId 
    LEFT JOIN agendamento_servicos S ON A.Id = S.AgendamentoId 
    GROUP BY 
        A.Id, C.Id, C.Nome, C.Email, C.Telefone, C.CPF, IFNULL(C.Status,'ATIVO'), 
        S.Tipo, S.Servico, S.Data, S.Horario, S.Status, S.Comparecimento 
    ORDER BY S.Data DESC, C.Nome ASC";

                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);

                    tabelaAgendamentos = new DataTable();
                    da.Fill(tabelaAgendamentos);

                    dgvPainelAdm_Agendamentos.DataSource = null;
                    dgvPainelAdm_Agendamentos.DataSource = tabelaAgendamentos;

                    if (dgvPainelAdm_Agendamentos.Columns.Contains("itemservicoid"))
                        dgvPainelAdm_Agendamentos.Columns["itemservicoid"].Visible = false;

                    dgvPainelAdm_Agendamentos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvPainelAdm_Agendamentos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvPainelAdm_Agendamentos.MultiSelect = false;
                    dgvPainelAdm_Agendamentos.ReadOnly = true;
                    dgvPainelAdm_Agendamentos.AllowUserToAddRows = false;

                    dgvPainelAdm_Agendamentos.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar agendamentos: " + ex.Message);
            }
        }

        private string PegarValorLinha(DataGridViewRow row, string nomeColuna)
        {
            if (row == null) return "";

            if (row.DataBoundItem is DataRowView drv)
            {
                if (drv.Row.Table.Columns.Contains(nomeColuna))
                    return drv[nomeColuna]?.ToString() ?? "";
            }
            return "";
        }

        private void Filtro_TextChanged(object sender, EventArgs e)
        {
            if (atualizandoPainel) return;

            try
            {
                string filtro = "";

                if (!string.IsNullOrWhiteSpace(txtPainelAdm_Nome.Text))
                    filtro += $"Nome LIKE '%{txtPainelAdm_Nome.Text.Replace("'", "''")}%'";

                if (!string.IsNullOrWhiteSpace(txtPainelAdm_Email.Text))
                    filtro += (filtro != "" ? " AND " : "") + $"Email LIKE '%{txtPainelAdm_Email.Text.Replace("'", "''")}%'";

                if (!string.IsNullOrWhiteSpace(mskPainelAdm_Telefone.Text))
                    filtro += (filtro != "" ? " AND " : "") + $"Telefone LIKE '%{mskPainelAdm_Telefone.Text.Replace("'", "''")}%'";

                if (!string.IsNullOrWhiteSpace(mskPainelAdm_CPF.Text))
                    filtro += (filtro != "" ? " AND " : "") + $"CPF LIKE '%{mskPainelAdm_CPF.Text.Replace("'", "''")}%'";

                tabelaAgendamentos.DefaultView.RowFilter = filtro;
            }
            catch { }
        }

        private void dgvPainelAdm_Agendamentos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvPainelAdm_Agendamentos.Rows[e.RowIndex];

            txtPainelAdm_Nome.Text = PegarValorLinha(row, "Nome");
            txtPainelAdm_Email.Text = PegarValorLinha(row, "Email");
            mskPainelAdm_Telefone.Text = PegarValorLinha(row, "Telefone");
            mskPainelAdm_CPF.Text = PegarValorLinha(row, "CPF");

            cbbPainelAdm_NovoHorario.Text = PegarValorLinha(row, "Horario");

            string dataTexto = PegarValorLinha(row, "Data");

            if (DateTime.TryParse(dataTexto, out DateTime dataSelecionada))
                dtpPainelAdm_NovaData.Value = dataSelecionada;
        }

        private void dgvPainelAdm_Agendamentos_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvPainelAdm_Agendamentos.Rows[e.RowIndex];

            // Pega os valores das colunas e joga para maiúsculo para evitar erros
            string statusItem = PegarValorLinha(row, "Status").ToUpper();
            string comparecimento = PegarValorLinha(row, "Comparecimento").ToUpper();
            string statusUsuario = PegarValorLinha(row, "statususuario").ToUpper();

            // USUÁRIO BLOQUEADO
            // Se estiver bloqueado, a linha toda fica vermelha e o código para por aqui.
            if (statusUsuario == "BLOQUEADO" || statusUsuario == "INATIVO")
            {
                row.DefaultCellStyle.BackColor = Color.MistyRose; // Fundo avermelhado
                row.DefaultCellStyle.ForeColor = Color.DarkRed;   // Letra vermelha
                return;
            }
            else
            {
                // Garante que clientes normais tenham a letra preta padrão
                row.DefaultCellStyle.ForeColor = Color.Black;
            }

            // REGRAS DE CANCELAMENTO E PAGAMENTO (Apenas para ativos)
            if (statusItem == "CANCELADO")
            {
                row.DefaultCellStyle.BackColor = Color.LightCoral; // Vermelho claro (Reembolsado)
            }
            else if (statusItem == "PENDENTE")
            {
                row.DefaultCellStyle.BackColor = Color.Orange; // Laranja (Aguardando pagar o link)
            }
            // REGRAS DE PRESENÇA (Só aplica se o pagamento estiver ATIVO)
            else if (statusItem == "ATIVO")
            {
                if (comparecimento == "COMPARECEU")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen; // Verde
                }
                else if (comparecimento == "NÃO COMPARECEU" || comparecimento == "FALTOU")
                {
                    row.DefaultCellStyle.BackColor = Color.Salmon; // Vermelho
                }
                else // Se for PENDENTE ou vazio
                {
                    row.DefaultCellStyle.BackColor = Color.LightYellow; // Amarelo
                }
            }
        }

        private void CarregarHorariosAdmin()
        {
            cbbPainelAdm_NovoHorario.Items.Clear();
            string[] horarios = { "08:00", "09:00", "10:00", "11:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00" };
            cbbPainelAdm_NovoHorario.Items.AddRange(horarios);

            if (cbbPainelAdm_NovoHorario.Items.Count > 0)
                cbbPainelAdm_NovoHorario.SelectedIndex = 0;
        }

        private void btnPainelAdmin_Reagendar_Click(object sender, EventArgs e)
        {
            if (dgvPainelAdm_Agendamentos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um item na lista para reagendar.");
                return;
            }

            if (!AuthPopup.PedirSenhaAdmin()) return;

            try
            {
                DataGridViewRow row = dgvPainelAdm_Agendamentos.SelectedRows[0];

                string strId = PegarValorLinha(row, "agendamentoid");
                string tipoServico = PegarValorLinha(row, "Tipo");
                string strData = PegarValorLinha(row, "Data");
                string horarioAntigo = PegarValorLinha(row, "Horario");
                string strQtd = PegarValorLinha(row, "QuantidadePessoas");

                if (!int.TryParse(strId, out int agendamentoId))
                {
                    MessageBox.Show("Erro: ID do agendamento não encontrado.");
                    return;
                }

                if (!DateTime.TryParse(strData, out DateTime dataAntiga))
                {
                    MessageBox.Show("Erro: A data original está em um formato inválido.");
                    return;
                }

                if (!int.TryParse(strQtd, out int qtdTotalPessoas)) qtdTotalPessoas = 1;

                //  CAIXINHA DE PERGUNTA (SÓ APARECE SE TIVER MAIS DE 1 PESSOA) 
                int qtdParaMover = qtdTotalPessoas;
                if (qtdTotalPessoas > 1)
                {
                    string resposta = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Existem {qtdTotalPessoas} pessoas agendadas neste serviço e horário.\nQuantas pessoas você deseja reagendar para o novo horário?",
                        "Reagendamento Parcial",
                        qtdTotalPessoas.ToString());

                    if (string.IsNullOrWhiteSpace(resposta) || !int.TryParse(resposta, out qtdParaMover) || qtdParaMover <= 0 || qtdParaMover > qtdTotalPessoas)
                    {
                        MessageBox.Show("Quantidade inválida ou operação cancelada.");
                        return;
                    }
                }

                DateTime novaData = dtpPainelAdm_NovaData.Value.Date;
                string novoHorario = cbbPainelAdm_NovoHorario.Text;

                // TRAVA CONTRA VIAGEM NO TEMPO (Datas e Horários no passado)

                if (novaData < DateTime.Today)
                {
                    MessageBox.Show("Operação cancelada: Não é permitido reagendar para um dia que já passou.", "Data Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Para o código aqui!
                }

                // Se for hoje, precisa conferir se a hora já não passou
                if (novaData == DateTime.Today)
                {
                    if (TimeSpan.TryParse(novoHorario, out TimeSpan horaEscolhida))
                    {
                        // Pega a hora atual do computador
                        TimeSpan horaAtual = DateTime.Now.TimeOfDay;

                        if (horaEscolhida <= horaAtual)
                        {
                            MessageBox.Show("Operação cancelada: Este horário já passou no dia de hoje.", "Horário Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                {
                    conn.Open();

                    if (tipoServico == "Doula")
                    {
                        string sqlVerificar = "SELECT COUNT(*) FROM agendamento_servicos WHERE Tipo='Doula' AND Data=@data AND Horario=@horario AND Status<>'CANCELADO' AND AgendamentoId<>@agendamentoId";
                        using (MySqlCommand cmdV = new MySqlCommand(sqlVerificar, conn))
                        {
                            cmdV.Parameters.AddWithValue("@data", novaData);
                            cmdV.Parameters.AddWithValue("@horario", novoHorario);
                            cmdV.Parameters.AddWithValue("@agendamentoId", agendamentoId);
                            if (Convert.ToInt32(cmdV.ExecuteScalar()) > 0)
                            {
                                MessageBox.Show("Esse horário já está ocupado por outra Doula.");
                                return;
                            }
                        }
                    }
                    else if (tipoServico == "Furo")
                    {
                        string sqlVerificarFuro = "SELECT COUNT(*) FROM agendamento_servicos WHERE Tipo='Furo' AND Data=@data AND Horario=@horario AND Status<>'CANCELADO' AND AgendamentoId<>@agendamentoId";
                        using (MySqlCommand cmdVF = new MySqlCommand(sqlVerificarFuro, conn))
                        {
                            cmdVF.Parameters.AddWithValue("@data", novaData);
                            cmdVF.Parameters.AddWithValue("@horario", novoHorario);
                            cmdVF.Parameters.AddWithValue("@agendamentoId", agendamentoId);

                            int ocupadasNoNovoHorario = Convert.ToInt32(cmdVF.ExecuteScalar());

                            if ((ocupadasNoNovoHorario + qtdParaMover) > 3)
                            {
                                MessageBox.Show($"Capacidade máxima! O novo horário possui apenas {3 - ocupadasNoNovoHorario} vaga(s) livre(s), mas você está tentando mover {qtdParaMover} pessoa(s).");
                                return;
                            }
                        }
                    }

                    // Move apenas a quantidade que o usuário digitou usando o LIMIT
                    string sql = $"UPDATE agendamento_servicos SET Data=@novaData, Horario=@novoHorario WHERE AgendamentoId=@agendamentoId AND Tipo=@tipo AND Data=@dataAntiga AND Horario=@horarioAntigo AND Status='ATIVO' LIMIT {qtdParaMover}";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@novaData", novaData);
                        cmd.Parameters.AddWithValue("@novoHorario", novoHorario);
                        cmd.Parameters.AddWithValue("@agendamentoId", agendamentoId);
                        cmd.Parameters.AddWithValue("@tipo", tipoServico);
                        cmd.Parameters.AddWithValue("@dataAntiga", dataAntiga);
                        cmd.Parameters.AddWithValue("@horarioAntigo", horarioAntigo);
                        cmd.ExecuteNonQuery();
                    }

                    string sqlZerarAvisos = "UPDATE agendamentos SET Notificacao24hEnviada = 0, Notificacao1hEnviada = 0 WHERE Id = @agendamentoId";
                    using (MySqlCommand cmdReset = new MySqlCommand(sqlZerarAvisos, conn))
                    {
                        cmdReset.Parameters.AddWithValue("@agendamentoId", agendamentoId);
                        cmdReset.ExecuteNonQuery();
                    }
                }

                //  INÍCIO DO ENVIO DE E-MAIL (REAGENDAMENTO) 
                try
                {
                    string emailCliente = PegarValorLinha(row, "Email");
                    string nomeCliente = PegarValorLinha(row, "Nome");
                    EmailService emailService = CriarEmailService();

                    string assuntoCliente = "Aviso de Reagendamento de Serviço";
                    string corpoCliente = $@"Olá, {nomeCliente}!

O seu agendamento ({qtdParaMover} pessoa(s) no serviço de {tipoServico}) foi reagendado com sucesso pela nossa equipe.

Nova Data: {novaData:dd/MM/yyyy}
Novo Horário: {novoHorario}

Qualquer dúvida, estamos à disposição!";
                    emailService.EnviarEmail(emailCliente, assuntoCliente, corpoCliente);

                    string assuntoDono = $"🔄 REAGENDAMENTO: {nomeCliente}";
                    string corpoDono = $@"Atenção, equipe! 
Um serviço foi reagendado através do Painel Admin.

Cliente: {nomeCliente} ({emailCliente})
Quantidade alterada: {qtdParaMover} pessoa(s)
Serviço: {tipoServico}
Nova Data: {novaData:dd/MM/yyyy} às {novoHorario}";
                    emailService.EnviarEmail("projetodoulaefuro01@gmail.com", assuntoDono, corpoDono);
                }
                catch { }
                //  FIM DO ENVIO DE E-MAIL 

                MessageBox.Show($"Agendamento de {tipoServico} reagendado com sucesso para {novaData:dd/MM/yyyy} às {novoHorario}!");
                CarregarAgendamentos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao reagendar: " + ex.Message);
            }
        }

        private void btnPainelAdmin_Reembolsar_Click(object sender, EventArgs e)
        {
            if (dgvPainelAdm_Agendamentos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um item na lista antes de reembolsar.");
                return;
            }

            if (!AuthPopup.PedirSenhaAdmin()) return;

            try
            {
                DataGridViewRow row = dgvPainelAdm_Agendamentos.SelectedRows[0];

                int agendamentoId = Convert.ToInt32(PegarValorLinha(row, "agendamentoid"));
                string tipoServico = PegarValorLinha(row, "Tipo");
                DateTime dataOriginal = Convert.ToDateTime(PegarValorLinha(row, "Data"));
                string horarioOriginal = PegarValorLinha(row, "Horario");
                string strQtd = PegarValorLinha(row, "QuantidadePessoas");

                if (!int.TryParse(strQtd, out int qtdTotalPessoas)) qtdTotalPessoas = 1;

                //  CAIXINHA DE PERGUNTA (SÓ APARECE SE TIVER MAIS DE 1 PESSOA) 
                int qtdParaCancelar = qtdTotalPessoas;
                if (qtdTotalPessoas > 1)
                {
                    string resposta = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Existem {qtdTotalPessoas} pessoas agendadas neste serviço e horário.\nQuantas pessoas você deseja CANCELAR/REEMBOLSAR?",
                        "Cancelamento Parcial",
                        qtdTotalPessoas.ToString());

                    if (string.IsNullOrWhiteSpace(resposta) || !int.TryParse(resposta, out qtdParaCancelar) || qtdParaCancelar <= 0 || qtdParaCancelar > qtdTotalPessoas)
                    {
                        MessageBox.Show("Quantidade inválida ou operação cancelada.");
                        return;
                    }
                }

                string emailCliente = PegarValorLinha(row, "Email");
                string nomeCliente = PegarValorLinha(row, "Nome");

                using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                {
                    conn.Open();

                    // Cancela apenas a quantidade digitada usando o LIMIT
                    string sql = $"UPDATE agendamento_servicos SET Status='CANCELADO' WHERE AgendamentoId=@agId AND Tipo=@tipo AND Data=@data AND Horario=@horario AND Status='ATIVO' LIMIT {qtdParaCancelar}";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@agId", agendamentoId);
                        cmd.Parameters.AddWithValue("@tipo", tipoServico);
                        cmd.Parameters.AddWithValue("@data", dataOriginal.Date);
                        cmd.Parameters.AddWithValue("@horario", horarioOriginal);
                        cmd.ExecuteNonQuery();
                    }
                }

                //  INÍCIO DO ENVIO DE E-MAIL (CANCELAMENTO) 
                try
                {
                    EmailService emailService = CriarEmailService();

                    string assuntoCliente = "Aviso de Cancelamento e Reembolso";
                    string corpoCliente = $@"Olá, {nomeCliente}.

Informamos que o agendamento de {qtdParaCancelar} pessoa(s) no serviço de {tipoServico} para o dia {dataOriginal:dd/MM/yyyy} foi cancelado e o processo de reembolso foi iniciado pela nossa equipe.

Lamentamos o imprevisto e esperamos ver você em breve.";
                    emailService.EnviarEmail(emailCliente, assuntoCliente, corpoCliente);

                    string assuntoDono = $"❌ CANCELAMENTO/REEMBOLSO: {nomeCliente}";
                    string corpoDono = $@"Atenção, equipe!

Um serviço foi CANCELADO e marcado para reembolso no Painel Admin.

Cliente: {nomeCliente} ({emailCliente})
Quantidade Cancelada: {qtdParaCancelar} pessoa(s)
Serviço: {tipoServico}
Data que estava marcada: {dataOriginal:dd/MM/yyyy}";
                    emailService.EnviarEmail("projetodoulaefuro01@gmail.com", assuntoDono, corpoDono);
                }
                catch { }
                //  FIM DO ENVIO DE E-MAIL 

                MessageBox.Show("Operação autorizada e serviço atualizado com sucesso!");
                CarregarAgendamentos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao reembolsar: " + ex.Message);
            }
        }

        private void AtualizarStatusUsuario(string status)
        {
            try
            {
                if (dgvPainelAdm_Agendamentos.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Selecione um usuário.");
                    return;
                }

                DataGridViewRow row = dgvPainelAdm_Agendamentos.SelectedRows[0];
                string emailUsuario = PegarValorLinha(row, "Email");

                using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                {
                    conn.Open();
                    string sql = "UPDATE usuarios SET Status=@status WHERE Email=@email";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@status", status.ToUpper());
                    cmd.Parameters.AddWithValue("@email", emailUsuario);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Status atualizado com sucesso!");
                CarregarAgendamentos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void btnPainelAdmin_Atualizar_Click(object sender, EventArgs e)
        {
            try
            {
                atualizandoPainel = true;

                txtPainelAdm_Nome.Clear();
                txtPainelAdm_Email.Clear();
                mskPainelAdm_Telefone.Clear();
                mskPainelAdm_CPF.Clear();

                CarregarHorariosAdmin();
                dtpPainelAdm_NovaData.Value = DateTime.Today;

                CarregarAgendamentos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar painel: " + ex.Message);
            }
            finally
            {
                atualizandoPainel = false;
            }
        }

        private void btnPainelAdmin_Agendamento_Click(object sender, EventArgs e)
        {
            // Verifica se tem algum cliente selecionado no Grid
            if (dgvPainelAdm_Agendamentos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um cliente na lista para fazer o agendamento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataGridViewRow row = dgvPainelAdm_Agendamentos.SelectedRows[0];
                string emailCliente = PegarValorLinha(row, "Email");

                if (string.IsNullOrWhiteSpace(emailCliente))
                {
                    MessageBox.Show("O item selecionado não possui um e-mail válido.");
                    return;
                }

                long idClienteEncontrado = 0;

                //  Busca o ID oficial do cliente no banco de dados usando o e-mail dele
                using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                {
                    conn.Open();
                    string sqlId = "SELECT Id FROM clientes WHERE Email = @email";
                    using (MySqlCommand cmd = new MySqlCommand(sqlId, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", emailCliente);
                        object resultado = cmd.ExecuteScalar();

                        if (resultado != null)
                        {
                            idClienteEncontrado = Convert.ToInt64(resultado);
                        }
                        else
                        {
                            MessageBox.Show("Erro: Cliente não encontrado na tabela de clientes.");
                            return;
                        }
                    }
                }

                //  Abre a tela usando o construtor PERFEITO (Passa o ID exato e o Email da Cliente)
                PaginaAgendamentoDoula D = new PaginaAgendamentoDoula(idClienteEncontrado, emailCliente);
                D.ShowDialog();

                //  Quando a tela fecha, atualiza o painel
                CarregarAgendamentos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir a tela de agendamento: " + ex.Message);
            }
        }

        private void btnPainelAdmin_Cadastrar_Funcionarios_Click(object sender, EventArgs e)
        {
            //  Pede a senha para garantir a segurança
            if (!AuthPopup.PedirSenhaAdmin()) return;

            //  O padrão é sempre funcionário
            string tipoParaCadastrar = "FUNCIONARIO";

            //  Se for o ADM principal, o sistema faz a pergunta
            if (tipoUsuario == "ADM")
            {
                DialogResult escolha = MessageBox.Show(
                    "Você deseja cadastrar um novo SUB-ADM?\n\n" +
                    "• Clique em [SIM] para criar um Sub-Adm.\n" +
                    "• Clique em [NÃO] para criar um Funcionário comum.",
                    "Escolha o Cargo",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                // Se ele cancelar ou fechar a janela, aborta a ação
                if (escolha == DialogResult.Cancel) return;

                // Se ele disser SIM, mudamos a variável para criar um Sub-Adm
                if (escolha == DialogResult.Yes)
                {
                    tipoParaCadastrar = "SUBADM";
                }
            }

            try
            {
                //  Abre a tela de cadastro já sabendo quem vai ser salvo!
                TelaDeCadastro telaCadastro = new TelaDeCadastro(tipoParaCadastrar);
                telaCadastro.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir tela de cadastro: " + ex.Message);
            }
        }

        private void btnPainelAdmin_Servicos_Click(object sender, EventArgs e)
        {
            TelaDeServicos t = new TelaDeServicos();
            t.ShowDialog();

            this.Show();

            CarregarAgendamentos();
        }

        private void btnPainelAdmin_Deletar_Usuario_Click(object sender, EventArgs e)
        {
            // Verifica se tem algum usuário selecionado na tabela
            if (dgvPainelAdm_Agendamentos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um usuário na lista primeiro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Extrai os dados do usuário da linha selecionada
            DataGridViewRow row = dgvPainelAdm_Agendamentos.SelectedRows[0];

            string nomeUsuario = PegarValorLinha(row, "Nome");
            string cpfUsuario = PegarValorLinha(row, "CPF");
            string emailUsuario = PegarValorLinha(row, "Email");

            if (string.IsNullOrWhiteSpace(cpfUsuario))
            {
                MessageBox.Show("O usuário selecionado não possui CPF válido para bloqueio.");
                return;
            }

            // VERIFICA SE O USUÁRIO JÁ ESTÁ BLOQUEADO (Usando o CPF único)
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                {
                    conn.Open();
                    string checkQuery = "SELECT Status FROM clientes WHERE CPF = @CPF";
                    using (MySqlCommand cmdCheck = new MySqlCommand(checkQuery, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@CPF", cpfUsuario);
                        object result = cmdCheck.ExecuteScalar();

                        // Se o resultado já for BLOQUEADO, avisa e encerra!
                        if (result != null && result.ToString().ToUpper() == "BLOQUEADO")
                        {
                            MessageBox.Show("Ação cancelada: Este usuário já se encontra BLOQUEADO no sistema.", "Usuário já bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar status do usuário: " + ex.Message);
                return;
            }

            // Confirmação dupla para evitar cliques acidentais
            DialogResult confirmacao = MessageBox.Show(
                $"Tem certeza que deseja BLOQUEAR permanentemente o usuário:\n\n{nomeUsuario}?\n\nEle não poderá mais acessar o sistema ou fazer agendamentos.",
                "Confirmação de Bloqueio (Lista Negra)",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmacao != DialogResult.Yes) return;

            // Barreira de Segurança: Pede a senha do ADM
            if (!AuthPopup.PedirSenhaAdmin()) return;

            // Salva na Blacklist e Altera o Status
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                {
                    conn.Open();

                    // Insere na Blacklist ignorando se já existir
                    string queryBlacklist = "INSERT IGNORE INTO blacklist (nome, cpf, email) VALUES (@Nome, @CPF, @Email)";
                    using (MySqlCommand cmd = new MySqlCommand(queryBlacklist, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nome", nomeUsuario);
                        cmd.Parameters.AddWithValue("@CPF", cpfUsuario);
                        cmd.Parameters.AddWithValue("@Email", emailUsuario);
                        cmd.ExecuteNonQuery();
                    }

                    // MUDA O STATUS PARA A PALAVRA CORRETA DO SEU BANCO E USA O CPF
                    string queryUpdate = "UPDATE clientes SET Status = 'BLOQUEADO' WHERE CPF = @CPF";
                    using (MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@CPF", cpfUsuario);
                        cmdUpdate.ExecuteNonQuery();
                    }

                    // Cancela os serviços pendentes apenas deste CPF exato
                    string queryCancelaServicos = @"
                UPDATE agendamento_servicos S 
                INNER JOIN agendamentos A ON S.AgendamentoId = A.Id 
                INNER JOIN clientes C ON A.ClienteId = C.Id 
                SET S.Status = 'CANCELADO' 
                WHERE C.CPF = @CPF AND S.Data >= CURDATE()";

                    using (MySqlCommand cmdCancela = new MySqlCommand(queryCancelaServicos, conn))
                    {
                        cmdCancela.Parameters.AddWithValue("@CPF", cpfUsuario);
                        cmdCancela.ExecuteNonQuery();
                    }

                    MessageBox.Show("Usuário banido e enviado para a Lista Negra com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Atualiza a tabela do painel
                    CarregarAgendamentos();

                    // Tira a seleção azul para a tela ficar limpa
                    dgvPainelAdm_Agendamentos.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao bloquear usuário: " + ex.Message);
            }
        }

        private void btnPainelAdmin_Sair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPainelAdmin_Novo_Agendamento_Click(object sender, EventArgs e)
        {
            CadastroCliente telaCadastro = new CadastroCliente();

           

            if (telaCadastro.ShowDialog() == DialogResult.OK)
            {
                string email = telaCadastro.EmailDoClienteSalvo;

                PaginaAgendamentoDoula telaAgendamento = new PaginaAgendamentoDoula(email);
                telaAgendamento.ShowDialog();
            }

            this.Show();
        }
        private EmailService CriarEmailService()
        {
            return new EmailService(
                "smtp.gmail.com",
                587,
                "projetodoulaefuro01@gmail.com",
                "qvxmylkwzrgqtiee",
                "Sistema Doula"
            );
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }

}