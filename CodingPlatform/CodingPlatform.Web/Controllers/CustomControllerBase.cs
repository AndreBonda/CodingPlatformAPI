using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace CodingPlatform.Web.Controllers;

public class CustomControllerBase : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public CustomControllerBase(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// TODO: move into service
    /// </summary>
    /// <returns></returns>
    protected long GetCurrentUserId()
    {
        var userId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Int64.Parse(userId);
    }

}