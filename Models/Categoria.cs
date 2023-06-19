using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noticias.Models
{
    internal class Categoria
    {
        [Column("idCategoria")] public int Id { get; set; }
        [Column("nome")] public required string Descricao { get; set; }
    }
}

//CREATE TABLE "Categoria" (
//	"idCategoria"	INTEGER NOT NULL,
//	"nome"	VARCHAR(90) NOT NULL,
//    PRIMARY KEY("idCategoria" AUTOINCREMENT)
//)