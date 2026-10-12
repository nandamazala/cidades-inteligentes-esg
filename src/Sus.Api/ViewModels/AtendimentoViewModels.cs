using System.ComponentModel.DataAnnotations;

namespace Sus.Api.ViewModels;

public class AtendimentoRequestViewModel
{
    [Range(1, long.MaxValue, ErrorMessage = "Paciente e obrigatorio")]
    public long PacienteId { get; set; }

    [Range(1, long.MaxValue, ErrorMessage = "Unidade de saude e obrigatoria")]
    public long UnidadeId { get; set; }

    public DateTime? DataAtendimento { get; set; }

    [Required(ErrorMessage = "Status e obrigatorio")]
    [MaxLength(30)]
    [RegularExpression("^(ABERTO|EM_ATENDIMENTO|FINALIZADO|CANCELADO)$", ErrorMessage = "Status deve ser ABERTO, EM_ATENDIMENTO, FINALIZADO ou CANCELADO")]
    public string Status { get; set; } = string.Empty;
}

public class AtendimentoResponseViewModel
{
    public long Id { get; set; }
    public long PacienteId { get; set; }
    public string PacienteNome { get; set; } = string.Empty;
    public long UnidadeId { get; set; }
    public string UnidadeNome { get; set; } = string.Empty;
    public DateTime DataEntrada { get; set; }
    public DateTime? DataAtendimento { get; set; }
    public string Status { get; set; } = string.Empty;
}
