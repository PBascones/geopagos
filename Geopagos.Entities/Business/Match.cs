namespace Geopagos.Entities.Business
{
    public class Match
    {
        private static readonly Random _random = new();

        public Player Player1 { get; }
        public Player Player2 { get; }

        public Match(Player p1, Player p2)
        {
            Player1 = p1;
            Player2 = p2;
        }

        public Player GetWinner()
        {
            double luck1 = GetLuckFactor();
            double luck2 = GetLuckFactor();

            double score1 = Player1.GetEffectiveScore() * luck1;
            double score2 = Player2.GetEffectiveScore() * luck2;

            return score1 >= score2 ? Player1 : Player2;
        }

        private double GetLuckFactor()
        {
            return 0.9 + _random.NextDouble() * 0.2; // 0.9 - 1.1
        }
    }
}
