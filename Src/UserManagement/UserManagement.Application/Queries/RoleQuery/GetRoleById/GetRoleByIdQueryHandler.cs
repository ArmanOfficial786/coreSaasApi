// File: Application/Queries/RoleQuery/GetRoleById/GetRoleByIdQueryHandler.cs

namespace UserManagement.Application.Queries.RoleQuery.GetRoleById;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Response<RoleViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRoleByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<RoleViewModel>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        // FIXED: Repository<T>() is synchronous — no await. Was calling
        // Repository<CompanyRole>(), swapped to Role since CompanyRole
        // doesn't exist in the current architecture (Role.CompanyId set
        // per company, no separate junction entity) — confirm if that's wrong.
        var roleRepo = _unitOfWork.Repository<Role>();

        // FIXED: cancellationToken was passed positionally into the
        // disableTracking (bool) slot — needs to be named.
        var role = await roleRepo.GetSingleOrDefaultAsync(
            predicate: r => r.Id == request.Id && r.ToDate == null,
            cancellationToken: cancellationToken);

        if (role is null)
            return Response<RoleViewModel>.FailureResponse("Role not found");

        var roleViewModel = _mapper.Map<RoleViewModel>(role);

        return Response<RoleViewModel>.SuccessResponse(roleViewModel, "Role retrieved successfully");
    }
}
