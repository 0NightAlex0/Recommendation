using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    class Euclidean : ISimiliartyCalculator
    {
        public double Calculate(Dictionary<int, double> user1, Dictionary<int, double> user2)
        {
            double sumRating = 0;
            foreach (KeyValuePair<int, double> user1Article in user1)
            {
                if (user2.ContainsKey(user1Article.Key))
                {
                    sumRating += Math.Pow(user1Article.Value - user2[user1Article.Key], 2);
                }
            }
            double distance = Math.Sqrt(sumRating);
            return Similarity(distance);
        }

        // for euclidian and manhatten
        static double Similarity(double distance)
        {
            return 1 / (1 + distance);
        }
    }

}
