using System.Security.Cryptography;

namespace NvsMarketFlow.Application.Services.BarCode;

public class BarCodeService : IBarCodeService
{
    public string Generate()
    {
        // Gera um código numérico de 12 dígitos (UPC-like)
        var bytes = new byte[6];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        var number = BitConverter.ToUInt64(bytes) % 1_000_000_000_000;

        return number.ToString("D12");
    }
}