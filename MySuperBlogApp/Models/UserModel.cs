using Newtonsoft.Json;
using System.Text;

namespace MySuperBlogApp.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Description { get; set; }

        public string Photo { get; set; }

        public byte[] GetPhoto()
        {
            try
            {
                return JsonConvert.DeserializeObject<byte[]>(Photo);
            }
            catch
            {
                try
                {
                    return JsonConvert.DeserializeObject<byte[]>("[" + Photo + "]");
                }
                catch
                {
                    return Array.Empty<byte>();
                }
            }
        }
    }
}
