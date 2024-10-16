using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ControleFinanceiroPessoal.Entities
{
    public class ReservaTransacao : IEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId ReservaId { get; set; }
        public decimal Valor { get; set; } // Valor utilizado ou adicionado
        public DateTime DataTransacao { get; set; }
        public string TipoTransacao { get; set; } // "Uso" ou "Adição"
        public string Descricao { get; set; } // Ex.: "Uso para compra de carro" ou "Adicionado da sobra do mês corrente"
    }
}
