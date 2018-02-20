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

        // for euclidian and manhatten
        static double Similarity(double distance)
        {
            return 1 / (1 + distance);
        }

        //static double Euclidean (Dictionary<int, double> user1, Dictionary<int, double> user2)
        //{
        //    double sumRating = 0;
        //    foreach(KeyValuePair<int,double> user1Article in user1)
        //    {
        //        if (user2.ContainsKey(user1Article.Key))
        //        {
        //            sumRating += Math.Pow(user1Article.Value - user2[user1Article.Key], 2);
        //        }
        //    }
        //    double distance = Math.Sqrt(sumRating);
        //    return Similarity(distance);
        //}

        //static double Pearson(Dictionary<int, double> user1, Dictionary<int, double> user2)
        //{
        //    double counter = 0;
        //    double sumXY = 0;
        //    double sumX = 0;
        //    double sumY = 0;
        //    double sumXX = 0;
        //    double sumYY = 0;

        //    foreach (KeyValuePair<int, double> user1Article in user1)
        //    {    
        //        if (user2.ContainsKey(user1Article.Key))
        //        {
        //            counter++;
        //            double x = user1Article.Value;
        //            double y = user2[user1Article.Key];
        //            sumXY += x * y;
        //            sumX += x;
        //            sumY += y;
        //            sumXX += x * x;
        //            sumYY += y * y;
        //        }
        //    }

        //    double upperResult = sumXY - (sumX * sumY) / counter;
        //    double lowerResult = Math.Sqrt(sumXX - Math.Pow(sumX, 2) / counter) * Math.Sqrt(sumYY - Math.Pow(sumY, 2) / counter);

        //    return upperResult / lowerResult;
        //}

        //static double Cosine(Dictionary<int, double> user1, Dictionary<int, double> user2)
        //{
        //    double sumXY = 0;
        //    double sumXX = 0;
        //    double sumYY = 0;

        //    foreach (KeyValuePair<int, double> user1Article in user1)
        //    {
        //        double x = user1Article.Value;
        //        sumXX += x * x;
        //        if (user2.ContainsKey(user1Article.Key))
        //        {
        //            double y = user2[user1Article.Key];
        //            sumXY += x * y;
        //        }
        //    }

        //    foreach (KeyValuePair<int, double> user2Article in user2)
        //    {
        //        double y = user2Article.Value;
        //        sumYY += y * y;
        //    }

        //    return sumXY/ Math.Sqrt(sumXX * sumYY);
        //}

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
