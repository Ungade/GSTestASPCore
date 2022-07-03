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
            var query = await context.Queries.SingleOrDefaultAsync(query=>query.QueryGuid==queryGUID );
            if(query==null)
                return NotFound();
            if(query.Percent>=100)
                query = await context.Queries.Include(r=>r.QueryResult).SingleOrDefaultAsync(query=>query.QueryGuid==queryGUID);
            if(query==null)
                return NotFound();
            return query;

        }

    }
}