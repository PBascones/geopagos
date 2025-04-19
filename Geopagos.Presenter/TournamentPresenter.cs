using Geopagos.Entities.Business;
using Geopagos.Presenter.Models;
using Geopagos.Services.Interfaces;
using Geopagos.Shared.Helpers;

namespace Geopagos.Presenter
{
    public class TournamentPresenter : ITournamentPresenter
    {
        private readonly ITournamentService _service;

        public TournamentPresenter(ITournamentService service)
        {
            _service = service;
        }

        public async Task<List<TournamentResult>> GetTournamentsAsync(DateTime? fromDate, DateTime? toDate, string? gender)
        {
            return await _service.GetTournamentsAsync(fromDate, toDate, gender);
        }

        public async Task<TournamentResult> SimulateTournamentAsync(List<PlayerModel> players)
        {
            var mappedPlayers = players
                .Select(PlayerFactory.Create)
                .ToList();

            return await _service.SimulateAndStoreTournamentAsync(mappedPlayers);
        }
    }
}
