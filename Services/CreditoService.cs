using ControleFinanceiroPessoal.Entities;
using ControleFinanceiroPessoal.Repositories;
using MongoDB.Bson;

namespace ControleFinanceiroPessoal.Services;

public class CreditoService
{
    private readonly GenericRepository<Credito> _repository;

    public CreditoService(GenericRepository<Credito> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Credito>> GetAllCreditosAsync() => await _repository.GetAllAsync();

    public async Task<Credito> GetCreditoByIdAsync(ObjectId id) => await _repository.GetByIdAsync(id);

    public async Task InsertCreditoAsync(Credito credito) => await _repository.InsertAsync(credito);

    public async Task UpdateCreditoAsync(Credito credito) => await _repository.UpdateAsync(credito);

    public async Task DeleteCreditoAsync(ObjectId id) => await _repository.DeleteAsync(id);
}
