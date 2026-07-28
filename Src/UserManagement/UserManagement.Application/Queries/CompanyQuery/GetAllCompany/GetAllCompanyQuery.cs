namespace UserManagement.Application.Queries.CompanyQuery.GetAllCompany;

public record GetAllCompanyQuery() : FilterDTO, IRequest<Response<PaginatedData<CompanyListViewModel>>>;
