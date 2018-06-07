using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using System.Text;

namespace WindowsFormsWCFHostServer.Hosting
{
    public class DomainService : IDomainService
    {
        string xml=@"<?xml version='1.0' encoding='utf-8'?>  
                     <access-policy>  
                      <cross-domain-access>  
                        <policy>  
                          <allow-from http-request-headers='*'>  
                            <domain uri='*'/>  
                          </allow-from>  
                          <grant-to>  
                            <resource path='/' include-subpaths='true'/>  
                          </grant-to>  
                        </policy>  
                      </cross-domain-access>  
                    </access-policy>  ";


        #region IDomainService 成员
        public Message ProvidePolicyFile()
        {
            xml = xml.Replace('\'', '"');
            byte[] bs = Encoding.UTF8.GetBytes(xml);
            MemoryStream ms = new System.IO.MemoryStream(bs);
            XmlReader reader = XmlReader.Create(ms);

            Message result = Message.CreateMessage(MessageVersion.None, "", reader);
            return result;
        }
        #endregion
    }
}