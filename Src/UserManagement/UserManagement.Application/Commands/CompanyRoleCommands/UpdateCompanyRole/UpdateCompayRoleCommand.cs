namespace UserManagement.Application.Commands.CompanyRoleCommands.UpdateCompanyRole;

public record UpdateCompanyRoleCommand(Guid Id, string Name, string Description, List<Guid> ModulePermissions) : IRequest<Response<RoleViewModel>>;
