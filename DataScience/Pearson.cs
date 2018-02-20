using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    class Pearson : ISimiliartyCalculator
    {
        public double Calculate(Dictionary<int, double> user1, Dictionary<int, double> user2)
        {
            double counter = 0;
            double sumXY = 0;
            double sumX = 0;
            double sumY = 0;
            double sumXX = 0;
            double sumYY = 0;

            foreach (KeyValuePair<int, double> user1Article in user1)
            {
                if (user2.ContainsKey(user1Article.Key))
                {
                    counter++;
                    double x = user1Article.Value;
                    double y = user2[user1Article.Key];
                    sumXY += x * y;
                    sumX += x;
                    sumY += y;
                    sumXX += x * x;
                    sumYY += y * y;
                }
            }
            double upperResult = sumXY - (sumX * sumY) / counter;
            double lowerResult = Math.Sqrt(sumXX - Math.Pow(sumX, 2) / counter) * Math.Sqrt(sumYY - Math.Pow(sumY, 2) / counter);

            return upperResult / lowerResult;
        }
    }
}
