using System.Text;
using Newtonsoft.Json;

namespace ApiAndClient.Entities
{
    public class MessageEntity
    {
        public int Id { get; set; }

        public long ResourceId { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public ResourceEntity Resource { get; set; }

        public override string ToString()
        {
            return $"ResourceId: {ResourceId} Message: {Message}";
        }
    }
}
