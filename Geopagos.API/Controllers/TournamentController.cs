using Geopagos.API.Controllers.Base;
using Geopagos.Presenter;
using Geopagos.Presenter.Models;
using Geopagos.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace Geopagos.API.Controllers
{
    [ApiController]
    [Route("api/tournaments")]
    public class TournamentController : BaseApiController
    {
        private readonly ITournamentPresenter _presenter;

        public TournamentController(ITournamentPresenter presenter)
        {
            _presenter = presenter;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TournamentResultModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ServiceError>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTournaments(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] string? gender)
        {
            return HandleResponse(await _presenter.GetTournamentsAsync(from, to, gender));
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ServiceError>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SimulateTournament([FromBody] List<PlayerModel> players)
        {
            return HandleResponse(await _presenter.SimulateTournamentAsync(players));
        }
    }
}
