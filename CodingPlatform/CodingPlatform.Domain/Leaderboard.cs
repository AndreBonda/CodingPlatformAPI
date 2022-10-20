using System;
namespace CodingPlatform.Domain
{
    public class Leaderboard
    {
        public enum Sorting { TOTAL_PTS, AVERAGE_PTS }

        private readonly IEnumerable<Placement> _placements;

        public Leaderboard(IEnumerable<Placement> placements)
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

        public Dictionary<int, Placement> GetSortedPlacements(Sorting sortingType, bool sortingAsc = false)
            => sortingType switch
            {
                Sorting.TOTAL_PTS => GetPlacementsSortedByTotalPoints(sortingAsc),
                Sorting.AVERAGE_PTS => GetPlacementsSortedByAveragePoints(sortingAsc),
                _ => throw new ArgumentException(nameof(sortingType))
            };

        private Dictionary<int, Placement> NumeratedPlacements(Placement[] placements)
        {
            var result = new Dictionary<int, Placement>();

            for (int i = 0; i < _placements.Count(); i++)
                result.Add(i + 1, placements[i]);

            return result;
        }
    }
}

