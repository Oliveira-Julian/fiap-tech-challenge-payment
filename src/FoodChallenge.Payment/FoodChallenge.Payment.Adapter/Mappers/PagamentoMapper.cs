using FoodChallenge.Infrastructure.Data.Postgres.Mongo.Repositories.Pedidos;
using FoodChallenge.Payment.Application.Pagamentos.Models.Requests;
using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Pagamentos;

namespace FoodChallenge.Payment.Adapter.Mappers;

public static class PagamentoMapper
{
    public static Pagamento ToDomain(PagamentoEntity pagamentoEntity)
    {
        if (pagamentoEntity is null) return default;

        return new Pagamento
        {
            Id = pagamentoEntity.Id,
            IdPedido = pagamentoEntity.IdPedido,
            IdMercadoPagoOrdem = pagamentoEntity.IdMercadoPagoOrdem,
            IdMercadoPagoPagamento = pagamentoEntity.IdMercadoPagoPagamento,
            DataAtualizacao = pagamentoEntity.DataAtualizacao,
            DataCriacao = pagamentoEntity.DataCriacao,
            DataExclusao = pagamentoEntity.DataExclusao,
            Ativo = pagamentoEntity.Ativo,
            ChaveMercadoPagoOrdem = pagamentoEntity.ChaveMercadoPagoOrdem,
            Metodo = (PagamentoMetodo)pagamentoEntity.Metodo,
            QrCode = pagamentoEntity.QrCode,
            Status = (PagamentoStatus)pagamentoEntity.Status,
            Valor = pagamentoEntity.Valor
        };
    }

    public static PagamentoEntity ToEntity(Pagamento pagamento)
    {
        if (pagamento is null) return default;

        return new PagamentoEntity
        {
            Id = pagamento.Id,
            IdPedido = pagamento.IdPedido,
            IdMercadoPagoOrdem = pagamento.IdMercadoPagoOrdem,
            IdMercadoPagoPagamento = pagamento.IdMercadoPagoPagamento,
            DataAtualizacao = pagamento.DataAtualizacao,
            DataCriacao = pagamento.DataCriacao,
            DataExclusao = pagamento.DataExclusao,
            Ativo = pagamento.Ativo,
            ChaveMercadoPagoOrdem = pagamento.ChaveMercadoPagoOrdem,
            Metodo = (int)pagamento.Metodo,
            QrCode = pagamento.QrCode,
            Status = (int)pagamento.Status,
            Valor = pagamento.Valor
        };
    }

    public static NotificacaoMercadoPago ToDomain(WebhookMercadoPagoPagamentoRequest request)
    {
        if (request is null) return default;

        return new NotificacaoMercadoPago
        {
            Id = request.Data?.Id,
            Acao = request.Action,
            DataCriacao = request.DateCreated,
            Tipo = request.Type
        };
    }
}
