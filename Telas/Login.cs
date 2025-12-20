using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoHelpDesk
{
    public partial class Login : Form
    {
        private static readonly HttpClient client = new HttpClient();

        // 🔹 Informações do usuário logado
        public static int UsuarioLogadoId { get; set; }
        public static string UsuarioLogadoNome { get; set; }
        public static string UsuarioLogadoPapel { get; set; }
        public static string UsuarioLogadoEmail { get; set; }
        public static string UsuarioLogadoRA { get; set; }
        public static string UsuarioLogadoCurso { get; set; }
        public static string TokenJWT { get; set; }

        public Login()
        {
            InitializeComponent();
        }

        private async void btnEntrar_Click(object sender, EventArgs e)
        {
            string email = txtEmailLogin.Text.Trim();
            string senha = txtSenhaLogin.Text.Trim();

            // ✅ Validação dos campos
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Por favor, preencha o email e a senha.", "Campos Vazios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Cria o objeto de login para enviar à API
            var dadosLogin = new
            {
                email = email,
                senha = senha
            };

            string apiUrl = "https://localhost:5051/api/Auth/login";

            try
            {
                string json = JsonConvert.SerializeObject(dadosLogin);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string retorno = await response.Content.ReadAsStringAsync();
                    var dados = JsonConvert.DeserializeObject<LoginResponse>(retorno);

                    if (dados != null && !string.IsNullOrEmpty(dados.email))
                    {
                        // ✅ Armazena dados do usuário logado
                        UsuarioLogadoEmail = dados.email;
                        UsuarioLogadoPapel = dados.papel;
                        TokenJWT = dados.token;

                        MessageBox.Show($"Login realizado com sucesso!\nBem-vindo(a), {dados.email}.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // ✅ Redireciona de acordo com o papel
                        if (dados.papel.Equals("Aluno", StringComparison.OrdinalIgnoreCase))
                        {
                            PainelAluno painelAluno = new PainelAluno();
                            painelAluno.Show();
                            this.Hide();
                        }
                        else if (dados.papel.Equals("Funcionario", StringComparison.OrdinalIgnoreCase))
                        {
                            PainelFuncionario painelFuncionario = new PainelFuncionario();
                            painelFuncionario.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Seu papel de usuário não foi reconhecido.", "Erro de Acesso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Não foi possível interpretar a resposta da API.", "Erro de Retorno", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    string erro = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erro ao fazer login:\n{erro}", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Erro de conexão com o servidor: {ex.Message}", "Erro de Rede", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblCadastreSe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (Cadastro telaCadastro = new Cadastro())
            {
                telaCadastro.ShowDialog(this);
            }
        }

        private void lblEsqueceuSenha_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Funcionalidade de recuperação de senha ainda não implementada.",
                "Em Construção", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    // ✅ Modelo compatível com a resposta JSON da API:
    // {
    //   "token": "...",
    //   "email": "vanda.mend54@gmail",
    //   "papel": "Aluno"
    // }
    public class LoginResponse
    {
        public string token { get; set; }
        public string email { get; set; }
        public string papel { get; set; }
    }
}
 