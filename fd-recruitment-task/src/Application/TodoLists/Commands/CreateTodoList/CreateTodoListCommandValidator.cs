using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Domain.ValueObjects;

namespace Todo_App.Application.TodoLists.Commands.CreateTodoList;

public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoListCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
            .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");
        
        RuleFor(v => v.Colour)
            .NotEmpty().WithMessage("Colour code is required.")
            .MaximumLength(7).WithMessage("Colour code must not exceed 7 characters");
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _context.TodoLists
            .AllAsync(l => l.Title != title, cancellationToken);
    }
}
