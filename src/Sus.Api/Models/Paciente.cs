using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sus.Api.Models;

[Table("T_PACIENTE")]
public class Paciente
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("NOME")]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(11)]
    [Column("CPF")]
    public string Cpf { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("EMAIL")]
    public string? Email { get; set; }

    [MaxLength(20)]
    [Column("TELEFONE")]
    public string? Telefone { get; set; }
}
