using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    interface ISimiliartyCalculator
    {
        double Calculate(Dictionary<int, double> ratings1, Dictionary<int, double> ratings2);
    }
}
