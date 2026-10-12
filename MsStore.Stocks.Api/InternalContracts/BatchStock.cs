namespace MsStore.Stocks.Api.InternalContracts;

public record BatchStock(int ProductId, string Category, int Available) : BatchBase(Category, Available);

public record CreateBatchRequest(string ProductId, string Category, int Quantity);
