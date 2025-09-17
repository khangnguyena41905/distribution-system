namespace IDENTITY.CONTRACT.Abstractions.Shared;


public class PagedResult<T>
{
    public IReadOnlyCollection<T> Items { get; private set; }
    public int TotalCount { get; private set; }
    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    private PagedResult(
        IReadOnlyCollection<T> items,
        int totalCount,
        int pageIndex,
        int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public static PagedResult<T> Create(
        IEnumerable<T> items,
        int totalCount,
        int pageIndex,
        int pageSize)
    {
        return new PagedResult<T>(
            items.ToList(),
            totalCount,
            pageIndex,
            pageSize);
    }
}