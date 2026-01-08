using QRCoder;

namespace FoodChallenge.Payment.Application.Helpers;

public static class QrCodeHelper
{
    public static byte[] GerarImagem(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return default;

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(texto, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(5);
    }
}
