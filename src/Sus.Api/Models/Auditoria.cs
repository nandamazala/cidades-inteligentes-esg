using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sus.Api.Models;

[Table("T_AUDITORIA")]
public class Auditoria
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [MaxLength(100)]
    [Column("ACAO")]
    public string Acao { get; set; } = string.Empty;

    [Column("DATA_EVENTO")]
    public DateTime DataEvento { get; set; }

    [Column("ATENDIMENTO_ID")]
    public long AtendimentoId { get; set; }

    public Atendimento Atendimento { get; set; } = null!;
}
