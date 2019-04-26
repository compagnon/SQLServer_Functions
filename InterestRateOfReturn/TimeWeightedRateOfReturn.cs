using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace InterestRateOfReturn
{

    class PortfolioTuple
    {
        public PortfolioTuple(double value, double? flow){ M = value; C = flow; }
        public double M { get; }
        public double? C { get;}
    }

    class PortfolioDateTuple
    {
        public PortfolioDateTuple(DateTime? date, double value, double? flow){ }
        public DateTime Date { get; }
        public double Value { get; }
        public double Flow { get; }
    }

    /// <summary>
    /// TWR is for Time Weighted Rate of Return 
    /// https://www.gipsstandards.org/standards/Documents/Guidance/gs_calculation_methodology_clean.pdf
    /// https://en.wikipedia.org/wiki/Time-weighted_return
    /// 
    /// When a portfolio experiences external cash flows during a period, the most accurate return is calculated by valuing the portfolio at the time of the external cash flow
    /// </summary>
    public class TWR
    {
        #region attributs
        private List<PortfolioTuple> ptf;
        private bool ptfValuedBeforeCF;
        private double? ptf_return = null;
        #endregion

        /// <summary>
        /// Constructor of Time-Weighted Rate of Return using a list of periods:
        /// each period has got a value for the portfolio , and optionally a external cashflow: + for inflows, - for withdrawals
        /// (default behaviour)If the portfolio is valued immediately before each external flow ,  ptfValuedBeforeCF is set to true
        /// If the portfolio is valued immediately after each external flow, ptfValuedBeforeCF is set to false
        /// </summary>
        /// <param name="portfolioValue">List of portfolio values for each period</param>
        /// <param name="cashFlow">associated list of dates</param>
        /// <param name="ptfValuedBeforeCF">True : for each period, the portfolio value is the one before cashflow enters(+) or exits(-) </param>
        public TWR(List<double> portfolioValue, List<double?> cashFlow, bool ptfValuedBeforeCF = true)
        {
            // preconditions
            if (portfolioValue is null || (portfolioValue.Where(x => x >= 0.0).Count() < 2) )
                throw new Exception("Calculation not possible / at least 2 portfolio values");
            else if (cashFlow!= null &&  (portfolioValue.Count < cashFlow.Count) )
                throw new Exception("Portfolio values are missing / not the same number than external flows");
            /*            else if (portfolioValue.Count > cashFlow.Count)
                            throw new Exception("external cashFlow do not have the same size than Portfolio value/ external cash flow could be null/0 values for no external flows during the period");
                            */
            // input validated

            this.ptfValuedBeforeCF = ptfValuedBeforeCF;
            if (cashFlow is null || (cashFlow.Count() == 0) )
            {
                // No external cash flows in periods
                // calculation is just simple:  (PVfinal-PVinitial)/PVinitial
                ptf_return = ( portfolioValue.Last<double>() - portfolioValue.First<double>() )/ portfolioValue.First<double>();
            }
            else
            {
                ptf = new List<PortfolioTuple>(portfolioValue.Count);
                IEnumerator<double?> cf_enum = cashFlow.GetEnumerator();
                foreach (double v in portfolioValue)
                {
                    ptf.Add(new PortfolioTuple(v, cf_enum.Current));
                    cf_enum.MoveNext();
                }                
            }

        }

        /// <summary>
        /// Constructor of Time-Weighted Rate of Return using a list of periods:
        /// each period has got a value for the portfolio , and optionally a external cashflow: + for inflows, - for withdrawals
        /// (default behaviour)If the portfolio is valued immediately before each external flow ,  ptfValuedBeforeCF is set to true
        /// If the portfolio is valued immediately after each external flow, ptfValuedBeforeCF is set to false
        /// </summary>
        /// <param name="portfolio">List of each portfolio values and  for each period</param>
        /// <param name="cashFlow">associated list of dates</param>
        /// <param name="ptfValuedBeforeCF">True : for each period, the portfolio value is the one before cashflow enters(+) or exits(-) </param>
        public TWR(List<Tuple<Double,Double >> portfolio, bool ptfValuedBeforeCF = true)
        {
            // TODO

        }


        /// <summary>
        /// Calculate the TWR
        /// </summary>
        /// <param name="precision">The precision behind floating points. by default 10E-8 like excel</param>
        /// <param name="decimals">The number of decimal places in the return value. by default 8</param>
        /// <returns></returns>
        public double Calculate(double precision = 0.00000001, int decimals = 8)
        {
            if (ptf_return is null)
            {
                ptf_return = 0;
                if (this.ptfValuedBeforeCF)
                {
                    IEnumerator<PortfolioTuple> enumFrom0toNminus1 = ptf.GetEnumerator();
                    IEnumerator<PortfolioTuple> enumFrom1toN = ptf.GetEnumerator();
                    enumFrom1toN.MoveNext();

                    while(enumFrom1toN.MoveNext())
                    {
                        enumFrom0toNminus1.MoveNext();
                        ptf_return += enumFrom1toN.Current.M / ( enumFrom0toNminus1.Current.M + ( enumFrom0toNminus1.Current.C ?? 0.0 ) );
                    }
                    ptf_return -= 1;
                }
                else
                {
                    IEnumerator<PortfolioTuple> enumFrom0toNminus1 = ptf.GetEnumerator();
                    IEnumerator<PortfolioTuple> enumFrom1toN = ptf.GetEnumerator();
                    enumFrom1toN.MoveNext();

                    while (enumFrom1toN.MoveNext())
                    {
                        ptf_return += ( enumFrom1toN.Current.M - ( enumFrom1toN.Current.C ?? 0.0 ) )/ enumFrom0toNminus1.Current.M;
                    }
                    ptf_return -= 1;
                }
            }

            return (double)ptf_return;
            /*
            double result = precision + 1;
            double previousResult = 0;

            while ((_cf.calculationLoop < int.MaxValue) && (Math.Abs(result - previousResult) > (precision / 10)))
            {
                previousResult = result;
                result = _cf.CashFlowIteration();
            }
            return _cf.getInternalRate(decimals);
            */
        }


    }

}
