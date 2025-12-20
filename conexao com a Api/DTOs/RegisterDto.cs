namespace Api_crudPim.DTOs
{
    public class RegisterRequest
    {
       
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Papel { get; set; } = "Aluno";

        
        public int CursoID { get; set; }
        public string? RegistroAluno { get; set; }
    }
}