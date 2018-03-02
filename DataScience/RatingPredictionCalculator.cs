using System.Collections.Generic;
using System.Linq;
using System;
namespace DataScience
{
    class RatingPredictionCalculator
    {
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
        // if >3 neighbours rated, then calculate rating



        public List<KeyValuePair<int, double>> PredictAll(List<KeyValuePair<int, UserPreferance>> neighbours, KeyValuePair<int, UserPreferance> target)
        {
            List<KeyValuePair<int, double>> ratingPredictions = new List<KeyValuePair<int, double>>();
            // which key does the target not contain
            List<int> missingKeys = new List<int>();

            foreach (KeyValuePair<int, UserPreferance> neighbour in neighbours)
            {
                foreach (int ratingId in neighbour.Value.UserRatings.Keys)
                {
                    if (!missingKeys.Any(x => x == ratingId) && !target.Value.UserRatings.ContainsKey(ratingId))
                    {
                        missingKeys.Add(ratingId);
                    }
                }
            }
            // a minimum of 3 neighbours must have this key
            foreach (int key in missingKeys)
            {
                int count = 0;
                foreach (KeyValuePair<int, UserPreferance> neighbour in neighbours)
                {
                    if (neighbour.Value.UserRatings.ContainsKey(key))
                    {
                        count++;
                    }
                    if (count >= 3)
                    {
                        // add to predictionList
                        double rating = CalculateRating(neighbours, key);
                        ratingPredictions.Add(new KeyValuePair<int, double>(key, rating));
                        break;
                    }

                }
            }
            
            return ratingPredictions.OrderByDescending(pair => pair.Value).ToList();
        }

        public List<KeyValuePair<int, double>> PredictGivenList(List<KeyValuePair<int, UserPreferance>> neighbours, List<int> articleIds)
        {
            List<KeyValuePair<int, double>> ratingPredictions = new List<KeyValuePair<int, double>>();
            foreach (int articleId in articleIds)
            {
                double rating = CalculateRating(neighbours, articleId);
                ratingPredictions.Add(new KeyValuePair<int, double>(articleId, rating));

            }
            return ratingPredictions.OrderByDescending(pair => pair.Value).ToList();
        }

    }
}
