using Expenses.API.Dtos;
using Expenses.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API
{
    public static class Extensions
    {
        public static ExpenseDto AsDto(this Expense expense)
        {
            return new ExpenseDto
            {
                Id = expense.Id,
                Category = expense.Category,
                Price = expense.Price,
                DateOfExpense = expense.DateOfExpense
            };
        }
    }
}
