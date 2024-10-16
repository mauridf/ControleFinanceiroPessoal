using ControleFinanceiroPessoal.Entities;
using ControleFinanceiroPessoal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ControleFinanceiroPessoal.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CreditoController : ControllerBase
{
    private readonly CreditoService _creditoService;

    public CreditoController(CreditoService creditoService)
    {
        _creditoService = creditoService;
    }

    /// <summary>
    /// Obtém todos os créditos.
    /// </summary>
    /// <returns>Uma lista de créditos.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllCreditos() =>
        Ok(await _creditoService.GetAllCreditosAsync());

    /// <summary>
    /// Obtém o crédito pelo Id.
    /// </summary>
    /// <param name="id">O ID do crédito.</param>
    /// <returns>O crédido correspondente ao Id Passado</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCreditoById(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
            return BadRequest("Invalid ID format.");

        var credito = await _creditoService.GetCreditoByIdAsync(objectId);
        if (credito == null)
            return NotFound();

        return Ok(credito);
    }

    /// <summary>
    /// Cadastra um novo crédito.
    /// </summary>
    /// <param name="credito">A entidade crédito.</param>
    /// <returns>O crédito criado.</returns>
    /// <response code="201">Retorna o crédito criado.</response>
    /// <response code="400">Se os dados do crédito não forem válidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Credito), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCredito(Credito credito)
    {
        await _creditoService.InsertCreditoAsync(credito);
        return CreatedAtAction(nameof(GetCreditoById), new { id = credito.Id }, credito);
    }

    /// <summary>
    /// Atualiza informações de um Crédito já cadastrado.
    /// </summary>
    /// <param name="id">O ID do crédito a ser atualizado.</param>
    /// <param name="updatedCredito">Os dados atualizados do crédito.</param>
    /// <returns>Uma resposta sem conteúdo se a atualização for bem-sucedida.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCredito(string id, Credito updatedCredito)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
            return BadRequest("Invalid ID format.");

        updatedCredito.Id = objectId;
        await _creditoService.UpdateCreditoAsync(updatedCredito);

        return NoContent();
    }

    /// <summary>
    /// Deleta um crédito.
    /// </summary>
    /// <param name="id">O ID do crédito a ser deletado.</param>
    /// <returns>Uma resposta sem conteúdo se a deleção for bem-sucedida.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCredito(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
            return BadRequest("Invalid ID format.");

        await _creditoService.DeleteCreditoAsync(objectId);
        return NoContent();
    }
}
