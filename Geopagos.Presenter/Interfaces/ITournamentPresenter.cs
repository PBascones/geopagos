using Geopagos.Entities.Business;
using Geopagos.Presenter.Models;

namespace Geopagos.Presenter
{
    public interface ITournamentPresenter
    {
        Task<List<TournamentResult>> GetTournamentsAsync(DateTime? fromDate, DateTime? toDate, string? gender);
        Task<TournamentResult> SimulateTournamentAsync(List<PlayerModel> players);
    }
}
