using ControleFinanceiroPessoal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ControleFinanceiroPessoal.Controllers;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DashBoardController : ControllerBase
{
    private readonly DashBoardService _dashBoardService;

    public DashBoardController(DashBoardService dashBoardService)
    {
        _dashBoardService = dashBoardService;
    }

    /// <summary>
    /// Obtém todos os créditos por mês.
    /// </summary>
    /// <returns>Todos os créditos no mês informado.</returns>
    [HttpGet("creditos/mes")]
    public async Task<IActionResult> GetCreditosPorMes(string usuarioId)
    {
        try
        {
            var result = await _dashBoardService.GetCreditosPorMesAsync(usuarioId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Registre o erro aqui
            return StatusCode(500, new { Message = ex.Message, Detailed = ex.ToString() });
        }
    }


    /// <summary>
    /// Obtém todos os débitos por mês.
    /// </summary>
    /// <returns>Todos os débitos no mês informado.</returns>
    [HttpGet("debitos/mes")]
    public async Task<IActionResult> GetDebitosPorMes() =>
        Ok(await _dashBoardService.GetDebitosPorMesAsync());

    /// <summary>
    /// Obtém todas as reservas por mês.
    /// </summary>
    /// <returns>Todas as reservas no mês informado.</returns>
    [HttpGet("reservas/mes")]
    public async Task<IActionResult> GetReservasPorMes() =>
        Ok(await _dashBoardService.GetReservasPorMesAsync());

    /// <summary>
    /// Obtém o saldo final por mês.
    /// </summary>
    /// <returns>O saldo final no mês informado.</returns>
    [HttpGet("saldofinal/mes")]
    public async Task<IActionResult> GetSaldoFinalPorMes() =>
        Ok(await _dashBoardService.GetSaldoFinalPorMesAsync());

    /// <summary>
    /// O Total de uso da reserva.
    /// </summary>
    /// <returns>Total de uso das reservas.</returns>
    [HttpGet("reservas/uso")]
    public async Task<IActionResult> GetTotalUsoReservas() =>
        Ok(await _dashBoardService.GetTotalUsoReservasAsync());

    /// <summary>
    /// Saldo atual das reservas por período.
    /// </summary>
    /// <returns>O saldo das reservas no período informado.</returns>
    [HttpGet("reservas/saldo")]
    public async Task<IActionResult> GetSaldoAtualReservas([FromQuery] DateTime? inicio, [FromQuery] DateTime? fim) =>
        Ok(await _dashBoardService.GetSaldoAtualReservasAsync(inicio, fim));

    /// <summary>
    /// Histórico de transações.
    /// </summary>
    /// <returns>Uso da reserva ao longo do tempo.</returns>
    [HttpGet("reservas/historico")]
    public async Task<IActionResult> GetHistoricoTransacoes() =>
        Ok(await _dashBoardService.GetHistoricoTransacoesAsync());

    /// <summary>
    /// Evolução de uso das reservas.
    /// </summary>
    /// <returns>Evolução de uso das reservas por mês.</returns>
    [HttpGet("reservas/evolucao")]
    public async Task<IActionResult> GetEvolucaoUsoReservasPorMes() =>
        Ok(await _dashBoardService.GetEvolucaoUsoReservasPorMesAsync());

    /// <summary>
    /// Valores Adicionados ou Usados das reservas.
    /// </summary>
    /// <returns>Valores adicionados ou usados das reservas em determinado período.</returns>
    [HttpGet("reservas/valores")]
    public async Task<IActionResult> GetValoresReservasPorPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim) =>
        Ok(await _dashBoardService.GetValoresReservasPorPeriodoAsync(inicio, fim));

    /// <summary>
    /// Reservas acumuladas e saldo atual.
    /// </summary>
    /// <returns>Reservas acumuladas e saldo atual das reservas.</returns>
    [HttpGet("reservas/acumuladas")]
    public async Task<IActionResult> GetReservasAcumuladasESaldo() =>
        Ok(await _dashBoardService.GetReservasAcumuladasESaldoAsync());
}
