namespace UserManagement.Application.Commands.CompanyRoleCommands.UpdateCompanyRole;

public class UpdateCompanyRoleCommandValidator : AbstractValidator<UpdateCompanyRoleCommand>
{
    public UpdateCompanyRoleCommandValidator()
    {
        _ = RuleFor(x => x.Id).NotEmpty();
        _ = RuleFor(x => x.Name).NotEmpty();
        _ = RuleFor(x => x.Description).NotEmpty();
    }
}
