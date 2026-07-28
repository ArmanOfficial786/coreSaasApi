using UserManagement.Application.Queries.CompanyQuery.GetAllCompany;

namespace UserManagement.Api.Controllers;

[ApiController]
[Route("UserManagement/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ISender _sender;
    public CompanyController(ISender sender)
    {
        _sender = sender;
    }
    [HttpGet("GetList")]
    public async Task<ActionResult<Response<PaginatedData<CompanyListViewModel>>>> GetAllCompanies([FromQuery] GetAllCompanyQuery request)
    {
        return await _sender.Send(request);
    }
    [HttpPost]
    public async Task<ActionResult<Response<string>>> CreateCompany([FromBody] CreateCompanyCommand request)
    {
        return await _sender.Send(request);
    }
}
