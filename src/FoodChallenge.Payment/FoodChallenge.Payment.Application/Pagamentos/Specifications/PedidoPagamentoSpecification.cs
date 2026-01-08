using FluentValidation;
using FoodChallenge.Common.Extensions;
using FoodChallenge.Common.Validators;
using FoodChallenge.Payment.Domain.Globalization;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Application.Pagamentos.Specifications;

public sealed class PedidoPagamentoSpecification : FluentValidatorBase<Pedido>
{
    public PedidoPagamentoSpecification()
    {
        RuleFor(pedido => pedido)
            .Must(pedido => pedido.PodeSerPago())
            .WithMessage(pedido => string.Format(Textos.PagamentoStatusNaoPermitido, pedido.Status.GetDescription()));
    }
}
