using ControleFinanceiroPessoal.Entities;
using ControleFinanceiroPessoal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ControleFinanceiroPessoal.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SaldoFinalController : ControllerBase
{
    private readonly SaldoFinalService _saldoFinalService;

    public SaldoFinalController(SaldoFinalService saldoFinalService)
    {
        _saldoFinalService = saldoFinalService;
    }

    /// <summary>
    /// Obtém todos os SaldoFinal.
    /// </summary>
    /// <returns>Uma lista de SaldoFinal.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaldoFinal>>> GetAllSaldoFinals()
    {
        var saldoFinals = await _saldoFinalService.GetAllSaldoFinalsAsync();
        return Ok(saldoFinals);
    }

    /// <summary>
    /// Obtém o SaldoFinal pelo Id.
    /// </summary>
    /// <param name="id">O ID do SaldoFinal.</param>
    /// <returns>O SaldoFinal correspondente ao Id Passado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<SaldoFinal>> GetSaldoFinalById(ObjectId id)
    {
        var saldoFinal = await _saldoFinalService.GetSaldoFinalByIdAsync(id);
        if (saldoFinal == null)
            return NotFound();

        return Ok(saldoFinal);
    }

    /// <summary>
    /// Cadastra um novo SaldoFinal.
    /// </summary>
    /// <param name="saldoFinal">A entidade SaldoFinal.</param>
    /// <returns>O SaldoFinal criado.</returns>
    /// <response code="201">Retorna o SaldoFinal criado.</response>
    /// <response code="400">Se os dados do SaldoFinal não forem válidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(SaldoFinal), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateSaldoFinal([FromBody] SaldoFinal saldoFinal)
    {
        if (saldoFinal == null)
            return BadRequest("Saldo final inválido.");

        await _saldoFinalService.InsertSaldoFinalAsync(saldoFinal);
        return CreatedAtAction(nameof(GetSaldoFinalById), new { id = saldoFinal.Id }, saldoFinal);
    }

    /// <summary>
    /// Atualiza informações de um SaldoFinal já cadastrado.
    /// </summary>
    /// <param name="id">O ID do SaldoFinal a ser atualizado.</param>
    /// <param name="saldoFinal">Os dados atualizados do SaldoFinal.</param>
    /// <returns>Uma resposta sem conteúdo se a atualização for bem-sucedida.</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateSaldoFinal([FromBody] SaldoFinal saldoFinal)
    {
        if (saldoFinal == null)
            return BadRequest("Saldo final inválido.");

        await _saldoFinalService.UpdateSaldoFinalAsync(saldoFinal);
        return NoContent();
    }

    /// <summary>
    /// Deleta um SaldoFinal.
    /// </summary>
    /// <param name="id">O ID do SaldoFinal a ser deletado.</param>
    /// <returns>Uma resposta sem conteúdo se a deleção for bem-sucedida.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteSaldoFinal(ObjectId id)
    {
        await _saldoFinalService.DeleteSaldoFinalAsync(id);
        return NoContent();
    }
}
