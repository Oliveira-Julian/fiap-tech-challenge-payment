using System.Text.Json.Serialization;

namespace FoodChallenge.Payment.Application.Pagamentos.Models.Requests;

public class WebhookMercadoPagoPagamentoRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("date_created")]
    public DateTime DateCreated { get; set; }

    [JsonPropertyName("data")]
    public WebhookData Data { get; set; }

    public class WebhookData
    {
        public string Id { get; set; }
    }
}
