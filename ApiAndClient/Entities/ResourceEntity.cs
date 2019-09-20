using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ApiAndClient.Entities;

namespace BridgePacketRateLimiterApi.Entities
{
    public class ResourceEntity
    {
        public const long DefaultWaitTimeInMilliseconds = 1000;

        public int Id { get; set; }
        public long ResourceId { get; set; }

        public long NextTimeInMilliseconds { get; set; }

        public List<MessageEntity> Messages { get; set; }

        [Timestamp] // this is the ConcurrencyToken
        public byte[] Timestamp { get; set; }
        public string Tag { get; set; }
        public long TagOrphanedTime { get; set; }

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
            if (NextTimeInMilliseconds == 0)  // if there are already messages in the queue, the next time is already set
            {
                NextTimeInMilliseconds = nowInMilliseconds + DefaultWaitTimeInMilliseconds;
            }
        }

        public MessageEntity GetNextMessageToSend()
        {
            var nextMessage = Messages.OrderBy(v => v.Id).First();
            Messages.Remove(nextMessage);
            return nextMessage;
        }

        public void ClearTags()
        {
            Tag = null;
            TagOrphanedTime = 0;
        }
    }
}
