namespace UserManagement.Application.Queries.ModulePermissionQuery.ModulePermissionGroupedQuery;

public record GetAllModulePermissionGroupedQuery : IRequest<Response<List<ModulePermissionGroupViewModel>>>;

