using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MongoApi.Models
{
    //Things that are in the db but not in the model are ignored
    //[BsonIgnoreExtraElements]
    public class Player
    {

        //Tells the model that this prop is the MongoDB identifier
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        public string FIRSTNAME { get; set; }

        public string LASTNAME { get; set; }

        public double AGE { get; set; }

        public double GP { get; set; }

        public double MINS { get; set; }

        public double PLUS_MINUS { get; set; }

        public double AST { get; set; }

        public double BLK { get; set; }

        public double BLKA { get; set; }

        public double OREB { get; set; }

        public double DREB { get; set; }

        public double FG_PCT { get; set; }

        public double FG3_PCT { get; set; }

        public double FG3A { get; set; }

        public double FG3M { get; set; }

        public double FGA { get; set; }

        public double FGM { get; set; }

        public double FT_PCT { get; set; }

        public double FTA { get; set; }

        public double FTM { get; set; }

        public double W { get; set; }

        public double L { get; set; }

        public double W_PCT { get; set; }

        public double PF { get; set; }

        public double PFD { get; set; }

        public double REB { get; set; }

        public double TOV { get; set; }

        public double STL { get; set; }

        public double PTS { get; set; }

        [BsonElement("TeamName")]
        public string TEAM { get; set; }

    }
}
