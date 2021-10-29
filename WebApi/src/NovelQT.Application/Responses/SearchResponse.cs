using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Responses
{
    public class SearchResponse<T> where T : class
    {
        public IEnumerable<T> SearchResult { get; set; }
        public long TotalRecords { get; set; }

        public SearchResponse(IEnumerable<T> searchResult, long totalRecords)
        {
            SearchResult = searchResult;
            TotalRecords = totalRecords;
        }

        public SearchResponse()
        {
        }
    }
}
