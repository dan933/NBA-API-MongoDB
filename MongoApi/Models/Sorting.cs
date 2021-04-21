using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoApi.Models
{
    public class Sorting
    {
        public string SortField { get; set; }
        public int AscOrDesc { get; set; }
        
        public Sorting()
        {
            this.SortField = "FIRSTNAME";
            this.AscOrDesc = 1;
        }

        public Sorting(string SortField, int AscOrDesc)
        {
            this.SortField = SortField;
            this.AscOrDesc = AscOrDesc;
        }

    
    }
}
