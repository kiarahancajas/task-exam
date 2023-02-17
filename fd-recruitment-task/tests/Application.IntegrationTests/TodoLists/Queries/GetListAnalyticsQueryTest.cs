using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.Tags.Queries;
using Todo_App.Application.TodoItems.Commands.CreateTodoItem;
using Todo_App.Application.TodoItems.Commands.UpdateTodoItemDetail;
using Todo_App.Application.TodoLists.Commands.CreateTodoList;
using Todo_App.Application.TodoLists.Queries.GetTodos;
using Todo_App.Domain.Enums;

namespace Todo_App.Application.IntegrationTests.TodoLists.Queries;

using static Testing;

public class GetListAnalyticsQueryTest : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnAnalyticsOfTags()
    {
        var createListCommand = new CreateTodoListCommand
        {
            Title = "Tasks"
        };
        
        var listId = await SendAsync(createListCommand);
        
        var createItemCommand = new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "Sample One"
        };
        
        var createSecondItemCommand = new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "Sample Two"
        };
        
        var firstItemId = await SendAsync(createItemCommand);
        var secondItemId = await SendAsync(createSecondItemCommand);
        
        var updateFirstItem = new UpdateTodoItemDetailCommand
        {
            Id = firstItemId,
            ListId = listId,
            Note = "A1",
            Priority = PriorityLevel.High,
            Tags = new List<TagsDto>
            {
                new TagsDto {ItemId = firstItemId, Name = "test one"},
                new TagsDto {ItemId = firstItemId, Name = "test two"}
            }
        };
        
        var updateSecondItem = new UpdateTodoItemDetailCommand
        {
            Id = secondItemId,
            ListId = listId,
            Note = "A1",
            Priority = PriorityLevel.High,
            Tags = new List<TagsDto>
            {
                new TagsDto {ItemId = firstItemId, Name = "test one"},
                new TagsDto {ItemId = firstItemId, Name = "test two"},
                new TagsDto {ItemId = firstItemId, Name = "test three"}
            }
        };

        await SendAsync(updateFirstItem);
        await SendAsync(updateSecondItem);

        var query = new GetListAnalyticsQuery(listId);
        var result = await SendAsync(query);
        
        result.Should().NotBeNull();
        result.TagAnalytics.Count.Should().Be(3);
    }

}