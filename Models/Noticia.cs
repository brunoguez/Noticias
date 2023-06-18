using System.ComponentModel.DataAnnotations.Schema;

namespace Noticias.Models
{
    public class Noticia
    {
        [Column("idNoticia")] public int Id { get; set; }
        [Column("autorId")] public required string AutorId { get; set; }
        [Column("autorName")] public required string AutorName { get; set; }
        [Column("titulo")] public required string Titulo { get; set; }
        [Column("dataPublicacao")] public DateTime DataPublicacao { get; set; }
        [Column("imagem")] public string? URL_imagem { get; set; }
        [Column("texto")] public string? Texto { get; set; }
        [Column("publicada")] public bool Publicada { get; set; }
        [Column("categoriaId")] public bool CategoriaId { get; set; }
    }
}


//	"idNoticia"	INTEGER NOT NULL,
//	"autorId"	INTEGER NOT NULL,
//	"titulo"	VARCHAR(100) NOT NULL,
//	"dataPublicacao"	DATETIME NOT NULL,
//	"imagem"	VARCHAR(255),
//	"texto" TEXT,
//	"publicada"	BIT NOT NULL,

