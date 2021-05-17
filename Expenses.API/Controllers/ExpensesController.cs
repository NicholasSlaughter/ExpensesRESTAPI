using Expenses.API.Dtos;
using Expenses.API.Entities;
using Expenses.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ExpensesController> logger;

        public ExpensesController(IExpensesRepository repository, ILogger<ExpensesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDto>> GetExpenseAsync(Guid id)
        {
            var expense = await repository.GetExpenseAsync(id);

            if (expense is null)
            {
                return NotFound();
            }

            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved Category: {expense.AsDto().Category}: Retrieved Price: {expense.AsDto().Price}");

            return Ok(expense.AsDto());
        }

        [HttpGet]
        public async Task<IEnumerable<ExpenseDto>> GetExpensesAsync()
        {
            var expenses = repository.GetExpensesAsync();

            return (await expenses).Select(expense => expense.AsDto());
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseDto>> CreateExpenseAsync(CreateExpenseDto createExpenseDto)
        {
            Expense expense = new()
            {
                Id = Guid.NewGuid(),
                Category = createExpenseDto.Category,
                Price = createExpenseDto.Price,
                DateOfExpense = DateTimeOffset.UtcNow
            };

            await repository.CreateExpenseAsync(expense);

            return CreatedAtAction(nameof(CreateExpenseAsync), new { id = expense.Id }, expense.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateExpenseAsync(Guid id, UpdateExpenseDto expenseDto)
        {
            //get the expense we want to update
            var existingExpense = await repository.GetExpenseAsync(id);

            //If we did not find the expense return NotFound()
            if (existingExpense is null)
            {
                return NotFound();
            }

            //Update the expense members
            Expense updatedExpense = existingExpense with
            {
                Category = expenseDto.Category,
                Price = expenseDto.Price
            };

            //Update the expense in the expense collection
            await repository.UpdateExpenseAsync(updatedExpense);

            //PUT returns no content
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpenseAsync(Guid id)
        {
            var existingExpense = await repository.GetExpenseAsync(id);

            if (existingExpense is null)
            {
                return NotFound();
            }

            await repository.DeleteExpenseAsync(existingExpense.Id);

            return NoContent();
        }
    }
}
