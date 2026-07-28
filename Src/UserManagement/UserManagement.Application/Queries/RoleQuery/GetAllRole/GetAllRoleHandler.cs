namespace UserManagement.Application.Queries.RoleQuery.GetAllRole;

public class GetAllRoleHandler : IRequestHandler<GetAllRoleQuery, Response<PaginatedData<RoleListViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    public GetAllRoleHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    public async Task<Response<PaginatedData<RoleListViewModel>>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
    {
        var roleRepo = _unitOfWork.Repository<Role>();
        var filter = _mapper.Map<Filter>(request);
        var roles = await roleRepo.GetPaginatedListAsync<RoleListViewModel>(
            filter,
            cancellationToken: cancellationToken);
        return Response<PaginatedData<RoleListViewModel>>.SuccessResponse(roles);
    }
}
