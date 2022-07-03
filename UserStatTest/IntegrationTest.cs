using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;




namespace UserStatTest
{
    [TestClass]
    public class IntegrationTest
    {
        private static CustomWebApplicationFactory<UserStat.Startup> factory;
        private static HttpClient client;
        private static TestContext _testContext;

        public TestContext TestContext
        {
            get{ return _testContext;}
            set{_testContext = value;}
        }

        [ClassInitialize]
        public static void SetUp(TestContext testContext)
        {
            factory = new CustomWebApplicationFactory<UserStat.Startup>();
            _testContext = testContext;
            client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
        }

        [TestMethod]        
        public async Task PostWithUserID_ShouldCreateUserAndReturnGUID_or_ReturnExistingGUID()
        {
            var uri = new System.Uri(@"https://localhost:5001/report/user_statistics");
            var userId = "123";
            var request = JsonContent.Create(new { UserId=userId});
            var response = await client.PostAsync(uri, request);
            var res = await response.Content.ReadAsStringAsync();
            System.Guid resGuid = System.Guid.NewGuid();
            try
            {
                resGuid = System.Guid.Parse(res);
            }
            catch(System.FormatException)
            {
                TestContext.WriteLine("Response is not Guid format = "+res); 
                               
            }

            TestContext.WriteLine("Response queryGuid = "+resGuid.ToString());
            Assert.AreEqual(resGuid.ToString(),res);
            await GetByQueryID_WhenProccesLess100Percent_ShouldReturn_QueryResulAsNull(resGuid.ToString(), userId);
            await WaitFor_BackGroundHostService_Calculate(resGuid.ToString());
            await WhenProgressIs100Percent_Then_APIShouldReturn_UserID_And_CountSignIn(userId,resGuid.ToString());
        }

       
        private async Task GetByQueryID_WhenProccesLess100Percent_ShouldReturn_QueryResulAsNull( string queryGuid, string userId)
        {
            var uri = new System.Uri(@"https://localhost:5001/report/info/"+queryGuid);
            var response = await client.GetAsync(uri);
            var result = await response.Content.ReadFromJsonAsync<UserStat.Models.Query>();
            Assert.AreEqual(queryGuid,result.QueryGuid);
            Assert.IsNull(result.QueryResult);
            
        }

        private async Task WaitFor_BackGroundHostService_Calculate(string queryGuid)
        {
            var wait = 20;
            using (var scope = factory.Services.CreateScope())
            {
                var conf = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var parsed = int.TryParse( conf["ProccesTime"], out wait);
            }

            var timer = 0;
            var timerPlus = 100;
            while (timer < wait)
            {
                timer += timerPlus;
                await Task.Delay(timerPlus);
            }
        }

        private async Task WhenProgressIs100Percent_Then_APIShouldReturn_UserID_And_CountSignIn(string userID, string queryGuid)
        {
            var uri = new System.Uri(@"https://localhost:5001/report/info/" + queryGuid);
            var response = await client.GetAsync(uri);
            var result = await response.Content.ReadFromJsonAsync<UserStat.Models.Query>();

            Assert.AreEqual(100, result.Percent);
            Assert.AreEqual(userID,result.QueryResult.UserId);
            Assert.IsTrue(result.QueryResult.Count_sign_in>=1);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            client.Dispose();
            factory.Dispose();
            
        }

        
    }
}
