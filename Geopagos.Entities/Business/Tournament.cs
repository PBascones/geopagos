namespace Geopagos.Entities.Business
{
    public class Tournament
    {
        public List<Player> Players { get; }

        public Tournament(List<Player> players)
        {
            if (players.Count == 0 || (players.Count & (players.Count - 1)) != 0)
                throw new ArgumentException("The number of players must be a power of 2.");

            Players = players;
        }

        public Player Simulate()
        {
            var roundPlayers = new List<Player>(Players);
            while (roundPlayers.Count > 1)
            {
                var nextRound = new List<Player>();
                for (int i = 0; i < roundPlayers.Count; i += 2)
                {
                    var match = new Match(roundPlayers[i], roundPlayers[i + 1]);
                    nextRound.Add(match.GetWinner());
                }
                roundPlayers = nextRound;
            }

            return roundPlayers.First();
        }
    }
}
