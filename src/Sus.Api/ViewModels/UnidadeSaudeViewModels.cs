using System.ComponentModel.DataAnnotations;

namespace Sus.Api.ViewModels;

public class UnidadeSaudeRequestViewModel
{
    [Required(ErrorMessage = "Nome e obrigatorio")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Endereco e obrigatorio")]
    [MaxLength(200)]
    public string Endereco { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Capacidade diaria deve ser maior que zero")]
    public int CapacidadeDiaria { get; set; }
}

public class UnidadeSaudeResponseViewModel
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public int CapacidadeDiaria { get; set; }
}
