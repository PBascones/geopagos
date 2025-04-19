using Geopagos.Entities.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Geopagos.Repository.Repositories
{
    public interface ITournamentRepository
    {
        Task<List<TournamentResult>> GetTournamentResultsAsync(DateTime? fromDate, DateTime? toDate, string? gender);
        Task SaveTournamentResultAsync(TournamentResult result);
        Task UpdateWinnerAsync(int tournamentId, int winnerSnapshotId);
    }

    public class TournamentRepository : ITournamentRepository
    {
        private readonly GeopagosDbContext _dbContext;
        private readonly ILogger<TournamentRepository> _logger;

        public TournamentRepository(GeopagosDbContext dbContext, ILogger<TournamentRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<TournamentResult>> GetTournamentResultsAsync(DateTime? fromDate, DateTime? toDate, string? gender)
        {
            var query = _dbContext
                .TournamentResult
                .Include(x => x.Players)
                .Include(x => x.WinnerSnapshot)
                .AsNoTracking();

            if (fromDate.HasValue)
                query = query.Where(x => x.PlayedDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(x => x.PlayedDate <= toDate.Value);

            if (!string.IsNullOrWhiteSpace(gender))
                query = query.Where(x => x.Gender == gender);

            return await query
                .OrderByDescending(x => x.PlayedDate)
                .ToListAsync();
        }

        public async Task SaveTournamentResultAsync(TournamentResult result)
        {
            try
            {
                _dbContext.TournamentResult.Add(result);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting tournament result.");
                throw new Exception("An error occurred while inserting the tournament result.", ex);
            }
        }

        public async Task UpdateWinnerAsync(int tournamentId, int winnerSnapshotId)
        {
            try
            {
                var dbTournamentResult = await _dbContext.TournamentResult.FindAsync(tournamentId);
                if (dbTournamentResult == null)
                    throw new Exception("Tournament not found.");

                dbTournamentResult.WinnerSnapshotId = winnerSnapshotId;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating winner.");
                throw new Exception("Update failed.", ex);
            }
        }
    }
}
