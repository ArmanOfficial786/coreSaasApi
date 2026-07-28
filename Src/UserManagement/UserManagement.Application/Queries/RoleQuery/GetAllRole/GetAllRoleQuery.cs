namespace UserManagement.Application.Queries.RoleQuery.GetAllRole;

public record GetAllRoleQuery() : FilterDTO, IRequest<Response<PaginatedData<RoleListViewModel>>>;

