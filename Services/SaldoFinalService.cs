using ControleFinanceiroPessoal.Entities;
using ControleFinanceiroPessoal.Repositories;
using MongoDB.Bson;

namespace ControleFinanceiroPessoal.Services;

public class SaldoFinalService
{
    private readonly GenericRepository<SaldoFinal> _repository;

    public SaldoFinalService(GenericRepository<SaldoFinal> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SaldoFinal>> GetAllSaldoFinalsAsync() => await _repository.GetAllAsync();

    public async Task<SaldoFinal> GetSaldoFinalByIdAsync(ObjectId id) => await _repository.GetByIdAsync(id);

    public async Task InsertSaldoFinalAsync(SaldoFinal saldoFinal) => await _repository.InsertAsync(saldoFinal);

    public async Task UpdateSaldoFinalAsync(SaldoFinal saldoFinal) => await _repository.UpdateAsync(saldoFinal);

    public async Task DeleteSaldoFinalAsync(ObjectId id) => await _repository.DeleteAsync(id);
}
