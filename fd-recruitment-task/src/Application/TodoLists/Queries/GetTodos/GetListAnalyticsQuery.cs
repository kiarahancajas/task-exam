using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Application.Tags.Queries;

namespace Todo_App.Application.TodoLists.Queries.GetTodos;

public record GetListAnalyticsQuery(int Id) : IRequest<ListAnalyticsDto>;

public class GetListAnalyticsHandler : IRequestHandler<GetListAnalyticsQuery, ListAnalyticsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetListAnalyticsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListAnalyticsDto> Handle(GetListAnalyticsQuery request, CancellationToken cancellationToken)
    {
        var todoList = await _context.TodoLists.Include(t => t.Items)
            .ThenInclude(i => i.Tags)
            .FirstAsync(x => x.Id == request.Id, cancellationToken);

        var allTags = todoList.Items.SelectMany(t => t.Tags).ToList();

        var tagItems = allTags.GroupBy(t => t.Name)
            .Where(g => g.Key != null)
            .Select(
                tag => new TagAnalyticsDto
                {
                    TagName = tag.Key, Count = tag.Count()
                }).ToList();
        

        return new ListAnalyticsDto { ListId = request.Id, TagAnalytics = tagItems };
    }

}