
using NovelQT.Domain.Models;

namespace NovelQT.Domain.Specifications
{
    public class ChapterFilterPaginatedSpecification : BaseSpecification<Chapter>
    {
        public ChapterFilterPaginatedSpecification(Guid bookId, int skip, int take, string query)
            : base(i => true)
        {
            AddWhere(i => i.BookId == bookId);
            ApplyPaging(skip, take);
            

            if (query == "" || query == null)
            {
                ApplyOrderBy(x => x.Order);
                return;
            }
            if (!query.Contains("orderBy"))
            {
                ApplyOrderBy(x => x.Order);
            }
            var queryList = query.Split(";");
            foreach (var item in queryList)
            {
                if (item.Contains("orderBy:"))
                {
                    switch (item)
                    {
                        case "orderBy:order:desc":
                            ApplyOrderByDescending(x => x.Order);
                            break;
                        case "orderBy:order:asc":
                            ApplyOrderBy(x => x.Order);
                            break;
                        default:
                            break;
                    }
                    continue;
                }

                if (item.Contains("where:"))
                {
                    var whereEqual = item.Split("=");
                    if (whereEqual.Length == 2)
                    {
                        switch (whereEqual[0])
                        {
                            case "where:order":
                                int order;
                                if (Int32.TryParse(whereEqual[1], out order))
                                {
                                    AddWhere(x => x.Order == order);
                                }
                                break;
                            default:
                                break;
                        }
                        continue;
                    }
                    var whereBigger = item.Split(">");
                    if (whereBigger.Length == 2)
                    {
                        switch (whereBigger[0])
                        {
                            case "where:order":
                                int order;
                                if (Int32.TryParse(whereBigger[1], out order))
                                {
                                    AddWhere(x => x.Order >= order);
                                }
                                break;
                            default:
                                break;
                        }
                        continue;
                    }
                    continue;

                }
                

            }

        }
    }
}
