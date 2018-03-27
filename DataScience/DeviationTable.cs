using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataScience
{
    class DeviationTable
    {
        public Dictionary<int, Dictionary<int, DeviationData>> data = new Dictionary<int, Dictionary<int, DeviationData>>();

        public void ComputeDeviations8sec(Dictionary<int, UserPreferance> users)
        {
            int itemA;// x
            int itemB;// y
            // for each person in the data
            foreach (KeyValuePair<int, UserPreferance> user in users)
            {
                Dictionary<int, double> ratings = user.Value.UserRatings;
                // 2 for loop to loop through every ratings to get x and y
                foreach (KeyValuePair<int, double> rating in ratings)
                {
                    itemA = rating.Key;
                    // itemA doesnt exist yet in x
                    if (!data.ContainsKey(itemA))
                    {
                        data.Add(itemA, new Dictionary<int, DeviationData>());
                    }

                    foreach (KeyValuePair<int, double> rating2 in ratings)
                    {
                        itemB = rating2.Key;

                        if (itemA != itemB)
                        {
                            if (!data[itemA].ContainsKey(itemB))
                            {
                                data[itemA].Add(itemB, new DeviationData(0.0, 0));
                            }
                            data[itemA][itemB].nrOfPeople++;
                            data[itemA][itemB].deviationValue += rating.Value - rating2.Value;
                        }
                    }
                }
            }

            foreach (KeyValuePair<int, Dictionary<int, DeviationData>> pair in data)
            {
                foreach (KeyValuePair<int, DeviationData> cell in pair.Value)
                {
                    DeviationData data = cell.Value;
                    data.deviationValue /= data.nrOfPeople;
                }
            }
        }

        //public void ComputeDeviations(Dictionary<int, UserPreferance> users)
        //{
        //    List<int> articles =  Program.GetAllArticleIds(users);

        //    foreach(int article in articles)
        //    {
        //        foreach(int article2 in articles)
        //        {
        //            // if smaller skip
        //            if(article != article2 && article2 > article)
        //            {
        //                if (!data.ContainsKey(article))
        //                {
        //                    data.Add(article, new Dictionary<int, DeviationData>());
        //                }
        //                if (!data.ContainsKey(article2))
        //                {
        //                    data.Add(article2, new Dictionary<int, DeviationData>());
        //                }
        //                double sumDifference = 0.0;
        //                int cardinality = 0;
        //                foreach (KeyValuePair<int, UserPreferance> user in users)
        //                {
        //                    Dictionary<int, double> ratings = user.Value.UserRatings;
        //                    if (ratings.ContainsKey(article) && ratings.ContainsKey(article2))
        //                    {
        //                        sumDifference += ratings[article] - ratings[article2];
        //                        cardinality++;
        //                    }

        //                }
        //                data[article].Add(article2, new DeviationData(sumDifference / cardinality, cardinality));
        //                data[article2].Add(article, new DeviationData(-1 * (sumDifference / cardinality), cardinality));
        //            }

        //        }
        //    }
        //}

        public double SlopeOneRecommendations(int target, Dictionary<int, double> ratings)
        {
            double numerator = 0.0;
            int denominator = 0;
            foreach (KeyValuePair<int, double> rating in ratings)
            {
                if (data[target].ContainsKey(rating.Key))
                {
                    DeviationData deviationCell = data[target][rating.Key];
                    numerator += (rating.Value + deviationCell.deviationValue) * deviationCell.nrOfPeople;
                    denominator += deviationCell.nrOfPeople;
                }

            }
            return numerator / denominator;
        }

        public void MultipleSlopeOne(List<int> targets, Dictionary<int, double> ratings)
        {
            foreach (int target in targets)
            {
                Console.WriteLine(target + ":" + SlopeOneRecommendations(target, ratings));
            }
        }

        public List<KeyValuePair<int, double>> Top5SlopeOne(Dictionary<int, double> ratings)
        {
            List<KeyValuePair<int, double>> result = new List<KeyValuePair<int, double>>();
            foreach (int dataKey in data.Keys)
            {
                if (!ratings.ContainsKey(dataKey))
                {
                    double prediction = SlopeOneRecommendations(dataKey, ratings);
                    result.Add(new KeyValuePair<int, double>(dataKey, prediction));
                }
            }
            
            return result.OrderByDescending(pair => pair.Value).ToList();
        }
        // the 105 is wrong. the value is only halve of what it should be
        public void Update(Dictionary<int, double> ratings, int article, double articleRating)
        {
            // 2 for loop to loop through every ratings to get x and y
            foreach (KeyValuePair<int, double> rating in ratings)
            {
                if (data.ContainsKey(article))
                {
                    if (data[article].ContainsKey(rating.Key))
                    {
                        DeviationData cell1 = data[article][rating.Key];
                        cell1.deviationValue = (cell1.deviationValue * cell1.nrOfPeople) + articleRating - rating.Value;
                        cell1.nrOfPeople++;
                        cell1.deviationValue /= cell1.nrOfPeople;

                        DeviationData cell2 = data[rating.Key][article];
                        cell2.deviationValue = -1 * cell1.deviationValue;
                        cell2.nrOfPeople = cell1.nrOfPeople;
                    }
                }
            }
            ratings.Add(article, articleRating);
        }
    }
} 
