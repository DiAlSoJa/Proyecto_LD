using SkiaSharp;

namespace MauiAppLogin.Common.Imaging;

public static class ImageCompressor
{
    public static Task<byte[]> ComprimirAsync(
        Stream original,
        int maxLado = 1920,
        int calidadJpeg = 80,
        CancellationToken ct = default)
    {
        return Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            using var bitmap = SKBitmap.Decode(original);
            if (bitmap is null)
                throw new InvalidOperationException("No se pudo decodificar la imagen.");
            return ComprimirBitmap(bitmap, maxLado, calidadJpeg);
        }, ct);
    }

    public static Task<byte[]> ComprimirAsync(
        byte[] original,
        int maxLado = 1920,
        int calidadJpeg = 80,
        CancellationToken ct = default)
    {
        return Task.Run(() =>
        {
            ct.ThrowIfCancellationRequested();
            using var bitmap = SKBitmap.Decode(original);
            if (bitmap is null)
                throw new InvalidOperationException("No se pudo decodificar la imagen.");
            return ComprimirBitmap(bitmap, maxLado, calidadJpeg);
        }, ct);
    }

    private static byte[] ComprimirBitmap(SKBitmap bitmap, int maxLado, int calidadJpeg)
    {
        int ancho     = bitmap.Width;
        int alto      = bitmap.Height;
        int ladoMayor = Math.Max(ancho, alto);

        SKBitmap destino      = bitmap;
        bool     redimensionado = false;

        if (ladoMayor > maxLado)
        {
            double escala  = (double)maxLado / ladoMayor;
            int nuevoAncho = (int)(ancho * escala);
            int nuevoAlto  = (int)(alto  * escala);

            destino        = bitmap.Resize(new SKImageInfo(nuevoAncho, nuevoAlto), SKFilterQuality.Medium);
            redimensionado = true;
        }

        try
        {
            using var image = SKImage.FromBitmap(destino);
            using var data  = image.Encode(SKEncodedImageFormat.Jpeg, calidadJpeg);
            return data.ToArray();
        }
        finally
        {
            if (redimensionado)
                destino.Dispose();
        }
    }
}
