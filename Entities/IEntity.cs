using MongoDB.Bson;

namespace ControleFinanceiroPessoal.Entities;

public interface IEntity
{
    ObjectId Id { get; set; }
}
