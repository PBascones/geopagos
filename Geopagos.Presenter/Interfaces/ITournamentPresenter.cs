using Geopagos.Presenter.Models;
using Geopagos.Services.Base;

namespace Geopagos.Presenter
{
    public interface ITournamentPresenter
    {
        Task<ServiceResponse<List<TournamentResultModel>>> GetTournamentsAsync(DateTime? fromDate, DateTime? toDate, string? gender);
        Task<ServiceResponse> SimulateTournamentAsync(List<PlayerModel> players);
    }
}
