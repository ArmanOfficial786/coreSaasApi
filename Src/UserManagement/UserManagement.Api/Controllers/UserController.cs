namespace UserManagement.Api.Controllers;

[ApiController]
[Route("UserManagement/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }



    [HttpPost]
    public async Task<ActionResult<Response<UserViewModel>>> save([FromBody] CreateUserCommand request)
    {
        return await _sender.Send(request);
    }


}
