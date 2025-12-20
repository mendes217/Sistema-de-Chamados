namespace Api_crudPim.Models
{
    public class Respostas
    {
        public int ID { get; set; }
        public int ChamadoID { get; set; }
        public int UsuarioID { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public DateTime DataEnvio { get; set; } = DateTime.Now;
    }
}
