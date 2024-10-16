using ControleFinanceiroPessoal.Entities;
using ControleFinanceiroPessoal.Services;
using ControleFinanceiroPessoal.Requests;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ControleFinanceiroPessoal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Cadastra um novo usuário.
    /// </summary>
    /// <param name="usuario">A entidade usuário.</param>
    /// <returns>O usuário criado.</returns>
    /// <response code="201">Retorna o usuário criado.</response>
    /// <response code="400">Se os dados do usuário não forem válidos.</response>
    [HttpPost("cadastrar")]
    [ProducesResponseType(typeof(Usuario), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CadastrarUsuario([FromBody] Usuario usuario)
    {
        if (usuario == null)
            return BadRequest("Dados do usuário são inválidos.");

        await _usuarioService.InsertUsuarioAsync(usuario);
        return CreatedAtAction(nameof(GetUsuarioByEmail), new { email = usuario.Email }, usuario);
    }

    /// <summary>
    /// Efetua o Login do usuário.
    /// </summary>
    /// <param name="loginRequest">O request com os dados para efetuar o Login.</param>
    /// <returns>O Token indicando sucesso no Login.</returns>
    /// <response code="201">Retorna o Token.</response>
    /// <response code="400">Se os dados do Login não forem válidos.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(Usuario), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Login([FromBody] Requests.LoginRequest loginRequest)
    {
        var usuarioEncontrado = await _usuarioService.GetUsuarioByEmailAsync(loginRequest.Email);
        if (usuarioEncontrado == null ||
            !_usuarioService.VerificarSenha(loginRequest.SenhaHash, usuarioEncontrado.SenhaHash))
        {
            return Unauthorized();
        }

        var token = _usuarioService.GerarToken(usuarioEncontrado);
        return Ok(new { Token = token });
    }


    /// <summary>
    /// Obtém o usuário pelo email.
    /// </summary>
    /// <param name="email">O E-mail do usuário.</param>
    /// <returns>O usuário correspondente ao E-mail Passado</returns>
    [HttpGet("{email}")]
    public async Task<ActionResult<Usuario>> GetUsuarioByEmail(string email)
    {
        var usuario = await _usuarioService.GetUsuarioByEmailAsync(email);
        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }
}
