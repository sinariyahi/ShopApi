using Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using XAct.Messages;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("GoldiranAPI")]
    [Authorize]
    public class BaseController : ControllerBase
    {
        public string UserIP
        {
            get
            {
                return Request.HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }

        public Guid UserId
        {
            get
            {
                return Guid.Parse(User.Claims.FirstOrDefault(q => q.Type == "userGuid").Value);
            }
        }

        public UserType UserType
        {
            get
            {
                return ((UserType)Convert.ToInt32(User.Claims.FirstOrDefault(q => q.Type == "userType").Value));
            }
        }

        public List<string> RoleCodes
        {
            get
            {
                var roles = User.Claims.FirstOrDefault(q => q.Type == "roleCodes").Value.ToUpper();
                if (roles.Contains(","))
                {
                    return roles.Split(",").ToList();
                }
                else
                {
                    return new List<string> { roles };
                }
            }
        }

        public List<int> Projects
        {
            get
            {
                var roles = User.Claims.FirstOrDefault(q => q.Type == "projects").Value.ToUpper();
                if (roles.Contains(","))
                {
                    return roles.Split(",").ToList().Select(q => Convert.ToInt32(q)).ToList();
                }
                else
                {
                    return new List<int> { Convert.ToInt32(roles) };
                }
            }
        }
    }
}
