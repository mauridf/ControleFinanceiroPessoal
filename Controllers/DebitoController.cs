using ControleFinanceiroPessoal.Entities;
using ControleFinanceiroPessoal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ControleFinanceiroPessoal.Controllers;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DebitoController : ControllerBase
{
    private readonly DebitoService _debitoService;

    public DebitoController(DebitoService debitoService)
    {
        _debitoService = debitoService;
    }

    /// <summary>
    /// Obtém todos os débitos.
    /// </summary>
    /// <returns>Uma lista de débitos.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllDebitos() =>
        Ok(await _debitoService.GetAllDebitosAsync());

    /// <summary>
    /// Obtém o débito pelo Id.
    /// </summary>
    /// <param name="id">O ID do débito.</param>
    /// <returns>O débito correspondente ao Id Passado</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDebitoById(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
            return BadRequest("Invalid ID format.");

        var debito = await _debitoService.GetDebitoByIdAsync(objectId);
        if (debito == null)
            return NotFound();

        return Ok(debito);
    }

    /// <summary>
    /// Cadastra um novo débito.
    /// </summary>
    /// <param name="debito">A entidade débito.</param>
    /// <returns>O débito criado.</returns>
    /// <response code="201">Retorna o débito criado.</response>
    /// <response code="400">Se os dados do débito não forem válidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Debito), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDebito(Debito debito)
    {
        await _debitoService.InsertDebitoAsync(debito);
        return CreatedAtAction(nameof(GetDebitoById), new { id = debito.Id }, debito);
    }

    /// <summary>
    /// Atualiza informações de um débito já cadastrado.
    /// </summary>
    /// <param name="id">O ID do débito a ser atualizado.</param>
    /// <param name="updatedDebito">Os dados atualizados do débito.</param>
    /// <returns>Uma resposta sem conteúdo se a atualização for bem-sucedida.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDebito(string id, Debito updatedDebito)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
            return BadRequest("Invalid ID format.");

        updatedDebito.Id = objectId;
        await _debitoService.UpdateDebitoAsync(updatedDebito);

        return NoContent();
    }

    /// <summary>
    /// Deleta um débito.
    /// </summary>
    /// <param name="id">O ID do débito a ser deletado.</param>
    /// <returns>Uma resposta sem conteúdo se a deleção for bem-sucedida.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteDebito(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
            return BadRequest("Invalid ID format.");

        await _debitoService.DeleteDebitoAsync(objectId);
        return NoContent();
    }
}
