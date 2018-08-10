using System;
using System.Collections.Generic;

namespace BudgetCalculator
{
    public class BudgetManager
    {
        private readonly IRepo<Budget> _repo;
        private List<Budget> _lsiBudgets;
        private decimal amt { get; set; }
        private string month { get; set; }

        public BudgetManager(IRepo<Budget> repo)
        {
            _repo = repo;
            _lsiBudgets = _repo.GetList();
        }

        public decimal checkDate(DateTime statDate, DateTime endDate)
        {
            if ((statDate.Month != endDate.Month)
                || (statDate.Year != endDate.Year && statDate.Month == endDate.Month))
            {
                int year, month;
                year = statDate.Year + ((statDate.Month) / 12);
                if (statDate.Month == 12)
                {
                    month = 1;
                }
                else
                {
                    month = (statDate.Month + 1);
                }
                DateTime tempEnd = new DateTime(year, month, 1).AddDays(-1);

                //DateTime tempStart = new DateTime(endDate.Year, endDate.Month, 1);
                return sumBudget(statDate, tempEnd) + checkDate(tempEnd.AddDays(1), endDate);
            }

            return sumBudget(statDate, endDate);
        }

        public decimal sumBudget(DateTime statDate, DateTime endDate)
        {
            Budget targetBudget = new Budget();;
            if (_lsiBudgets.Exists(x => x.month == statDate.ToString("yyyyMMdd").Substring(0, 6)))
            {
                targetBudget = _lsiBudgets.Find(x => x.month == statDate.ToString("yyyyMMdd").Substring(0, 6));
            }
            else
            {
                return 0;
            }

            int daydiff = (endDate - statDate).Days + 1;
            decimal totalAmt = daydiff * caluateDayBudget(targetBudget);
            return totalAmt;
        }

        public decimal caluateDayBudget(Budget budget)
        {
            decimal Amt = budget.amt;
            int month = Int32.Parse(budget.month.Substring(4));
            int year = Int32.Parse(budget.month.Substring(0, 4));
            int Days = DateTime.DaysInMonth(year, month);
            decimal budgetPerDay = Amt / Days;
            return budgetPerDay;
        }
    }
}