using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    abstract class DistanceCalculatorClass : ISimiliartyCalculator
    {
        public abstract double Calculate(Dictionary<int, double> ratings1, Dictionary<int, double> ratings2);

        public double Similarity(double distance)
        {
            return 1 / (1 + distance);
        }

    }
}
