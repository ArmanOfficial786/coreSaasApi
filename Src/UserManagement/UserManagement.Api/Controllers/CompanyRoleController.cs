
using UserManagement.Application.Queries.RoleQuery.GetRoleById;

namespace UserManagement.Api.Controllers;

[ApiController]
[Route("UserManagement/[controller]")]

public class CompanyRoleController : ControllerBase
{
    private readonly ISender _sender;

    public CompanyRoleController(ISender sender)
    {
        _sender = sender;
    }
    [HttpGet("GetList")]
    public async Task<ActionResult<Response<PaginatedData<RoleListViewModel>>>> GetAllCompanyRoles([FromQuery] GetAllRoleQuery query)
    {
        var response = await _sender.Send(query);
        return Ok(response);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Response<RoleViewModel>>> GetCompanyRoleById([FromRoute] Guid id)
    {
        GetRoleByIdQuery query = new(id);
        var response = await _sender.Send(query);
        return Ok(response);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<RoleViewModel>>> CreateCompanyRole([FromBody] CreateCompanyRoleCommand command)
    {
        var response = await _sender.Send(command);
        return Ok(response);
    }
}
