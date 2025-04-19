using System.ComponentModel.DataAnnotations.Schema;

namespace Geopagos.Entities.Business
{
    public class TournamentResult
    {
        public int Id { get; set; }
        public DateTime PlayedDate { get; set; }
        public string Gender { get; set; }

        public int WinnerSnapshotId { get; set; }

        [ForeignKey(nameof(WinnerSnapshotId))]
        public virtual PlayerSnapshot WinnerSnapshot { get; set; }

        public List<PlayerSnapshot> Players { get; set; } = [];
    }
}
