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
            if (tipoUsuario != "ADM")
            {
                btnPainelAdmin_Reembolsar.Visible = false;
            }
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
        S.Id AS itemservicoid, 
        U.Nome, 
        U.Email, 
        U.Telefone, 
        U.CPF, 
        IFNULL(U.Status,'ATIVO') AS statususuario, 
        S.Tipo, 
        S.Servico, 
        S.Data, 
        S.Horario, 
        A.QuantidadePessoas, 
        S.Valor, 
        S.Status 
    FROM agendamento_servicos S 
    INNER JOIN agendamentos A ON A.Id = S.AgendamentoId 
    INNER JOIN usuarios U ON U.Id = A.ClienteId 
    ORDER BY S.Data DESC";

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

            // Forçando para maiúsculas para evitar erro de Case-Sensitive nas cores
            string statusItem = PegarValorLinha(row, "Status").ToUpper();
            string statusUsuario = PegarValorLinha(row, "statususuario").ToUpper();

            if (statusItem == "ATIVO")
                row.DefaultCellStyle.BackColor = Color.LightGreen;
            else if (statusItem == "CANCELADO")
                row.DefaultCellStyle.BackColor = Color.LightCoral;
            else if (statusItem == "PENDENTE")
                row.DefaultCellStyle.BackColor = Color.Orange;

            if (!string.IsNullOrWhiteSpace(statusUsuario) && dgvPainelAdm_Agendamentos.Columns.Contains("statususuario"))
            {
                if (statusUsuario == "INATIVO" || statusUsuario == "BLOQUEADO")
                    row.Cells["statususuario"].Style.ForeColor = Color.DarkRed;
                else
                    row.Cells["statususuario"].Style.ForeColor = Color.DarkBlue;
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

            // Acesso via popup de segurança
            if (!AuthPopup.PedirSenhaAdmin()) return;

            try
            {
                DataGridViewRow row = dgvPainelAdm_Agendamentos.SelectedRows[0];
                int idServico = Convert.ToInt32(PegarValorLinha(row, "itemservicoid"));
                string tipoServico = PegarValorLinha(row, "Tipo");
                DateTime novaData = dtpPainelAdm_NovaData.Value.Date;
                string novoHorario = cbbPainelAdm_NovoHorario.Text;

                using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                {
                    conn.Open();

                    if (tipoServico == "Doula")
                    {
                        string sqlVerificar = "SELECT COUNT(*) FROM agendamento_servicos WHERE Tipo='Doula' AND Data=@data AND Horario=@horario AND Status<>'CANCELADO' AND Id<>@id";
                        using (MySqlCommand cmdV = new MySqlCommand(sqlVerificar, conn))
                        {
                            cmdV.Parameters.AddWithValue("@data", novaData);
                            cmdV.Parameters.AddWithValue("@horario", novoHorario);
                            cmdV.Parameters.AddWithValue("@id", idServico);
                            if (Convert.ToInt32(cmdV.ExecuteScalar()) > 0)
                            {
                                MessageBox.Show("Esse horário já está ocupado pela Doula.");
                                return;
                            }
                        }
                    }
                    else if (tipoServico == "Furo")
                    {
                        string sqlVerificarFuro = "SELECT COUNT(*) FROM agendamento_servicos WHERE Tipo='Furo' AND Data=@data AND Horario=@horario AND Status<>'CANCELADO'";
                        using (MySqlCommand cmdVF = new MySqlCommand(sqlVerificarFuro, conn))
                        {
                            cmdVF.Parameters.AddWithValue("@data", novaData);
                            cmdVF.Parameters.AddWithValue("@horario", novoHorario);
                            int ocupadas = Convert.ToInt32(cmdVF.ExecuteScalar());

                            if (ocupadas >= 3)
                            {
                                MessageBox.Show("Capacidade máxima de 3 pessoas atingida para este horário de Furo.");
                                return;
                            }
                        }
                    }

                    string sql = "UPDATE agendamento_servicos SET Data=@data, Horario=@horario WHERE Id=@id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@data", novaData);
                        cmd.Parameters.AddWithValue("@horario", novoHorario);
                        cmd.Parameters.AddWithValue("@id", idServico);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Reagendado com sucesso!");
                CarregarAgendamentos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void btnPainelAdmin_Reembolsar_Click(object sender, EventArgs e)
        {
            //  Verifica se existe um item selecionado primeiro
            if (dgvPainelAdm_Agendamentos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um item na lista antes de reembolsar.");
                return;
            }

            //  Chama a tela de senha (Permite ADM e SUBADM conforme a regra do seu sistema)
            if (!AuthPopup.PedirSenhaAdmin())
            {
                return; // Ação cancelada ou senha incorreta
            }

            try
            {
                DataGridViewRow row = dgvPainelAdm_Agendamentos.SelectedRows[0];
                int idServico = Convert.ToInt32(PegarValorLinha(row, "itemservicoid"));

                using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                {
                    conn.Open();

                    // Cancela o serviço na tabela agendamento_servicos
                    string sql = "UPDATE agendamento_servicos SET Status='CANCELADO' WHERE Id=@id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idServico);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Reembolso autorizado e serviço cancelado com sucesso!");
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
            //  Verifica se tem algum cliente selecionado no Grid
            if (dgvPainelAdm_Agendamentos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um cliente na lista para fazer o agendamento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                //  Extrai os dados do cliente selecionado
                DataGridViewRow row = dgvPainelAdm_Agendamentos.SelectedRows[0];

                string nomeCliente = PegarValorLinha(row, "Nome");
                string emailCliente = PegarValorLinha(row, "Email");
                string telefoneCliente = PegarValorLinha(row, "Telefone");
                string cpfCliente = PegarValorLinha(row, "CPF");

                if (string.IsNullOrWhiteSpace(emailCliente))
                {
                    MessageBox.Show("O item selecionado não possui um e-mail válido.");
                    return;
                }

                //  Abre a tela passando os dados do cliente
                PaginaAgendamentoDoula D = new PaginaAgendamentoDoula(codigoUsuario, nomeCliente, emailCliente, telefoneCliente, cpfCliente);

                
                D.ShowDialog();

                //  Quando a tela fecha, essa linha executa e traz a nova compra para o painel!
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
                // 4. Abre a tela de cadastro já sabendo quem vai ser salvo!
                TelaDeCadastro telaCadastro = new TelaDeCadastro(tipoParaCadastrar);
                telaCadastro.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir tela de cadastro: " + ex.Message);
            }
        }

        private void btnPainelAdmin_Novo_Agendamento_Click(object sender, EventArgs e)
        {

        }

        private void btnPainelAdmin_Servicos_Click(object sender, EventArgs e)
        {
            TelaDeServicos t = new TelaDeServicos();
            t.ShowDialog();

            this.Show();
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

            if (string.IsNullOrWhiteSpace(cpfUsuario) && string.IsNullOrWhiteSpace(emailUsuario))
            {
                MessageBox.Show("O usuário selecionado não possui CPF ou Email válido para bloqueio.");
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

            //  Barreira de Segurança: Pede a senha do ADM
            if (!AuthPopup.PedirSenhaAdmin()) return;

            //  Salva na Blacklist no Banco de Dados
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                {
                    conn.Open();

                    // Insere o usuário na tabela blacklist
                    string queryBlacklist = "INSERT INTO blacklist (nome, cpf, email) VALUES (@Nome, @CPF, @Email)";

                    using (MySqlCommand cmd = new MySqlCommand(queryBlacklist, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nome", nomeUsuario);
                        cmd.Parameters.AddWithValue("@CPF", cpfUsuario);
                        cmd.Parameters.AddWithValue("@Email", emailUsuario);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Usuário banido e enviado para a Lista Negra com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Atualiza a tabela do painel
                    CarregarAgendamentos();
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
    }
}