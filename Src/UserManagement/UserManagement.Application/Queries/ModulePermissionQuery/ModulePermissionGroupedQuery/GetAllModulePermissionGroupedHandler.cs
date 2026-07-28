// File: Application/Queries/ModulePermissionQuery/GetAllModulePermissionHandler.cs

using Microsoft.EntityFrameworkCore;

namespace UserManagement.Application.Queries.ModulePermissionQuery.ModulePermissionGroupedQuery;

public class GetAllModulePermissionGroupedHandler
    : IRequestHandler<GetAllModulePermissionGroupedQuery, Response<List<ModulePermissionGroupViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllModulePermissionGroupedHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<List<ModulePermissionGroupViewModel>>> Handle(
        GetAllModulePermissionGroupedQuery request, CancellationToken cancellationToken)
    {
        var modulePermissionRepo = _unitOfWork.Repository<ModulePermission>();

        var modulePermissions = await modulePermissionRepo
            .GetAll(includes: x => x.Module)
            .ToListAsync(cancellationToken);

        var flatList = _mapper.Map<List<ModulePermissionViewModel>>(modulePermissions);

        var grouped = flatList
            .GroupBy(mp => new { mp.ModuleId, mp.ModuleName })
            .Select(g => new ModulePermissionGroupViewModel
            {
                ModuleId = g.Key.ModuleId,
                ModuleName = g.Key.ModuleName ?? "Unknown",
                Permissions = g.ToList()
            })
            .OrderBy(g => g.ModuleName)
            .ToList();

        return Response<List<ModulePermissionGroupViewModel>>.SuccessResponse(grouped);
    }
}
