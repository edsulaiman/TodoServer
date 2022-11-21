using FluentValidation;

namespace TodoServer.Dtos;

public class InTodoUpdateDto
{
    public long? Id { get; set; }
    public long? ParentId { get; set; }
    public string? TaskName { get; set; }
    public bool? IsDone { get; set; }
    public long? StartDate { get; set; }
    public long? DueDate { get; set; }
    public bool? Starred { get; set; }
}

public class InTodoUpdateDtoValidator : AbstractValidator<InTodoUpdateDto>
{
    public InTodoUpdateDtoValidator()
    {
        RuleFor(e => e.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(e => e.ParentId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0);

        RuleFor(e => e.TaskName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();

        RuleFor(e => e.IsDone)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();

        RuleFor(e => e.Starred)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}