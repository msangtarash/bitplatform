﻿using Boilerplate.Client.Core.Controllers.Todo;
using Boilerplate.Server.Models.Todo;
using Boilerplate.Shared.Dtos.Todo;

namespace Boilerplate.Server.Controllers.Todo;

[Route("api/[controller]/[action]")]
[ApiController]
public partial class TodoItemController : AppControllerBase, ITodoItemController
{
    [HttpGet, EnableQuery]
    public IQueryable<TodoItemDto> Get()
    {
        var userId = User.GetUserId();

        return DbContext.TodoItems
            .Where(t => t.UserId == userId)
            .Project();
    }

    [HttpGet]
    public async Task<PagedResult<TodoItemDto>> GetTodoItems(ODataQueryOptions<TodoItemDto> odataQuery, CancellationToken cancellationToken)
    {
        var query = (IQueryable<TodoItemDto>)odataQuery.ApplyTo(Get(), ignoreQueryOptions: AllowedQueryOptions.Top | AllowedQueryOptions.Skip);

        var totalCount = await query.LongCountAsync(cancellationToken);

        if (odataQuery.Skip is not null)
            query = query.Skip(odataQuery.Skip.Value);

        if (odataQuery.Top is not null)
            query = query.Take(odataQuery.Top.Value);

        return new PagedResult<TodoItemDto>(await query.ToArrayAsync(cancellationToken), totalCount);
    }

    [HttpGet("{id}")]
    public async Task<TodoItemDto> Get(int id, CancellationToken cancellationToken)
    {
        var dto = await Get().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (dto is null)
            throw new ResourceNotFoundException(Localizer[nameof(AppStrings.ToDoItemCouldNotBeFound)]);

        return dto;
    }

    [HttpPost]
    public async Task<TodoItemDto> Create(TodoItemDto dto, CancellationToken cancellationToken)
    {
        var entityToAdd = dto.Map();

        entityToAdd.UserId = User.GetUserId();

        entityToAdd.Date = DateTimeOffset.UtcNow;

        await DbContext.TodoItems.AddAsync(entityToAdd, cancellationToken);

        await DbContext.SaveChangesAsync(cancellationToken);

        return entityToAdd.Map();
    }

    [HttpPut]
    public async Task<TodoItemDto> Update(TodoItemDto dto, CancellationToken cancellationToken)
    {
        var entityToUpdate = await DbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == dto.Id, cancellationToken);

        if (entityToUpdate is null)
            throw new ResourceNotFoundException(Localizer[nameof(AppStrings.ToDoItemCouldNotBeFound)]);

        dto.Patch(entityToUpdate);

        await DbContext.SaveChangesAsync(cancellationToken);

        return entityToUpdate.Map();
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id, CancellationToken cancellationToken)
    {
        DbContext.Remove(new TodoItem { Id = id });

        var affectedRows = await DbContext.SaveChangesAsync(cancellationToken);

        if (affectedRows < 1)
            throw new ResourceNotFoundException(Localizer[nameof(AppStrings.ToDoItemCouldNotBeFound)]);
    }
}

