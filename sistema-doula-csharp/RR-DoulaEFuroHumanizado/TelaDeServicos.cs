using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace RR_DoulaEFuroHumanizado
{
    public partial class TelaDeServicos : Form
    {
        public TelaDeServicos()
        {
            InitializeComponent();
        }

        private void btnTelaServical_Buscar_Click(object sender, EventArgs e)
        {
            // Verifica o que o funcionario escolheu no ComboBox
            string filtro = cbbTelaServical_Buscar.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(filtro))
            {
                MessageBox.Show("Por favor, selecione um período (dia, semana, mês, ano) primeiro.");
                return;
            }

            CarregarComanda(filtro);
        }

        private void CarregarComanda(string filtro)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                {
                    conn.Open();

                    // Lógica para montar o filtro de tempo no banco de dados
                    string condicaoTempo = "";

                    // Ajuste: Oculta os horários que já passaram no dia de hoje
                    if (filtro == "dia" || filtro == "hoje")
                        condicaoTempo = "DATE(S.Data) = CURDATE() AND S.Horario >= CURTIME()";
                    else if (filtro == "semana")
                        condicaoTempo = "YEARWEEK(S.Data, 1) = YEARWEEK(CURDATE(), 1)";
                    else if (filtro == "mes" || filtro == "mês")
                        condicaoTempo = "MONTH(S.Data) = MONTH(CURDATE()) AND YEAR(S.Data) = YEAR(CURDATE())";
                    else if (filtro == "ano")
                        condicaoTempo = "YEAR(S.Data) = YEAR(CURDATE())";
                    else
                        condicaoTempo = "1=1"; // Prevenção de erro: traz tudo

                    // =====================================================================
                    // SQL BLINDADO: Puxa o comparecimento, BARRA quem estiver bloqueado
                    // ou quem estiver dentro da tabela blacklist!
                    // =====================================================================
                    string sql = $@"
                SELECT 
                    S.Id, 
                    C.Nome AS Cliente, 
                    S.Servico, 
                    S.Data, 
                    S.Horario, 
                    IFNULL(S.Comparecimento, 'PENDENTE') AS Comparecimento 
                FROM agendamento_servicos S
                INNER JOIN agendamentos A ON A.Id = S.AgendamentoId
                INNER JOIN clientes C ON C.Id = A.ClienteId
                WHERE {condicaoTempo} 
                  AND S.Status = 'ATIVO' 
                  AND IFNULL(C.Status, 'ATIVO') <> 'BLOQUEADO' 
                  AND C.Email NOT IN (SELECT email FROM blacklist)
                ORDER BY S.Data ASC, S.Horario ASC";

                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                    DataTable tabela = new DataTable();
                    da.Fill(tabela);

                    dgvTelaServical_Comanda.DataSource = tabela;

                    // Esconde a coluna ID (ela serve só para o sistema saber quem atualizar)
                    if (dgvTelaServical_Comanda.Columns.Contains("Id"))
                        dgvTelaServical_Comanda.Columns["Id"].Visible = false;

                    // Transforma a coluna "Comparecimento" em uma coluna com Setinha (ComboBox)
                    if (dgvTelaServical_Comanda.Columns["Comparecimento"] != null && !(dgvTelaServical_Comanda.Columns["Comparecimento"] is DataGridViewComboBoxColumn))
                    {
                        int indexStatus = dgvTelaServical_Comanda.Columns["Comparecimento"].Index;
                        dgvTelaServical_Comanda.Columns.Remove("Comparecimento"); // Remove a coluna de texto

                        DataGridViewComboBoxColumn comboStatus = new DataGridViewComboBoxColumn();
                        comboStatus.Name = "Comparecimento";
                        comboStatus.DataPropertyName = "Comparecimento"; // Conecta com a coluna do banco
                        comboStatus.HeaderText = "Presença";

                        // Garante que o texto digitado seja exatamente igual ao do banco
                        comboStatus.Items.Add("PENDENTE");
                        comboStatus.Items.Add("COMPARECEU");
                        comboStatus.Items.Add("NÃO COMPARECEU");
                        comboStatus.FlatStyle = FlatStyle.Flat;

                        dgvTelaServical_Comanda.Columns.Insert(indexStatus, comboStatus); // Adiciona a coluna com setinha
                    }

                    dgvTelaServical_Comanda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Trava de segurança contra cliques acidentais (deixa a tabela desmarcada)
                    dgvTelaServical_Comanda.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar a comanda: " + ex.Message);
            }
        }

        private void dgvTelaServical_Comanda_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvTelaServical_Comanda.IsCurrentCellDirty && dgvTelaServical_Comanda.CurrentCell is DataGridViewComboBoxCell)
            {
                dgvTelaServical_Comanda.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvTelaServical_Comanda_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se a linha é válida e se a coluna alterada foi a de "Comparecimento"
            if (e.RowIndex >= 0 && dgvTelaServical_Comanda.Columns[e.ColumnIndex].Name == "Comparecimento")
            {
                // Pega o novo status escolhido e o ID escondido daquele serviço
                string novoStatus = dgvTelaServical_Comanda.Rows[e.RowIndex].Cells["Comparecimento"].Value.ToString();
                string idServico = dgvTelaServical_Comanda.Rows[e.RowIndex].Cells["Id"].Value.ToString();

                // Salva a alteração no banco de dados na coluna CORRETA
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                    {
                        conn.Open();
                        string queryUpdate = "UPDATE agendamento_servicos SET Comparecimento = @Comparecimento WHERE Id = @Id";

                        using (MySqlCommand cmd = new MySqlCommand(queryUpdate, conn))
                        {
                            cmd.Parameters.AddWithValue("@Comparecimento", novoStatus);
                            cmd.Parameters.AddWithValue("@Id", idServico);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao atualizar a presença: " + ex.Message);
                }
            }
        }
    }
}