namespace FoodChallenge.Infrastructure.Clients.Orders.Constants;

public static class Logs
{
    public const string InicioExecucao = "Executa o cliente do FoodChallenge Orders. Endpoint: {Endpoint}";
    public const string FimExecucao = "Retorno da execução do cliente do FoodChallenge Orders. Endpoint: {@Endpoint}. Response: {@Response}";
    public const string ErroResponse = "Ocorreu um erro no retorno do FoodChallenge Orders. Endpoint: {Endpoint}. StatusCode: {StatusCode}. Mensagem: {@Mensagem}";
    public const string ErroGenerico = "Ocorreu um erro ao executar o cliente do FoodChallenge Orders. Endpoint: {Endpoint}";
}
