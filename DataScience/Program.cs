using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataScience
{

    class Program
    {

        static void Main(string[] args)
        {
            ISimiliartyCalculator euclidean = new Euclidean();
            ISimiliartyCalculator pearson = new Pearson();
            ISimiliartyCalculator cosine = new Cosine();

            // similarity
            //Dictionary<int, double> testUser1 = dataSet[7].UserRatings;
            //Dictionary<int, double> testUser2 = dataSet[4].UserRatings;
            //Console.WriteLine(euclidean.Calculate(testUser1, testUser2) + " euclidean");
            //Console.WriteLine(pearson.Calculate(testUser1, testUser2) + " pearson");
            //Console.WriteLine(cosine.Calculate(testUser1, testUser2) + " cosine");

            Dictionary<int, UserPreferance> dataSet = ParseDataSet(@"D:\OneDrive\INF\data-science\userItem.data", ",");
            //// neighbour and ratings
            //int testId = 7;
            ////dataSet[testId].UserRatings.Add(106, 5);
            //KeyValuePair<int, UserPreferance> testPair = new KeyValuePair<int, UserPreferance>(testId, dataSet[testId]);
            //List<KeyValuePair<int, UserPreferance>> neighbours = GetNearestNeighbours(dataSet, testPair, pearson, 3);
            //List<KeyValuePair<int, double>> ratingPrediction = new RatingPredictionCalculator().PredictGivenList(neighbours, new List<int>(new int[] { 101, 103, 106 }));

            //Dictionary<int, UserPreferance> dataSet = ParseDataSet(@"D:\OneDrive\INF\data-science\u.data", "\t");
            //int testId = 186;
            //KeyValuePair<int, UserPreferance> testPair = new KeyValuePair<int, UserPreferance>(testId, dataSet[testId]);
            //List<KeyValuePair<int, UserPreferance>> neighbours = GetNearestNeighbours(dataSet, testPair, pearson, 25);
            //List<KeyValuePair<int, double>> ratingPrediction = new RatingPredictionCalculator().PredictAll(neighbours, testPair).GetRange(0, 8);

            //var watch = System.Diagnostics.Stopwatch.StartNew();
            //watch.Stop();
            //var elapsedMs = watch.ElapsedMilliseconds;
            //Console.WriteLine(elapsedMs);
            Console.ReadLine();


        }
        // tab seperated
        public static Dictionary<int, UserPreferance> ParseDataSet(string path, string seperator)
        {
            string[] lines = File.ReadAllLines(path);

            Dictionary<int, UserPreferance> dataSet = new Dictionary<int, UserPreferance>();
            foreach (string line in lines)
            {
                string[] lineSplit = line.Split(seperator);
                int userId = int.Parse(lineSplit[0]);
                int articleId = int.Parse(lineSplit[1]);
                double rating = double.Parse(lineSplit[2]);

                if (!dataSet.ContainsKey(userId))
                {
                    dataSet.Add(userId, new UserPreferance());
                }
                dataSet[userId].UserRatings.Add(articleId, rating);
            }
            return dataSet;
        }

        public static void PrintDataSet(Dictionary<int, UserPreferance> dataSet)
        {
            foreach (KeyValuePair<int, UserPreferance> user in dataSet)
            {
                Console.WriteLine("Key = {0}", user.Key);
                foreach (KeyValuePair<int, double> article in user.Value.UserRatings)
                {
                    Console.WriteLine("Key = {0}, Value = {1}", article.Key, article.Value);
                }
            }
        }

        public static bool DictionaryHasExtra<T, U>(Dictionary<T, U> current, Dictionary<T, U> target)
        {
            return current.Keys.Any(key => !target.ContainsKey(key));
        }

        public static List<KeyValuePair<int, UserPreferance>> GetNearestNeighbours(
            Dictionary<int, UserPreferance> users,
            KeyValuePair<int, UserPreferance> target,
            ISimiliartyCalculator similarityCalculator,
            int maxNeighbours)
        {

            List<KeyValuePair<int, UserPreferance>> neighbours = new List<KeyValuePair<int, UserPreferance>>();
            double similarityThreshold = 0.35;

            foreach (KeyValuePair<int, UserPreferance> user in users)
            {
                if (user.Key != target.Key)
                {
                    double similarity = similarityCalculator.Calculate(user.Value.UserRatings, target.Value.UserRatings);
                    bool hasExtra = DictionaryHasExtra(user.Value.UserRatings, target.Value.UserRatings);

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
            return neighbours;
        }

        public static List<int> GetAllArticleIds(Dictionary<int, UserPreferance> users)
        {
            List<int> articleIds = new List<int>();
            foreach (KeyValuePair<int, UserPreferance> user in users)
            {
                foreach(int articleId in user.Value.UserRatings.Keys)
                {
                    if(!articleIds.Any(x => x == articleId))
                    {
                        articleIds.Add(articleId);
                    }
                }
            }
            return articleIds.OrderByDescending(x => x).ToList();
        }

        public static Dictionary<int, Dictionary<int, DeviationData>> GetDeviationTable(Dictionary<int, UserPreferance> users)
        {
            Dictionary<int, Dictionary<int, DeviationData>> result = new Dictionary<int, Dictionary<int, DeviationData>>();
            List<int> articleIds = GetAllArticleIds(users);
            for (int i =0; i < articleIds.Count; i++)
            {
                for(int j = 0; j < articleIds.Count; j++)
                {
                    //GetDeviation(users, articleIds[i], articleIds[i++]);
                    //result.Add(i, new Dictionary<j>)
                }
                
            }

            
            //deviation(i, j) =>
            //         currdev = 0
            //         foreach user u who rated both items i and j
            //          currdev += (rating of user u to item i) - (rating of user u to item j)
            //         currdev/(how many users rated both i and j)

            return result;
        }

        public static DeviationData GetDeviationData(Dictionary<int, UserPreferance> users, int itemA, int itemB)
        {
            double currentDeviation = 0;
            int count = 0;
            foreach (KeyValuePair<int, UserPreferance> user in users)
            {
                Dictionary<int, double> ratings = user.Value.UserRatings;
                if (ratings.ContainsKey(itemA) && ratings.ContainsKey(itemB))
                {
                    
                    currentDeviation += ratings[itemA] - ratings[itemB];
                    count++;
                }
            }
            return new DeviationData(currentDeviation/count, count);
        }
    }
}
