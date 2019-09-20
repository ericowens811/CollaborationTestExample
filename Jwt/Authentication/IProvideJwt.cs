using System.Collections.Generic;

namespace CollaborationTestExample.Jwt.Authentication
{
    public interface IProvideJwt
    {
        Dictionary<string, string> DefaultClaims { get; set; }
        string GetJsonWebToken(Dictionary<string, string> customClaims = null);
    }
}
