namespace Microsoft.Maui.Graphics;

using Microsoft.Maui.Graphics.Platform;
using System.Reflection;

public static class IImageExtensions
{
    public static Size GetDimensions(this IImage image, int maxDimension = int.MaxValue)
    {
        var width = image.Width;
        var height = image.Height;
        if (width > maxDimension || height > maxDimension)
        {
            var ratio = width / (double)height;
            if (ratio > 1)
            {
                width = maxDimension;
                height = (int)(width / ratio);
            }
            else
            {
                height = maxDimension;
                width = (int)(height * ratio);
            }
        }
        return new Size(width, height);
    }

    public static IImage ToImage(this Stream stream) => PlatformImage.FromStream(stream);

    public static Uri ToDataUri(this IImage image, string mimeType = "image/png")
    {
        var bytes = image.AsBytes();
        return new Uri($"data:{mimeType};base64,{Convert.ToBase64String(bytes)}");
    }
}
