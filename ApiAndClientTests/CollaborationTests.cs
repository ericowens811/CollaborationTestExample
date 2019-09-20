using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiAndClient.Client;
using ApiAndClient.Entities;
using ApiAndClientTests.Support;
using CollaborationTestExample.Jwt.Authentication;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiAndClientTests
{
    [TestClass]
    [TestCategory("Collaboration")]
    public class CollaborationTests
    {
        [TestMethod]
        public async Task Api_ThreeResources_ThreeMessagesEach()
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
                    }
                );
                var resource123Message1 = Encoding.ASCII.GetBytes("ABCD123");
                var resource123XMessage1 = new MessageEntity { ResourceId = 123, Message = resource123Message1 };

                var resource123Message2 = Encoding.ASCII.GetBytes("EFGH123");
                var resource123XMessage2 = new MessageEntity { ResourceId = 123, Message = resource123Message2 };

                var resource123Message3 = Encoding.ASCII.GetBytes("IJKL123");
                var resource123XMessage3 = new MessageEntity { ResourceId = 123, Message = resource123Message3 };

                var resource456Message1 = Encoding.ASCII.GetBytes("ABCD456");
                var resource456XMessage1 = new MessageEntity { ResourceId = 456, Message = resource456Message1 };

                var resource456Message2 = Encoding.ASCII.GetBytes("EFGH456");
                var resource456XMessage2 = new MessageEntity { ResourceId = 456, Message = resource456Message2 };

                var resource456Message3 = Encoding.ASCII.GetBytes("IJKL456");
                var resource456XMessage3 = new MessageEntity { ResourceId = 456, Message = resource456Message3 };

                var resource789Message1 = Encoding.ASCII.GetBytes("ABCD789");
                var resource789XMessage1 = new MessageEntity { ResourceId = 789, Message = resource789Message1 };

                var resource789Message2 = Encoding.ASCII.GetBytes("EFGH789");
                var resource789XMessage2 = new MessageEntity { ResourceId = 789, Message = resource789Message2 };

                var resource789Message3 = Encoding.ASCII.GetBytes("IJKL789");
                var resource789XMessage3 = new MessageEntity { ResourceId = 789, Message = resource789Message3 };



                testNowProvider.NextNow = 1000;
                await resourcesApiClient.AddMessageAsync(resource123XMessage1);

                testNowProvider.NextNow = 1010;
                await resourcesApiClient.AddMessageAsync(resource123XMessage2);

                testNowProvider.NextNow = 1020;
                await resourcesApiClient.AddMessageAsync(resource123XMessage3);

                testNowProvider.NextNow = 1030;
                await resourcesApiClient.AddMessageAsync(resource456XMessage1);

                testNowProvider.NextNow = 1040;
                await resourcesApiClient.AddMessageAsync(resource456XMessage2);

                testNowProvider.NextNow = 1050;
                await resourcesApiClient.AddMessageAsync(resource456XMessage3);

                testNowProvider.NextNow = 1060;
                await resourcesApiClient.AddMessageAsync(resource789XMessage1);

                testNowProvider.NextNow = 1070;
                await resourcesApiClient.AddMessageAsync(resource789XMessage2);

                testNowProvider.NextNow = 1080;
                await resourcesApiClient.AddMessageAsync(resource789XMessage3);

                testNowProvider.NextNow = 1200;
                var emptyMessagesToSend1 = await resourcesApiClient.GetMessagesToSendAsync();

                testNowProvider.NextNow = 2100;
                var messagesToSend2 = await resourcesApiClient.GetMessagesToSendAsync();
                var send1m123 = messagesToSend2.FindAll(m => m.ResourceId == 123);
                var send1m456 = messagesToSend2.FindAll(m => m.ResourceId == 456);
                var send1m789 = messagesToSend2.FindAll(m => m.ResourceId == 789);

                testNowProvider.NextNow = 2900;
                var emptyMessagesToSend3 = await resourcesApiClient.GetMessagesToSendAsync();

                testNowProvider.NextNow = 3101;
                var messagesToSend4 = await resourcesApiClient.GetMessagesToSendAsync();
                var send2m123 = messagesToSend4.FindAll(m => m.ResourceId == 123);
                var send2m456 = messagesToSend4.FindAll(m => m.ResourceId == 456);
                var send2m789 = messagesToSend4.FindAll(m => m.ResourceId == 789);

                testNowProvider.NextNow = 4100;
                var emptyMessagesToSend5 = await resourcesApiClient.GetMessagesToSendAsync();

                testNowProvider.NextNow = 4102;
                var messagesToSend6 = await resourcesApiClient.GetMessagesToSendAsync();
                var send3m123 = messagesToSend6.FindAll(m => m.ResourceId == 123);
                var send3m456 = messagesToSend6.FindAll(m => m.ResourceId == 456);
                var send3m789 = messagesToSend6.FindAll(m => m.ResourceId == 789);

                var finalResources = await resourcesApiClient.GetAllResourcesAsync();

                Assert.AreEqual(0, emptyMessagesToSend1.Count);
                Assert.AreEqual(3, messagesToSend2.Count);
                Assert.AreEqual(1, send1m123.Count);
                Assert.AreEqual(1, send1m456.Count);
                Assert.AreEqual(1, send1m789.Count);
                CollectionAssert.AreEqual(resource123Message1, send1m123[0].Message);
                CollectionAssert.AreEqual(resource456Message1, send1m456[0].Message);
                CollectionAssert.AreEqual(resource789Message1, send1m789[0].Message);

                Assert.AreEqual(0, emptyMessagesToSend3.Count);
                Assert.AreEqual(3, messagesToSend4.Count);
                Assert.AreEqual(1, send2m123.Count);
                Assert.AreEqual(1, send2m456.Count);
                Assert.AreEqual(1, send2m789.Count);
                CollectionAssert.AreEqual(resource123Message2, send2m123[0].Message);
                CollectionAssert.AreEqual(resource456Message2, send2m456[0].Message);
                CollectionAssert.AreEqual(resource789Message2, send2m789[0].Message);

                Assert.AreEqual(0, emptyMessagesToSend5.Count);
                Assert.AreEqual(3, messagesToSend6.Count);
                Assert.AreEqual(1, send3m123.Count);
                Assert.AreEqual(1, send3m456.Count);
                Assert.AreEqual(1, send3m789.Count);
                CollectionAssert.AreEqual(resource123Message3, send3m123[0].Message);
                CollectionAssert.AreEqual(resource456Message3, send3m456[0].Message);
                CollectionAssert.AreEqual(resource789Message3, send3m789[0].Message);
                   
                Assert.AreEqual(0, finalResources.Count);
            }
            finally
            {
                connection.Close();
            }
        }


        [TestMethod]
        public async Task Api_OneResource_ThreeMessages()
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
                    }
                );
                var message1 = Encoding.ASCII.GetBytes("ABCD");
                var xMessage1 = new MessageEntity {ResourceId = 123, Message = message1};

                var message2 = Encoding.ASCII.GetBytes("EFGH");
                var xMessage2 = new MessageEntity { ResourceId = 123, Message = message2 };

                var message3 = Encoding.ASCII.GetBytes("IJKL");
                var xMessage3 = new MessageEntity { ResourceId = 123, Message = message3 };

                testNowProvider.NextNow = 1000;
                await resourcesApiClient.AddMessageAsync(xMessage1);

                testNowProvider.NextNow = 1100;
                await resourcesApiClient.AddMessageAsync(xMessage2);

                testNowProvider.NextNow = 1150;
                await resourcesApiClient.AddMessageAsync(xMessage3);

                testNowProvider.NextNow = 1200;
                var emptyMessagesToSend1 = await resourcesApiClient.GetMessagesToSendAsync();

                testNowProvider.NextNow = 2001;
                var messagesToSend2 = await resourcesApiClient.GetMessagesToSendAsync();

                testNowProvider.NextNow = 2900;
                var emptyMessagesToSend3 = await resourcesApiClient.GetMessagesToSendAsync();

                testNowProvider.NextNow = 3100;
                var messagesToSend4 = await resourcesApiClient.GetMessagesToSendAsync();

                testNowProvider.NextNow = 4099;
                var emptyMessagesToSend5 = await resourcesApiClient.GetMessagesToSendAsync();

                testNowProvider.NextNow = 4101;
                var messagesToSend6 = await resourcesApiClient.GetMessagesToSendAsync();

                var finalResources = await resourcesApiClient.GetAllResourcesAsync();

                Assert.AreEqual(0, emptyMessagesToSend1.Count);
                Assert.AreEqual(1, messagesToSend2.Count);
                Assert.AreEqual("ABCD", Encoding.ASCII.GetString(messagesToSend2[0].Message));
                Assert.AreEqual(0, emptyMessagesToSend3.Count);
                Assert.AreEqual(1, messagesToSend4.Count);
                Assert.AreEqual("EFGH", Encoding.ASCII.GetString(messagesToSend4[0].Message));
                Assert.AreEqual(0, emptyMessagesToSend5.Count);
                Assert.AreEqual(1, messagesToSend6.Count);
                Assert.AreEqual("IJKL", Encoding.ASCII.GetString(messagesToSend6[0].Message));            
                Assert.AreEqual(0, finalResources.Count);
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
            var bridgeResourceUrls = new ResourceApiUrls()
            {
                ResourceUrl = "http://localhost/api/resources",
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
