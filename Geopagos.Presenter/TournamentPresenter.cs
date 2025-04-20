using Geopagos.Presenter.Helpers;
using Geopagos.Presenter.Models;
using Geopagos.Services.Base;
using Geopagos.Services.Constants;
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

        public async Task<ServiceResponse<List<TournamentResultModel>>> GetTournamentsAsync(DateTime? fromDate, DateTime? toDate, string? gender)
        {
            var sr = new ServiceResponse<List<TournamentResultModel>>();

            var data = await _service.GetTournamentsAsync(fromDate, toDate, gender);

            if (data == null || data.Count == 0)
            {
                sr.AddError(ErrorMessages.NoTournamentsFound);
                return sr;
            }

            sr.Data = data.Select(x => x.ToModel()).ToList();

            return sr;
        }

        public async Task<ServiceResponse> SimulateTournamentAsync(List<PlayerModel> players)
        {
            var mappedPlayers = players
                .Select(PlayerFactory.Create)
                .ToList();

            return await _service.SimulateAndStoreTournamentAsync(mappedPlayers);
        }
    }
}
