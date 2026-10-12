namespace Sus.Api.ViewModels;

public class AlertaResponseViewModel
{
    public long Id { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public DateTime DataAlerta { get; set; }
    public long AtendimentoId { get; set; }
}
