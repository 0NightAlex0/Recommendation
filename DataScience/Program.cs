using System;
using System.Collections.Generic;
using System.IO;

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
            int testUser1 = 1;
            int testUser2 = 6;
            Console.WriteLine(euclidean.Calculate(dataSet[testUser1], dataSet[testUser2]) + " euclidean");
            Console.WriteLine(pearson.Calculate(dataSet[testUser1], dataSet[testUser2]) + " pearson");
            Console.WriteLine(cosine.Calculate(dataSet[testUser1], dataSet[testUser2]) + " cosine");

            //KeyValuePair<int, Dictionary<int, double>> target = new KeyValuePair<int, Dictionary<int, double>>(7, dataSet[7]);
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

        //static Dictionary<int, Dictionary<int, double>> KNearestNeighbours(
        //    Dictionary<int, Dictionary<int, double>> users,
        //    KeyValuePair<int, Dictionary<int, double>> target, 
        //    Func<Dictionary<int, double>, Dictionary<int, double>, double> similarityFunction)
        //{
        //    int maxListLength = 3;
        //    double similarityThreshold = 0.35;
        //    foreach(KeyValuePair<int, Dictionary<int, double>> user in users)
        //    {
        //        if (user.Key != target.Key)
        //        {
        //            // find the similarity
        //        }
        //    }
        //}
    }
}
