using ImageMagick;

namespace Shop.Infrastructures
{
    public interface ImageService
    {
        byte[] GetThumbnail(byte[] fileBytes, int size);
    }

    public class MagickImageService : ImageService
    {
        public byte[] GetThumbnail(byte[] fileBytes, int size)
        {
            using var image = new MagickImage(fileBytes);
            image.Resize(size, size);
            return image.ToByteArray();
        }
    }
}
