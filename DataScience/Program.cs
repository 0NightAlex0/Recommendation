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
            
            //Dictionary<int, UserPreferance> dataSet = ParseSmallDataSet(@"D:\OneDrive\INF\data-science\userItem.data");
            //Dictionary<int, UserPreferance> dataSet = ParseDataSet(@"D:\OneDrive\INF\data-science\ratings.csv");


            //PrintDataSet(dataSet);
            ISimiliartyCalculator euclidean = new Euclidean();
            ISimiliartyCalculator pearson = new Pearson();
            ISimiliartyCalculator cosine = new Cosine();

            // similarity
            //Dictionary<int, double> testUser1 = dataSet[7].UserRatings;
            //Dictionary<int, double> testUser2 = dataSet[4].UserRatings;
            //Console.WriteLine(euclidean.Calculate(testUser1, testUser2) + " euclidean");
            //Console.WriteLine(pearson.Calculate(testUser1, testUser2) + " pearson");
            //Console.WriteLine(cosine.Calculate(testUser1, testUser2) + " cosine");

            // neighbour and ratings
            //int testId = 7;
            //dataSet[testId].UserRatings.Add(106, 5);
            //KeyValuePair<int, UserPreferance> testPair = new KeyValuePair<int, UserPreferance>(testId, dataSet[testId]);
            //List<KeyValuePair<int, UserPreferance>> neighboursTo7 = NearestNeighbours(dataSet, testPair, pearson);
            //Dictionary<int, double> ratingPredictionOf7 = new RatingPredictionCalculator().Calculate(neighboursTo7, new List<int>(new int[] { 101, 103 ,106}));

            //int testId = 4;
            //KeyValuePair<int, UserPreferance> testPair = new KeyValuePair<int, UserPreferance>(testId, dataSet[testId]);
            //List<KeyValuePair<int, UserPreferance>> neighboursTo4 = NearestNeighbours(dataSet, testPair, pearson);
            //Dictionary<int, double> ratingPredictionOf4 = new RatingPredictionCalculator().Calculate(neighboursTo4, new List<int>(new int[] { 101 }));

            //Dictionary<int, UserPreferance> dataSet = ParseSmallDataSet(@"D:\OneDrive\INF\data-science\userItem.data");
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            //watch.Stop();
            //var elapsedMs = watch.ElapsedMilliseconds;
            //Console.WriteLine(elapsedMs);
            Console.ReadLine();

        }
        
        static Dictionary<int, UserPreferance> ParseDataSet(string path, bool hasTitle)
        {
            //if (hasTitle)
            //{
            //    string[] lines = File.ReadAllLines(path).se;
            //}
            //else
            //{
            //    string[] lines = File.ReadAllLines(path);
            //}
            string[] lines = File.ReadAllLines(path);

            Dictionary<int, UserPreferance> dataSet = new Dictionary<int, UserPreferance>();
            foreach (string line in lines)
            {
                string[] lineSplit = line.Split(",");
                int userId = int.Parse(lineSplit[0]);
                int articleId = int.Parse(lineSplit[1]);
                double rating = double.Parse(lineSplit[2]);
                UserPreferance preference = new UserPreferance();

                if (!dataSet.ContainsKey(userId))
                {
                    dataSet.Add(userId, preference);
                }
                dataSet[userId].UserRatings.Add(articleId, rating);
            }
            return dataSet;
        }

        static void PrintDataSet(Dictionary<int, UserPreferance> dataSet)
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

        static bool DictionaryHasExtra<T, U>(Dictionary<T, U> current, Dictionary<T, U> target)
        {
            return current.Keys.Any(key => !target.ContainsKey(key));
        }

        static List<KeyValuePair<int, UserPreferance>> NearestNeighbours(
            Dictionary<int, UserPreferance> users,
            KeyValuePair<int, UserPreferance> target,
            ISimiliartyCalculator similarityCalculator)
        {

            List<KeyValuePair<int, UserPreferance>> neighbours = new List<KeyValuePair<int, UserPreferance>>();
            int maxListLength = 3;
            double similarityThreshold = 0.35;

            foreach (KeyValuePair<int, UserPreferance> user in users)
            {
                if (user.Key != target.Key)
                {
                    double similarity = similarityCalculator.Calculate(user.Value.UserRatings, target.Value.UserRatings);
                    bool hasExtra = DictionaryHasExtra(user.Value.UserRatings, target.Value.UserRatings);

                    if (similarity > similarityThreshold && hasExtra)
                    {
                        if (neighbours.Count < maxListLength)
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

                    if (neighbours.Count == maxListLength)
                    {
                        similarityThreshold = neighbours.Min(entry => entry.Value.similarity);
                    }
                }
            }
            return neighbours;
        }
    }
}
