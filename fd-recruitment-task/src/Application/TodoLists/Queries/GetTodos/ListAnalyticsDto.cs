using Todo_App.Application.Tags.Queries;

namespace Todo_App.Application.TodoLists.Queries.GetTodos;

public class ListAnalyticsDto
{
    public int ListId { get; set; }
    public List<TagAnalyticsDto> TagAnalytics { get; set; }
}
