// File: Application/Commands/RoleCommands/UpdateRole/UpdateRoleCommandHandler.cs

using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Commands.CompanyRoleCommands.UpdateCompanyRole;

namespace UserManagement.Application.Commands.RoleCommands.UpdateRole;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateCompanyRoleCommand, Response<RoleViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateRoleCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Response<RoleViewModel>> Handle(UpdateCompanyRoleCommand request, CancellationToken cancellationToken)
    {
        // ─── 1. Validate Authentication ─────────────────────────────────────────
        var userInfo = _currentUserService.UserInfo;
        if (userInfo is null)
            return Response<RoleViewModel>.FailureResponse(Errors.Unauthorized);

        var companyId = userInfo.CompanyId;

        // ─── 2. Get Repositories ──────────────────────────────────────────────
        var roleRepo = _unitOfWork.Repository<Role>();
        var modulePermissionRepo = _unitOfWork.Repository<ModulePermission>();

        // ─── 3. Fetch Role with its permissions ──────────────────────────────
        var role = await roleRepo.GetAll(
            x => x.Id == request.Id && x.CompanyId == companyId, // Only allow updating roles within the same company
            includes: x => x.RoleModulePermissions!
        )
        .FirstOrDefaultAsync(cancellationToken);

        if (role is null)
            return Response<RoleViewModel>.FailureResponse(Errors.RoleNotFound);

        // ─── 4. Validate ModulePermissions Exist ──────────────────────────────
        // request.ModulePermissions is List<Guid> already — use it directly, no ".Id" needed.
        var requestedPermissionIds = request.ModulePermissions ?? new List<Guid>();

        if (requestedPermissionIds.Count > 0)
        {
            var existingPermissions = await modulePermissionRepo.GetListAsync(
                predicate: mp => requestedPermissionIds.Contains(mp.Id),
                cancellationToken: cancellationToken
            );

            if (existingPermissions.Count != requestedPermissionIds.Count)
                return Response<RoleViewModel>.FailureResponse(Errors.ModulePermissionNotFound);
        }

        // ─── 5. Update Role Name and Description ──────────────────────────────
        role.Update(request.Name, request.Description);

        // ─── 6. Sync Permissions ──────────────────────────────────────────────
        var existingPermissionIds = role.RoleModulePermissions
            .Select(rmp => rmp.ModulePermissionId)
            .ToHashSet();

        var newPermissionIds = requestedPermissionIds.ToHashSet();

        // Permissions to add
        var permissionIdsToAdd = newPermissionIds.Except(existingPermissionIds).ToList();

        // Permissions to remove
        var permissionIdsToRemove = existingPermissionIds.Except(newPermissionIds).ToList();

        // Add new permissions
        if (permissionIdsToAdd.Count > 0)
        {
            var permissionsToAdd = await modulePermissionRepo.GetListAsync(
                predicate: mp => permissionIdsToAdd.Contains(mp.Id),
                cancellationToken: cancellationToken
            );

            foreach (var permission in permissionsToAdd)
            {
                role.AddRoleModulePermission(permission);
            }
        }

        // Remove permissions
        foreach (var permissionId in permissionIdsToRemove)
        {
            role.RemoveRoleModulePermission(permissionId);
        }

        // ─── 7. Save Changes ──────────────────────────────────────────────────
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // ─── 8. Map to ViewModel and Return ──────────────────────────────────
        var viewModel = _mapper.Map<RoleViewModel>(role);

        // Load permission details for the response.
        // Note: your AutoMapper profile already maps RoleViewModel.ModulePermissions
        // from Role.RoleModulePermissions, so this step may be redundant depending
        // on whether a RoleModulePermission -> ModulePermissionViewModel map exists.
        // Kept here for explicitness/safety.
        var finalPermissionIds = role.RoleModulePermissions
            .Select(rmp => rmp.ModulePermissionId)
            .ToList();

        if (finalPermissionIds.Count > 0)
        {
            var permissions = await modulePermissionRepo.GetListAsync(
                predicate: mp => finalPermissionIds.Contains(mp.Id),
                cancellationToken: cancellationToken
            );
            viewModel.ModulePermissions = _mapper.Map<List<ModulePermissionViewModel>>(permissions);
        }

        return Response<RoleViewModel>.SuccessResponse(
            viewModel,
            Messages.UpdatedSuccessfully
        );
    }
}
