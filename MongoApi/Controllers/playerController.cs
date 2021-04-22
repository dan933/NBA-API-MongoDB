using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoApi.Models;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;

namespace MongoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private IMongoCollection<Player> _playerCollection;
        private IMongoCollection<BsonDocument> _AddplayerCollection;

        public PlayerController(IMongoClient client)
        {
            var database = client.GetDatabase("NBA");
            _playerCollection = database.GetCollection<Player>("Players");
            _AddplayerCollection = database.GetCollection<BsonDocument>("Players");

        }

        

        ////Get All Players
        [HttpGet]
        //public  IEnumerable<Player> Get([FromQuery] PaginationFilter filter)
        public object Get([FromQuery] PaginationFilter filter,[FromQuery] Sorting sort)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var skip = (validFilter.PageNumber - 1) * validFilter.PageSize;
            var PageSize = validFilter.PageSize;

            var SortFilter = new Sorting(sort.SortField, sort.AscOrDesc);

            //user input 1 for ascending and -1 for descending
            SortDefinition<Player> SortAscDsc = new BsonDocument(SortFilter.SortField , SortFilter.AscOrDesc);
                  
            var data = _playerCollection.Find(new BsonDocument())
                                        .Sort(SortAscDsc)        
                                        .Skip(skip)
                                        .Limit(PageSize)
                                        .ToList();
            
            //Total Number of Pages
            var totalRecords = _playerCollection.Find(s => true).CountDocuments();
            var pages = (decimal)totalRecords / (decimal)PageSize;
            pages = pages % 1 != 0 ? Decimal.ToInt32(pages += 1) : pages;

            return new { data, pages };

        }

        //get stat headers
        [HttpGet("/getheaders")]
        public object GetHeaders()
        {           
            //Get Headers Endpoint can be improved
            //look more into how to get headers from Mongo surely there is a way
            var HeaderList = new List<string>();

            Type type = typeof(Player);
            var propertyInfo = type.GetProperties();
            
            foreach(var key in propertyInfo)
            {
                HeaderList.Add(key.Name);
            }

            return HeaderList.ToJson();

        }
            //Search Players
            [HttpGet("/searchPlayer")]
        public object SearchPlayer([FromQuery] SearchPaginationFilter filter,[FromQuery] Sorting sort)
        {
            filter.PageSize = filter.PageSize == 0 ? 30 : filter.PageSize;

            var validFilter = new SearchPaginationFilter(filter.searchstring, filter.PageNumber, filter.PageSize);
            var skip = (validFilter.PageNumber - 1) * validFilter.PageSize;
            var page = validFilter.PageSize;
            

            string[]? splitString = filter.searchstring?.Split(' ');

            var SortFilter = new Sorting(sort.SortField, sort.AscOrDesc);

            SortDefinition<Player> SortAscDsc = new BsonDocument(SortFilter.SortField, SortFilter.AscOrDesc);

            if (splitString?.Length == 2)
            {
                var FirstNameFilter = Builders<Player>.Filter
                    .Regex(p => p.FIRSTNAME, new BsonRegularExpression($"/^{splitString[0]}.*/i"));
                var LastNameFilter = Builders<Player>.Filter
                    .Regex(p => p.LASTNAME, new BsonRegularExpression($"/^{splitString[1]}.*/i"));
                var CombinedFilter = Builders<Player>.Filter
                    .And(FirstNameFilter, LastNameFilter);
                

                var data = _playerCollection.Find(CombinedFilter)
                    .Sort(SortAscDsc)
                    .Skip(skip)
                    .Limit(page)
                    .ToList();

                var totalRecords =
                    _playerCollection.Find(CombinedFilter)
                    .CountDocuments();

                var pages = (decimal)totalRecords / (decimal)page;
                pages = pages % 1 != 0 ? Decimal.ToInt32(pages += 1) : pages;

                return new { data, pages };


            }
            else

            {
                var FirstNameFilter = Builders<Player>.Filter.Regex(p => p.FIRSTNAME, new BsonRegularExpression($"/^{filter.searchstring}.*/i"));


                var LastNameFilter = Builders<Player>.Filter.Regex(p => p.LASTNAME, new BsonRegularExpression($"/(^{filter.searchstring}.*)|(\\s{filter.searchstring}.*)/i"));
                var CombinedFilter = Builders<Player>.Filter.Or(FirstNameFilter, LastNameFilter);


                var data = _playerCollection.Find(CombinedFilter)
                    .Sort(SortAscDsc)
                    .Skip(skip)
                    .Limit(page)
                    .ToList();

                var totalRecords =
                    _playerCollection.Find(CombinedFilter)
                    .CountDocuments();

                var pages = (decimal)totalRecords / (decimal)page;
                pages = pages % 1 != 0 ? Decimal.ToInt32(pages += 1) : pages;



                return new { data, pages };
            }


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

