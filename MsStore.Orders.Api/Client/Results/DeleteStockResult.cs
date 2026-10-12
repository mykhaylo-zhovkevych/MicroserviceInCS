namespace MsStore.Orders.Api.Client.Results;

public enum DeleteStockResult
{
    Deleted,
    HasActiveReservation,
    HasInactiveReservation,
    Unavailable
}