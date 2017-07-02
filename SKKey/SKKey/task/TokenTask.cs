using System;
using System.Collections.Generic;
using System.Text;
using SKKey.websocket;
using SKKey.ocx;
using SKKey.http;
using System.Threading;
using SKKey.config;
using SKKey.socket;
namespace SKKey.task
{
    class TokenTask
    {
        private static readonly log4net.ILog log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string ERROR_LICENSE = "1";

        private static Boolean tockenTaskRequestThreadIsInit = false;
        public static Object tokenLock = new Object();
        public static bool   errorPassword = false;
        public static bool   errorLicense = false;
        public static string CODE_SUCCESS = "0";
        public static string CODE_SH_ERROR = "902";
        public static string CODE_OCX_ERROR = "907";
        public static string CODE_SERVER_ERROR = "903";
        public static string CODE_PASS_ERROR = "908";
        public static string CODE_USB_CLUB_ERROR = "901";



        public static List<TaskSocketMessage> requestTaskSocketMessages = new List<TaskSocketMessage>();

        public TaskSocketMessage handle(TaskSocketMessage requestTaskSocketMessage)
        {
            log.Info("执行取token任务");
            TaskSocketMessage returnSocketMessage = null;
            lock (requestTaskSocketMessages)
            {
                requestTaskSocketMessages.Add(requestTaskSocketMessage);
            }
            lock (tokenLock)
            {
                TaskSocketMessage taskSocketMessage = null;
                lock (requestTaskSocketMessages)
                {
                    if (requestTaskSocketMessages.Count > 0)
                    {
                        taskSocketMessage = requestTaskSocketMessages[0];
                        requestTaskSocketMessages.RemoveAt(0);
                    }
                }
                if (taskSocketMessage == null)
                {
                    return null;
                }
                returnSocketMessage = doHandle(requestTaskSocketMessage);
            }

            return returnSocketMessage;
        }

        public TaskSocketMessage getTocken(String ym, String ymbb, String sh, String password, String rwId)
        {
            TaskSocketMessage returnSocketMessage = new TaskSocketMessage();
            returnSocketMessage.type = TaskHandle.POST_TOKEN;
            returnSocketMessage.parameters["rwid"] = rwId;

            if (ymbb == null || ymbb == "")
            {
                ymbb = "3.0.08";
            }
            String oxcSh = null;

            try
            {
                oxcSh = WebOcxAccess.openAndGetCert(password);
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                returnSocketMessage.parameters["code"] = CODE_OCX_ERROR;
                returnSocketMessage.parameters["msg"] = "控件不可用";
                return returnSocketMessage;
            }

            if (sh != null && !oxcSh.Equals(sh))
            {
                returnSocketMessage.parameters["code"] = CODE_SH_ERROR;
                returnSocketMessage.parameters["msg"] = "税号不一致!，当前税号：" + oxcSh;
                return returnSocketMessage;
            }
            if (sh == null)
            {
                sh = oxcSh;
            }
            returnSocketMessage.parameters["sh"] = sh;

            String clientHello = WebOcxAccess.clientHello();
            FpdkHttpResult fpdkHttpResult = null;
            try
            {
                fpdkHttpResult = firstLogin(ym, ymbb, clientHello, password, 0);
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                returnSocketMessage.parameters["code"] = CODE_SERVER_ERROR;
                returnSocketMessage.parameters["msg"] = "调用服务器失败!";
                return returnSocketMessage;
            }

            if (!fpdkHttpResult.key1.Equals("01"))
            {
                returnSocketMessage.parameters["code"] = CODE_SERVER_ERROR;
                returnSocketMessage.parameters["msg"] = "调用服务器失败!";
                return returnSocketMessage;
            }

            string serverPacket = fpdkHttpResult.key2;
            string serverRandom = fpdkHttpResult.key3;

            string clientAuthCode = WebOcxAccess.ClientAuth(serverPacket);
            try
            {
                fpdkHttpResult = secondLogin(ym, ymbb, clientAuthCode, serverRandom, sh, 0);
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                returnSocketMessage.parameters["code"] = CODE_SERVER_ERROR;

                returnSocketMessage.parameters["msg"] = "调用服务器失败!";
                return returnSocketMessage;
            }
            if (fpdkHttpResult.key1.Equals("04"))
            {
                returnSocketMessage.parameters["code"] = CODE_PASS_ERROR;
                returnSocketMessage.parameters["msg"] = "密码不正确!";
                errorPassword = true;
                return returnSocketMessage;
            }
            else if (fpdkHttpResult.key1.Equals("03"))
            {
                returnSocketMessage.parameters["code"] = CODE_SUCCESS;
                returnSocketMessage.parameters["msg"] = "调用服务器成功!";
                returnSocketMessage.parameters["token"] = fpdkHttpResult.key2;
                return returnSocketMessage;
            }
            else
            {
                returnSocketMessage.parameters["code"] = CODE_SERVER_ERROR;
                returnSocketMessage.parameters["msg"] = "调用服务器失败!";
                return returnSocketMessage;
            }
        }

        public TaskSocketMessage doHandle(TaskSocketMessage requestTaskSocketMessage)
        {
            String sh = requestTaskSocketMessage.parameters["sh"];
            String password = requestTaskSocketMessage.parameters["mm"];
            String portInfo = null;
            if ("usb".Equals(ConfigManager.Instance.Config.clientType))
            {
                password = ConfigManager.Instance.Config.password;
            }
            else
            {
                portInfo = requestTaskSocketMessage.parameters["portInfo"];
                log.Info("打开机柜...");
                Dictionary<String, String> openInfo = UsbclubOperator.openPort(portInfo);
                Thread.Sleep(30000);
                if (!"0".Equals(openInfo["result"]))
                {
                    log.Error("打开设备失败：" + openInfo);

                    TaskSocketMessage returnSocketMessage = new TaskSocketMessage();
                    returnSocketMessage.type = TaskHandle.POST_TOKEN;
                    returnSocketMessage.parameters["rwid"] = requestTaskSocketMessage.parameters["rwid"];

                    returnSocketMessage.parameters["code"] = CODE_USB_CLUB_ERROR;
                    returnSocketMessage.parameters["msg"] = "打开机柜端口失败";
                }
                log.Info("打开机柜成功");
            }
            String ym = requestTaskSocketMessage.parameters["ym"];
            String ymbb = requestTaskSocketMessage.parameters["ymbb"];
            String rwId = requestTaskSocketMessage.parameters["rwid"];

            TaskSocketMessage returnTaskSocketMessage = getTocken(ym, ymbb, sh, password, rwId);
            if (portInfo != null)
            {
                UsbclubOperator.closePort(portInfo);
            }
            return returnTaskSocketMessage;
        }

        public FpdkHttpResult firstLogin(String ym, String ymbb, String clientHello, String password, int retryTimes)
        {
            Dictionary<String, String> para = new Dictionary<string, string>();
            para.Add("type", "CLIENT-HELLO");
            para.Add("clientHello", clientHello);
            para.Add("ymbb", ymbb);
            FpdkHttpResult fpdkHttpResult = null;
            try
            {
                fpdkHttpResult = RestClient.postJSONPCallback<FpdkHttpResult>(ym + "/SbsqWW/login.do", para);
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                if (retryTimes >= 1)
                {
                    throw new Exception("调用服务器接口失败");
                }
                retryTimes++;
                return firstLogin(ym, ymbb, clientHello, password, retryTimes);
            }

            if (fpdkHttpResult.key1.Equals("01"))
            {
                return fpdkHttpResult;
            }
            else
            {

                if (retryTimes >= 1)
                {
                    throw new Exception("调用服务器接口失败");
                }
                retryTimes++;
                return firstLogin(ym, ymbb, clientHello, password, retryTimes);
            }
        }

        public FpdkHttpResult secondLogin(String ym, String ymbb, String clientAuthCode, String serverRandom, String sh, int retryTimes)
        {
            Dictionary<String, String> para = new Dictionary<string, string>();
            para.Add("type", "CLIENT-AUTH");
            para.Add("clientAuthCode", clientAuthCode);
            para.Add("serverRandom", serverRandom);
            para.Add("ymbb", ymbb);
            para.Add("cert", sh);
            FpdkHttpResult fpdkHttpResult = null;
            try
            {
                fpdkHttpResult = RestClient.postJSONPCallback<FpdkHttpResult>(ym + "/SbsqWW/login.do", para);
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                if (retryTimes >= 1)
                {
                    throw new Exception("调用服务器接口失败");
                }
                retryTimes++;
                return secondLogin(ym, ymbb, clientAuthCode, serverRandom, sh, retryTimes);
            }


            if (fpdkHttpResult.key1.Equals("03"))
            {
                return fpdkHttpResult;
            }
            else if (fpdkHttpResult.key1.Equals("04"))
            {
                return fpdkHttpResult;
            }
            else
            {
                if (retryTimes >= 1)
                {
                    throw new Exception("调用服务器接口失败");
                }
                retryTimes++;
                return secondLogin(ym, ymbb, clientAuthCode, serverRandom, sh, retryTimes);
            }
        }

        public static bool isEmpty()
        {
            int count = 0;
            lock (requestTaskSocketMessages)
            {
                count = requestTaskSocketMessages.Count;
            }
            return count == 0;
        }

        public static void tockenTaskRequestThreadInit()
        {
            if (tockenTaskRequestThreadIsInit)
            {
                return;
            }
            Thread tockenThread = new Thread(delegate()
            {
                while (true)
                {
                    if ("usb".Equals(ConfigManager.Instance.Config.clientType))
                    {
                        if (errorPassword)
                        {
                            log.Error("密码错误没有重新配置");
                        }
                        else if (errorLicense)
                        {
                            log.Error("授权码错误，请重新输入");
                        }
                        else if (ConfigManager.Instance.Config.sh != null && ConfigManager.Instance.Config.password != null && ConfigManager.Instance.Config.license != null)
                        {
                            requestTockenTask();
                        }
                        else
                        {
                            log.Error("没有配置税号、密码、授权码");
                        }
                    }
                    else
                    {
                        requestTockenTask();
                    }
                    Thread.Sleep(1000);
                }

            });
            tockenThread.Name = "tokenThread";
            tockenThread.Start();
            tockenTaskRequestThreadIsInit = true;
        }

        private static void requestTockenTask()
        {
            try
            {
                if (requestTaskSocketMessages.Count > 0)
                {
                    return;
                }
                TaskSocketMessage taskSocketMessage = new TaskSocketMessage();
                taskSocketMessage.type = TaskHandle.GET_TOKEN;
                String sh = ConfigManager.Instance.Config.sh;
                String license = ConfigManager.Instance.Config.license;

                taskSocketMessage.parameters["sh"] = sh;
                taskSocketMessage.parameters["license"] = license;
                if (errorLicense)
                {
                    log.Error("授权码错误需要重新输入");
                    return;
                }

                if (errorPassword)
                {
                    log.Error("税号错误需要重新输入");
                    return;
                }

                taskSocketMessage = TaskWebsocketClient.Instance.sendSyncRequest(taskSocketMessage);
                if (taskSocketMessage.parameters.ContainsKey("rwid") && taskSocketMessage.parameters.ContainsKey("code") && "0".Equals(taskSocketMessage.parameters["code"]))
                {
                    TaskHandle.handle(taskSocketMessage);
                    return;
                }
                if (taskSocketMessage.parameters.ContainsKey("code"))
                {
                    String code = taskSocketMessage.parameters["code"];
                    if (ERROR_LICENSE.Equals(code))
                    {
                        errorLicense = true;
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
            }
        }
    }
}
