using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace RR_DoulaEFuroHumanizado
{
    public partial class TelaDeCadastro : Form
    {
        public string emailDoUsuario;

        // Por padrão, quem for cadastrado aqui será um funcionário
        private string cargoEquipe = "FUNCIONARIO";

        public TelaDeCadastro()
        {
            InitializeComponent();
            this.Text = "Cadastro de Funcionário";
        }

        // Usado caso para cadastrar o sub-adm na mesma tela de cadastro
        public TelaDeCadastro(string cargo)
        {
            InitializeComponent();
            cargoEquipe = cargo; // Recebe "SUBADM" se vier da tela do adm principal
            this.Text = "Cadastro de " + cargoEquipe;
        }

        private void SalvarUsuario()
        {
            using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
            {
                conn.Open();

                // Verificação da Blacklist (Segurança)
                string sqlCheck = "SELECT COUNT(*) FROM blacklist WHERE cpf = @CPF OR email = @Email";
                MySqlCommand cmdCheck = new MySqlCommand(sqlCheck, conn);
                cmdCheck.Parameters.AddWithValue("@CPF", mskCadastro_CPF.Text);
                cmdCheck.Parameters.AddWithValue("@Email", txtCadastro_Email.Text);

                if (Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0)
                {
                    MessageBox.Show("Este usuário está bloqueado pelo sistema.");
                    return;
                }

                // GERAÇÃO DO CÓDIGO SEQUENCIAL
                string prefixo = (cargoEquipe == "SUBADM") ? "SUB-" : "FUN-";
                string codigoAcesso = prefixo + "01"; // Padrão se não houver ninguém

                // Busca o último código gerado APENAS para este tipo de cargo
                string sqlCodigo = "SELECT codigoacesso FROM usuarios WHERE tipousuario = @Tipo ORDER BY id DESC LIMIT 1";
                using (MySqlCommand cmdCodigo = new MySqlCommand(sqlCodigo, conn))
                {
                    cmdCodigo.Parameters.AddWithValue("@Tipo", cargoEquipe);
                    object resultado = cmdCodigo.ExecuteScalar();

                    if (resultado != null)
                    {
                        string ultimoCodigo = resultado.ToString(); // Ex: "FUN-01"
                        string numeroTexto = ultimoCodigo.Replace(prefixo, ""); // Remove "FUN-" sobra "01"

                        if (int.TryParse(numeroTexto, out int ultimoNumero))
                        {
                            // Soma 1 e formata com dois dígitos (D2)
                            codigoAcesso = prefixo + (ultimoNumero + 1).ToString("D2");
                        }
                    }
                }

                string query = @"
            INSERT INTO usuarios (nome, idade, cpf, cep, sexo, estadocivil, telefone, email, senha, tipousuario, codigoacesso, endereco, naturalidade)
            VALUES (@Nome, @Idade, @CPF, @CEP, @Sexo, @EstadoCivil, @Telefone, @Email, @Senha, @TipoUsuario, @CodigoAcesso, @Endereco, @Naturalidade)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nome", txtCadastro_Nome.Text);
                    cmd.Parameters.AddWithValue("@Idade", int.Parse(txtCadastro_Idade.Text));
                    cmd.Parameters.AddWithValue("@CPF", mskCadastro_CPF.Text);
                    cmd.Parameters.AddWithValue("@CEP", mskCadastro_CEP.Text);
                    cmd.Parameters.AddWithValue("@Sexo", cbbCadastro_Sexualidade.Text);
                    cmd.Parameters.AddWithValue("@EstadoCivil", cbbCadastro_eCivil.Text);
                    cmd.Parameters.AddWithValue("@Telefone", mskCadastro_Telefone.Text);
                    cmd.Parameters.AddWithValue("@Email", txtCadastro_Email.Text);
                    cmd.Parameters.AddWithValue("@Senha", txtCadastro_Senha.Text);
                    cmd.Parameters.AddWithValue("@TipoUsuario", cargoEquipe);
                    cmd.Parameters.AddWithValue("@CodigoAcesso", codigoAcesso);
                    cmd.Parameters.AddWithValue("@Endereco", txtCadastro_Endereco.Text);
                    cmd.Parameters.AddWithValue("@Naturalidade", cbbCadastro_Naturalidade.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"{cargoEquipe} cadastrado com sucesso!\n\nCódigo de Acesso gerado: {codigoAcesso}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }



        private void txtCadastro_Nome_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCadastro_Nome.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCadastro_Nome, "Nome é obrigatório!");
            }
            else
            {
                errorProvider1.SetError(txtCadastro_Nome, "");
            }
        }

        private void txtCadastro_Idade_Validating(object sender, CancelEventArgs e)
        {
            if (!int.TryParse(txtCadastro_Idade.Text, out int idade))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCadastro_Idade, "Idade inválida");
                return;
            }

            if (idade < 0)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCadastro_Idade, "Idade não pode ser negativa.");
                return;
            }

            if (idade < 18)
            {
                errorProvider1.SetError(txtCadastro_Idade,
                    "Você é menor de idade, faça tudo sempre com autorização dos responsáveis.");
            }
            else
            {
                errorProvider1.SetError(txtCadastro_Idade, "Aproveite nosso app.");
            }
        }

        private void cbbCadastro_Sexualidade_Validating(object sender, CancelEventArgs e)
        {
            if (cbbCadastro_Sexualidade.SelectedIndex == -1)
            {
                e.Cancel = true;
                errorProvider1.SetError(cbbCadastro_Sexualidade, "Selecione uma opção");
            }
            else
            {
                errorProvider1.SetError(cbbCadastro_Sexualidade, "");
            }
        }

        private void mskCadastro_CPF_Validating(object sender, CancelEventArgs e)
        {
            if (!mskCadastro_CPF.MaskFull)
            {
                e.Cancel = true;
                errorProvider1.SetError(mskCadastro_CPF, "CPF incompleto!");
            }
            else
            {
                errorProvider1.SetError(mskCadastro_CPF, "");
            }
        }

        private void mskCadastro_Telefone_Validating(object sender, CancelEventArgs e)
        {
            if (!mskCadastro_Telefone.MaskFull)
            {
                e.Cancel = true;
                errorProvider1.SetError(mskCadastro_Telefone, "Telefone incompleto!");
            }
            else
            {
                errorProvider1.SetError(mskCadastro_Telefone, "");
            }
        }

        private void cbbCadastro_Naturalidade_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbbCadastro_Naturalidade.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(cbbCadastro_Naturalidade, "Campo obrigatório");
            }
            else
            {
                errorProvider1.SetError(cbbCadastro_Naturalidade, "");
            }
        }

        private void cbbCadastro_eCivil_Validating(object sender, CancelEventArgs e)
        {
            if (cbbCadastro_eCivil.SelectedIndex == -1)
            {
                e.Cancel = true;
                errorProvider1.SetError(cbbCadastro_eCivil, "Selecione o estado civil.");
            }
            else
            {
                errorProvider1.SetError(cbbCadastro_eCivil, "");
            }
        }

        private void txtCadastro_Endereco_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCadastro_Endereco.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCadastro_Endereco, "Endereço é obrigatório.");
            }
            else
            {
                errorProvider1.SetError(txtCadastro_Endereco, "");
            }
        }

        private void mskCadastro_CEP_Validating(object sender, CancelEventArgs e)
        {
            if (!mskCadastro_CEP.MaskFull)
            {
                e.Cancel = true;
                errorProvider1.SetError(mskCadastro_CEP, "CEP incompleto!");
            }
            else
            {
                errorProvider1.SetError(mskCadastro_CEP, "");
            }
        }

        private void txtCadastro_Email_Validating(object sender, CancelEventArgs e)
        {
            string email = txtCadastro_Email.Text;

            if (string.IsNullOrWhiteSpace(email))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCadastro_Email, "Email obrigatório.");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCadastro_Email, "Digite o email corretamente");
            }
            else
            {
                errorProvider1.SetError(txtCadastro_Email, "");
            }
        }

        private void txtCadastro_Senha_Validating(object sender, CancelEventArgs e)
        {
            string senha = txtCadastro_Senha.Text;

            if (string.IsNullOrWhiteSpace(senha))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCadastro_Senha, "Senha é obrigatória!");
                return;
            }

            if (senha.Length < 6)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCadastro_Senha, "Senha deve ter no mínimo 6 caracteres.");
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(
                senha, @"^(?=.*[A-Za-z])(?=.*\d).+$"))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCadastro_Senha,
                    "Senha deve conter letras e números.");
            }
            else
            {
                errorProvider1.SetError(txtCadastro_Senha, "");
            }
        }

        private void txtCadastro_ConfirmarSenha_Validating(object sender, CancelEventArgs e)
        {
            if (txtCadastro_ConfirmarSenha.Text != txtCadastro_Senha.Text)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCadastro_ConfirmarSenha, "Senhas não coincidem");
            }
            else
            {
                errorProvider1.SetError(txtCadastro_ConfirmarSenha, "");
            }
        }

        private void btnCadastro_Cadastrar_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                SalvarUsuario();

                MessageBox.Show("Cadastrado com sucesso! ✅");

                LimparCampos();
            }
            else
            {
                MessageBox.Show("Corrija os campos inválidos!");
            }
        }

        private void btnCadastro_LimparTudo_Click(object sender, EventArgs e)
        {
            var confirmar = MessageBox.Show(
                "Deseja apagar TODOS os cadastros?",
                "Limpar lista",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmar != DialogResult.Yes)
                return;

            LimparCampos();

            MessageBox.Show("Lista de cadastros apagada! 🗑️");
        }

        private void LimparCampos()
        {
            txtCadastro_Nome.Clear();
            txtCadastro_Idade.Clear();
            mskCadastro_CPF.Clear();
            mskCadastro_Telefone.Clear();
            txtCadastro_Endereco.Clear();
            mskCadastro_CEP.Clear();
            txtCadastro_Email.Clear();
            txtCadastro_Senha.Clear();
            txtCadastro_ConfirmarSenha.Clear();

            cbbCadastro_Sexualidade.SelectedIndex = -1;
            cbbCadastro_Naturalidade.SelectedIndex = -1;
            cbbCadastro_eCivil.SelectedIndex = -1;

            errorProvider1.Clear();
            txtCadastro_Nome.Focus();
        }

        private void btnCadastro_Sair_Click(object sender, EventArgs e)
        {
            var confirmar = MessageBox.Show(
                "Deseja sair da pagina de cadastro?",
                "Tela de cadastro",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmar != DialogResult.Yes)
                return;

            this.Close();
        }
    }

}
