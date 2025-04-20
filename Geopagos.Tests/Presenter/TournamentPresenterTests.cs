using Geopagos.Entities.Business;
using Geopagos.Entities.Enums;
using Geopagos.Presenter;
using Geopagos.Presenter.Models;
using Geopagos.Services.Base;
using Geopagos.Services.Interfaces;
using Moq;

namespace Geopagos.Tests.Presenter
{
    public class TournamentPresenterTests
    {
        private readonly Mock<ITournamentService> _serviceMock;
        private readonly TournamentPresenter _presenter;

        public TournamentPresenterTests()
        {
            _serviceMock = new Mock<ITournamentService>();
            _presenter = new TournamentPresenter(_serviceMock.Object);
        }

        [Fact]
        public async Task SimulateTournamentAsync_ShouldMapModelsAndCallService()
        {
            // Arrange
            var models = new List<PlayerModel>
            {
                new PlayerModel { Name = "Paola", SkillLevel = 95, Gender = Gender.Female, ReactionTime = 90 },
                new PlayerModel { Name = "Gladys", SkillLevel = 88, Gender = Gender.Female, ReactionTime = 92 }
            };

            var expectedResponse = new ServiceResponse { ReturnValue = 1 };
            _serviceMock.Setup(s => s.SimulateAndStoreTournamentAsync(It.IsAny<List<Player>>()))
                        .ReturnsAsync(expectedResponse);

            // Act
            var result = await _presenter.SimulateTournamentAsync(models);

            // Assert
            Assert.True(result.Status);
            Assert.Equal(1, result.ReturnValue);
            _serviceMock.Verify(s => s.SimulateAndStoreTournamentAsync(It.IsAny<List<Player>>()), Times.Once);
        }

        [Fact]
        public async Task GetTournamentsAsync_ShouldMapEntitiesToModelsCorrectly()
        {
            // Arrange
            var tournaments = new List<TournamentResult>
            {
                new TournamentResult
                {
                    Id = 1,
                    Gender = "Female",
                    PlayedDate = DateTime.UtcNow,
                    WinnerSnapshotId = 1,
                    Players = new List<PlayerSnapshot>
                    {
                        new PlayerSnapshot { Id = 1, Name = "Paola", Gender = "Female", SkillLevel = 95, ReactionTime = 90 },
                        new PlayerSnapshot { Id = 2, Name = "Gladys", Gender = "Female", SkillLevel = 88, ReactionTime = 92 }
                    }
                }
            };

            _serviceMock.Setup(s => s.GetTournamentsAsync(null, null, null))
                        .ReturnsAsync(tournaments);

            // Act
            var result = await _presenter.GetTournamentsAsync(null, null, null);

            // Assert
            Assert.True(result.Status);
            Assert.Single(result.Data);
            Assert.True(result.Data[0].Players.First(p => p.Name == "Paola").IsWinner);
            Assert.False(result.Data[0].Players.First(p => p.Name == "Gladys").IsWinner);
        }

        [Fact]
        public async Task GetTournamentsAsync_ShouldReturnError_WhenNoResults()
        {
            _serviceMock.Setup(s => s.GetTournamentsAsync(null, null, null))
                        .ReturnsAsync(new List<TournamentResult>());

            var result = await _presenter.GetTournamentsAsync(null, null, null);

            Assert.False(result.Status);
            Assert.Single(result.Errors);
            Assert.Contains("No tournaments found", result.Errors[0].ErrorMessage);
        }
    }
}
