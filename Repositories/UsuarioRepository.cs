using ControleFinanceiroPessoal.Entities;
using ControleFinanceiroPessoal.Infrastructure;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ControleFinanceiroPessoal.Repositories;

public class UsuarioRepository : GenericRepository<Usuario>
{
    public UsuarioRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        : base(mongoClient, settings)
    {
    }

    public async Task<Usuario> GetByEmailAsync(string email)
    {
        var filter = Builders<Usuario>.Filter.Eq(u => u.Email, email);
        var usuario = await _collection.Find(filter).FirstOrDefaultAsync();

        return usuario; // A propriedade IdString estará disponível
    }
}