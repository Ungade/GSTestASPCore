using Microsoft.AspNetCore.Mvc;
using UserStat.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UserStat.Controllers
{
    [ApiController]
    [Route("report/info")]
    public class UserQueriesInfoController:ControllerBase
    {
        private readonly UserQueryContext context;
        public UserQueriesInfoController(UserQueryContext context)
        {
            this.context = context;
        }

        
        [HttpGet("{queryGUID}")]
        public async Task<ActionResult<Query>> GetUserQueryResults(string queryGUID)
        {
            var userQuery = await context.Queries.SingleOrDefaultAsync(query=>query.QueryGuid==queryGUID );
            if(userQuery==null)
                return NotFound();
            if(userQuery.Percent>=100)
                userQuery = await context.Queries.Include(r=>r.QueryResult).SingleOrDefaultAsync(query=>query.QueryGuid==queryGUID);
            return userQuery;

        }

    }
}