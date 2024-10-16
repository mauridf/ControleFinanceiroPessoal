using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ControleFinanceiroPessoal.Entities;

public class SaldoFinal : IEntity
{
    [BsonId]
    public ObjectId Id { get; set; }
    [BsonRequired]
    public ObjectId UsuarioId { get; set; }
    public decimal Valor { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
}
