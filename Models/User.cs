using System.ComponentModel.DataAnnotations.Schema;

namespace Noticias.Models
{
    [Table("Usuarios")]
    public class User
    {
        [Column("idUsuario")] public int Id { get; set; }
        [Column("email")] public string Email { get; set; }
        [Column("senha")] public string Password { get; set; }
        [Column("perfil")] public string Perfil { get; set; }
        [Column("nome")] public string? Nome { get; set; }
        [Column("foto")] public string? Foto { get; set; }
    }
}

