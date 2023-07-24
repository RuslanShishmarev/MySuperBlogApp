using Newtonsoft.Json;

namespace MySuperBlogApp.Services
{
    public class ImageService
    {
        public static byte[] GetPhoto(string photo)
        {
            try
            {
                return JsonConvert.DeserializeObject<byte[]>(photo);
            }
            catch
            {
                try
                {
                    return JsonConvert.DeserializeObject<byte[]>("[" + photo + "]");
                }
                catch
                {
                    return Array.Empty<byte>();
                }
            }
        }
    }
}
