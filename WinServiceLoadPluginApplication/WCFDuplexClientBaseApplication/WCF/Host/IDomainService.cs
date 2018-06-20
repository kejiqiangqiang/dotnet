using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 跨域中间层服务接口
    /// </summary>
    [ServiceContract]
    public interface IDomainService
    {
        [OperationContract]
        [WebGet(UriTemplate = "clientaccesspolicy.xml")]
        Message ProvidePolicyFile();
    }
}