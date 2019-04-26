using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestRateOfReturn
{
    /// <summary>
    /// object of a cachflow with its duration from the initial flow
    /// </summary>
    public class Cashflow
    {
        public double amount { get; set; }
        public double duration { get; set; }
    }

    /// <summary>
    /// set of cash flows
    /// </summary>
    public class Cashflows
    {
        public Cashflows(List<Double> cashFlow, List<Double> dates, double internalRateMin = -1 + 10E-10, double internalRateMax = +10000)
        {
            internalRateInf = internalRateMin;
            internalRateSup = internalRateMax;

            _cf = new List<Cashflow>();
            for (int index = 0; index < cashFlow.Count; index++)
            {
                _cf.Add(new Cashflow { amount = cashFlow[index], duration = dates[index] });
            }

            sum1 = CashFlowSum(internalRateInf);
            sum2 = CashFlowSum(internalRateSup);

            if (Double.IsNaN(sum1) || Double.IsNaN(sum2) || Math.Sign(sum1) == Math.Sign(sum2))
                throw new Exception("Not solvable|estimate or guessed rates are incorrect : rate not between " + internalRateInf + " and " + internalRateMax);
        }
        /// <summary>
        /// IR after a round of calculation, will be between the 2 values Inf<IRR<Sup
        /// </summary>
        public double internalRateInf { get; set; }
        public double internalRateSup { get; set; }

        public double getInternalRate(int decimals = 5)
        {
            return Math.Round((internalRateInf + internalRateSup) / 2, decimals);
        }

        /// <summary>
        /// caulcation of the sum of cash flow , that should be near of 0 , each other around 0
        /// </summary>
        public double sum1 { get; set; }
        public double sum2 { get; set; }



        private List<Cashflow> _cf { get; set; }

        /// <summary>
        /// number of loop of calculation
        /// </summary>
        public int calculationLoop { get; set; } = 0;

        public double CashFlowIteration()
        {
            double middleRate = (internalRateInf + internalRateSup) / 2;
            double middleSum = CashFlowSum(middleRate);
            if (Math.Sign(middleSum) == Math.Sign(sum1))
            {
                sum1 = middleSum;
                internalRateInf = middleRate;
            }
            else
            {
                sum2 = middleSum;
                internalRateSup = middleRate;
            }

            return middleSum;
        }

        private double CashFlowSum(double interestRate)
        {
            calculationLoop++;
            return _cf.Select(x => (x.amount / (Math.Pow((1 + interestRate), x.duration)))).Sum(x => x);
        }


    }
}
