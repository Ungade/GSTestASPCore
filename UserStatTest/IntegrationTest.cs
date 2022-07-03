using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;


using System.Collections.Generic;


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
            await CheckResultAt_30_45_60_seconds_ShouldReturn_50_75_100_Percent(resGuid.ToString());
        }

       
        private async Task GetByQueryID_WhenProccesLess100Percent_ShouldReturn_QueryResulAsNull( string queryGuid, string userId)
        {
            var uri = new System.Uri(@"https://localhost:5001/report/info/"+queryGuid);
            var response = await client.GetAsync(uri);
            var result = await response.Content.ReadFromJsonAsync<UserStat.Models.Query>();
            Assert.AreEqual(queryGuid,result.QueryGuid);
            Assert.IsNull(result.QueryResult);
            
        }

        public async Task CheckResultAt_30_45_60_seconds_ShouldReturn_50_75_100_Percent(string queryGuid)
        {
            
            var wait = 5000;
            var timer = 0;
            var timerPlus = 50;
            
            while(timer <wait)
            {
                timer+=timerPlus;
                await Task.Delay(timerPlus);
                
                var res=0;
                testRequestTimesPercents.TryGetValue(timer,out res);
                if(timer==res)
                {
                    var uri = new System.Uri(@"https://localhost:5001/report/info/"+queryGuid);
                    var response = await client.GetAsync(uri);
                    var result = await response.Content.ReadFromJsonAsync<UserStat.Models.Query>();

                    TestContext.WriteLine(result.QueryGuid);
                    TestContext.WriteLine(result.QueryId.ToString());
                    TestContext.WriteLine(result.Percent.ToString());
                    
                    Assert.AreEqual(res,result.Percent);
                }
            }

            
        }

        private static Dictionary<int, int> testRequestTimesPercents = new Dictionary<int, int>()
        {
            {500,10},
            {2500,50},
            {3750,75},
            {5000,100}
            
        };

        



        

        [ClassCleanup]
        public static void ClassCleanup()
        {
            client.Dispose();
            factory.Dispose();
            
        }

        
    }
}
