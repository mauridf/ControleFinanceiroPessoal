using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ControleFinanceiroPessoal.Entities;

public class Usuario : IEntity
{
    [BsonId]
    public ObjectId Id { get; set; }

    // Propriedade para expor o ID como string
    [BsonIgnore]
    public string IdString => Id.ToString();

    [BsonRequired]
    public string Nome { get; set; }

    [BsonRequired]
    public string Email { get; set; }

    [BsonRequired]
    public string SenhaHash { get; set; }

    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
}
