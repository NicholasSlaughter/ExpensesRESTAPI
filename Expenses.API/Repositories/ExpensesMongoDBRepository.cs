using Expenses.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.Repositories
{
    public class ExpensesMongoDBRepository : IExpensesRepository
    {
        private const string databaseName = "budget";
        private const string collectionName = "expenses";

        //The collection of expenses
        private readonly IMongoCollection<Expense> expensesCollection;
        
        //Filter object to get specific items in a collection
        private readonly FilterDefinitionBuilder<Expense> filterBuilder = Builders<Expense>.Filter;
        public ExpensesMongoDBRepository(IMongoClient mongoClient)
        {
            //Establish connection to the database
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);

            //Get the expenses collection from the database
            expensesCollection = database.GetCollection<Expense>(collectionName);
        }
        public async Task CreateExpenseAsync(Expense expense)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteExpenseAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Expense> GetExpenseAsync(Guid id)
        {
            //Search collection of Expenses for matching Id
            var filter = filterBuilder.Eq(expense => expense.Id, id);

            //Return the matching expense
            return await expensesCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            throw new NotImplementedException();
        }
    }
}
