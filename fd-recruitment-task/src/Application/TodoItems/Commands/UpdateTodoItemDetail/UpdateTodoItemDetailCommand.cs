using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Application.Tags.Queries;
using Todo_App.Domain.Entities;
using Todo_App.Domain.Enums;

namespace Todo_App.Application.TodoItems.Commands.UpdateTodoItemDetail;

public record UpdateTodoItemDetailCommand : IRequest
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public PriorityLevel Priority { get; init; }

    public string? Note { get; init; }
    
    public List<TagsDto>? Tags { get; init; }
    
}

public class UpdateTodoItemDetailCommandHandler : IRequestHandler<UpdateTodoItemDetailCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemDetailCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems.Include(t => t.Tags)
            .FirstAsync(x=> x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }
        
        entity.ListId = request.ListId;
        entity.Priority = request.Priority;
        entity.Note = request.Note;

        // Update the Tag entities associated with the TodoItem
        var newTags = new List<Domain.Entities.Tags>();
        
        if (request.Tags != null && request.Tags?.Count > 0)
        {
            foreach (var tag in request.Tags)
            {
                var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tag.Name, cancellationToken);

                if (existingTag == null)
                {
                    existingTag = new  Domain.Entities.Tags { Name = tag.Name, ItemId = tag.ItemId };
                    _context.Tags.Add(existingTag);
                }
                newTags.Add(existingTag);
            }

        }

        entity.Tags = newTags;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
