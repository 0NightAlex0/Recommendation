using System.Collections.Generic;
using System.Linq;
using System;
namespace DataScience
{
    class UserItem
    {
        private List<KeyValuePair<int, UserPreferance>> neighbours = new List<KeyValuePair<int, UserPreferance>>();
        private Dictionary<int, UserPreferance> users;
        public UserItem(Dictionary<int, UserPreferance> dataset)
        {
            this.users = dataset;
        }

        private double CalculateRating(int articleId)
        {
            double weightedRating = 0;
            double sumSimilarties = 0;
            foreach (KeyValuePair<int, UserPreferance> neighbour in neighbours)
            {
                UserPreferance userPreferance = neighbour.Value;
                Dictionary<int, double> ratings = userPreferance.UserRatings;
                if (ratings.ContainsKey(articleId))
                {
                    double similarity = userPreferance.similarity;
                    weightedRating += ratings[articleId] * similarity;
                    sumSimilarties += similarity;
                }
            }
            return weightedRating / sumSimilarties;
        }

        // if >3 neighbours rated, then calculate rating
        public void ComputeNearestNeighbours(
           KeyValuePair<int, UserPreferance> target,
           ISimiliartyCalculator similarityCalculator,
           int maxNeighbours)
        {
            double similarityThreshold = 0.35;

            foreach (KeyValuePair<int, UserPreferance> user in users)
            {
                if (user.Key != target.Key)
                {
                    double similarity = similarityCalculator.Calculate(user.Value.UserRatings, target.Value.UserRatings);
                    bool hasExtra = Program.DictionaryHasExtra(user.Value.UserRatings, target.Value.UserRatings);

                    if (similarity > similarityThreshold && hasExtra)
                    {
                        if (neighbours.Count < maxNeighbours)
                        {
                            user.Value.similarity = similarity;
                            neighbours.Add(user);
                        }
                        else
                        {
                            double minSimilarity = neighbours.Min(entry => entry.Value.similarity);
                            if (similarity > minSimilarity)
                            {
                                KeyValuePair<int, UserPreferance> furthestNeighbour = neighbours.Find(entry => entry.Value.similarity == minSimilarity);
                                neighbours.Remove(furthestNeighbour);
                                user.Value.similarity = similarity;
                                neighbours.Add(user);
                            }
                        }
                    }

                    if (neighbours.Count == maxNeighbours)
                    {
                        similarityThreshold = neighbours.Min(entry => entry.Value.similarity);
                    }
                }
            }
            neighbours.OrderByDescending(x => x.Value.similarity).ToList();
        }

        public List<KeyValuePair<int, double>> PredictAll(KeyValuePair<int, UserPreferance> target)
        {
            List<KeyValuePair<int, double>> ratingPredictions = new List<KeyValuePair<int, double>>();
            // which key does the target not contain that the neighbors have
            List<int> missingKeys = new List<int>();
            foreach (KeyValuePair<int, UserPreferance> neighbour in neighbours)
            {
                foreach (int articleId in neighbour.Value.UserRatings.Keys)
                {
                    if (!missingKeys.Any(x => x == articleId) && !target.Value.UserRatings.ContainsKey(articleId))
                    {
                        missingKeys.Add(articleId);
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
                        double rating = CalculateRating(key);
                        ratingPredictions.Add(new KeyValuePair<int, double>(key, rating));
                        break;
                    }

                }
            }

            return ratingPredictions.OrderByDescending(pair => pair.Value).ToList();
        }

        public List<KeyValuePair<int, double>> PredictGivenList(List<int> articleIds)
        {
            List<KeyValuePair<int, double>> ratingPredictions = new List<KeyValuePair<int, double>>();
            foreach (int articleId in articleIds)
            {
                double rating = CalculateRating(articleId);
                ratingPredictions.Add(new KeyValuePair<int, double>(articleId, rating));

            }
            return ratingPredictions.OrderByDescending(pair => pair.Value).ToList();
        }

    }
}
