using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataScience
{
    public class Pair<T1, T2>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }
    }
    enum DataSetSize { Small, Big};
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

            Dictionary<int, UserPreferance> dataSet = GetDataSet(DataSetSize.Small);
            //// neighbour and ratings
            int testId = 7;
            //dataSet[testId].UserRatings.Add(106, 5);
            KeyValuePair<int, UserPreferance> testPair = new KeyValuePair<int, UserPreferance>(testId, dataSet[testId]);
            UserItem userItem = new UserItem(dataSet);
            userItem.GetNearestNeighbours(testPair, cosine, 3);
            List<KeyValuePair<int, double>> ratingPrediction = userItem.PredictGivenList(new List<int>(new int[] { 101, 103, 106 }));


            //int testId = 186;
            //KeyValuePair<int, UserPreferance> testPair = new KeyValuePair<int, UserPreferance>(testId, dataSet[testId]);
            //UserItem userItem = new UserItem(dataSet);
            //userItem.GetNearestNeighbours(testPair, pearson, 25);
            //List<KeyValuePair<int, double>> ratingPrediction = userItem.PredictAll(testPair).GetRange(0, 8);

            //DeviationTable table = new DeviationTable();
            //table.ComputeDeviations8sec(dataSet);
            //table.Update(dataSet[3].UserRatings, 105, 4.0);

            //dataSet[3].UserRatings.Add(105, 4.0);
            //table.ComputeDeviations8sec(dataSet);
            //PrintDeviationTable(table);
            //table.MultipleSlopeOne(new List<int>(new int[] { 101, 103, 106 }), dataSet[7].UserRatings);

            //List<KeyValuePair<int, double>> top5 = table.Top5SlopeOne(dataSet[186].UserRatings).GetRange(0, 5);

            Console.WriteLine("hello");
            Console.ReadLine();


        }

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
        public static Dictionary<int, UserPreferance> GetDataSet(DataSetSize size)
        {
            if (size == DataSetSize.Small)
            {
                return ParseDataSet(@"D:\github\DataScience\userItem.data", ",");
            }
            else if (size == DataSetSize.Big)
            {
                return ParseDataSet(@"D:\github\DataScience\u.data", "\t");
            }
            else
            {
                throw new Exception("Size not found");
            }
            
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

        public static bool DictionaryHasExtra(Dictionary<int, double> current, Dictionary<int, double> target)
        {
            // current has a key that target does not have.
            // since cosine fills in everything empty with 0, it will think it doesnt have any extra keys.
            return current.Keys.Any(key => !target.ContainsKey(key) || target[key] == 0);
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
            return articleIds.OrderBy(x => x).ToList();
        }

        public static void PrintDeviationTable(DeviationTable table)
        {
            foreach (KeyValuePair<int,Dictionary<int,DeviationData>> itemA in table.data)
            {
                Console.WriteLine("row: "+ itemA.Key);
                foreach (KeyValuePair<int, DeviationData> itemB in itemA.Value)
                {
                    Console.WriteLine("column: "+ itemB.Key + "\tvalue: " + itemB.Value.deviationValue + "\tcount: "+ itemB.Value.nrOfPeople);
                }
            }
        }
    }
}
