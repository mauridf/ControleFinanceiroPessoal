namespace ControleFinanceiroPessoal.Requests;

public class LoginRequest
{
    public string Email { get; set; }
    public string SenhaHash { get; set; } // Pode ser senha em texto claro, ou se você armazenar hash, então é senhaHash
}
