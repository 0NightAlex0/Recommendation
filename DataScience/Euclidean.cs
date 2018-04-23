using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    class Euclidean : DistanceCalculatorClass
    {
        public override double Calculate(Dictionary<int, double> ratings1, Dictionary<int, double> ratings2)
        {
            double sumRating = 0;
            foreach (KeyValuePair<int, double> pair1 in ratings1)
            {
                if (ratings2.ContainsKey(pair1.Key))
                {
                    sumRating += Math.Pow(pair1.Value - ratings2[pair1.Key], 2);
                }
            }
            double distance = Math.Sqrt(sumRating);
            return Similarity(distance);
        }
    }
}
