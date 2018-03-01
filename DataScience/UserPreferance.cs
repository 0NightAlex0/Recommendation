using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    class UserPreferance
    {
        Dictionary<int, double> _userRatings = new Dictionary<int, double>();
        // temp similarity here
        public double similarity;
        public UserPreferance()
        {
        }
        public Dictionary<int, double> UserRatings
        {
            get
            {
                return _userRatings;
            }

        }
    }
}
