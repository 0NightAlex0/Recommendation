using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    class Pearson : ISimiliartyCalculator
    {
        public double Calculate(Dictionary<int, double> ratings1, Dictionary<int, double> ratings2)
        {
            double counter = 0;
            double sumXY = 0;
            double sumX = 0;
            double sumY = 0;
            double sumXX = 0;
            double sumYY = 0;

            foreach (KeyValuePair<int, double> pair1 in ratings1)
            {
                if (ratings2.ContainsKey(pair1.Key))
                {
                    counter++;
                    double x = pair1.Value;
                    double y = ratings2[pair1.Key];
                    sumXY += x * y;
                    sumX += x;
                    sumY += y;
                    sumXX += x * x;
                    sumYY += y * y;
                }
            }
            double nominator = sumXY - (sumX * sumY) / counter;
            double denominator = Math.Sqrt(sumXX - Math.Pow(sumX, 2) / counter) * Math.Sqrt(sumYY - Math.Pow(sumY, 2) / counter);

            return nominator / denominator;
        }
    }
}
