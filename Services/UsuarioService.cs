using ControleFinanceiroPessoal.Entities;
using ControleFinanceiroPessoal.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ControleFinanceiroPessoal.Services;

public class UsuarioService
{
    private readonly UsuarioRepository _repository;
    private readonly IConfiguration _configuration;

    public UsuarioService(UsuarioRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<Usuario> GetUsuarioByEmailAsync(string email)
    {
        return await _repository.GetByEmailAsync(email);
    }

    public async Task InsertUsuarioAsync(Usuario usuario)
    {
        usuario.SenhaHash = GerarHashSenha(usuario.SenhaHash);
        await _repository.InsertAsync(usuario);
    }

    private string GerarHashSenha(string senha)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToBase64String(bytes);
        }
    }

    public bool VerificarSenha(string senha, string senhaHash)
    {
        var hashTentativa = GerarHashSenha(senha);
        return hashTentativa == senhaHash;
    }

    public string GerarToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]); // Pegando a chave diretamente do appsettings.json
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(7), // Defina a expiração do token
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
