using System;
using System.Threading.Tasks;
using Expenses.API.Repositories;
using Expenses.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Expenses.API.Entities;
using Expenses.API.Dtos;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace Expenses.UnitTests
{
    public class ExpensesControllerTests
    {
        private readonly Mock<IExpensesRepository> repositoryStub = new();
        private readonly Mock<ILogger<ExpensesController>> loggerStub = new();

        private readonly Random rand = new();

        [Fact]
        public async Task GetExpenseAsync_WithUnexistingItem_ReturnsNotFound()
        {
            try
            {
                //Arrange
                repositoryStub.Setup(repo => repo.GetExpenseAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((Expense)null);

                var controller = new ExpensesController(repositoryStub.Object, loggerStub.Object);

                //Act
                var result = await controller.GetExpenseAsync(Guid.NewGuid());

                //Assert
                result.Result.Should().BeOfType<NotFoundResult>();
            }
            catch (Exception e)
            {
                Assert.False(1 == 1);
            }
        }

        [Fact]
        public async Task GetExpenseAsync_WithExistingItem_ReturnsExpectedExpense()
        {
            try
            {
                //Arrange
                var expectedExpense = CreateRandomExpense();

                repositoryStub.Setup(repo => repo.GetExpenseAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(expectedExpense);

                var controller = new ExpensesController(repositoryStub.Object,loggerStub.Object);

                //Act
                var result = await controller.GetExpenseAsync(Guid.NewGuid());

                //Convert the result to an OK Object as an ExpenseDto
                var expenseDtoResult = (result.Result as OkObjectResult).Value as ExpenseDto;

                //Assert
                expenseDtoResult.Should().NotBeNull();
                expenseDtoResult.Should().BeEquivalentTo(
                    expectedExpense,
                    options => options.ComparingByMembers<Expense>());
            }
            catch (Exception e)
            {
                Assert.False(1 == 1);
            }
        }

        [Fact]
        public async Task GetExpensesAsync_WithExistingItem_ReturnsAllExpenses()
        {
            try
            {
                //Arrange
                var expectedExpenses = new[] { CreateRandomExpense(), CreateRandomExpense(), CreateRandomExpense() };
                repositoryStub.Setup(repo => repo.GetExpensesAsync())
                    .ReturnsAsync(expectedExpenses);

                var controller = new ExpensesController(repositoryStub.Object, loggerStub.Object);

                //Act
                var result = await controller.GetExpensesAsync();

                //Assert
                result.Should().NotBeNull();
                result.Should().BeEquivalentTo(
                    expectedExpenses,
                    options => options.ComparingByMembers<Expense>());
            }
            catch(Exception e)
            {
                Assert.False(1 == 1);
            }
        }

        [Fact]
        public async Task CreateExpenseAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            try
            {
                //Arrange
                var expenseToCreate = new CreateExpenseDto()
                {
                    Category = Guid.NewGuid().ToString(),
                    Price = rand.Next(1, 1000),
                };

                var controller = new ExpensesController(repositoryStub.Object, loggerStub.Object);

                //Act
                var result = await controller.CreateExpenseAsync(expenseToCreate);

                var createdExpense = (result.Result as CreatedAtActionResult).Value as ExpenseDto;

                //Assert
                expenseToCreate.Should().BeEquivalentTo(
                    createdExpense,
                    options => options.ComparingByMembers<ExpenseDto>().ExcludingMissingMembers());

                //Check the missing members
                createdExpense.Id.Should().NotBeEmpty();
                //should be within 1000 seconds
                createdExpense.DateOfExpense.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
            }
            catch (Exception e)
            {
                Assert.False(1 == 1);
            }
        }

        [Fact]
        public async Task UpdateExpenseAsync_WithExistingItem_ReturnsNoContent()
        {
            try
            {
                //Arrange
                var existingExpense = CreateRandomExpense();

                repositoryStub.Setup(repo => repo.GetExpenseAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(existingExpense);

                var expenseId = existingExpense.Id;
                var expenseToUpdate = new UpdateExpenseDto()
                {
                    Category = Guid.NewGuid().ToString(),
                    Price = existingExpense.Price + 5
                };

                var controller = new ExpensesController(repositoryStub.Object, loggerStub.Object);

                //Act
                var result = await controller.UpdateExpenseAsync(expenseId, expenseToUpdate);

                //Assert
                result.Should().BeOfType<NoContentResult>();
            }
            catch (Exception e)
            {
                Assert.False(1 == 1);
            }
        }

        [Fact]
        public async Task UpdateExpenseAsync_WithUnexistingItem_ReturnsNotFound()
        {
            try
            {
                //Arrange
                repositoryStub.Setup(repo => repo.GetExpenseAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((Expense)null);

                var controller = new ExpensesController(repositoryStub.Object, loggerStub.Object);

                //Act
                var result = await controller.UpdateExpenseAsync(Guid.NewGuid(), (UpdateExpenseDto)null);

                //Assert
                result.Should().BeOfType<NotFoundResult>();
            }
            catch (Exception e)
            {
                Assert.False(1 == 1);
            }
        }

        [Fact]
        public async Task DeleteExpenseAsync_WithExistingItem_ReturnsNoContent()
        {
            try
            {
                //Arrange
                var existingExpense = CreateRandomExpense();

                repositoryStub.Setup(repo => repo.GetExpenseAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(existingExpense);

                var controller = new ExpensesController(repositoryStub.Object, loggerStub.Object);

                //Act
                var result = await controller.DeleteExpenseAsync(existingExpense.Id);

                //Assert
                result.Should().BeOfType<NoContentResult>();
            }
            catch (Exception e)
            {
                Assert.False(1 == 1);
            }
        }

        [Fact]
        public async Task DeleteExpenseAsync_WithUnexistingItem_ReturnsNotFound()
        {
            try
            {
                //Arrange
                repositoryStub.Setup(repo => repo.GetExpenseAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((Expense)null);

                var controller = new ExpensesController(repositoryStub.Object, loggerStub.Object);

                //Act
                var result = await controller.DeleteExpenseAsync(Guid.NewGuid());

                //Assert
                result.Should().BeOfType<NotFoundResult>();
            }
            catch (Exception e)
            {
                Assert.False(1 == 1);
            }
        }

        private Expense CreateRandomExpense()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Category = Guid.NewGuid().ToString(),
                Price = rand.Next(1, 1000),
                DateOfExpense = DateTimeOffset.UtcNow
            };

        }
    }
}
