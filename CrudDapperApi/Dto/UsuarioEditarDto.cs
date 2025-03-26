namespace CrudDapperApi.Dto
{
    public class UsuarioEditarDto
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string cargo { get; set; }
        public double Salario { get; set; }
        public string Cpf { get; set; }
        public bool Situacao { get; set; }
    }
}
