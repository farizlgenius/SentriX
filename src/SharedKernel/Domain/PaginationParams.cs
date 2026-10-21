using System;
using SharedKernel.Helpers;

namespace SharedKernel.Domain;


public sealed class PaginationParams
{
    public int pageNumber { get; set; } = 1;
    public int pageSize { get; set; } = 10;
    public string search { get; set; } = string.Empty;
    public DateTime? startDate { get; set; }
    public DateTime? endDate { get; set; }
    public string? sortBy {get; set;}
    public bool isDescending {get; set; } = true;
    public Guid locationGuid { get; set; }

    public PaginationParams() { }

    public PaginationParams(int pageNumber, int pageSize, string search, DateTime? startDate, DateTime? endDate, Guid locationGuid,string? sortBy,bool isDescending=true)
    {
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
        this.search = search.Trim();
        this.startDate = startDate;
        this.endDate = endDate;
        this.locationGuid = locationGuid;
        this.sortBy = sortBy;
        this.isDescending = isDescending;
    }


}
