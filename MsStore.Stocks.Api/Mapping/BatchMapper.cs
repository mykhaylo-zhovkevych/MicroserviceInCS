using MsStore.Stocks.Api.Domain;
using MsStore.Stocks.Api.InternalContracts;
using Riok.Mapperly.Abstractions;

namespace MsStore.Stocks.Api.Mapping;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class BatchMapper
{
    [MapProperty(nameof(Batch.InStock), nameof(BatchStock.Available))]
    public static partial BatchStock ToStock(this Batch batch);
    public static partial IQueryable<BatchStock> ProjectToStock(this IQueryable<Batch> batches);
}
