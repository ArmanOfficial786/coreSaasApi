// File: GetAllCompanyQueryHandler.cs
namespace UserManagement.Application.Queries.CompanyQuery.GetAllCompany;

public class GetAllCompanyQueryHandler : IRequestHandler<GetAllCompanyQuery, Response<PaginatedData<CompanyListViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetAllCompanyQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Response<PaginatedData<CompanyListViewModel>>> Handle(GetAllCompanyQuery request, CancellationToken cancellationToken)
    {


        var companyRepo = _unitOfWork.Repository<Company>();
        var filter = _mapper.Map<Filter>(request);

        var companies = await companyRepo.GetPaginatedListAsync<CompanyListViewModel>(
            filter,
            cancellationToken: cancellationToken);

        return Response<PaginatedData<CompanyListViewModel>>.SuccessResponse(companies);
    }
}
