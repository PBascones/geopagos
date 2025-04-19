using Geopagos.Entities.Business;
using Geopagos.Repository.Repositories;
using Geopagos.Services.Interfaces;

namespace Geopagos.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;

        public TournamentService(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<TournamentResult> SimulateAndStoreTournamentAsync(List<Player> players)
        {
            if (players == null || players.Count < 2)
                throw new ArgumentException("At least two players are required.");

            var tournament = new Tournament
            {
                Players = players,
            };

            // Simulate the tournament and determine the winner
            var winner = RunTournament(players);

            var result = new TournamentResult
            {
                PlayedDate = DateTime.UtcNow,
                Gender = players.First().Gender.ToString(),
                Players = players.Select(p => p.ToSnapshot()).ToList()
            };

            try
            {
                await _tournamentRepository.SaveTournamentResultAsync(result);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the tournament result.", ex);
            }

            var winnerSnapshot = result.Players.First(s => s.Name == winner.Name);
            result.WinnerSnapshotId = winnerSnapshot.Id;

            return result;
        }

        public async Task<List<TournamentResult>> GetTournamentsAsync(DateTime? fromDate, DateTime? toDate, string? gender)
        {
            return await _tournamentRepository.GetTournamentResultsAsync(fromDate, toDate, gender);
        }

        public Player RunTournament(List<Player> players)
        {
            // Validate input: a tournament must have at least one player
            if (players == null || players.Count == 0)
                throw new ArgumentException("Tournament must have at least one player.");

            // Tournament simulation proceeds in rounds until a single winner remains
            while (players.Count > 1)
            {
                var nextRound = new List<Player>();

                // Process players in pairs for each match
                for (int i = 0; i < players.Count; i += 2)
                {
                    var player1 = players[i];
                    var player2 = (i + 1 < players.Count) ? players[i + 1] : null;

                    if (player2 == null)
                    {
                        // Odd number of players: player1 advances automatically
                        nextRound.Add(player1);
                    }
                    else
                    {
                        // Simulate the match and determine the winner by effective score
                        var winner = player1.GetEffectiveScore() >= player2.GetEffectiveScore()
                            ? player1
                            : player2;

                        nextRound.Add(winner);
                    }
                }

                // Prepare for the next round with the remaining winners
                players = nextRound;
            }

            // Only one player remains — the tournament winner
            return players[0];
        }
    }
}
