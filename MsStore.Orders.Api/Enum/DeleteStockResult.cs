namespace MsStore.Orders.Api.Enum;

public enum DeleteStockResult
{
    Deleted,
    HasActiveReservation,
    HasInactiveReservation,
    Unavailable
}