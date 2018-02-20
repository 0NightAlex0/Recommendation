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

            //PrintDictionary(ParseDataSet(@"D:\OneDrive\INF\data-science\DataScience\userItem.data"));
            Dictionary<int, Dictionary<int, double>> dataSet = ParseDataSet(@"D:\OneDrive\INF\data-science\DataScience\userItem.data");

            ISimiliartyCalculator euclidean = new Euclidean();
            ISimiliartyCalculator pearson = new Pearson();
            ISimiliartyCalculator cosine = new Cosine();
            //Dictionary<int, double> testUser1 = dataSet[1];
            //Dictionary<int, double> testUser2 = dataSet[2];
            //Console.WriteLine(euclidean.Calculate(testUser1, testUser2) + " euclidean");
            //Console.WriteLine(pearson.Calculate(testUser1, testUser2) + " pearson");
            //Console.WriteLine(cosine.Calculate(testUser1, testUser2) + " cosine");
            int testId = 7;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<User> neighboursTo7 =  NearestNeighbours(dataSet, new KeyValuePair<int, Dictionary<int, double>>(testId, dataSet[testId]), pearson);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);
            Console.ReadLine();

        }
        
        static Dictionary<int, Dictionary<int, double>> ParseDataSet(string path)
        {
            string[] lines = File.ReadAllLines(path);
            Dictionary<int, Dictionary<int, double>> dataSet = new Dictionary<int, Dictionary<int, double>>();
            foreach (string line in lines)
            {
                string[] lineSplit = line.Split(",");
                int user = int.Parse(lineSplit[0]);
                int article = int.Parse(lineSplit[1]);
                double rating = double.Parse(lineSplit[2]);
                if (!dataSet.ContainsKey(user))
                {
                    dataSet.Add(user, new Dictionary<int, double>());
                }
                dataSet[user].Add(article, rating);
            }
            return dataSet;
        }

        static void PrintDictionary<T,U>(Dictionary<T, Dictionary<T, U>> dataSet)
        {
            foreach (KeyValuePair<T, Dictionary<T, U>> user in dataSet)
            {
                Console.WriteLine("Key = {0}", user.Key);
                foreach (KeyValuePair<T, U> article in user.Value)
                {
                    Console.WriteLine("Key = {0}, Value = {1}", article.Key, article.Value);
                }
            }
        }

        static List<User> NearestNeighbours(
            Dictionary<int, Dictionary<int, double>> users,
            KeyValuePair<int, Dictionary<int, double>> target,
            ISimiliartyCalculator similarityCalculator)
        {

            List<User> neighbours = new List<User>();
            int maxListLength = 3;
            double similarityThreshold = 0.35;

            foreach (KeyValuePair<int, Dictionary<int, double>> user in users)
            {

                if (user.Key != target.Key && user.Value.Count > target.Value.Count)
                {
                    double similarity = similarityCalculator.Calculate(user.Value, target.Value);
                    if (similarity > similarityThreshold)
                    {
                        if (neighbours.Count < maxListLength)
                        {
                            neighbours.Add(new User(user, similarity));
                        }
                        else
                        {
                            double minSimilarity = neighbours.Min(entry => entry.similarity);
                            if(similarity > minSimilarity)
                            {
                                User furthestNeighbour = neighbours.Find(entry => entry.similarity == minSimilarity);
                                neighbours.Remove(furthestNeighbour);
                                neighbours.Add(new User(user, similarity));
                            }
                        }
                    }

                    if (neighbours.Count == maxListLength)
                    {
                        similarityThreshold = neighbours.Min(entry => entry.similarity);
                    }
                }
            }
            return neighbours;
        }
    }
}
