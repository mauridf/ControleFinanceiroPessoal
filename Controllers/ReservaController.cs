using ControleFinanceiroPessoal.Entities;
using ControleFinanceiroPessoal.Requests;
using ControleFinanceiroPessoal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ControleFinanceiroPessoal.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReservaController : ControllerBase
{
    private readonly ReservaService _reservaService;

    public ReservaController(ReservaService reservaService)
    {
        _reservaService = reservaService;
    }

    /// <summary>
    /// Obtém todos as reservas.
    /// </summary>
    /// <returns>Uma lista de reservas.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reserva>>> GetAllReservas()
    {
        var reservas = await _reservaService.GetAllReservasAsync();
        return Ok(reservas);
    }

    /// <summary>
    /// Obtém a reserva pelo Id.
    /// </summary>
    /// <param name="id">O ID da reserva.</param>
    /// <returns>A reserva correspondente ao Id Passado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Reserva>> GetReservaById(ObjectId id)
    {
        var reserva = await _reservaService.GetReservaByIdAsync(id);
        if (reserva == null)
            return NotFound();

        return Ok(reserva);
    }

    /// <summary>
    /// Cadastra uma nova reserva.
    /// </summary>
    /// <param name="reserva">A entidade reserva.</param>
    /// <returns>A reserva criada.</returns>
    /// <response code="201">Retorna a reserva criada.</response>
    /// <response code="400">Se os dados da reserva não forem válidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Reserva), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateReserva([FromBody] Reserva reserva)
    {
        if (reserva == null)
            return BadRequest("Reserva inválida.");

        await _reservaService.InsertReservaAsync(reserva);
        return CreatedAtAction(nameof(GetReservaById), new { id = reserva.Id }, reserva);
    }

    /// <summary>
    /// Atualiza informações de uma reserva já cadastrada.
    /// </summary>
    /// <param name="id">O ID da reserva a ser atualizada.</param>
    /// <param name="reserva">Os dados atualizados da reserva.</param>
    /// <returns>Uma resposta sem conteúdo se a atualização for bem-sucedida.</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateReserva([FromBody] Reserva reserva)
    {
        if (reserva == null)
            return BadRequest("Reserva inválida.");

        await _reservaService.UpdateReservaAsync(reserva);
        return NoContent();
    }

    /// <summary>
    /// Deleta uma reserva.
    /// </summary>
    /// <param name="id">O ID da reserva a ser deletada.</param>
    /// <returns>Uma resposta sem conteúdo se a deleção for bem-sucedida.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteReserva(ObjectId id)
    {
        await _reservaService.DeleteReservaAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Subtrai (Usa) um valor da Reserva.
    /// </summary>
    /// <param name="id">O id da Reserva na qual o valor será subtraido (usado).</param>
    /// <param name="request">O request do Uso de Reserva.</param>
    /// <returns>Uma resposta sem conteúdo se a adição for bem-sucedida</returns>
    [HttpPost("{id}/usar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UsarReserva(ObjectId id, [FromBody] UsarReservaRequest request)
    {
        if (request == null || request.Valor <= 0)
            return BadRequest("Valor inválido.");

        await _reservaService.UsarReservaAsync(id, request.Valor, request.Descricao);
        return NoContent();
    }

    /// <summary>
    /// Adiciona um novo valor a Reserva.
    /// </summary>
    /// <param name="id">O id da Reserva na qual o valor será adicionado (somado).</param>
    /// <param name="request">O request da Adição de Reserva.</param>
    /// <returns>Uma resposta sem conteúdo se a adição for bem-sucedida</returns>
    [HttpPost("{id}/adicionar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AdicionarValorReserva(ObjectId id, [FromBody] AdicionarValorReservaRequest request)
    {
        if (request == null || request.Valor <= 0)
            return BadRequest("Valor inválido.");

        await _reservaService.AdicionarValorReservaAsync(id, request.Valor, request.Descricao);
        return NoContent();
    }

    /// <summary>
    /// Obtém as transações feitas com a reserva pelo Id.
    /// </summary>
    /// <param name="id">O ID da reserva.</param>
    /// <returns>As transações realizadas com a reserva correspondente ao Id Passado</returns>
    [HttpGet("{id}/transacoes")]
    public async Task<ActionResult<IEnumerable<ReservaTransacao>>> GetTransacoesByReserva(ObjectId id)
    {
        var transacoes = await _reservaService.GetTransacoesByReservaAsync(id);
        return Ok(transacoes);
    }
}
