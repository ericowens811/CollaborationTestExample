using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ApiAndClient.Entities
{
    public class ResourceEntity
    {
        public const long DefaultWaitTimeInMilliseconds = 1000;

        public int Id { get; set; }
        public long ResourceId { get; set; }

        public List<MessageEntity> Messages { get; set; }

        public ResourceEntity()
        {
        }

        public ResourceEntity(long resourceId)
        {
            ResourceId = resourceId;
            Messages = new List<MessageEntity>();
        }

        public void AddMessage(MessageEntity message, long nowInMilliseconds)
        {
            Messages.Add(message);
        }

    }
}
