using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjetoHelpDesk
{
    public partial class PainelAluno : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MinhaConexao"]?.ConnectionString;

        private bool painelConfigVisivel = false;
        private int alturaAlvoPainelConfig = 103;
        private int passoAnimacao = 15;
        private int topInicialPainelConfig;
        private int topFinalPainelConfig;

        public PainelAluno()
        {
            InitializeComponent();

            // 🔹 Garante que existe um usuário logado
            if (string.IsNullOrEmpty(Login.UsuarioLogadoEmail))
            {
                MessageBox.Show("Não é possível identificar o usuário logado. Faça login novamente.",
                                "Erro de Autenticação",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                this.Close();
                return;
            }

            // 🔹 Atualiza as labels com as informações do login
            lblNomePainel.Text = $"Bem-vindo(a), {Login.UsuarioLogadoEmail}";
            lblNomeCursoPainel.Text = $"Papel: {Login.UsuarioLogadoPapel}";
        }

        private void PainelAluno_Load(object sender, EventArgs e)
        {
            // 🔹 Carrega o nome do curso do aluno
            string nomeCurso = BuscarNomeCurso(Login.UsuarioLogadoId);
            lblNomeCursoPainel.Text = nomeCurso ?? "Curso não informado";

            EstilizarGrid();
            CarregarChamados();
        }

        private string BuscarNomeCurso(int alunoId)
        {
            if (string.IsNullOrEmpty(connectionString))
                return null;

            try
            {
                using (SqlConnection conexao = new SqlConnection(connectionString))
                {
                    conexao.Open();
                    string query = "SELECT Curso FROM Alunos WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conexao);
                    cmd.Parameters.AddWithValue("@Id", alunoId);

                    object result = cmd.ExecuteScalar();
                    return result?.ToString();
                }
            }
            catch
            {
                return null;
            }
        }

        private void EstilizarGrid()
        {
            dgvChamados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChamados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChamados.MultiSelect = false;
            dgvChamados.RowHeadersVisible = false;
        }

        private void CarregarChamados()
        {
            if (string.IsNullOrEmpty(connectionString))
                return;

            try
            {
                using (SqlConnection conexao = new SqlConnection(connectionString))
                {
                    conexao.Open();
                    string query = "SELECT Id, Titulo, Descricao, Status, DataCriacao FROM Chamados WHERE EmailAluno = @Email";
                    SqlCommand cmd = new SqlCommand(query, conexao);
                    cmd.Parameters.AddWithValue("@Email", Login.UsuarioLogadoEmail);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvChamados.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar chamados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNovoChamado_Click(object sender, EventArgs e)
        {
            using (NovoChamado novoChamado = new NovoChamado())
            {
                novoChamado.ShowDialog();
                CarregarChamados();
            }
        }

        private void btnConfiguracoes_Click(object sender, EventArgs e)
        {
            painelConfigVisivel = !painelConfigVisivel;
            timerAnimacaoConfig.Start();
        }

        private void btnModoEscuro_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Alternar modo escuro ainda não implementado.");
        }

        private void btnMudarSenha_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Função de mudar senha em breve!");
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("Deseja realmente sair?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                Login.UsuarioLogadoEmail = null;
                Login.UsuarioLogadoPapel = null;
                Login.UsuarioLogadoId = 0;
                this.Close();
            }
        }

        private void timerAnimacaoConfig_Tick(object sender, EventArgs e)
        {
            if (painelConfigVisivel)
            {
                if (pnlBotoesConfig.Height < alturaAlvoPainelConfig)
                {
                    pnlBotoesConfig.Height += passoAnimacao;
                }
                else
                {
                    timerAnimacaoConfig.Stop();
                }
            }
            else
            {
                if (pnlBotoesConfig.Height > 0)
                {
                    pnlBotoesConfig.Height -= passoAnimacao;
                }
                else
                {
                    timerAnimacaoConfig.Stop();
                }
            }
        }
       

        private void dgvChamados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                MessageBox.Show("Duplo clique em um chamado — função ainda não implementada.",
                                "Informação",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        private void dgvChamados_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
         .
        }

        private void dgvChamados_Sorted(object sender, EventArgs e)
        {
           
        }

    }
}
