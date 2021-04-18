using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoApi.Models
{
    public class SearchPaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string searchstring { get; set; }

        public SearchPaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 30;
            this.searchstring = "";
        }

        public SearchPaginationFilter(string searchstring, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 30 ? 30 : pageSize;
            this.searchstring = searchstring;
        }
    }
}
