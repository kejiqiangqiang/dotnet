using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.IO;
using System.Xml;

namespace WCFDuplexClientBaseApplication
{
    /// <summary>
    /// 跨域中间层服务
    /// </summary>
    public class DomainService : IDomainService
    {
        string xml = @"<?xml version='1.0' encoding='utf-8'?>  
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

        public Message ProvidePolicyFile()
        {
            xml = xml.Replace('\'', '"');
            byte[] bs = Encoding.UTF8.GetBytes(xml);
            MemoryStream ms = new MemoryStream(bs);
            XmlReader reader = XmlReader.Create(ms);

            Message result = Message.CreateMessage(MessageVersion.None, "", reader);
            return result;
        }
    }
}