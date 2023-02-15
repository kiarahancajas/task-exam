using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Domain.Enums;

namespace Todo_App.Application.Tags.Queries;

public record GetTagsQuery : IRequest<TagsVM>;

public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, TagsVM>
{ 
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public GetTagsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<TagsVM> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        return new TagsVM
        {
            AllTags = await _context.Tags
                .AsNoTracking()
                .ProjectTo<TagsDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken)
        };
    }

}