using System;

namespace Advent2020
{
    public class Range
    {
        public readonly int Min;
        public readonly int Max;

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
            if (Min > Max) throw new ArgumentException($"invalid range: Min ({Min}) > Max ({Max})");
        }

        public bool Contains(int value) => Min <= value && value <= Max;

        public override string ToString()
        {
            return $"{Min}-{Max}";
        }

        public Range ExtendTo(int value)
        {
            if (Contains(value)) return this;
            if (value < Min) return new Range(value, Max);
            if (value > Max) return new Range(Min, value);
            throw new InvalidOperationException("wat?");
        }

        public bool FullyContains(Range r)
        {
            return Contains(r.Min) && Contains(r.Max);
        }

        public bool Overlaps(Range r)
        {
            return Contains(r.Max) || Contains(r.Min) || r.Contains(Min) || r.Contains(Max);
        }
    }
}
