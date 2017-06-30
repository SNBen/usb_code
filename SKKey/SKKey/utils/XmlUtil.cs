using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SKKey.utils
{
    /**
     * 
     * 
     * XML 工具类
     * 
     * 
     * */
    class XmlUtil
    {
        public static string getNodeText(XmlDocument doc, string xpath)
        {
            var node = doc.SelectSingleNode(xpath);
            if (node != null)
            {
                return node.InnerText;
            }
            else
            {
                return null;
            }
               
        }

        public static string getNodeText(XmlNode parentNode, string childNodeName)
        {
            if (parentNode == null)
            {
                return null;
            }
            if (parentNode[childNodeName] == null)
            {
                return null;
            }
             
            return parentNode[childNodeName].InnerText;
        }
    }
}
