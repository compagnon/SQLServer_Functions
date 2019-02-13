# SQLServer_Functions
## Financial library for SQL Server


##### XIRR

Use XIRR to calculate an internal rate of return for a series of cash flows on different dates.

Signature of the method is `public static double XIRR(List<Double> cashflows, List<DateTime> dates, int maxFloatingPoints = 3, double maxRate = 100000)`
Example:
```cs
           var cashFlows	  = new List<Double>() { -10000, 2750, 4250, 3250, 10000 };
		   var cashFlowsDates = new List<DateTime>() { DateTime.Parse("01/01/2008"), DateTime.Parse("01/03/2008"), DateTime.Parse("30/10/2008"), DateTime.Parse("15/02/2009"), DateTime.Parse("01/04/2009") };
           var xirr = Financial.XIRR(cashFlows,cashFlowsDates, 6);

```
<TO BE DONE>
to follow the same functionnalities than
http://westclintech.com/SQL-Server-Financial-Functions/SQL-Server-XIRR-function

* Rates of Return - IRR, XIRR, NPV, RATE, DIETZ, etc.
* Bond Figurations - ACCRINT, PRICE and YIELD for ODD-First and ODD-Last coupons, etc.
* Capital Asset Pricing Model - INFORATIO, SHARPE, SORTINO, TREYNOR, etc.
* Loans - PMT, IPMT, CUMPRINC, AMORTSCHED, etc.
* Depreciation - DB, DDB, SLN, SYD,VDB
* Yield Curve Construction - SWAPCURVE, NELSONSIEGEL, INTERPDFACT, ZEROCOUPON, etc.
* Business Dates - EOMONTH, FIRSTWEEKDAY, BUSDAYS, DAYS360, YEARFRAC, TENOR2DATE, etc.