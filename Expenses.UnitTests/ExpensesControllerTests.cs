using System;
using System.Threading.Tasks;
using Expenses.API.Repositories;
using Expenses.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Expenses.API.Entities;

namespace Expenses.UnitTests
{
    public class ExpensesControllerTests
    {
        [Fact]
        public async Task GetExpenseAsync_WithUnexistingItem_ReturnsNotFound()
        {
            try
            {
                //Arrange
                var repositoryStub = new Mock<IExpensesRepository>();
                repositoryStub.Setup(repo => repo.GetExpenseAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((Expense)null);

                var controller = new ExpensesController(repositoryStub.Object);

                //Act
                var result = await controller.GetExpenseAsync(Guid.NewGuid());

                //Assert
                Assert.IsType<NotFoundResult>(result.Result);
            }
            catch (Exception e)
            {
                Assert.False(1==1);
            }
        }
    }
}
