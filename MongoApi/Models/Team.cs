using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoApi.Models
{
    
    public class Team
    {
        //Tells the model that this prop is the MongoDB identifier
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        public string TeamName{ get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string[] Players { get; set; }

    }
    
    
    //public class Team
    //{

    //    public ObjectId ID { get; set; }
    //    public string TeamName { get; set; }
    //    public string[] Players { get; set; }
    //}

    //public class Id
    //{
    //    public ObjectId oid { get; set; }
    //}

}
