using ControleFinanceiroPessoal.Entities;
using ControleFinanceiroPessoal.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ControleFinanceiroPessoal.Services;

public class ReservaService
{
    private readonly GenericRepository<Reserva> _repository;
    private readonly GenericRepository<ReservaTransacao> _transacaoRepository;

    public ReservaService(GenericRepository<Reserva> repository, GenericRepository<ReservaTransacao> transacaoRepository)
    {
        _repository = repository;
        _transacaoRepository = transacaoRepository;
    }

    public async Task<IEnumerable<Reserva>> GetAllReservasAsync() => await _repository.GetAllAsync();

    public async Task<Reserva> GetReservaByIdAsync(ObjectId id) => await _repository.GetByIdAsync(id);

    public async Task InsertReservaAsync(Reserva reserva) => await _repository.InsertAsync(reserva);

    public async Task UpdateReservaAsync(Reserva reserva) => await _repository.UpdateAsync(reserva);

    public async Task DeleteReservaAsync(ObjectId id) => await _repository.DeleteAsync(id);

    public async Task UsarReservaAsync(ObjectId reservaId, decimal valor, string descricao)
    {
        var reserva = await _repository.GetByIdAsync(reservaId);

        if (reserva == null)
            throw new Exception("Reserva não encontrada.");

        if (valor > reserva.SaldoAtual)
            throw new Exception("Saldo insuficiente na reserva.");

        // Atualizar o saldo da reserva
        reserva.SaldoAtual -= valor;
        await _repository.UpdateAsync(reserva);

        // Registrar a transação de uso da reserva
        var transacao = new ReservaTransacao
        {
            ReservaId = reservaId,
            Valor = valor,
            DataTransacao = DateTime.UtcNow,
            TipoTransacao = "Uso",
            Descricao = descricao
        };
        await _transacaoRepository.InsertAsync(transacao);
    }

    public async Task AdicionarValorReservaAsync(ObjectId reservaId, decimal valor, string descricao)
    {
        var reserva = await _repository.GetByIdAsync(reservaId);

        if (reserva == null)
            throw new Exception("Reserva não encontrada.");

        // Atualizar o saldo da reserva
        reserva.SaldoAtual += valor;
        await _repository.UpdateAsync(reserva);

        // Registrar a transação de adição de valor na reserva
        var transacao = new ReservaTransacao
        {
            ReservaId = reservaId,
            Valor = valor,
            DataTransacao = DateTime.UtcNow,
            TipoTransacao = "Adição",
            Descricao = descricao
        };
        await _transacaoRepository.InsertAsync(transacao);
    }

    public async Task<IEnumerable<ReservaTransacao>> GetTransacoesByReservaAsync(ObjectId reservaId)
    {
        var filter = Builders<ReservaTransacao>.Filter.Eq(rt => rt.ReservaId, reservaId);
        return await _transacaoRepository.FindByConditionAsync(filter);
    }
}
