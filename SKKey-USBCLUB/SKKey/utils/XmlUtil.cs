﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

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

        public static bool GetParamByTaxCode(String TaxCode, ref String USBID, ref String PWD)
        {
            XPathDocument doc = new XPathDocument("USBData.xml");
            XPathNavigator xPathNav = doc.CreateNavigator();
            String str = String.Format("/Root/Item[TaxCode = '{0}']", TaxCode);
            //使用xPath取rss中最新的10条随笔

            XPathNodeIterator nodeIterator = xPathNav.Select(str);
            if (nodeIterator.MoveNext())
            {
                XPathNavigator itemNav = nodeIterator.Current;
                USBID = String.Format("{0}_127.0.0.1_{1}", itemNav.SelectSingleNode("USBID").Value, itemNav.SelectSingleNode("USBPort").Value);
                PWD = itemNav.SelectSingleNode("PWD").Value;
                Console.WriteLine("{0} = {1}", USBID, PWD);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}