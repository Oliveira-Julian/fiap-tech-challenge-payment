using FoodChallenge.Payment.Domain.Entities;
using FoodChallenge.Payment.Domain.Enums;
using FoodChallenge.Payment.Domain.Pedidos;

namespace FoodChallenge.Payment.Domain.Preparos;

public class OrdemPedido : DomainBase
{
    public Guid? IdPedido { get; set; }
    public Pedido Pedido { get; set; }
    public PreparoStatus Status { get; set; } = PreparoStatus.NaFilaParaPreparacao;
    public DateTime? DataInicioPreparacao { get; set; }
    public DateTime? DataFimPreparacao { get; set; }

    public OrdemPedido()
    {
    }

    public void Cadastrar(Guid idPedido)
    {
        Id = Guid.NewGuid();
        IdPedido = idPedido;
    }

    public bool PermitirAlterarStatus(PreparoStatus status)
    {
        return Status switch
        {
            PreparoStatus.NaFilaParaPreparacao => status == PreparoStatus.EmPreparacao,
            PreparoStatus.EmPreparacao => status == PreparoStatus.Concluido,
            _ => false
        };
    }

    public void IniciarPreparacao()
    {
        Status = PreparoStatus.EmPreparacao;
        DataInicioPreparacao = DateTime.UtcNow;
        Atualizar();
    }

    public void FinalizarPreparacao()
    {
        Status = PreparoStatus.Concluido;
        DataFimPreparacao = DateTime.UtcNow;
        Atualizar();
    }
}
