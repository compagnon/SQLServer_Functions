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

            Double result = new TWR(ptfValue, null).Calculate();
            Assert.AreEqual((800000- 100000)/ 100000, result);
        }

        [TestMethod]
        public void TestSimple_1InFlow()
        {
            List<double> ptfValue = new List<Double>() { 100000, 175000,250000,400000,500000,550000,1300000,1250000,1100000,1000000,950000,875000,800000};
            List<double?> cashFlows = new List<double?>() { null, null, null , null, null, null,900000 };

            Double result = new TWR(ptfValue, cashFlows, false).Calculate();
            Assert.AreEqual(1.46153846, result);
        }
        [TestMethod]
        public void TestSimple_1Withdrawal()
        {
            List<double> ptfValue = new List<Double>() { 100000, 175000, 250000, 400000, 500000, 550000, 1300000, 1250000, 1100000, 1000000, 950000, 875000, 800000 };
            List<double?> cashFlows = new List<double?>() { null, null, null, null, null,null,  -900000 };

            Double result = new TWR(ptfValue, cashFlows,false).Calculate();
            Assert.AreEqual(12.53846154, result);
        }

        [TestMethod]
        public void Test1()
        {
            List<double> ptfValue = new List<Double>() { 100000, 175000, 250000, 400000, 500000, 550000, 400000, 355000, 400000, 450000, 500000, 575000, 600000 };
            List<double?> cashFlows = new List<double?>() { null, 50000, 50000, 100000, 100000, 50000, -50000 , -20000, -30000, 50000, 50000, 50000,80000 };            

            Double result = new TWR(ptfValue, cashFlows, false).Calculate();
            Assert.AreEqual(0.51240884, result);
        }

    }
}
