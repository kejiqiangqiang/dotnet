using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace WindowsFormsWCFHostServer.Hosting
{
    [ServiceContract]
    public interface IDomainService
    {
        [OperationContract]
        [WebGet(UriTemplate = "clientaccesspolicy.xml")]
        Message ProvidePolicyFile();
    }
}