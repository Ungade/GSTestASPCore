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
        public bool IsStarted;
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
                IsStarted=true;
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        await ProccessQueries(userQueryContext);
                        await Task.Delay(100, cancellationToken);
                    }
                    catch(OperationCanceledException)
                    {
                        return;
                    }


                }
                IsStarted=false;
            }
        }

        

        private async Task ProccessQueries(UserQueryContext context)
        {
            var allToProcces =await context.Queries.Where(r=>r.Percent<100).ToListAsync();            
            foreach (var query in allToProcces)
            {
                var s = await context.QueriesBridges.Where(r=>r.QueryId==query.QueryId).FirstOrDefaultAsync();
                if (s != null)
                {
                    var elapsedTime = DateTime.Now - s.queryCreateTime;
                    query.Percent = (ushort)(elapsedTime.TotalMilliseconds * 100 / taskProccesTime);
                }
                else
                    throw new System.Data.DataException("Bridge not created");
            }
            if (allToProcces != null)
            {
                if (allToProcces.Count > 0)
                {
                    context.UpdateRange(allToProcces);
                    await context.SaveChangesAsync();
                }
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