using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace RR_DoulaEFuroHumanizado
{
    public partial class CadastroCliente : Form
    {
        public string EmailDoClienteSalvo { get; private set; }

        public CadastroCliente()
        {
            InitializeComponent();
        }

        private void txtCadastroCliente_Nome_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCadastroCliente_Nome.Text))
            {
                e.Cancel = true;
                Comfirmaçao_Cadastro_Cliente.SetError(txtCadastroCliente_Nome, "Nome é obrigatório!");
            }
            else
            {
                Comfirmaçao_Cadastro_Cliente.SetError(txtCadastroCliente_Nome, "");
            }
        }

        private void txtCadastroCliente_Idade_Validating(object sender, CancelEventArgs e)
        {
            if (!int.TryParse(txtCadastroCliente_Idade.Text, out int idade))
            {
                e.Cancel = true;
                Comfirmaçao_Cadastro_Cliente.SetError(txtCadastroCliente_Idade, "Idade inválida");
                return;
            }

            if (idade < 0)
            {
                e.Cancel = true;
                Comfirmaçao_Cadastro_Cliente.SetError(txtCadastroCliente_Idade, "Idade não pode ser negativa.");
                return;
            }

            if (idade < 18)
            {
                Comfirmaçao_Cadastro_Cliente.SetError(txtCadastroCliente_Idade,
                    "Por ser menor de idade , venha acompanhada(o) com os responsaveis.");
            }
            else
            {
                Comfirmaçao_Cadastro_Cliente.SetError(txtCadastroCliente_Idade, "Seja Bem-Vindo!");
            }
        }

        private void mskCadastroCliente_Telefone_Validating(object sender, CancelEventArgs e)
        {
            if (!mskCadastroCliente_Telefone.MaskFull)
            {
                e.Cancel = true;
                Comfirmaçao_Cadastro_Cliente.SetError(mskCadastroCliente_Telefone, "Telefone incompleto!");
            }
            else
            {
                Comfirmaçao_Cadastro_Cliente.SetError(mskCadastroCliente_Telefone, "");
            }
        }

        private void cbbCadastroCliente_eCivil_Validating(object sender, CancelEventArgs e)
        {
            if (cbbCadastroCliente_eCivil.SelectedIndex == -1)
            {
                e.Cancel = true;
                Comfirmaçao_Cadastro_Cliente.SetError(cbbCadastroCliente_eCivil, "Selecione o estado civil.");
            }
            else
            {
                Comfirmaçao_Cadastro_Cliente.SetError(cbbCadastroCliente_eCivil, "");
            }
        }

        private void mskCadastroCliente_CPF_Validating(object sender, CancelEventArgs e)
        {
            if (!mskCadastroCliente_CPF.MaskFull)
            {
                e.Cancel = true;
                Comfirmaçao_Cadastro_Cliente.SetError(mskCadastroCliente_CPF, "CPF incompleto!");
            }
            else
            {
                Comfirmaçao_Cadastro_Cliente.SetError(mskCadastroCliente_CPF, "");
            }
        }

        private void cbbCadastroCliente_Sexualidade_Validating(object sender, CancelEventArgs e)
        {
            if (cbbCadastroCliente_Sexualidade.SelectedIndex == -1)
            {
                e.Cancel = true;
                Comfirmaçao_Cadastro_Cliente.SetError(cbbCadastroCliente_Sexualidade, "Selecione uma opção");
            }
            else
            {
                Comfirmaçao_Cadastro_Cliente.SetError(cbbCadastroCliente_Sexualidade, "");
            }
        }

        private void txtCadastroCliente_Bairro_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCadastroCliente_Bairro.Text))
            {
                e.Cancel = true;
                Comfirmaçao_Cadastro_Cliente.SetError(txtCadastroCliente_Bairro, "Bairro é obrigatório.");
            }
            else
            {
                Comfirmaçao_Cadastro_Cliente.SetError(txtCadastroCliente_Bairro, "");
            }
        }

        private void cbbCadastroCliente_Nacionalidade_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbbCadastroCliente_Nacionalidade.Text))
            {
                e.Cancel = true;
                Comfirmaçao_Cadastro_Cliente.SetError(cbbCadastroCliente_Nacionalidade, "Campo obrigatório");
            }
            else
            {
                Comfirmaçao_Cadastro_Cliente.SetError(cbbCadastroCliente_Nacionalidade, "");
            }
        }


        private void mskCadastroCliente_CEP_Validating(object sender, CancelEventArgs e)
        {
            if (!mskCadastroCliente_CEP.MaskFull)
            {
                e.Cancel = true;
                Comfirmaçao_Cadastro_Cliente.SetError(mskCadastroCliente_CEP, "CEP incompleto!");
            }
            else
            {
                Comfirmaçao_Cadastro_Cliente.SetError(mskCadastroCliente_CEP, "");
            }
        }

        private void txtCadastroCliente_Email_Validating(object sender, CancelEventArgs e)
        {
            string email = txtCadastroCliente_Email.Text;

            if (string.IsNullOrWhiteSpace(email))
            {
                e.Cancel = true;
                Comfirmaçao_Cadastro_Cliente.SetError(txtCadastroCliente_Email, "Email obrigatório.");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                e.Cancel = true;
                Comfirmaçao_Cadastro_Cliente.SetError(txtCadastroCliente_Email, "Digite o email corretamente");
            }
            else
            {
                Comfirmaçao_Cadastro_Cliente.SetError(txtCadastroCliente_Email, "");
            }
        }

        private void btnCadastroCliente_Cadastrar_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios corretamente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string cpf = mskCadastroCliente_CPF.Text.Trim();
            string email = txtCadastroCliente_Email.Text.Trim();
            long idClienteGerado = 0; // Variável para guardar o ID do cliente novo

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
                {
                    conn.Open();

                    // VERIFICAÇÃO DA BLACKLIST (Lista Negra)
                    string queryBlacklist = "SELECT COUNT(*) FROM blacklist WHERE cpf = @CPF OR email = @Email";
                    using (MySqlCommand cmdBlack = new MySqlCommand(queryBlacklist, conn))
                    {
                        cmdBlack.Parameters.AddWithValue("@CPF", cpf);
                        cmdBlack.Parameters.AddWithValue("@Email", email);
                        int bloqueado = Convert.ToInt32(cmdBlack.ExecuteScalar());

                        if (bloqueado > 0)
                        {
                            MessageBox.Show("Cadastro não permitido: Este CPF ou E-mail encontra-se bloqueado no sistema por violação de termos.", "Acesso Negado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // VERIFICAÇÃO SE O CLIENTE JÁ ESTÁ CADASTRADO 
                    string queryExiste = "SELECT COUNT(*) FROM clientes WHERE CPF = @CPF OR Email = @Email";
                    using (MySqlCommand cmdExiste = new MySqlCommand(queryExiste, conn))
                    {
                        cmdExiste.Parameters.AddWithValue("@CPF", cpf);
                        cmdExiste.Parameters.AddWithValue("@Email", email);
                        int jaExiste = Convert.ToInt32(cmdExiste.ExecuteScalar());

                        if (jaExiste > 0)
                        {
                            MessageBox.Show("Este CPF ou E-mail já está cadastrado no sistema.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }


                    // INSERÇÃO DO NOVO CLIENTE NO BANCO (COM STATUS 'ATIVO')
                    string queryInsert = @"
                INSERT INTO clientes 
                (Nome, Idade, CPF, CEP, Bairro, Sexo, EstadoCivil, Nacionalidade, Email, Telefone, Status) 
                VALUES 
                (@Nome, @Idade, @CPF, @CEP, @Bairro, @Sexo, @EstadoCivil, @Nacionalidade, @Email, @Telefone, 'ATIVO')";

                    using (MySqlCommand cmdInsert = new MySqlCommand(queryInsert, conn))
                    {
                        cmdInsert.Parameters.AddWithValue("@Nome", txtCadastroCliente_Nome.Text.Trim());
                        cmdInsert.Parameters.AddWithValue("@Idade", Convert.ToInt32(txtCadastroCliente_Idade.Text.Trim()));
                        cmdInsert.Parameters.AddWithValue("@CPF", cpf);
                        cmdInsert.Parameters.AddWithValue("@CEP", mskCadastroCliente_CEP.Text.Trim());
                        cmdInsert.Parameters.AddWithValue("@Bairro", txtCadastroCliente_Bairro.Text.Trim());
                        cmdInsert.Parameters.AddWithValue("@Sexo", cbbCadastroCliente_Sexualidade.Text);
                        cmdInsert.Parameters.AddWithValue("@EstadoCivil", cbbCadastroCliente_eCivil.Text);
                        cmdInsert.Parameters.AddWithValue("@Nacionalidade", cbbCadastroCliente_Nacionalidade.Text);
                        cmdInsert.Parameters.AddWithValue("@Email", email);
                        cmdInsert.Parameters.AddWithValue("@Telefone", mskCadastroCliente_Telefone.Text.Trim());

                        cmdInsert.ExecuteNonQuery();

                        idClienteGerado = cmdInsert.LastInsertedId; //Captura o ID do cliente
                    }

                    MessageBox.Show("Cadastro de cliente realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    btnCadastroCliente_LimparTudo_Click(sender, e);
                }

                // Abre a tela de agendamento repassando o ID criado
                using (PaginaAgendamentoDoula Doula = new PaginaAgendamentoDoula(idClienteGerado, email))
                {
                    this.Hide();
                    Doula.ShowDialog();
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar cliente no banco de dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCadastroCliente_Voltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCadastroCliente_LimparTudo_Click(object sender, EventArgs e)
        {
            txtCadastroCliente_Nome.Clear();
            txtCadastroCliente_Idade.Clear();
            txtCadastroCliente_Bairro.Clear();
            txtCadastroCliente_Email.Clear();

            mskCadastroCliente_Telefone.Clear();
            mskCadastroCliente_CPF.Clear();
            mskCadastroCliente_CEP.Clear();

            cbbCadastroCliente_eCivil.SelectedIndex = -1;
            cbbCadastroCliente_Sexualidade.SelectedIndex = -1;
            cbbCadastroCliente_Nacionalidade.SelectedIndex = -1;
            cbbCadastroCliente_Nacionalidade.Text = "";

            Comfirmaçao_Cadastro_Cliente.Clear();
        }

    }
}
