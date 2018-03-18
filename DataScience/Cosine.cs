using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    class Cosine : ISimiliartyCalculator
    {
        public double Calculate(Dictionary<int, double> ratings1, Dictionary<int, double> ratings2)
        {
            double sumXY = 0;
            double sumXX = 0;
            double sumYY = 0;

            foreach (KeyValuePair<int, double> pair2 in ratings2)
            {
                if (!ratings1.ContainsKey(pair2.Key))
                {
                    ratings1.Add(pair2.Key, 0);
                }
            }

            foreach (KeyValuePair<int, double> pair1 in ratings1)
            {
                if (!ratings2.ContainsKey(pair1.Key))
                {
                    ratings2.Add(pair1.Key, 0);
                }
                double x = pair1.Value;
                double y = ratings2[pair1.Key];
                sumXX += x * x;
                sumYY += y * y;
                sumXY += x * y;
            }

            return sumXY / Math.Sqrt(sumXX * sumYY);
        }
    }
}
//Nearest neighbour 1: 6 with similarity 0.80555004051484
//Nearest neighbour 2: 2 with similarity 0.770024275094105
//Nearest neighbour 3: 5 with similarity 0.734178651201811