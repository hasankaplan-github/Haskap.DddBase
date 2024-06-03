namespace Haskap.DddBase.Utilities.Paging;

public class Pagination<T>
{
    //private const int _defaultPageSize = 10;

    public IReadOnlyList<T> CurrentPageItems { get; private set; }
    public int PageSize { get; }
    public int CurrentPageIndex { get; } // starts from 1
    public int TotalItemCount { get; }
    public int PageCount { get; }
    public bool HasNextPage
    {
        get
        {
            return (PageCount > 1 && CurrentPageIndex > 0 && CurrentPageIndex < PageCount);
        }
    }
    public bool HasPreviousPage
    {
        get
        {
            return (PageCount > 1 && CurrentPageIndex > 0 && CurrentPageIndex > 1);
        }
    }

    public Pagination(List<T> currentPageItems, /*int pageSize,*/ int currentPageIndex, int totalItemCount)
    {
        ArgumentNullException.ThrowIfNull(currentPageItems);

        CurrentPageItems = currentPageItems.AsReadOnly();
        PageSize = CurrentPageItems.Count;  //pageSize < 1 ? _defaultPageSize : pageSize;
        TotalItemCount = totalItemCount<0 ? 0 : totalItemCount;
        PageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
        CurrentPageIndex = currentPageIndex < 1 || currentPageIndex > PageCount ? 0 : currentPageIndex;
    }

    public int GetFirstPageIndex(int visiblePageIndexCount)
    {
        if (CurrentPageIndex < 1)
        {
            return 1;
        }

        if (visiblePageIndexCount < 1)
        {
            visiblePageIndexCount = 1;
        }
        var pageIndexCountBeforeCurrentPageIndex = CurrentPageIndex == PageCount ? visiblePageIndexCount - 1 : visiblePageIndexCount - 2;
        if (pageIndexCountBeforeCurrentPageIndex < 0)
        {
            pageIndexCountBeforeCurrentPageIndex = 0;
        }
        
        return CurrentPageIndex - pageIndexCountBeforeCurrentPageIndex < 1 ? 1 : CurrentPageIndex - pageIndexCountBeforeCurrentPageIndex;
    }

    public int GetLastPageIndex(int visiblePageIndexCount)
    {
        if (CurrentPageIndex < 1)
        {
            return 1;
        }

        if (visiblePageIndexCount < 1)
        {
            visiblePageIndexCount = 1;
        }
        var pageIndexCountAfterCurrentPageIndex = CurrentPageIndex == 1 ? visiblePageIndexCount - 1 : visiblePageIndexCount - 2;
        if (pageIndexCountAfterCurrentPageIndex < 0)
        {
            pageIndexCountAfterCurrentPageIndex = 0;
        }

        return CurrentPageIndex + pageIndexCountAfterCurrentPageIndex > PageCount ? PageCount : CurrentPageIndex + pageIndexCountAfterCurrentPageIndex;
    }

    
}
