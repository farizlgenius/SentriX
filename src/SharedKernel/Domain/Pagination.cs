using System;

namespace SharedKernel.Domain;

public sealed record Pagination<T>(int Page, int PageSize, int TotalItems, int TotalPages, List<T> Items);