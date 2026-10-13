using MsStore.Stocks.Api.Domain;
using MsStore.Stocks.Api.InternalContracts;
using Riok.Mapperly.Abstractions;


namespace MsStore.Stocks.Api.Mapping;


[Mapper]
public static partial class ReservationMapper
{
    public static partial ReservationResponse ToResponse(this UserReservation reservation);
    public static partial IQueryable<ReservationResponse> ProjectToResponse(this IQueryable<UserReservation> reservations);
    
}