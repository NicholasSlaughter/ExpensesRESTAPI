using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.Dtos
{
    public class CreateExpenseDto
    {
        [Required]
        public string Category { get; init; }

        [Required]
        [Range(1,1000)]
        public decimal Price { get; init; }
    }
}
