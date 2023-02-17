using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.TodoLists.Queries.GetTodos;
using Todo_App.Domain.Entities;
using Todo_App.Domain.ValueObjects;

namespace Todo_App.Application.IntegrationTests.TodoLists.Queries;

using static Testing;

public class GetTodosTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnPriorityLevels()
    {
        await RunAsDefaultUserAsync();

        var query = new GetTodosQuery();

        var result = await SendAsync(query);

        result.PriorityLevels.Should().NotBeEmpty();
    }

    [Test]
    public async Task ShouldReturnAllListsAndItems()
    {
        await RunAsDefaultUserAsync();

        await AddAsync(new TodoList
        {
            Title = "Shopping",
            Colour = Colour.Blue,
            Items =
                    {
                        new TodoItem { Title = "Apples", Done = true, IsSoftDeleted = false },
                        new TodoItem { Title = "Milk", Done = true, IsSoftDeleted = false },
                        new TodoItem { Title = "Bread", Done = true, IsSoftDeleted = false },
                        new TodoItem { Title = "Toilet paper", IsSoftDeleted = true},
                        new TodoItem { Title = "Pasta", IsSoftDeleted = false },
                        new TodoItem { Title = "Tissues", IsSoftDeleted = false },
                        new TodoItem { Title = "Tuna", IsSoftDeleted = false }
                    },
            IsSoftDeleted = false
        });

        var query = new GetTodosQuery();

        var result = await SendAsync(query);

        result.Lists.Should().HaveCount(1);
        result.Lists.First().Items.Should().HaveCount(6);
    }

    [Test]
    public async Task ShouldAcceptAnonymousUser()
    {
        var query = new GetTodosQuery();

        var action = () => SendAsync(query);

        await action.Should().NotThrowAsync<UnauthorizedAccessException>();
    }
}
