using Geopagos.Presenter;
using Geopagos.Presenter.Models;
using Microsoft.AspNetCore.Mvc;

namespace Geopagos.API.Controllers
{
    [ApiController]
    [Route("api/tournaments")]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentPresenter _presenter;

        public TournamentController(ITournamentPresenter presenter)
        {
            _presenter = presenter;
        }

        [HttpGet]
        public async Task<IActionResult> GetTournaments(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] string? gender)
        {
            var results = await _presenter.GetTournamentsAsync(from, to, gender);
            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> SimulateTournament([FromBody] List<PlayerModel> players)
        {
            if (players == null || players.Count < 2)
                return BadRequest("At least two players are required.");

            try
            {
                var result = await _presenter.SimulateTournamentAsync(players);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing tournament: {ex.Message}");
            }
        }
    }
}
