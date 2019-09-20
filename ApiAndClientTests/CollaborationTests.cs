using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiAndClient.Client;
using ApiAndClient.Entities;
using ApiAndClientTests.Support;
using CollaborationTestExample.Jwt.Authentication;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiAndClientTests
{
    [TestClass]
    [TestCategory("Collaboration")]
    public class CollaborationTests
    {
        [TestMethod]
        public async Task Get_Resources_After_Initialization()
        {
            var guid = Guid.NewGuid().ToString();
            var connectionString = GetTestConnectionString(guid);
            var connection = new SqliteConnection(connectionString);
            var testNowProvider = new TestNowProvider();
            var resourcesApiClient = GetResourceApiClient(connectionString, testNowProvider);

            try
            {
                DbInitializer.Initialize(connection, TestContextProvider.GetContext,
                    new List<IEnumerable<object>>
                    {
                        new List<ResourceEntity>
                        {
                            new ResourceEntity
                            {
                                Id = 1, ResourceId = 1,
                                Messages = new List<MessageEntity>
                                {
                                    new MessageEntity {Id = 1, ResourceId = 1, Message="Message1 in Resource1" },
                                    new MessageEntity {Id = 2, ResourceId = 1, Message="Message2 in Resource1" }
                                }
                            },
                            new ResourceEntity
                            {
                                Id = 2, ResourceId = 2,
                                Messages = new List<MessageEntity>
                                {
                                    new MessageEntity {Id = 3, ResourceId = 2, Message="Message3 in Resource2" },
                                    new MessageEntity {Id = 4, ResourceId = 2, Message="Message4 in Resource2" }
                                }
                            }
                        }
                    }
                );

                var messages = await resourcesApiClient.GetAllMessagesAsync();
                var resources = await resourcesApiClient.GetAllResourcesAsync();

                var message5 = new MessageEntity {ResourceId = 2, Message = "Message5 in Resource2"};
                await resourcesApiClient.AddMessageAsync(message5);

                var messages2 = await resourcesApiClient.GetAllMessagesAsync();
                var resources2 = await resourcesApiClient.GetAllResourcesAsync();

                var resourceId2 = resources2.FirstOrDefault(r => r.ResourceId == 2);

                var resource3 = new ResourceEntity {ResourceId = 3};
                await resourcesApiClient.AddResourceAsync(resource3);
                var resources3 = await resourcesApiClient.GetAllResourcesAsync();

                Assert.AreEqual(4, messages.Count);
                Assert.AreEqual(5, messages2.Count);
                Assert.AreEqual(2, resources.Count);
                Assert.AreEqual(3, resourceId2?.Messages?.Count);
                Assert.AreEqual(3, resources3.Count);
            }
            finally
            {
                connection.Close();
            }
        }

        /*
         * Test support methods
         *
         */

        public static TestJwtProvider JwtProvider = new TestJwtProvider { DefaultClaims = new Dictionary<string, string> { { "jobLevel", "Level9" } } };

        public static ResourceApiClient GetResourceApiClient(string connectionString, TestNowProvider nowProvider)
        {
            var client = GetTestClient(connectionString, nowProvider);
            var requestBuilder = new RequestBuilder(JwtProvider);
            var baseUrl = "http://localhost/api/";
            var bridgeResourceUrls = new ResourceApiUrls()
            {
                ResourceUrl = $"{baseUrl}resources",
                MessageUrl = $"{baseUrl}messages"
            };
            return new ResourceApiClient(client, requestBuilder, bridgeResourceUrls);
        }

        public static string GetTestConnectionString(string guid)
        {
            var connectionString = $"DataSource={guid};Mode=Memory;Cache=Shared";
            return connectionString;
        }

        public static TestClient GetTestClient(string connectionString, TestNowProvider nowProvider)
        {
            var connectionStringProvider = new TestConnectionStringProvider(connectionString);
            var testServer = TestServerBuilder.CreateServer<TestStartup>(connectionStringProvider, nowProvider);
            return new TestClient(testServer.CreateClient());
        }
    }
}
