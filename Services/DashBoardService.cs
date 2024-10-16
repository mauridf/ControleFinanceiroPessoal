using ControleFinanceiroPessoal.Entities;
using ControleFinanceiroPessoal.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleFinanceiroPessoal.Services;

public class DashBoardService
{
    private readonly GenericRepository<Credito> _creditoRepository;
    private readonly GenericRepository<Debito> _debitoRepository;
    private readonly GenericRepository<Reserva> _reservaRepository;
    private readonly GenericRepository<ReservaTransacao> _transacaoRepository;
    private readonly GenericRepository<SaldoFinal> _saldoFinalRepository;

    public DashBoardService(
        GenericRepository<Credito> creditoRepository,
        GenericRepository<Debito> debitoRepository,
        GenericRepository<Reserva> reservaRepository,
        GenericRepository<ReservaTransacao> transacaoRepository,
        GenericRepository<SaldoFinal> saldoFinalRepository)
    {
        _creditoRepository = creditoRepository;
        _debitoRepository = debitoRepository;
        _reservaRepository = reservaRepository;
        _transacaoRepository = transacaoRepository;
        _saldoFinalRepository = saldoFinalRepository;
    }

    // 1. Créditos por mês (Ano corrente)
    public async Task<IEnumerable<BsonDocument>> GetCreditosPorMesAsync()
    {
        var currentYear = DateTime.UtcNow.Year;
        var pipeline = new[]
        {
            new BsonDocument
            {
                { "$match", new BsonDocument
                    {
                        { "Data", new BsonDocument
                            {
                                { "$gte", new DateTime(currentYear, 1, 1) },
                                { "$lt", new DateTime(currentYear + 1, 1, 1) }
                            }
                        }
                    }
                }
            },
            new BsonDocument
            {
                { "$group", new BsonDocument
                    {
                        { "_id", new BsonDocument
                            {
                                { "mes", new BsonDocument("$month", "$Data") },
                                { "ano", new BsonDocument("$year", "$Data") }
                            }
                        },
                        { "total", new BsonDocument("$sum", "$Valor") }
                    }
                }
            },
            new BsonDocument { { "$sort", new BsonDocument("_id.mes", 1) } }
        };

        var result = await _creditoRepository.AggregateAsync(pipeline);
        return result;
    }

    // 2. Débitos por mês (Ano corrente)
    public async Task<IEnumerable<BsonDocument>> GetDebitosPorMesAsync()
    {
        var currentYear = DateTime.UtcNow.Year;
        var pipeline = new[]
        {
            new BsonDocument
            {
                { "$match", new BsonDocument
                    {
                        { "Data", new BsonDocument
                            {
                                { "$gte", new DateTime(currentYear, 1, 1) },
                                { "$lt", new DateTime(currentYear + 1, 1, 1) }
                            }
                        }
                    }
                }
            },
            new BsonDocument
            {
                { "$group", new BsonDocument
                    {
                        { "_id", new BsonDocument
                            {
                                { "mes", new BsonDocument("$month", "$Data") },
                                { "ano", new BsonDocument("$year", "$Data") }
                            }
                        },
                        { "total", new BsonDocument("$sum", "$Valor") }
                    }
                }
            },
            new BsonDocument { { "$sort", new BsonDocument("_id.mes", 1) } }
        };

        var result = await _debitoRepository.AggregateAsync(pipeline);
        return result;
    }

    // 3. Reservas por mês (Ano corrente)
    public async Task<IEnumerable<BsonDocument>> GetReservasPorMesAsync()
    {
        var currentYear = DateTime.UtcNow.Year;
        var pipeline = new[]
        {
            new BsonDocument
            {
                { "$match", new BsonDocument
                    {
                        { "Data", new BsonDocument
                            {
                                { "$gte", new DateTime(currentYear, 1, 1) },
                                { "$lt", new DateTime(currentYear + 1, 1, 1) }
                            }
                        }
                    }
                }
            },
            new BsonDocument
            {
                { "$group", new BsonDocument
                    {
                        { "_id", new BsonDocument
                            {
                                { "mes", new BsonDocument("$month", "$Data") },
                                { "ano", new BsonDocument("$year", "$Data") }
                            }
                        },
                        { "total", new BsonDocument("$sum", "$Valor") }
                    }
                }
            },
            new BsonDocument { { "$sort", new BsonDocument("_id.mes", 1) } }
        };

        var result = await _reservaRepository.AggregateAsync(pipeline);
        return result;
    }

    // 4. Saldo Final por mês (Ano corrente)
    public async Task<IEnumerable<BsonDocument>> GetSaldoFinalPorMesAsync()
    {
        var currentYear = DateTime.UtcNow.Year;
        var pipeline = new[]
        {
            new BsonDocument
            {
                { "$match", new BsonDocument
                    {
                        { "Data", new BsonDocument
                            {
                                { "$gte", new DateTime(currentYear, 1, 1) },
                                { "$lt", new DateTime(currentYear + 1, 1, 1) }
                            }
                        }
                    }
                }
            },
            new BsonDocument
            {
                { "$group", new BsonDocument
                    {
                        { "_id", new BsonDocument
                            {
                                { "mes", new BsonDocument("$month", "$Data") },
                                { "ano", new BsonDocument("$year", "$Data") }
                            }
                        },
                        { "total", new BsonDocument("$sum", "$ValorFinal") }
                    }
                }
            },
            new BsonDocument { { "$sort", new BsonDocument("_id.mes", 1) } }
        };

        var result = await _saldoFinalRepository.AggregateAsync(pipeline);
        return result;
    }

    // 5. Total de uso de reservas (Ano corrente)
    public async Task<decimal> GetTotalUsoReservasAsync()
    {
        var currentYear = DateTime.UtcNow.Year;
        var filter = Builders<ReservaTransacao>.Filter.Gte("Data", new DateTime(currentYear, 1, 1)) &
                     Builders<ReservaTransacao>.Filter.Lt("Data", new DateTime(currentYear + 1, 1, 1));

        var total = await _transacaoRepository.GetCollection().Aggregate()
            .Match(filter)
            .Group(new BsonDocument { { "_id", BsonNull.Value }, { "total", new BsonDocument("$sum", "$Valor") } })
            .FirstOrDefaultAsync();

        return total != null ? total["total"].ToDecimal() : 0;
    }

    // 6. Saldo atual das reservas (Possível ver por período)
    public async Task<decimal> GetSaldoAtualReservasAsync(DateTime? inicio, DateTime? fim)
    {
        var filter = Builders<Reserva>.Filter.Empty;
        if (inicio.HasValue && fim.HasValue)
        {
            filter = Builders<Reserva>.Filter.Gte("Data", inicio.Value) &
                     Builders<Reserva>.Filter.Lte("Data", fim.Value);
        }

        var total = await _reservaRepository.GetCollection().Aggregate()
            .Match(filter)
            .Group(new BsonDocument { { "_id", BsonNull.Value }, { "saldoAtual", new BsonDocument("$sum", "$Saldo") } })
            .FirstOrDefaultAsync();

        return total != null ? total["saldoAtual"].ToDecimal() : 0;
    }

    // 7. Histórico de transações (Uso da reserva ao longo do tempo)
    public async Task<IEnumerable<BsonDocument>> GetHistoricoTransacoesAsync()
    {
        var pipeline = new[]
        {
            new BsonDocument
            {
                { "$group", new BsonDocument
                    {
                        { "_id", new BsonDocument
                            {
                                { "mes", new BsonDocument("$month", "$Data") },
                                { "ano", new BsonDocument("$year", "$Data") }
                            }
                        },
                        { "total", new BsonDocument("$sum", "$Valor") }
                    }
                }
            },
            new BsonDocument { { "$sort", new BsonDocument("_id.mes", 1) } }
        };

        var result = await _transacaoRepository.AggregateAsync(pipeline);
        return result;
    }

    // 8. Evolução do uso de reservas por mês
    public async Task<IEnumerable<BsonDocument>> GetEvolucaoUsoReservasPorMesAsync()
    {
        var currentYear = DateTime.UtcNow.Year;
        var pipeline = new[]
        {
            new BsonDocument
            {
                { "$match", new BsonDocument
                    {
                        { "Data", new BsonDocument
                            {
                                { "$gte", new DateTime(currentYear, 1, 1) },
                                { "$lt", new DateTime(currentYear + 1, 1, 1) }
                            }
                        }
                    }
                }
            },
            new BsonDocument
            {
                { "$group", new BsonDocument
                    {
                        { "_id", new BsonDocument
                            {
                                { "mes", new BsonDocument("$month", "$Data") },
                                { "ano", new BsonDocument("$year", "$Data") }
                            }
                        },
                        { "total", new BsonDocument("$sum", "$Valor") }
                    }
                }
            },
            new BsonDocument { { "$sort", new BsonDocument("_id.mes", 1) } }
        };

        var result = await _transacaoRepository.AggregateAsync(pipeline);
        return result;
    }

    // 9. Valores adicionados ou usados das reservas em determinado período
    public async Task<IEnumerable<BsonDocument>> GetValoresReservasPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        // Criação do filtro para o período
        var filter = Builders<ReservaTransacao>.Filter.Gte(r => r.DataTransacao, inicio) &
                     Builders<ReservaTransacao>.Filter.Lte(r => r.DataTransacao, fim);

        var pipeline = new[]
        {
        new BsonDocument
        {
            { "$match", BsonDocument.Parse(filter.ToJson()) } // Convertendo o filtro para BsonDocument
        },
        new BsonDocument
        {
            { "$group", new BsonDocument
                {
                    { "_id", new BsonDocument
                        {
                            { "ano", new BsonDocument("$year", "$Data") },
                            { "mes", new BsonDocument("$month", "$Data") }
                        }
                    },
                    { "totalAdicionado", new BsonDocument("$sum", "$ValorAdicionado") },
                    { "totalUsado", new BsonDocument("$sum", "$ValorUsado") }
                }
            }
        },
        new BsonDocument
        {
            { "$sort", new BsonDocument
                {
                    { "_id.ano", 1 },
                    { "_id.mes", 1 }
                }
            }
        }
    };

        return await _transacaoRepository.AggregateAsync(pipeline);
    }

    // 10. Reservas acumuladas e saldo atual das reservas
    public async Task<(decimal reservasAcumuladas, decimal saldoAtual)> GetReservasAcumuladasESaldoAsync()
    {
        var reservasAcumuladas = await _reservaRepository.GetCollection()
            .Aggregate()
            .Group(new BsonDocument
            {
            { "_id", BsonNull.Value },
            { "total", new BsonDocument("$sum", "$Saldo") }
            })
            .FirstOrDefaultAsync();

        var saldoAtual = await GetSaldoAtualReservasAsync(null, null);

        return (reservasAcumuladas != null ? reservasAcumuladas["total"].ToDecimal() : 0, saldoAtual);
    }
}
