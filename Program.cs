using IdGen;
using TodoServer.Dtos;
using FluentValidation;
using System.Reflection;

var todos = new List<OutTodoDto>();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped((sp) => new IdGenerator(0, new IdGeneratorOptions { SequenceOverflowStrategy = SequenceOverflowStrategy.SpinWait }));
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(Program)));

builder.Services.Configure<RouteHandlerOptions>(o => o.ThrowOnBadRequest = true);

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Message = "Success",
    Data = todos
}));

app.MapPost("/", async (IdGenerator idGen, IValidator<InTodoAddDto> validator, InTodoAddDto dto) =>
{
    var validationError = await validator.ValidateAsync(dto);
    if (!validationError.IsValid)
    {
        return Results.ValidationProblem(validationError.ToDictionary());
    }

    var isParentIdExist = todos.FirstOrDefault(e => e.ParentId == dto.ParentId) != null;
    if (dto.ParentId != null && !isParentIdExist)
    {
        return Results.NotFound(new
        {
            Message = "Parent id not found"
        });
    }

    var todo = new OutTodoDto
    {
        Id = idGen.CreateId(),
        ParentId = dto.ParentId,
        TaskName = dto.TaskName,
        IsDone = false,
        StartDate = dto.StartDate,
        DueDate = dto.DueDate,
        Starred = false,
        CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
    };

    todos.Add(todo);

    return Results.Ok(new
    {
        Message = "Success",
        Data = todo
    });
});

app.MapPut("/", async (IValidator<InTodoUpdateDto> validator, InTodoUpdateDto dto) =>
{
    var validationError = await validator.ValidateAsync(dto);
    if (!validationError.IsValid)
    {
        return Results.ValidationProblem(validationError.ToDictionary());
    }

    var todoIndex = todos.FindIndex(e => e.Id == dto.Id);
    if (todoIndex == -1)
    {
        return Results.NotFound(new
        {
            Message = "Todo not found"
        });
    }

    var isParentIdExist = todos.FirstOrDefault(e => e.ParentId == dto.ParentId) != null;
    if (dto.ParentId != null && !isParentIdExist)
    {
        return Results.NotFound(new
        {
            Message = "Parent id not found"
        });
    }

    todos[todoIndex] = new OutTodoDto
    {
        ParentId = dto.ParentId,
        TaskName = dto.TaskName,
        IsDone = dto.IsDone,
        StartDate = dto.StartDate,
        DueDate = dto.DueDate,
        Starred = dto.Starred,
        UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
    };

    return Results.Ok(new
    {
        Message = "Success",
        Data = todos[todoIndex]
    });
});

app.Run();
