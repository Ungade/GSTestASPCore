using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using UserStat.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.Extensions.Configuration;


namespace UserStat
{
    
    public class QueriesProccessor : IHostedService
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private Task scheduler;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly float taskProccesTime;
        public QueriesProccessor(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            using(var scope = serviceScopeFactory.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var parsed = float.TryParse( configuration["ProccesTime"],out taskProccesTime);
                if(!parsed)
                {
                    taskProccesTime=60000;
                    Console.WriteLine("Procces time is set to default "+taskProccesTime+" ms - cant parse float");
                }
            }
            
        }
        private async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using(var scope = serviceScopeFactory.CreateScope())
            {
                var userQueryContext = scope.ServiceProvider.GetRequiredService<UserQueryContext>();

                while (!cancellationToken.IsCancellationRequested)
                {
                    //check db table
                    //if not completed work then resume
                    //if new work errands then start new
                    await ProccessQueries(userQueryContext);
                    await Task.Delay(100);


                }
            }
        }

        

        private async Task ProccessQueries(UserQueryContext context)
        {
            var allToProcces =await context.Queries.Where(r=>r.Percent<100).ToListAsync();            
            foreach (var query in allToProcces)
            {
                var s = await context.QueriesBridges.Where(r=>r.QueryId==query.QueryId).FirstAsync();
                var elapsedTime = DateTime.Now - s.queryCreateTime;
                query.Percent =  (ushort)(elapsedTime.TotalMilliseconds*100/taskProccesTime);
            }
            if(allToProcces.Count>0)
            {
                context.UpdateRange(allToProcces);
                await context.SaveChangesAsync();
            }
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            scheduler = ExecuteAsync(cancellationToken);
            if(scheduler.IsCompleted)
                return scheduler;
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if(scheduler==null)
                return;
            try
            {
                cancellationTokenSource.Cancel();
            }
            finally
            {
                await Task.WhenAny(scheduler,Task.Delay(Timeout.Infinite,cancellationToken));
            }
            
        }
    }
}