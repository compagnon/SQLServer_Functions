using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterestRateOfReturn;
using System.Collections.Generic;

namespace TWRUnitTest
{
    [TestClass]
    public class TWRUnitTest1
    {
        [TestMethod]
        public void TestSimple()
        {
            List<double> ptfValue = new List<Double>() { 100000, 175000, 250000, 400000, 500000, 550000, 1300000, 1250000, 1100000, 1000000, 950000, 875000, 800000 };            
            //List<double> dates = new List<DateTime>() { new DateTime(2008, 01, 01), new DateTime(2008, 03, 01), new DateTime(2008, 10, 30), new DateTime(2009, 02, 15), new DateTime(2009, 04, 01) };

            Double result = new TWR(ptfValue, null).Calculate();
            Assert.AreEqual((800000- 100000)/ 100000, result);
        }

        [TestMethod]
        public void TestSimple_1InFlow()
        {
            List<double> ptfValue = new List<Double>() { 100000, 175000,250000,400000,500000,550000,1300000,1250000,1100000,1000000,950000,875000,800000};
            List<double?> cashFlows = new List<double?>() { null, null, null , null, null,900000 };
            //List<double> dates = new List<DateTime>() { new DateTime(2008, 01, 01), new DateTime(2008, 03, 01), new DateTime(2008, 10, 30), new DateTime(2009, 02, 15), new DateTime(2009, 04, 01) };

            Double result = new TWR(ptfValue, cashFlows).Calculate();
            Assert.AreEqual(1.46153846, result);
        }
        [TestMethod]
        public void TestSimple_1Withdrawal()
        {
            List<double> ptfValue = new List<Double>() { 100000, 175000, 250000, 400000, 500000, 550000, 1300000, 1250000, 1100000, 1000000, 950000, 875000, 800000 };
            List<double?> cashFlows = new List<double?>() { null, null, null, null, null, -900000 };
            //List<double> dates = new List<DateTime>() { new DateTime(2008, 01, 01), new DateTime(2008, 03, 01), new DateTime(2008, 10, 30), new DateTime(2009, 02, 15), new DateTime(2009, 04, 01) };

            Double result = new TWR(ptfValue, cashFlows).Calculate();
            Assert.AreEqual(12.53846154, result);
        }

    }
}
