using System;
using System.Collections.Generic;

namespace BudgetCalculator
{
    public class BudgetManager
    {
        private readonly IRepo<Budget> _repo;
        private List<Budget> listBudget;
        private decimal amt { get; set; }
        private string month { get; set; }

        public BudgetManager(IRepo<Budget> repo)
        {
            _repo = repo;
            listBudget = _repo.GetList();
        }

        public decimal SumBudget(DateTime startDate, DateTime endDate)
        {
            decimal totalBudget = 0;

            if (startDate > endDate)
            {
                throw new InvalidCaseException();
            }

            while (startDate <= endDate)
            {
                var tempEnd = endDate;
                if (IsDiffMonth(startDate, endDate))
                {
                    tempEnd = new DateTime(startDate.Year, startDate.Month,
                        DateTime.DaysInMonth(startDate.Year, startDate.Month));
                }

                totalBudget += CalculateMonthlyBudget(startDate, tempEnd);
                startDate = FirstDayOfNextMonth(tempEnd);
            }

            return totalBudget;
        }

        private bool IsDiffMonth(DateTime startDate, DateTime endDate)
        {
            if ((startDate.Year == endDate.Year) && (startDate.Month == endDate.Month))
            {
                return false;
            }

            return true;
        }

        private static DateTime FirstDayOfNextMonth(DateTime tempEnd)
        {
            return tempEnd.AddDays(1);
        }

        private decimal CalculateMonthlyBudget(DateTime startDate, DateTime endDate)
        {
            decimal totalAmt = GetDayDiff(startDate, endDate) * CalculateDailyBudget(startDate);
            return totalAmt;
        }

        private decimal GetDayDiff(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days + 1;
        }

        private decimal CalculateDailyBudget(DateTime startDate)
        {
            Budget budget = new Budget();
            if (!listBudget.Exists(x => x.month == startDate.ToString("yyyyMM")))
            {
                return 0;
            }

            budget = listBudget.Find(x => x.month == startDate.ToString("yyyyMM"));

            decimal budgetPerDay =
                budget.amt / DateTime.DaysInMonth(GetBudgetYear(budget.month), GetBudgetMonth(budget.month));
            return budgetPerDay;
        }

        private int GetBudgetMonth(string budgetMonth)
        {
            return Int32.Parse(budgetMonth.Substring(4));
        }

        private int GetBudgetYear(string budgetMonth)
        {
            return Int32.Parse(budgetMonth.Substring(0, 4));
        }
    }

    public class InvalidCaseException : Exception
    {

    }
}