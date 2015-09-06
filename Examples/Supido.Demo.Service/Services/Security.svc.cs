using Supido.Business;
using Supido.Core.Container;
using Supido.Demo.Service.DTO;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace Supido.Demo.Service.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Security" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Security.svc or Security.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Security : ISecurity
    {
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/login?login={user}&password={password}")]
        public UserDto Login(string user, string password)
        {
            return (UserDto)IoC.Get<ISecurityManager>().Login(user, password);
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/logout?sessionToken={sessionToken}")]
        public string Logout(string sessionToken)
        {
            IoC.Get<ISecurityManager>().Logout(sessionToken);
            return "OK";
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/getUserInfo?sessionToken={sessionToken}")]
        public UserDto GetUserInfo(string sessionToken)
        {
            return (UserDto)IoC.Get<ISecurityManager>().GetUserInfo(sessionToken);
        }

    }
}
