using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoApi.Models
{
    //Things that are in the db but not in the model are ignored
    [BsonIgnoreExtraElements]
    public class Player
    {

        //Tells the model that this prop is the MongoDB identifier
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string FIRSTNAME { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string LASTNAME { get; set; }

        [BsonRepresentation(BsonType.Int32,AllowTruncation = true)]
        public int AGE { get; set; }

        [BsonRepresentation(BsonType.Int32, AllowTruncation = true)]
        public int GP { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double MINS { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double PLUS_MINUS { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double AST { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double BLK { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double BLKA { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double OREB { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double DREB { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double FG_PCT { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double FG3_PCT { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double FG3A { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double FG3M { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double FGA { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double FGM { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double FT_PCT { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double FTA { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double FTM { get; set; }

        [BsonRepresentation(BsonType.Int32, AllowTruncation = true)]
        public int W { get; set; }

        [BsonRepresentation(BsonType.Int32, AllowTruncation = true)]
        public int L { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double W_PCT { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double PF { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double PFD { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double REB { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double TOV { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double STL { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public double PTS { get; set; }

    }
}
