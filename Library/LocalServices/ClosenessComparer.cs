using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RdLibrary.LocalServices
{
    public class ClosenessComparer : IEqualityComparer<float>
    {
        private readonly float delta;

        public ClosenessComparer(float delta)
        {
            this.delta = delta;
        }

        public bool Equals(float x, float y)
        {
            return Math.Abs((x + y) / 2f - y) < delta;
        }

        public int GetHashCode(float obj)
        {
            return 0;
        }
    }
}