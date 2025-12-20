using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ProjetoHelpDesk
{
    public partial class Cadastro : Form
    {
        private static readonly HttpClient client = new HttpClient();

        public Cadastro()
        {
            InitializeComponent();

            
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;

            CarregarCursos();
        }

        private void CarregarCursos()
        {
            
            cmbCurso.Items.Clear();
            cmbCurso.Items.Add("Administração");
            cmbCurso.Items.Add("Engenharia de Software");
            cmbCurso.Items.Add("Direito");
            cmbCurso.Items.Add("Medicina");
            cmbCurso.Text = "Selecione o curso...";
        }

        private async void btnCadastrar_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(txtNomeCadastro.Text.Trim()) ||
                string.IsNullOrWhiteSpace(txtEmailCadastro.Text.Trim()) ||
                string.IsNullOrWhiteSpace(txtRaAlunoCadastro.Text.Trim()) ||
                string.IsNullOrWhiteSpace(txtSenhaCadastro.Text.Trim()) ||
                string.IsNullOrWhiteSpace(txtConfirmaSenha.Text.Trim()) ||
                string.IsNullOrWhiteSpace(cmbCurso.Text.Trim()) ||
                cmbCurso.Text == "Selecione o curso...")
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.",
                                "Campos Vazios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // RA deve ter 7 caracteres
            if (txtRaAlunoCadastro.Text.Trim().Length != 7)
            {
                MessageBox.Show("O RA deve ter 7 caracteres.", "RA Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRaAlunoCadastro.Focus();
                txtRaAlunoCadastro.SelectAll();
                return;
            }

            // Confirmação de senha
            if (txtSenhaCadastro.Text != txtConfirmaSenha.Text)
            {
                MessageBox.Show("As senhas não coincidem.", "Erro de Senha", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmaSenha.Focus();
                txtConfirmaSenha.SelectAll();
                return;
            }

            // Tamanho mínimo da senha
            if (txtSenhaCadastro.Text.Length < 8)
            {
                MessageBox.Show("A senha deve ter pelo menos 8 caracteres.", "Senha Curta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenhaCadastro.Focus();
                return;
            }

            
            var novoUsuario = new
            {
                nome = txtNomeCadastro.Text.Trim(),
                email = txtEmailCadastro.Text.Trim(),
                senha = txtSenhaCadastro.Text.Trim(),
                papel = "Aluno",
                curso = cmbCurso.Text.Trim(),
                ra = txtRaAlunoCadastro.Text.Trim()
            };

            
            string apiUrl = "https://localhost:5051/api/Auth/register";

            try
            {
                string json = JsonConvert.SerializeObject(novoUsuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cadastro realizado com sucesso!\nVocê já pode fazer login.",
                                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    string erro = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao cadastrar: {erro}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Erro de conexão com o servidor: {ex.Message}", "Erro de Rede", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblVoltarLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
    }
}
