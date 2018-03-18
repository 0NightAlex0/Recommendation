using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    class DeviationTable
    {
        //public Dictionary<int, Dictionary<int, int>> frequencies = new Dictionary<int, Dictionary<int, int>>();
        //public Dictionary<int, Dictionary<int, double>> deviations = new Dictionary<int, Dictionary<int, double>>();
        public Dictionary<int, Dictionary<int, DeviationData>> table = new Dictionary<int, Dictionary<int, DeviationData>>();
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
                    //if (!frequencies.ContainsKey(itemA))
                    //{
                    //    frequencies.Add(itemA, new Dictionary<int, int>());
                    //}
                    //if (!deviations.ContainsKey(itemA))
                    //{
                    //    deviations.Add(itemA, new Dictionary<int, double>());
                    //}
                    if (!table.ContainsKey(itemA))
                    {
                        table.Add(itemA, new Dictionary<int, DeviationData>());
                    }
                    foreach (KeyValuePair<int, double> rating2 in ratings)
                    {
                        itemB = rating2.Key;
                        if (itemA != itemB)
                        {
                            //if (!frequencies[itemA].ContainsKey(itemB))
                            //{
                            //    frequencies[itemA].Add(itemB, 0);
                            //}
                            //if (!deviations[itemA].ContainsKey(itemB))
                            //{
                            //    deviations[itemA].Add(itemB, 0.0);
                            //}
                            //frequencies[itemA][itemB] += 1;
                            //deviations[itemA][itemB] += rating.Value - rating2.Value;
                            if (!table[itemA].ContainsKey(itemB))
                            {
                                table[itemA].Add(itemB, new DeviationData(0.0, 0));
                            }
                            table[itemA][itemB].nrOfPeople++;
                            table[itemA][itemB].deviationValue += rating.Value - rating2.Value;
                        }
                    }
                }
            }


            //foreach (KeyValuePair<int,Dictionary<int,double>> deviation in deviations)
            //{
            //    foreach (KeyValuePair<int, double> item in deviation.Value)
            //    {
            //        deviation.Value[item.Key] /= frequencies[deviation.Key][item.Key];
            //    }
            //}

            foreach (KeyValuePair<int, Dictionary<int, DeviationData>> row in table)
            {
                foreach(KeyValuePair<int, DeviationData> cell in row.Value)
                {
                    DeviationData data = cell.Value;
                    data.deviationValue = data.deviationValue / data.nrOfPeople;
                }
            }
        }
    }
}
