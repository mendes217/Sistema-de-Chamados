namespace Api_crudPim.DTOs
{
    public class ChamadoConsultaDTO
    {
        public int Id { get; set; }
        public int UsuarioID { get; set; }
        public int CategoriaID { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
        public string Prioridade { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }

        
        public string Resposta { get; set; }
    }
}
