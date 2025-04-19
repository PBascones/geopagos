using Geopagos.Entities.Business;
using Geopagos.Services.Base;

namespace Geopagos.Services.Interfaces
{
    public interface ITournamentService
    {
        Task<List<TournamentResult>> GetTournamentsAsync(DateTime? fromDate, DateTime? toDate, string? gender);
        Task<ServiceResponse> SimulateAndStoreTournamentAsync(List<Player> players);
    }
}
