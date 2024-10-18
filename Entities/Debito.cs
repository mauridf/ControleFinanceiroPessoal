using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ControleFinanceiroPessoal.Entities;

public class Debito : IEntity
{
    [BsonId]
    public ObjectId Id { get; set; }
    // Propriedade para expor o ID como string
    [BsonIgnore]
    public string IdString => Id.ToString();
    [BsonRequired]
    public ObjectId UsuarioId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public bool Pago { get; set; } = false;
}
