using System.ComponentModel.DataAnnotations;

namespace Sus.Api.ViewModels;

public class PacienteRequestViewModel
{
    [Required(ErrorMessage = "Nome e obrigatorio")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "CPF e obrigatorio")]
    [MaxLength(11)]
    [RegularExpression("^\\d{11}$", ErrorMessage = "CPF deve conter 11 digitos numericos")]
    public string Cpf { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "E-mail invalido")]
    [MaxLength(100)]
    public string? Email { get; set; }

    [RegularExpression("^\\+?[0-9]{10,15}$", ErrorMessage = "Telefone deve conter de 10 a 15 digitos")]
    public string? Telefone { get; set; }
}

public class PacienteResponseViewModel
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefone { get; set; }
}
