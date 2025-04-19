using Geopagos.Entities.Business;

namespace Geopagos.Services.Interfaces
{
    public interface ITournamentService
    {
        Task<TournamentResult> SimulateAndStoreTournamentAsync(List<Player> players);
        Task<List<TournamentResult>> GetTournamentsAsync(DateTime? fromDate, DateTime? toDate, string? gender);
    }
}
