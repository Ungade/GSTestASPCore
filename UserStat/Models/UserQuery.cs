using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserStat.Models
{
    public class Query
    {        
        [Key]
        public long QueryId{get;set;}
        [Required]
        public string QueryGuid{get;set;}
        public ushort Percent{get;set;}
        public QueryResult QueryResult{get;set;}

        public QueriesBridge QueriesBridge{get;set;}
    }

    public class UserQuery
    {
        [Key]
        public long UserQueryId{get;set;}
        [Required]
        public string UserId{get;set;}
        public DateTime StartDate{get;set;}
        public DateTime EndDate{get;set;}
    }

    public class QueryResult
    {
        [Key]
        [JsonIgnore]
        public long QueryResultId{get;set;}
        public string UserId {get;set;}
        public long Count_sign_in{get;set;}
        
        [JsonIgnore]
        public long QueryId{get;set;}
        [JsonIgnore]
        public Query Query{get;set;}
    }

    public class QueriesBridge
    {
        [Key]
        public long QueriesBridgeId{get;set;}
        public long UserQueryId{get;set;}
        
        [JsonIgnore]
        public long QueryId{get;set;}
        [JsonIgnore]
        public Query Query{get;set;}

        public DateTime queryCreateTime{get;set;}
    }
}
