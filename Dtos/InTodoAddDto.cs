using FluentValidation;

namespace TodoServer.Dtos;

public class InTodoAddDto
{
    public long? ParentId { get; set; }
    public string? TaskName { get; set; }
    public long? StartDate { get; set; }
    public long? DueDate { get; set; }
}

public class InTodoAddDtoValidator : AbstractValidator<InTodoAddDto>
{
    public InTodoAddDtoValidator()
    {
        RuleFor(e => e.ParentId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0);

        RuleFor(e => e.TaskName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}