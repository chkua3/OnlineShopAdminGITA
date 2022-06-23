using Microsoft.EntityFrameworkCore;

namespace OnlineShopAdmin.Common.API;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)PageSize);
        AddRange(items);
    }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }

    public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var data = source.ToList();
        var count = data.Count;

        var items = data.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}