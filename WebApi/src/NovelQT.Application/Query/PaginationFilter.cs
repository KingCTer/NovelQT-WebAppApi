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

        public PaginationFilter()
        {
            this.page_number = 1;
            this.page_size = 10;
        }
        public PaginationFilter(int page_number, int page_size)
        {
            this.page_number = page_number < 1 ? 1 : page_number;
            this.page_size = page_size > 10 ? 10 : page_size;
        }
    }
}
