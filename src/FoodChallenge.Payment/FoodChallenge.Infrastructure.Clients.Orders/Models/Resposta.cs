namespace FoodChallenge.Infrastructure.Clients.Orders.Models;

public class Resposta<T>
{
    public bool Sucesso { get; set; }
    public IEnumerable<string> Mensagens { get; set; }
    public T Dados { get; set; }
}
