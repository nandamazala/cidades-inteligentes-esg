using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sus.Api.Models;

[Table("T_ALERTA")]
public class Alerta
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [MaxLength(255)]
    [Column("MENSAGEM")]
    public string Mensagem { get; set; } = string.Empty;

    [Column("DATA_ALERTA")]
    public DateTime DataAlerta { get; set; }

    [Column("ATENDIMENTO_ID")]
    public long AtendimentoId { get; set; }

    public Atendimento Atendimento { get; set; } = null!;
}
