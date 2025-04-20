using Geopagos.Entities.Business;
using Geopagos.Repository.Repositories;
using Geopagos.Services;
using Moq;

namespace Geopagos.Tests.Services
{
    public class TournamentServiceTests
    {
        private readonly Mock<ITournamentRepository> _repoMock;
        private readonly TournamentService _service;

        public TournamentServiceTests()
        {
            _repoMock = new Mock<ITournamentRepository>();
            _service = new TournamentService(_repoMock.Object);
        }

        [Fact]
        public async Task SimulateAndStoreTournamentAsync_ShouldReturnWinnerAndSnapshots()
        {
            // Arrange
            TournamentResult? capturedResult = null;

            _repoMock.Setup(x => x.SaveTournamentResultAsync(It.IsAny<TournamentResult>()))
                .Callback<TournamentResult>(r => capturedResult = r)
                .Returns(Task.CompletedTask);

            _repoMock.Setup(x => x.UpdateWinnerAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var players = new List<Player>
            {
                new FemalePlayer("Paola", 95, 90),
                new FemalePlayer("Beatriz", 88, 92)
            };

            // Act
            var result = await _service.SimulateAndStoreTournamentAsync(players);

            // Assert
            Assert.True(result.Status);
            Assert.NotNull(capturedResult);
            Assert.Contains(capturedResult.Players, p => p.Id == capturedResult.WinnerSnapshotId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task SimulateAndStoreTournamentAsync_ShouldThrow_WhenPlayerCountInvalid(int count)
        {
            var players = new List<Player>();

            for (int i = 0; i < count; i++)
                players.Add(new FemalePlayer($"Player{i}", 80, 90));

            await Assert.ThrowsAsync<ArgumentException>(() => _service.SimulateAndStoreTournamentAsync(players));
        }

        [Fact]
        public async Task GetTournamentsAsync_ShouldCallRepositoryAndReturnResults()
        {
            var fakeData = new List<TournamentResult>
            {
                new TournamentResult { Id = 1, Gender = "Female", PlayedDate = DateTime.UtcNow }
            };

            _repoMock.Setup(r => r.GetTournamentResultsAsync(null, null, null))
                .ReturnsAsync(fakeData);

            var result = await _service.GetTournamentsAsync(null, null, null);

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task SimulateAndStoreTournamentAsync_ShouldHandleRepositoryException()
        {
            // Arrange
            _repoMock.Setup(x => x.SaveTournamentResultAsync(It.IsAny<TournamentResult>()))
                .ThrowsAsync(new Exception("Simulated DB error"));

            var players = new List<Player>
            {
                new FemalePlayer("Paola", 95, 90),
                new FemalePlayer("Beatriz", 88, 92)
            };

            // Act
            var result = await _service.SimulateAndStoreTournamentAsync(players);

            // Assert
            Assert.False(result.Status);
            Assert.Single(result.Errors);
            Assert.Contains("Simulated DB error", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task SimulateAndStoreTournamentAsync_ShouldCallUpdateWinnerWithCorrectId()
        {
            int winnerId = -1;

            _repoMock.Setup(r => r.SaveTournamentResultAsync(It.IsAny<TournamentResult>()))
                .Callback<TournamentResult>(r =>
                {
                    // Simular que se asignaron IDs post-insert
                    int id = 1;
                    foreach (var p in r.Players)
                        p.Id = id++;

                    r.Id = 99;
                })
                .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.UpdateWinnerAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Callback<int, int>((tournamentId, snapshotId) => winnerId = snapshotId)
                .Returns(Task.CompletedTask);

            var players = new List<Player>
            {
                new FemalePlayer("Paola", 95, 90),
                new FemalePlayer("Beatriz", 88, 92)
            };

            // Act
            var result = await _service.SimulateAndStoreTournamentAsync(players);

            // Assert
            Assert.True(winnerId > 0);
        }
    }
}
