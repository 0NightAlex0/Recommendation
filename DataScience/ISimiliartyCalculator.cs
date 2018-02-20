using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    interface ISimiliartyCalculator
    {
        double Calculate(Dictionary<int, double> user1, Dictionary<int, double> user2);
    }
}
