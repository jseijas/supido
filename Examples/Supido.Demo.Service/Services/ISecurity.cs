using Supido.Demo.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace Supido.Demo.Service.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISecurity" in both code and config file together.
    [ServiceContract]
    public interface ISecurity
    {
        [OperationContract]
        UserDto Login(string user, string password);

        [OperationContract]
        string Logout(string sessionToken);

        [OperationContract]
        UserDto GetUserInfo(string sessionToken);
    }
}
