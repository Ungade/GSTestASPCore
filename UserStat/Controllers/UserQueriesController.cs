using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UserStat.Models;
using System;

namespace UserStat.Controllers
{
    [ApiController]
    [Route("report/user_statistics")]
    public class UserQueriesController: ControllerBase
    {
        private readonly UserQueryContext context;
        public UserQueriesController(UserQueryContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostUserQuery(UserQuery userQuery)
        {
            var foundUserQuery = await context.UserQueries.SingleOrDefaultAsync(userId=>userId.UserId==userQuery.UserId);
            if(foundUserQuery==null)
            {
                    context.Add(userQuery);
                    await context.SaveChangesAsync();
                    Query query = new Query()
                    {
                        Percent = 0,
                        QueryGuid = CreateGUID(),
                        QueryResult = new QueryResult()
                        {
                            Count_sign_in=1,
                            UserId = userQuery.UserId,
                        }
                    };
                    context.Add(query);
                    await context.SaveChangesAsync();
            }     
            else
            {
                var queryResult = await context.Queries.Include(res=>res.QueryResult).SingleOrDefaultAsync(queryId=>queryId.QueryResult.UserId==userQuery.UserId);
                queryResult.QueryResult.Count_sign_in++;
                context.Update(queryResult);
                await context.SaveChangesAsync();
            }       
            
            var queryGuid = await context.Queries.SingleOrDefaultAsync(queryId=>queryId.QueryResult.UserId==userQuery.UserId);
            return CreatedAtAction(nameof(RetriveQueryGUID),new {query_id=queryGuid.QueryId},queryGuid.QueryGuid);
        }

        private string CreateGUID()
        {
            return Guid.NewGuid().ToString();
        }


       

        public async Task<ActionResult<string>> RetriveQueryGUID(long query_id)
        {
            var query = await context.Queries.FindAsync(query_id);
            if(query==null)
            {
                return NotFound();
            }
            return query.QueryGuid;
        }
    

        

       
    }
}