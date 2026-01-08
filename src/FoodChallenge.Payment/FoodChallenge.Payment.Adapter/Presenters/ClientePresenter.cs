using FoodChallenge.CrossCutting.Paging;
using FoodChallenge.Payment.Application.Clientes.Models.Responses;
using FoodChallenge.Payment.Domain.Clientes;

namespace FoodChallenge.Adapter.Presenters;

public static class ClientePresenter
{
    public static ClienteResponse ToResponse(Cliente cliente)
    {
        if (cliente is null) return default;

        return new ClienteResponse()
        {
            Id = cliente.Id,
            Cpf = cliente.Cpf?.Valor,
            Nome = cliente.Nome,
            Email = cliente.Email?.Valor
        };
    }

    public static Pagination<ClienteResponse> ToPaginationResponse(Pagination<Cliente> pagination)
    {
        if (pagination is null) return default;

        return new Pagination<ClienteResponse>(
            pagination.Page,
            pagination.SizePage,
            pagination.TotalRecords,
            pagination.Records?.Select(ToResponse));
    }
}
