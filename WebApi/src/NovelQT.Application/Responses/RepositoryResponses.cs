using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelQT.Application.Responses
{
    public class RepositoryResponses<T> where T : class
    {
        public IEnumerable<T> ViewModel { get; set; }
        public int TotalRecords { get; set; }

        public RepositoryResponses(IEnumerable<T> viewModel, int totalRecords)
        {
            ViewModel = viewModel;
            TotalRecords = totalRecords;
        }

        public RepositoryResponses()
        {
        }
    }
}
