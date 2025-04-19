using Geopagos.Entities.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Geopagos.Repository.Repositories
{
    public interface ITournamentRepository
    {
        Task SaveTournamentResultAsync(TournamentResult result);
        Task<List<TournamentResult>> GetTournamentResultsAsync(DateTime? fromDate, DateTime? toDate, string? gender);
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

        public async Task SaveTournamentResultAsync(TournamentResult result)
        {
            try
            {
                _dbContext.TournamentResult.Add(result);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving tournament result.");
                throw new Exception("An error occurred while saving the tournament result.", ex);
            }
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
    }
}
