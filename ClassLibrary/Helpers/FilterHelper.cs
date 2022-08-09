using System.Linq;
using ClassLibrary.Models.Database;
using ClassLibrary.Models.Filter;

namespace ClassLibrary.Helpers
{
    public static class FilterHelper
    {

        public static IQueryable<Art> Filter(this IQueryable<Art> query, ArtFilter filter)
        {
            if (filter.Category != null)
                query = query.Where(q => q.Category == filter.Category);
            return query;
        }
        
        public static IQueryable<Category> Filter(this IQueryable<Category> query, CategoryFilter filter)
        {
            if (filter.IsPrivate.HasValue)
                query = query.Where(c => c.IsPrivate == filter.IsPrivate.Value);
            return query;
        }

        public static IQueryable<TextPage> Filter(this IQueryable<TextPage> query, TextFilter filter)
        {
            if (filter.IsPrivate.HasValue)
                query = query.Where(p => p.IsPrivate == filter.IsPrivate.Value);
            if (filter.Type.HasValue)
                query = query.Where(p => p.Type == filter.Type.Value);
            return query;
        }
        
    }
}