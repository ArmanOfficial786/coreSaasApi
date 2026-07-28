namespace UserManagement.Application.Queries.ModulePermissionQuery.ModulePermissionListQuery;

public record GetAllModulePermissionQuery : IRequest<Response<List<ModulePermissionViewModel>>>;
