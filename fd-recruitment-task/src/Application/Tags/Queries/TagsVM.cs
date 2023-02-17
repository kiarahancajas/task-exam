namespace Todo_App.Application.Tags.Queries;

public class TagsVM
{
    public IList<TagsDto> AllTags { get; set; } = new List<TagsDto>();
}