using System;
using System.Collections.Generic;
using System.Linq;

namespace InterestRateOfReturn
{
     /// <summary>
    /// IRR is a specific case of XIRR, simple with every periods egals
    /// </summary>
    public class IRR:XIRR    
    {
        /// <summary>
        /// Constructor of the IRR
        /// </summary>
        /// <param name="cashFlow"></param>
        /// <param name="Guess"> by default : 10%</param>
        public IRR(List<Double> cashFlow, Nullable<Double> Guess = null) : this(cashFlow, 0.5 * Guess, 2 * Guess)
        { }

        /// <summary>
        /// Constructor of the IRR calculation
        /// </summary>
        /// <param name="cashFlow">list of double value</param>
        /// <param name="Inf">rate must be between Inf value and Sup Value</param>
        /// <param name="Sup"></param>
        public IRR(List<Double> cashFlow,  Nullable<Double> Inf, Nullable<Double> Sup): base(cashFlow, cashFlowDuration: Enumerable.Range(0, cashFlow.Count).Select(x => (double)x).ToList(), Inf:Inf, Sup: Sup)
        { }
    }

    /// <summary>
    /// XIRR is the internal rate of return for a schedule of cash flows that is not necessarily periodic. To calculate the internal rate of return for a series of periodic cash flows, use the IRR function.
    /// XIRR uses the dichotomy method // XIRR is also used to calcultate Moey-Weighted Return
    /// </summary>
    public class XIRR
    {
        #region attributs
        public const int DAYS_YEARS = 365;
        private Cashflows _cf;
        #endregion

        /// <summary>
        /// Constructor of the XIRR
        /// </summary>
        /// <param name="cashFlow"></param>
        /// <param name="cashFlowDate"></param>
        /// <param name="Guess"> by default : 10%</param>
        public XIRR(List<Double> cashFlow, List<DateTime> cashFlowDate, Nullable<Double> Guess =null ):this(cashFlow,cashFlowDate, 0.5 * Guess,  2* Guess )
        {
        }
        /// <summary>
        /// Constructor of XIRR
        /// </summary>
        /// <param name="cashFlow">List of amount values</param>
        /// <param name="cashFlowDate">associated list of dates</param>
        /// <param name="Inf">rate must be between Inf value and Sup Value</param>
        /// <param name="Sup"></param>
        public XIRR(List<Double> cashFlow, List<DateTime> cashFlowDate, Nullable<Double> Inf, Nullable<Double> Sup) : this(cashFlow, ToFractionOfYears(cashFlowDate, cashFlow.Count), Inf, Sup)
        {
        }

        /// <summary>
        /// Constructor of XIRR
        /// </summary>
        /// <param name="cashFlow">List of amount values</param>
        /// <param name="cashFlowDuration">associated list of duration (0 is the duration of the first flow)</param>
        /// <param name="Inf">rate must be between Inf value and Sup Value</param>
        /// <param name="Sup"></param>
        public XIRR(List<Double> cashFlow, List<double> cashFlowDuration, Nullable<Double> Inf, Nullable<Double> Sup)
        {
                // preconditions
                if (cashFlow.Count > cashFlowDuration.Count)
                    throw new Exception("Date(s) in Cashflow is missing / not the same number");
                else if (cashFlow.Count < cashFlowDuration.Count)
                    throw new Exception("Cashflow(s) value is missing / or too many date");
                else if (cashFlow.Where(x => x < 0).Count() == 0)
                    throw new Exception("Internal Rate not possible / no negative cash flow");
                else if (cashFlow.Where(x => x > 0).Count() == 0)
                    throw new Exception("Internal Rate not possible / no positive cash flow");
                // input validated

                if (Inf is null || Sup is null)
                    _cf =  new Cashflows(cashFlow, cashFlowDuration);
                else
                    _cf = new Cashflows(cashFlow, cashFlowDuration, Math.Min(Inf.Value, Sup.Value), Math.Max(Inf.Value, Sup.Value));
        }
        

        private static List<Double> ToFractionOfYears(List<DateTime> dates, int count)
        {
            DateTime firstDate = dates.Min(x => x.Date);
            return dates
                .Select(x => ((double)x.Date.Subtract(firstDate).Days) / DAYS_YEARS)
                .ToList();
        }

        /// <summary>
        /// Calculate the IRR
        /// </summary>
        /// <param name="precision">The precision behind floating points. by default 10E-8 like excel</param>
        /// <param name="decimals">The number of decimal places in the return value. by default 8</param>
        /// <returns></returns>
        public double Calculate(double precision = 0.00000001, int decimals = 8)
        {
            double result = precision + 1;
            double previousResult = 0;

            while ((_cf.calculationLoop < int.MaxValue) && (Math.Abs(result-previousResult) > (precision/10) ) )
            {
                previousResult = result;
                result = _cf.CashFlowIteration();
            }
            return _cf.getInternalRate(decimals);                
        }

    }

}
