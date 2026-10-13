namespace MsStore.Stocks.Api.Domain.Enums;

public enum BatchStatus
{
    //Incoming,   // confirmed and on its way
    Available,  // received and sellable
    Cancelled   // the delivery never came, or was written off
}
