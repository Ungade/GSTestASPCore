using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

//using UserStat;

namespace UserStatTest
{
    [TestClass]
    public class UnitTestAPI
    {
        private static CustomWebApplicationFactory<UserStat.Startup> factory;
        private static HttpClient client;
        private static TestContext _testContext;

        [ClassInitialize]
        public static void SetUp(TestContext testContext)
        {
            factory = new CustomWebApplicationFactory<UserStat.Startup>();
            client = factory.CreateClient();
            _testContext = testContext;
        }        

        [TestMethod]
        public async Task APIPost_ShouldReturnCREATED_And_QueryGuid()
        {
            var request = CreateClientPostRequest();            
            var response = await client.PostAsync(request.Uri,request.Content);
            _testContext.WriteLine(request.Uri.ToString());
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.Created,response.StatusCode);
            var res = await response.Content.ReadAsStringAsync();
            System.Guid resGuid = System.Guid.NewGuid();
            try
            {
                resGuid = System.Guid.Parse(res);
            }
            catch(System.FormatException)
            {
                _testContext.WriteLine("Response is not Guid format = "+res); 
                               
            }

            _testContext.WriteLine("Response queryGuid = "+resGuid.ToString());
            Assert.AreEqual(resGuid.ToString(),res);
        }

        private ClientRequest CreateClientPostRequest()
        {
            var uri = new System.Uri(@"http://localhost:5001/report/user_statistics");
            var userId = "123";
            var userQuery = JsonContent.Create(new {UserID=userId});
            return new ClientRequest(){ Uri=uri,Content=userQuery};
        }
        private ClientRequest CreateClientGetRequest(string queryGUID)
        {
            var uri = new System.Uri(@"http://localhost:5001/report/info"+@"/"+queryGUID);
            var content = JsonContent.Create(new {queryGUID=queryGUID});
            return new ClientRequest(){ Uri=uri, Content=content};
        }

        private struct ClientRequest
        {
            public System.Uri Uri{get;set;}
            public JsonContent Content{get; set;}
            
        }

        [TestMethod]
        public async Task APIGET_ShouldReturnOk_And_QueryGUID()
        {
            var postRequest = CreateClientPostRequest();
            var postResponse = await client.PostAsync(postRequest.Uri,postRequest.Content);
            var queryGuid = await postResponse.Content.ReadAsStringAsync();
            var getRequest = CreateClientGetRequest(queryGuid);
            var getResponse = await client.GetAsync(getRequest.Uri);
            getResponse.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);
            var data = await getResponse.Content.ReadFromJsonAsync<UserStat.Models.Query>();
            Assert.AreEqual(queryGuid,data.QueryGuid);
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            client.Dispose();
            factory.Dispose();
        }

    }
}