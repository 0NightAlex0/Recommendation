using System;
using System.Collections.Generic;
using System.Text;

namespace DataScience
{
    class DeviationData
    {
        public double deviationValue;
        public int nrOfPeople;
        public DeviationData(double deviationValue, int nrOfPeople)
        {
            this.deviationValue = deviationValue;
            this.nrOfPeople = nrOfPeople;
        }

        public void ComputeDeviationCell()
        {
            this.deviationValue /= nrOfPeople;
        }
    }
}
