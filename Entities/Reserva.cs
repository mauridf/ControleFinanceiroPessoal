using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ControleFinanceiroPessoal.Entities;

public class Reserva : IEntity
{
    [BsonId]
    public ObjectId Id { get; set; }
    // Propriedade para expor o ID como string
    [BsonIgnore]
    public string IdString => Id.ToString();
    [BsonRequired]
    public ObjectId UsuarioId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal SaldoAtual { get; set; } // Saldo restante da reserva
    public decimal ValorInicial { get; set; } // Valor inicial da reserva
    public int Mes { get; set; }
    public int Ano { get; set; }
}
