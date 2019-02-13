using System;
using System.Collections.Generic;
using System.Linq;

namespace Excel.Lib
{
    /// <summary>
    /// object of a cachflow with its duration from the initial flow
    /// </summary>
    class Cashflow
    {
        public double amount { get; set; }
        public double duration { get; set; }
    }

    /// <summary>
    /// set of cash flows
    /// </summary>
    class Cashflows
    {

        public Cashflows(List<Double> cashFlow, List<Double> dates, double internalRateMin = -1 + 10E-10 , double internalRateMax = +10000)
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

            if( Double.IsNaN(sum1) || Double.IsNaN(sum2) || Math.Sign(sum1) == Math.Sign(sum2))
                throw new Exception("Not solvable|estimate or guessed rates are incorrect : rate not between "+internalRateInf + " and "+ internalRateMax);
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
            if (Math.Sign(middleSum) == Math.Sign(sum1) )
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

    /// <summary>
    /// XIR uses the dichotomy method
    /// </summary>
    public class XIRR
    {
        #region attributs
        public const int DAYS_YEARS = 365;
        private Cashflows _cf;
        #endregion

        /// <summary>
        /// Constructor of the XIRR c
        /// </summary>
        /// <param name="cashFlow"></param>
        /// <param name="cashFlowDate"></param>
        /// <param name="Guess"> by default : 10%</param>
        public XIRR(List<Double> cashFlow, List<DateTime> cashFlowDate, Nullable<Double> Guess =null ):this(cashFlow,cashFlowDate, 0.5 * Guess,  2* Guess )
        {
        }

        public XIRR( List<Double> cashFlow, List<DateTime> cashFlowDate, Nullable<Double> Inf, Nullable<Double> Sup)
        {
            // preconditions
            if (cashFlow.Count > cashFlowDate.Count)
                throw new Exception("Date(s) in Cashflow is missing / not the same number");
            else if (cashFlow.Count < cashFlowDate.Count)
                throw new Exception("Cashflow(s) value is missing / or too many date");
            else if (cashFlow.Where(x => x < 0).Count() == 0)
                throw new Exception("Internal Rate not possible / no negative cash flow");
            else if ( cashFlow.Where(x => x>0).Count() == 0 )
                throw new Exception("Internal Rate not possible / no positive cash flow");                        
       
            // input validated
            List<double> cashFlowDuration = ToFractionOfYears(cashFlowDate);

            if( Inf is null || Sup is null )
                _cf = new Cashflows(cashFlow, cashFlowDuration);
            else            
                _cf = new Cashflows(cashFlow, cashFlowDuration, Math.Min(Inf.Value,Sup.Value), Math.Max(Inf.Value, Sup.Value));            
        }


        private static List<Double> ToFractionOfYears(List<DateTime> dates)
        {            
            var firstDate = dates.Min(x => x.Date);
            return dates
                .Select(x => ((double)x.Date.Subtract(firstDate).Days) / DAYS_YEARS)
                .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="precision">The number of zero behind floating points.</param>
        /// <param name="decimals">The number of decimal places in the return value.</param>
        /// <returns></returns>
        public double Calculate(double precision = 0.00000001, int decimals = 8)
        {
            double result = precision + 1;

            while ((_cf.calculationLoop < int.MaxValue) && (Math.Abs(result) > precision) )
            {
                result = _cf.CashFlowIteration();
            }
            return _cf.getInternalRate(decimals);                

        }

    }

}
