using Infrastructure.Common.Models;

namespace Infrastructure.Common.Mappings;
public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IEnumerable<TDestination> source, int pageNumber, int pageSize) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(source.AsQueryable(), pageNumber, pageSize);
}
