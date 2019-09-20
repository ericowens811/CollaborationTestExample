using System.Text;
using BridgePacketRateLimiterApi.Entities;
using Newtonsoft.Json;

namespace ApiAndClient.Entities
{
    // byte[] bytes = Encoding.ASCII.GetBytes(someString);
    // string someString = Encoding.ASCII.GetString(bytes);
    public class MessageEntity
    {
        public int Id { get; set; }

        // ResourceId is for the Bridge itself
        public long ResourceId { get; set; }
        public byte[] Message { get; set; }
        [JsonIgnore]
        public ResourceEntity Resource { get; set; }

        public override string ToString()
        {
            return $"ResourceId: {ResourceId} Message: {Encoding.ASCII.GetString(Message)}";
        }
    }
}
