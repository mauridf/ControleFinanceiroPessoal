using ControleFinanceiroPessoal.Entities;
using ControleFinanceiroPessoal.Repositories;
using MongoDB.Bson;

namespace ControleFinanceiroPessoal.Services;

public class DebitoService
{
    private readonly GenericRepository<Debito> _repository;

    public DebitoService(GenericRepository<Debito> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Debito>> GetAllDebitosAsync() => await _repository.GetAllAsync();

    public async Task<Debito> GetDebitoByIdAsync(ObjectId id) => await _repository.GetByIdAsync(id);

    public async Task InsertDebitoAsync(Debito debito) => await _repository.InsertAsync(debito);

    public async Task UpdateDebitoAsync(Debito debito) => await _repository.UpdateAsync(debito);

    public async Task DeleteDebitoAsync(ObjectId id) => await _repository.DeleteAsync(id);
}
