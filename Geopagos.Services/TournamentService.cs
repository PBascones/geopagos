using Geopagos.Entities.Business;
using Geopagos.Repository.Repositories;
using Geopagos.Services.Base;
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

        public async Task<List<TournamentResult>> GetTournamentsAsync(DateTime? fromDate, DateTime? toDate, string? gender)
        {
            return await _tournamentRepository.GetTournamentResultsAsync(fromDate, toDate, gender);
        }

        public async Task<ServiceResponse> SimulateAndStoreTournamentAsync(List<Player> players)
        {
            var sr = new ServiceResponse();

            if (players == null || players.Count < 2)
                throw new ArgumentException("At least two players are required.");

            try
            {
                var winner = RunTournament(players);

                // Map each Player to its corresponding Snapshot
                var playerToSnapshotMap = players.ToDictionary(
                    p => p,
                    p => p.ToSnapshot()
                );

                var result = new TournamentResult
                {
                    PlayedDate = DateTime.UtcNow,
                    Gender = players.First().Gender.ToString(),
                    Players = playerToSnapshotMap.Values.ToList()
                };

                await _tournamentRepository.SaveTournamentResultAsync(result);

                // After saving, the snapshot IDs are populated
                result.WinnerSnapshotId = playerToSnapshotMap[winner].Id;

                // Update winner ID
                await _tournamentRepository.UpdateWinnerAsync(result.Id, result.WinnerSnapshotId.Value);

                sr.ReturnValue = result.Id;
            }
            catch (Exception ex)
            {
                sr.AddError($"An error occurred while saving the tournament result: {ex.InnerException?.Message ?? ex.Message}");
            }

            return sr;
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
