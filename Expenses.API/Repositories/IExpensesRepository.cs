using Expenses.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.Repositories
{
    public interface IExpensesRepository
    {
        Task<Expense> GetExpenseAsync(Guid id);
        Task<IEnumerable<Expense>> GetExpensesAsync();
        Task CreateExpenseAsync(Expense expense);
        Task UpdateExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(Guid id);
    }
}
