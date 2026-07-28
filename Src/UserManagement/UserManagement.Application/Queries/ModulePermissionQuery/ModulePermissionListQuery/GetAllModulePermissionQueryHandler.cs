
namespace UserManagement.Application.Queries.ModulePermissionQuery.ModulePermissionListQuery;

public class GetAllModulePermissionQueryHandler : IRequestHandler<GetAllModulePermissionQuery, Response<List<ModulePermissionViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetAllModulePermissionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Response<List<ModulePermissionViewModel>>> Handle(GetAllModulePermissionQuery request, CancellationToken cancellationToken)
    {
        var modulePermissionRepo = _unitOfWork.Repository<ModulePermission>();

        // Load all ModulePermission rows with their Module navigation included
        // (Module must be included, otherwise ModuleName mapping resolves to null)
        var modulePermissions = await modulePermissionRepo
            .GetAll(includes: x => x.Module)
            .ToListAsync(cancellationToken);

        // Uses your existing CreateMap<ModulePermission, ModulePermissionViewModel>()
        var result = _mapper.Map<List<ModulePermissionViewModel>>(modulePermissions);

        return Response<List<ModulePermissionViewModel>>.SuccessResponse(result);
    }
}
