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
            var existingTags = _context.Tags.Where(x => x.ItemId == entity.Id).ToList();
            //remove all existing tags
            foreach (var tags in existingTags)
            {
                _context.Tags.Remove(tags);
            }
            //replace it with the new one
            foreach (var tag in request.Tags)
            {
                var newTag = new  Domain.Entities.Tags { Name = tag.Name, ItemId = tag.ItemId };
                newTags.Add(newTag);
            }
        }

        entity.Tags = newTags;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
