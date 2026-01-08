using System.ComponentModel;

namespace FoodChallenge.Payment.Domain.Enums;

public enum PreparoStatus
{
    [Description("Na Fila Para Preparação")]
    NaFilaParaPreparacao = 1,
    [Description("Em Preparação")]
    EmPreparacao = 2,
    [Description("Preparação Concluida")]
    Concluido = 3
}
