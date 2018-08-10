
using System.Collections.Generic;

namespace BudgetCalculator
{
    public interface IRepo <Budget>
    {
        List<Budget> GetList();
    }
}