using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoApi.Models;
using System;
using System.Collections.Generic;

using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;

namespace MongoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
        private IMongoCollection<Team> _TeamCollection;
        private IMongoCollection<BsonDocument> _AddTeamCollection;
        private IMongoCollection<Player> _PlayerCollection;
        


        public TeamController(IMongoClient client)
        {
            var database = client.GetDatabase("NBA");
            _TeamCollection = database.GetCollection<Team>("Teams");
            _AddTeamCollection = database.GetCollection<BsonDocument>("Teams");
            _PlayerCollection = database.GetCollection<Player>("Players");
        }

        //Add Team
        [HttpGet("addteam")]
        public string AddTeam([FromQuery] string TeamName)
        {
            var isTeam = Builders<Team>.Filter.Regex(T => T.TeamName, new BsonRegularExpression($"/{TeamName}/i"));

            var data = _TeamCollection.Find(isTeam).FirstOrDefault();

            if (data == null)
            {
                var document = new BsonDocument { { "TeamName", TeamName },
                    {"Players", new BsonArray { } } };
                _AddTeamCollection.InsertOne(document);
                

                return $"{TeamName} added";
            }
            return $"{TeamName} already exists";

        }

        //Remove Team
        [HttpGet("removeteam")]
        public string RemoveTeam([FromQuery] string TeamName)
        {
            var isTeam = Builders<Team>.Filter.Regex(T => T.TeamName, new BsonRegularExpression($"/{TeamName}/i"));

            var data = _TeamCollection.Find(isTeam).FirstOrDefault();

            if (data != null)
            {
                var document = new BsonDocument { { "TeamName", TeamName }};
                _AddTeamCollection.DeleteOne(document);


                return $"{TeamName} Removed.";
            }
            return $"{TeamName} Doesnot exist.";
        }

            [HttpGet("getallteams")]
        public object GetallTeams([FromQuery] PaginationFilter pagination, int AscOrDesc)
        {
            var validFilter = new PaginationFilter(pagination.PageNumber, pagination.PageSize);
            var skip = (validFilter.PageNumber - 1) * validFilter.PageSize;
            var page = validFilter.PageSize;

            //sort logic
            AscOrDesc = AscOrDesc == 0 ? 1 : AscOrDesc;
            SortDefinition<Team> SortAscDsc = new BsonDocument("TeamName", AscOrDesc);

            var projection = Builders<Team>.Projection
                .Include(t => t.ID)
                .Include(t => t.TeamName);

            var data = _TeamCollection
                .Find(t => true)
                .Project(t => new { t.ID, t.TeamName })                
                .Sort(SortAscDsc)
                .Skip(skip)
                .Limit(page)
                .ToList();

            var totalRecords =
                    _TeamCollection.Find(t => true)
                    .CountDocuments();

            var pages = (decimal)totalRecords / (decimal)page;
            pages = pages % 1 != 0 ? Decimal.ToInt32(pages += 1) : pages;

            return new { data, pages };           
        }
        
            
            //Assign Player to Team
            [HttpGet("assignplayer")]
            public string AssignPlayer([FromQuery] string TeamID, string PlayerID)
            {
                

                var playerFilter = Builders<Player>.Filter.Eq("_id", ObjectId.Parse(PlayerID));

                var teamFilter = Builders<Team>.Filter.Eq("_id", ObjectId.Parse(TeamID));

            //check Player Count
            var PlayerCount = _TeamCollection.Find(teamFilter).Project(t => t.Players.Length).ToList();

            if (PlayerCount[0] >= 15) { return "Player Limit Reached";}

                var playerdata = _PlayerCollection.Find(playerFilter).FirstOrDefault();
                var teamdata = _TeamCollection.Find(teamFilter).FirstOrDefault();     
                           

                if (playerdata != null && teamdata !=null)
                    {
                
                        var PlayerExistsFilter = Builders<Team>.Filter.Eq("Players",PlayerID);
                        var combined = Builders<Team>.Filter.And(PlayerExistsFilter, teamFilter);
                        var PlayerCheckData = _TeamCollection.Find(combined).FirstOrDefault();

                        var PlayerCheck = PlayerCheckData == null ? false : true;
                        if (PlayerCheck) { return "Player is already on the team"; }
                        
                        var update = Builders<Team>.Update.Push<string>(t => t.Players, PlayerID);
                        _TeamCollection.FindOneAndUpdate(teamFilter, update);  
                
                    return "Player Added to Team";

                    }
                    return "Incorrect ID Please Check Team ID or PlayerID ";
            }

            //Remove Player from Team
            [HttpGet("removeplayer")]
            public string RemovePlayer([FromQuery] string TeamID, string PlayerID)
            {
                var playerdata = _PlayerCollection.Find(p => p.ID == PlayerID).FirstOrDefault();

                var teamdata = _TeamCollection.Find(t => t.ID == TeamID).FirstOrDefault();

                if (playerdata != null && teamdata != null)
                {
                    var filter = Builders<Team>.Filter.Eq("_id", ObjectId.Parse(TeamID));
                    var update = Builders<Team>.Update.Pull<string>(t => t.Players, PlayerID);
                    _TeamCollection.FindOneAndUpdate(filter, update);

                    return "Player removed from Team";

                }
                return "Player was not in Team";
            }


        [HttpGet("getplayersfromteam")]
        public object GetPlayers([FromQuery] PaginationFilter pagination, string TeamID, [FromQuery] Sorting sort)
        {
            //Pagination
            pagination.PageSize = pagination.PageSize == 0 ? 30 : pagination.PageSize;

            var validFilter = new PaginationFilter( pagination.PageNumber, pagination.PageSize);
            var skip = (validFilter.PageNumber - 1) * validFilter.PageSize;
            var page = validFilter.PageSize;

            //Team filter
            var filter = Builders<Team>.Filter.Eq("_id",  ObjectId.Parse(TeamID));            
            var data = _TeamCollection.Find(filter).Project(t => t.Players).FirstOrDefault();
                        
            //Selecet all Players from team 
            var PlayerFromTeam = Builders<Player>.Filter.In(p => p.ID, data);

            //Sorting 
            var SortFilter = new Sorting(sort.SortField, sort.AscOrDesc);
            SortDefinition<Player> SortAscDsc = new BsonDocument(SortFilter.SortField, SortFilter.AscOrDesc);

            var Players = _PlayerCollection
                          .Find(PlayerFromTeam)
                          .Sort(SortAscDsc)
                          .Skip(skip)
                          .Limit(page)
                          .ToList();

            var totalRecords =
                    _PlayerCollection.Find(PlayerFromTeam)
                    .CountDocuments();

            var pages = (decimal)totalRecords / (decimal)page;
            pages = pages % 1 != 0 ? Decimal.ToInt32(pages += 1) : pages;


            return new { Players, pages };

                        
        }


    }
}

// limit
//return _playerCollection.Find(s => true).Limit(100).ToList();            

//select all Players            
//return _playerCollection.AsQueryable().Select(x => x);


// how to filter
//.Find(s => s.AGE > 20).ToList();           

//Get Column Headers
//Add Players to Team
//Remove Players from Team
//Once you are comfortable create this in an API project should have done this to begin with
//sort by column

