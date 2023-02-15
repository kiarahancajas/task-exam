using FluentValidation;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Application.Tags.Queries;

namespace Todo_App.Application.TodoItems.Commands.UpdateTodoItemDetail;

public class UpdateTodoItemDetailCommandValidator : AbstractValidator<UpdateTodoItemDetailCommand>
{
    private readonly IApplicationDbContext _context;
    
    public UpdateTodoItemDetailCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Note)
            .NotNull().MaximumLength(200).WithMessage("Note must not exceed 200 characters.");
        RuleFor(v => v.Tags)
            .NotNull().Must(BeUniqueTags).WithMessage("Tags must have unique names.");
    }
    
    private bool BeUniqueTags(List<TagsDto>? tags)
    {
        if (tags == null) return true;
        // Get the list of tag names
        var tagNames = tags.Select(t => t.Name).ToList();

        // Check if there are any duplicates in the list
        return tagNames.Count == tagNames.Distinct().Count();
    }
    
}