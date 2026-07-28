namespace UserManagement.Application.Queries.RoleQuery.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IRequest<Response<RoleViewModel>>;

