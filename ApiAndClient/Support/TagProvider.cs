using System;
using ApiAndClient.Interfaces;

namespace ApiAndClient.Support
{
    public class TagProvider : ITagProvider
    {
        public string NextTag()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
