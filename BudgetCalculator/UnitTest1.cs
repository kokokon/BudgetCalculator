using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BudgetCalculator
{
    [TestClass]
    public class UnitTest1
    {
        private IRepo<Budget> _repo = Substitute.For<IRepo<Budget>>();
        private BudgetManager _budgetManager;
        private DateTime startDate;
        private DateTime endDate;
        List<Budget> listbudget = new List<Budget>();

        public void initList()
        {
            SetBudget("201802", 140);
            SetBudget("201804", 300);
            SetBudget("201805", 217);
            _repo.GetList().Returns(listbudget);
            _budgetManager = new BudgetManager(_repo);
        }

        [TestMethod]
        public void OneDay()
        {
            initList();
            startDate = new DateTime(2018, 02, 01);
            endDate = new DateTime(2018, 02, 01);
            GetBudgetResult(5);
        }

        [TestMethod]
        public void MultiDays()
        {
            initList();
            startDate = new DateTime(2018, 02, 01);
            endDate = new DateTime(2018, 02, 02);
            GetBudgetResult(10);
        }

        [TestMethod]
        public void SingleMonth()
        {
            initList();
            startDate = new DateTime(2018, 04, 01);
            endDate = new DateTime(2018, 04, 30);
            GetBudgetResult(300);
        }

        [TestMethod]
        public void TwoMonth()
        {
            initList();
            startDate = new DateTime(2018, 04, 30);
            endDate = new DateTime(2018, 05,01);
            GetBudgetResult(17);
        }

        [TestMethod]
        public void CrossMonth()
        {
            initList();
            startDate = new DateTime(2018, 02, 01);
            endDate = new DateTime(2018, 05, 31);
            GetBudgetResult(657);
        }

        [TestMethod]
        public void SearchBeforeBudgetExists()
        {
            initList();
            startDate = new DateTime(2018, 01, 31);
            endDate = new DateTime(2018, 02, 01);
            GetBudgetResult(5);
        }

        [TestMethod]
        public void SearchAfterBudgetExists()
        {
            initList();
            startDate = new DateTime(2018, 05, 31);
            endDate = new DateTime(2018, 06, 01);
            GetBudgetResult(7);
        }

        [TestMethod]
        public void SearchForNoBudgetExists()
        {
            initList();
            startDate = new DateTime(2018, 03, 01);
            endDate = new DateTime(2018, 03, 02);
            GetBudgetResult(0);           
        }

        [TestMethod]
        public void SearchForALongTime()
        {
            initList();
            startDate = new DateTime(2017, 12, 01);
            endDate = new DateTime(2019, 01, 01);
            GetBudgetResult(657);            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCaseException))]
        public void StartDateIsLarger()
        {
            initList();
            startDate = new DateTime(2018, 02, 05);
            endDate = new DateTime(2018, 02, 02);
            GetBudgetResult(657);
        }

        public void GetBudgetResult(int expected)
        {
            Assert.AreEqual(expected, _budgetManager.SumBudget(startDate, endDate));
        }

        public void SetBudget(string _month, decimal _amt)
        {
            Budget budget = new Budget
            {
                month = _month,
                amt = _amt                 
            };
            listbudget.Add(budget);
        }
    }
}
