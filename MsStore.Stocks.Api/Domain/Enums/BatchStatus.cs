namespace MsStore.Stocks.Api.Domain.Enums;

public enum BatchStatus
{
    Planned,    // pre-released: announced, not sellable
    Incoming,   // confirmed and on its way
    Available,  // received and sellable
    OnHold,     // physically there but blocked (quality check, recall)
    Cancelled   // the delivery never came, or was written off
}
