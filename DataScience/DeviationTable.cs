using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    class DeviationTable
    {
        public Dictionary<int, Dictionary<int, DeviationData>> data = new Dictionary<int, Dictionary<int, DeviationData>>();
        public void ComputeDeviations(Dictionary<int, UserPreferance> users)
        {
            int itemA;
            int itemB;
            foreach (KeyValuePair<int, UserPreferance> user in users)
            {
                Dictionary<int, double> ratings = user.Value.UserRatings;
                foreach (KeyValuePair<int, double> rating in ratings)
                {
                    itemA = rating.Key;
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

            foreach (KeyValuePair<int, Dictionary<int, DeviationData>> row in data)
            {
                foreach (KeyValuePair<int, DeviationData> cell in row.Value)
                {
                    DeviationData data = cell.Value;
                    data.deviationValue = data.deviationValue / data.nrOfPeople;
                }
            }
        }

        public double SlopeOneRecommendations(Dictionary<int, double> ratings, int target)
        {
            double numerator = 0.0;
            int denominator = 0;
            foreach (KeyValuePair<int, double> rating in ratings)
            {
                DeviationData deviationCell = data[target][rating.Key];
                numerator += (rating.Value + deviationCell.deviationValue) * deviationCell.nrOfPeople;
                denominator += deviationCell.nrOfPeople;
            }

            return numerator / denominator;
        }

        public void MultipleSlopeOne(Dictionary<int, double> ratings, List<int> targets)
        {
            foreach(int target in targets)
            {
                Console.WriteLine(target+ ":" + SlopeOneRecommendations(ratings, target));
            }
        }

        public void UpdateTable(KeyValuePair<int, UserPreferance> user, int article, double articleRating)
        {

        }
    }
}
