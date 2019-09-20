using System.Collections.Generic;

namespace CollaborationTestExample.Jwt.Authentication
{
    public class TestJwtProvider : IProvideJwt
    {
        public Dictionary<string, string> DefaultClaims { get; set; }

        public string GetJsonWebToken(Dictionary<string, string> customClaims = null)
        {
            var claimsList = new List<TestClaim>();
            var claimDictionary = customClaims ?? DefaultClaims;
            foreach (var keyValuePair in claimDictionary)
            {
                claimsList.Add(new TestClaim { Name = keyValuePair.Key, Value = keyValuePair.Value });
            }
            var jwtTokenBuilder = new TestTokenGenerator();
            var token = jwtTokenBuilder.CreateTestToken(claimsList);
            return token;
        }
    }
}
