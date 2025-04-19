namespace Geopagos.Presenter.Models
{
    public class TournamentResultModel
    {
        public int Id { get; set; }
        public DateTime PlayedDate { get; set; }
        public string Gender { get; set; }
        public string WinnerName { get; set; }
        public List<PlayerModel> Players { get; set; }
    }
}
