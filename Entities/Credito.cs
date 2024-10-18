using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ControleFinanceiroPessoal.Entities;

public class Credito : IEntity
{
    [BsonId]
    public ObjectId Id { get; set; }

    // Propriedade para expor o ID como string
    [BsonIgnore]
    public string IdString => Id.ToString();

    [BsonRequired]
    public string UsuarioId { get; set; } // Aqui está como string

    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
}
