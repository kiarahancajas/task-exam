using Todo_App.Application.Common.Mappings;
using Todo_App.Application.Tags.Queries;
using Todo_App.Application.TodoLists.Queries.GetTodos;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.TodoItems.Queries.GetTodoItemsWithPagination;

public class TodoItemBriefDto : IMapFrom<TodoItem>
{
    public TodoItemBriefDto()
    {
        Tags = new List<TagsDto>();
    }
    public int Id { get; set; }

    public int ListId { get; set; }

    public string? Title { get; set; }

    public bool Done { get; set; }

    public List<TagsDto> Tags { get; set; }
}
