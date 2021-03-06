using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.Dtos
{
    public record ExpenseDto
    {
        public Guid Id { get; init; }
        public string Category { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset DateOfExpense { get; init; }
    }
}
