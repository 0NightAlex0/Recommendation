using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataScience
{
    class DeviationTable
    {
        public Dictionary<int, Dictionary<int, DeviationData>> data = new Dictionary<int, Dictionary<int, DeviationData>>();

        public void ComputeDeviations(Dictionary<int, UserPreferance> users)
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
                    // something is wrong with the second loop/ its not adding all B/ count A and count b not the same
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

            // maybe check if its nan and not infinity, then set it to 0
            foreach (KeyValuePair<int, Dictionary<int, DeviationData>> pair in data)
            {
                foreach (KeyValuePair<int, DeviationData> cell in pair.Value)
                {
                    DeviationData data = cell.Value;
                    data.deviationValue = data.deviationValue / data.nrOfPeople;
                }
            }
        }

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

        public void UpdateTable(KeyValuePair<int, UserPreferance> user, int article, double articleRating)
        {

        }
    }
}
