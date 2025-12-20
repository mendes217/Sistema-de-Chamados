using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProjetoHelpDesk.ApiClient
{
    public class ApiService
    {
        private readonly HttpClient _http;

        public ApiService()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001/api/")
            };
        }

        // 🔹 Pega lista de cursos
        public async Task<List<Curso>> GetCursosAsync()
        {
            return await _http.GetFromJsonAsync<List<Curso>>("cursos");
        }

        // 🔹 Cadastra novo usuário (Aluno)
        public async Task<bool> CadastrarUsuarioAsync(UsuarioCadastroRequest usuario)
        {
            var response = await _http.PostAsJsonAsync("usuarios/cadastrar", usuario);
            return response.IsSuccessStatusCode;
        }
    }

    // ==== MODELOS ====
    public class Curso
    {
        public int ID { get; set; }
        public string NomeCurso { get; set; }
    }

    public class UsuarioCadastroRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public int CursoID { get; set; }
        public string RegistroAluno { get; set; }
    }
}
