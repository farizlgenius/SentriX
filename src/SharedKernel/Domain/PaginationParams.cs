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
    public int locationId { get; set; }

    public PaginationParams() { }

    public PaginationParams(int pageNumber, int pageSize, string search, DateTime? startDate, DateTime? endDate, int locationId)
    {
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
        this.search = search.Trim();
        this.startDate = startDate;
        this.endDate = endDate;
        this.locationId = locationId;
    }


}
