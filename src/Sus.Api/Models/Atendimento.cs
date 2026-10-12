using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sus.Api.Models;

[Table("T_ATENDIMENTO")]
public class Atendimento
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("PACIENTE_ID")]
    public long PacienteId { get; set; }

    public Paciente Paciente { get; set; } = null!;

    [Column("UNIDADE_ID")]
    public long UnidadeId { get; set; }

    public UnidadeSaude Unidade { get; set; } = null!;

    [Column("DATA_ENTRADA")]
    public DateTime DataEntrada { get; set; }

    [Column("DATA_ATENDIMENTO")]
    public DateTime? DataAtendimento { get; set; }

    [Required]
    [MaxLength(30)]
    [Column("STATUS")]
    public string Status { get; set; } = string.Empty;
}
