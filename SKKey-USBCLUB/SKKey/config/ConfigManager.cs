using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using SKKey.utils;
using Newtonsoft.Json;

namespace SKKey.config
{
    class ConfigManager
    {
        private static readonly log4net.ILog log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ConfigManager instance = null;

        public Config Config
        {
            get;
            set;
        }
        private ConfigManager()
        {
        }

        public static ConfigManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigManager();
                    instance.init();
                }
                return instance;
            }
        }

        public static void writeJSONConfigAndInit(Dictionary<String,String> configMap)
        {
            string filepath = Path.Combine(FileUtil.getAppPath(), "config.json");
            File.WriteAllText(filepath, JsonConvert.SerializeObject(configMap), Encoding.GetEncoding("UTF-8"));
            instance = new ConfigManager();
            instance.init();
        }

        public void init()
        {
            Config = new Config();
             loadXML();
             loadJSON();
        }

        public void loadXML()
        {
            string filepath = Path.Combine(FileUtil.getAppPath(), "config.xml");
            log.Info("load 配置文件 " + filepath);
            if (!File.Exists(filepath))
            {
                log.Error("配置文件不存在");
                return;
            }

            string xml = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                xml = File.ReadAllText(filepath, Encoding.GetEncoding("UTF-8"));
                doc.LoadXml(xml);
               
                Config.clientType = XmlUtil.getNodeText(doc, "//client-type");
                Config.taskServer = XmlUtil.getNodeText(doc, "//task-server");
                Config.usbClubPort = XmlUtil.getNodeText(doc, "//usb-club-port");
            }
            catch (Exception ex)
            {
                log.Error("载入配置文件失败", ex);
                return;
            }
            log.Info("配置载入成功");
        }

        public void loadJSON()
        {
            string filepath = Path.Combine(FileUtil.getAppPath(), "config.json");
            log.Info("load 配置文件 " + filepath);
            if (!File.Exists(filepath))
            {
                log.Info("配置文件不存在");
                return;
            }

            string json = "";
            try
            {
                json = File.ReadAllText(filepath, Encoding.GetEncoding("UTF-8"));
                if (json == null || json == "")
                {
                    log.Info("解析JSON配置文件为空");
                    return;
                }
                Dictionary<String, String> map = JsonConvert.DeserializeObject<Dictionary<String, String>>(json);

                if (map.ContainsKey("sh"))
                {
                    Config.sh = map["sh"];
                }
                if (map.ContainsKey("password"))
                {
                    Config.password = map["password"];
                }
                if (map.ContainsKey("license"))
                {
                    Config.license = map["license"];
                }

                if (map.ContainsKey("taskServerIP"))
                {
                    Config.taskServerIP = map["taskServerIP"];
                }
                if (map.ContainsKey("taskServerPort"))
                {
                    Config.taskServerPort = map["taskServerPort"];
                }

                if (map.ContainsKey("controlVersion"))
                {
                    Config.controlVersion = map["controlVersion"];
                }
            }
            catch (Exception ex)
            {
                log.Error("载入配置文件失败", ex);
                return;
            }
            log.Info("配置载入成功");
        }
    }
}
