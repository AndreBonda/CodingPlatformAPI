using System;
namespace CodingPlatform.Domain
{
    public class Leaderboard
    {
        public class Placement
        {
            public string Username { get; private set; }
            public int TotalChallenges { get; private set; }
            public decimal TotalPoints { get; private set; }
            public decimal AveragePoints { get; private set; }
        }

        private readonly IEnumerable<Placement> _placements;

        private Leaderboard(IEnumerable<Placement> placements)
        {
            _placements = placements ?? new List<Placement>();
        }

        public Dictionary<int, Placement> GetPlacementsSortedByTotalPoints(bool sortingAsc = false)
        {
            IEnumerable<Placement> sortedPlacements;

            if (sortingAsc)
                sortedPlacements = _placements
                    .OrderBy(p => p.TotalPoints);
            else
                sortedPlacements = _placements
                    .OrderByDescending(p => p.TotalPoints);

            return NumeratedPlacements(sortedPlacements.ToArray());
        }

        public Dictionary<int, Placement> GetPlacementsSortedByAveragePoints(bool sortingAsc = false)
        {
            IEnumerable<Placement> sortedPlacements;

            if (sortingAsc)
                sortedPlacements = _placements
                    .OrderBy(p => p.AveragePoints);
            else
                sortedPlacements = _placements
                    .OrderByDescending(p => p.AveragePoints);

            return NumeratedPlacements(sortedPlacements.ToArray());
        }

        private Dictionary<int, Placement> NumeratedPlacements(Placement[] placements)
        {
            var result = new Dictionary<int, Placement>();

            for (int i = 0; i < _placements.Count(); i++)
                result.Add(i + 1, placements[i]);

            return result;
        }
    }
}

