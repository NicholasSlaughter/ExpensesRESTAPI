using Expenses.API.Dtos;
using Expenses.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpensesRepository repository;
        
        public ExpensesController(IExpensesRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDto>> GetExpenseAsync(Guid id)
        {
            var expense = await repository.GetExpenseAsync(id);

            if (expense is null)
            {
                return NotFound();
            }

            return Ok(expense.AsDto());
        }
    }
}
