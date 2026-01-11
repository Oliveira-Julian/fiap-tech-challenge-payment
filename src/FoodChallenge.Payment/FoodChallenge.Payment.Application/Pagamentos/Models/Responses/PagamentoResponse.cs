namespace FoodChallenge.Payment.Application.Pagamentos.Models.Responses
{
    public sealed class PagamentoResponse
    {
        public Guid IdPagamento { get; set; }
        public string QrCode { get; set; }
        public int Status { get; set; }
        public string DescricaoStatus { get; set; }
        public string IdMercadoPagoPagamento { get; set; }
    }
}
