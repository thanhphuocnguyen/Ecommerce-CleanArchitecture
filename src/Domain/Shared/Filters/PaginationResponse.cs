namespace Ecommerce.Domain.Shared;

public class PaginationResponse<T>
{
    public PaginationResponse(List<T> data, int count, int page, int pageSize)
    {
        Data = data;
        Count = count;
        Page = page;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
    }

    public List<T> Data { get; }

    public int Count { get; }

    public int Page { get; }

    public int TotalPages { get; private set; }

    public int PageSize { get; }

    public bool HasPrevious => Page > 1;

    public bool HasNext => Page < TotalPages;
}