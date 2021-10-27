using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Query
{
    public class PaginationFilter
    {
        public int page_number { get; set; }
        public int page_size { get; set; }
        public string query { get; set; }

        public PaginationFilter()
        {
            this.page_number = 0;
            this.page_size = 10;
            this.query = "";
        }
        public PaginationFilter(int page_number, int page_size, string query = "")
        {
            this.page_number = page_number < 1 ? 0 : page_number - 1;
            if (page_size < 1 || page_size > 200)
            {
                this.page_size = 10;
            }
            else this.page_size = page_size;
            this.query = query;
        }
    }
}
