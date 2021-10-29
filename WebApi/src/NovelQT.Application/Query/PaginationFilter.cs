using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Query
{
    public class PaginationFilter
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;
        private string _query = "";

        [FromQuery(Name = "page_number")]
        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value < 1 ? 1 : value; }
        }

        [FromQuery(Name = "page_size")]
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value < 0 || value > 200) ? 10 : value;}
        }

        [FromQuery(Name = "query")]
        public string Query
        {
            get { return _query; }
            set { _query = value; }
        }

    }
}
