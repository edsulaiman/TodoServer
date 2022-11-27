using System.Reflection;
using FluentValidation;
using IdGen;
using TodoServer.Dtos;

var todos = new List<OutTodoDto>();

void _deleteSubTaskRecursive(long id)
{
    var deletedTodos = todos!.Where(e => e.ParentId == id).ToList();
    foreach (var todo in deletedTodos)
    {
        todos!.Remove(todo);
        _deleteSubTaskRecursive(todo.Id.GetValueOrDefault());
    }
}

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

    var isParentIdExist = todos.FirstOrDefault(e => e.Id == dto.ParentId) != null;
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

    var isParentIdExist = todos.FirstOrDefault(e => e.Id == dto.ParentId) != null;
    if (dto.ParentId != null && !isParentIdExist)
    {
        return Results.NotFound(new
        {
            Message = "Parent id not found"
        });
    }

    todos[todoIndex].ParentId = dto.ParentId;
    todos[todoIndex].TaskName = dto.TaskName;
    todos[todoIndex].IsDone = dto.IsDone;
    todos[todoIndex].StartDate = dto.StartDate;
    todos[todoIndex].DueDate = dto.DueDate;
    todos[todoIndex].Starred = dto.Starred;
    todos[todoIndex].UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    return Results.Ok(new
    {
        Message = "Success",
        Data = todos[todoIndex]
    });
});

app.MapDelete("/{id}", (long id) =>
{
    var todo = todos.FirstOrDefault(e => e.Id == id);
    if (todo == null)
    {
        return Results.NotFound(new
        {
            Message = "Todo not found"
        });
    }

    todos.Remove(todo);
    _deleteSubTaskRecursive(todo.Id.GetValueOrDefault());

    return Results.Ok(new { Message = "Success" });
});

app.Run();