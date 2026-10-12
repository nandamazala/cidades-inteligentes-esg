using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sus.Api.Models;

[Table("T_UNIDADE_SAUDE")]
public class UnidadeSaude
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [MaxLength(100)]
    [Column("NOME")]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(200)]
    [Column("ENDERECO")]
    public string Endereco { get; set; } = string.Empty;

    [Column("CAPACIDADE_DIARIA")]
    public int CapacidadeDiaria { get; set; }
}
