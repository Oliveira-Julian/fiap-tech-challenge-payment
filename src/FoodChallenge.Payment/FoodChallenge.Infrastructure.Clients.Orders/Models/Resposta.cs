namespace FoodChallenge.Infrastructure.Clients.Orders.Models;

public class Resposta
{
    public bool Sucesso { get; set; }
    public IEnumerable<string> Mensagens { get; set; }
}

public class Resposta<T> : Resposta
{
    public T Dados { get; set; }
}
