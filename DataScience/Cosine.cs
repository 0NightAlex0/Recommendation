using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    class Cosine : ISimiliartyCalculator
    {
        public double Calculate(Dictionary<int, double> user1, Dictionary<int, double> user2)
        {
            double sumXY = 0;
            double sumXX = 0;
            double sumYY = 0;

            foreach (KeyValuePair<int, double> user1Article in user1)
            {
                double x = user1Article.Value;
                sumXX += x * x;
                if (user2.ContainsKey(user1Article.Key))
                {
                    double y = user2[user1Article.Key];
                    sumXY += x * y;
                }
            }

            foreach (KeyValuePair<int, double> user2Article in user2)
            {
                double y = user2Article.Value;
                sumYY += y * y;
            }

            return sumXY / Math.Sqrt(sumXX * sumYY);
        }
    }
}
