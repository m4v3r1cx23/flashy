namespace Flashy.API.Models;

public class PageResult
{
  public int TotalCount { get; set; }
  public int PageNumber { get; set; }
  public int PageSize { get; set; }
  public object Data { get; set; }
}
