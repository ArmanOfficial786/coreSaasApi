using UserManagement.Application.Queries.ModulePermissionQuery.ModulePermissionGroupedQuery;
using UserManagement.Application.Queries.ModulePermissionQuery.ModulePermissionListQuery;

namespace UserManagement.Api.Controllers;

[ApiController]
[Route("UserManagement/[controller]")]
public class ModulePermissionController : ControllerBase
{
    private readonly ISender _sender;

    public ModulePermissionController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("GetGroupedList")]
    public async Task<ActionResult<Response<List<ModulePermissionGroupViewModel>>>> GetModulePermissions([FromQuery] GetAllModulePermissionGroupedQuery query)
    {
        var result = await _sender.Send(query);
        return Ok(result);
    }
    [HttpGet("GetList")]
    public async Task<ActionResult<Response<List<ModulePermissionViewModel>>>> GetModulePermissionsByRoleId([FromQuery] GetAllModulePermissionQuery query)
    {
        var result = await _sender.Send(query);
        return Ok(result);
    }
}
