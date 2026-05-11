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

    public PaginationParams() { }

    public PaginationParams(int pageNumber, int pageSize, string search, DateTime? startDate, DateTime? endDate)
    {
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
        this.search = ValidateRequiredString(search, nameof(search));
        this.startDate = startDate;
        this.endDate = endDate;
    }

    private static string ValidateRequiredString(string value, string field)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, field);
        var trimmed = value.Trim();
        if (!RegexHelper.IsValidName(trimmed) && !RegexHelper.IsValidOnlyCharAndDigit(trimmed.Replace("-", string.Empty).Replace("_", string.Empty)))
        {
            throw new ArgumentException($"{field} invalid.", field);
        }

        return value;
    }
}
