
using NovelQT.Domain.Models;

namespace NovelQT.Domain.Specifications
{
    public class BookFilterPaginatedSpecification : BaseSpecification<Book>
    {
        public BookFilterPaginatedSpecification(int skip, int take, string query)
            : base(i => true)
        {
            ApplyPaging(skip, take);
            AddInclude(x => x.Author);
            AddInclude(x => x.Category);

            var queryList = query.Split(";");
            foreach (var item in queryList)
            {
                switch (item)
                {
                    case "orderBy:view:desc":
                        ApplyOrderByDescending(orderByDescendingExpression: x => x.View);
                        break;
                    case "orderBy:view:asc":
                        ApplyOrderBy(orderByExpression: x => x.View);
                        break;
                    case "orderBy:like:desc":
                        ApplyOrderByDescending(orderByDescendingExpression: x => x.Like);
                        break;
                    case "orderBy:like:asc":
                        ApplyOrderBy(orderByExpression: x => x.Like);
                        break;
                    case "orderBy:key:desc":
                        ApplyOrderByDescending(orderByDescendingExpression: x => x.Key);
                        break;
                    case "orderBy:key:asc":
                        ApplyOrderBy(orderByExpression: x => x.Key);
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
