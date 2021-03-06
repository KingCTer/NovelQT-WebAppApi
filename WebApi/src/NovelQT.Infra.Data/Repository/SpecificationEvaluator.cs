using Microsoft.EntityFrameworkCore;
using NovelQT.Domain.Interfaces;
using NovelQT.Domain.Specifications;
using System.Linq;

namespace NovelQT.Infra.Data.Repository
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static SpecificationResponse<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            // modify the IQueryable using the specification's criteria expression
            //if (specification.Criteria != null)
            //{
            //    query = query.Where(specification.Criteria);
            //}
            // Wheres all expression-based wheres
            query = specification.Criterias.Aggregate(query,
                                    (current, where) => current.Where(where));

            // Includes all expression-based includes
            query = specification.Includes.Aggregate(query,
                                    (current, include) => current.Include(include));

            // Include any string-based include statements
            query = specification.IncludeStrings.Aggregate(query,
                                    (current, include) => current.Include(include));

            // Apply ordering if expressions are set
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }

            var totalRecords = query.Count();

            // Apply paging if enabled
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip)
                             .Take(specification.Take);
            }
            return new SpecificationResponse<T>(query, totalRecords);
        }
    }
}
