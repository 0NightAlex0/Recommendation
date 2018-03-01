using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    class RatingPredictionCalculator
    {

        public Dictionary<int, double> Calculate(List<KeyValuePair<int, UserPreferance>> neighbours, List<int> articleIds)
        {
            Dictionary<int, double> ratingPredictions = new Dictionary<int, double>();
            foreach (int articleId in articleIds)
            {
                double weightedRating = 0;
                double sumSimilarties = 0;
                foreach (KeyValuePair<int, UserPreferance> neighbour in neighbours)
                {
                    Dictionary<int, double> ratings = neighbour.Value.UserRatings;
                    if (ratings.ContainsKey(articleId))
                    {
                        double similarity = neighbour.Value.similarity;
                        weightedRating += ratings[articleId] * similarity;
                        sumSimilarties += similarity;
                    }
                }
                ratingPredictions.Add(articleId, weightedRating / sumSimilarties);

            }
            return ratingPredictions;
        }
    }
}
