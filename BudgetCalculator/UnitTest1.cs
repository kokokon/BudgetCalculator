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
        List<Budget> _lsiBudgets = new List<Budget>();
        private DateTime startDate;
        private DateTime endDate;

        [TestMethod]
        public void OneDay()
        {
            setBudget("201802", 140);
            _repo.GetList().Returns(_lsiBudgets);
            _budgetManager = new BudgetManager(_repo);
            startDate = new DateTime(2018, 02, 01);
            endDate = new DateTime(2018, 02, 01);
            Assert.AreEqual(5, _budgetManager.checkDate(startDate,  endDate));
        }

        [TestMethod]
        public void MultiDays()
        {
            setBudget("201802", 140);
            _repo.GetList().Returns(_lsiBudgets);
            _budgetManager = new BudgetManager(_repo);
            startDate = new DateTime(2018, 02, 01);
            endDate = new DateTime(2018, 02, 02);
            Assert.AreEqual(10, _budgetManager.checkDate(startDate, endDate));
        }

        [TestMethod]
        public void SingleMonth()
        {
            setBudget("201804", 300);
            _repo.GetList().Returns(_lsiBudgets);
            _budgetManager = new BudgetManager(_repo);
            startDate = new DateTime(2018, 04, 01);
            endDate = new DateTime(2018, 04, 30);
            Assert.AreEqual(300, _budgetManager.checkDate(startDate, endDate));
        }

        [TestMethod]
        public void TwoMonth()
        {
            setBudget("201804", 300);
            setBudget("201805", 217);
            _repo.GetList().Returns(_lsiBudgets);
            _budgetManager = new BudgetManager(_repo);
            startDate = new DateTime(2018, 04, 30);
            endDate = new DateTime(2018, 05,01);
            Assert.AreEqual(17, _budgetManager.checkDate(startDate, endDate));
        }

        [TestMethod]
        public void CrossMonth()
        {
            setBudget("201802", 140);
            setBudget("201804", 300);
            setBudget("201805", 217);
            _repo.GetList().Returns(_lsiBudgets);
            _budgetManager = new BudgetManager(_repo);
            startDate = new DateTime(2018, 02, 01);
            endDate = new DateTime(2018, 05, 31);
            Assert.AreEqual(657, _budgetManager.checkDate(startDate, endDate));
        }

        [TestMethod]
        public void SerchBeforBudgetExists()
        {
            setBudget("201802", 140);

            _repo.GetList().Returns(_lsiBudgets);
            _budgetManager = new BudgetManager(_repo);
            startDate = new DateTime(2018, 01, 31);
            endDate = new DateTime(2018, 02, 01);
            Assert.AreEqual(5, _budgetManager.checkDate(startDate, endDate));
        }

        [TestMethod]
        public void SerchAfterBudgetExists()
        {
            setBudget("201805", 217);

            _repo.GetList().Returns(_lsiBudgets);
            _budgetManager = new BudgetManager(_repo);
            startDate = new DateTime(2018, 05, 31);
            endDate = new DateTime(2018, 06, 01);
            Assert.AreEqual(7, _budgetManager.checkDate(startDate, endDate));
        }

        [TestMethod]
        public void SerchForNoBudgetExists()
        {
            setBudget("201802", 140);
            setBudget("201804", 300);
            setBudget("201805", 217);
            _repo.GetList().Returns(_lsiBudgets);
            _budgetManager = new BudgetManager(_repo);
            startDate = new DateTime(2018, 03, 01);
            endDate = new DateTime(2018, 03, 02);
            Assert.AreEqual(0, _budgetManager.checkDate(startDate, endDate));
        }

        [TestMethod]
        public void SerchForALongTime()
        {
            setBudget("201802", 140);
            setBudget("201804", 300);
            setBudget("201805", 217);
            _repo.GetList().Returns(_lsiBudgets);
            _budgetManager = new BudgetManager(_repo);
            startDate = new DateTime(2017, 12, 01);
            endDate = new DateTime(2019, 01, 01);
            Assert.AreEqual(657, _budgetManager.checkDate(startDate, endDate));
        }

        public void setBudget(string _month, decimal _amt)
        {
            Budget budget = new Budget
            {
                month = _month,
                amt = _amt                 
            };

            _lsiBudgets.Add(budget);
        }
    }
}
