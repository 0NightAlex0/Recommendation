using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    class RatingPredictionCalculator
    {

        public Dictionary<int, double> CalculateGivenList(List<KeyValuePair<int, UserPreferance>> neighbours, List<int> articleIds)
        {
            Dictionary<int, double> ratingPredictions = new Dictionary<int, double>();
            foreach (int articleId in articleIds)
            {
                double rating = this.CalculateRating(neighbours, articleId);
                ratingPredictions.Add(articleId, rating);

            }
            return ratingPredictions;
        }

        private double CalculateRating(List<KeyValuePair<int, UserPreferance>> neighbours, int articleId)
        {
            double weightedRating = 0;
            double sumSimilarties = 0;
            foreach (KeyValuePair<int, UserPreferance> neighbour in neighbours)
            {
                UserPreferance user = neighbour.Value;
                Dictionary<int, double> ratings = user.UserRatings;
                if (ratings.ContainsKey(articleId))
                {
                    double similarity = user.similarity;
                    weightedRating += ratings[articleId] * similarity;
                    sumSimilarties += similarity;
                }
            }
            return weightedRating / sumSimilarties;
        }
    }
}
