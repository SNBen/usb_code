using System;
using System.Collections.Generic;
using System.Text;

namespace SKKey.ocx
{
    class WebOcxAccess
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AxCryp_Ctl.AxCryptCtl ocx { get; set; }

        private static WebOcxAccess instance;

        private WebOcxAccess(AxCryp_Ctl.AxCryptCtl ocx)
        {
            this.ocx = ocx;
        }

        public static WebOcxAccess Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new Exception("WebOcxAccess 没有被初始化");
                }
                return instance;
            }
        }

        public static void init(AxCryp_Ctl.AxCryptCtl ocx)
        {
            WebOcxAccess.instance = new WebOcxAccess(ocx);
        }

        public static String openAndGetCert(String password)
        {
            log.Info("打开设备");
            close();
            Instance.ocx.OpenDeviceEx(password);
            if (Instance.ocx.ErrCode != 0)
            {
                log.Error("打开设备失败：" + Instance.ocx.ErrMsg);
                throw new Exception("打开设备失败:" + Instance.ocx.ErrMsg);
            }
            Instance.ocx.GetCertInfo("", 71);
            if (Instance.ocx.ErrCode != 0)
            {
                log.Error("getCert失败：" + Instance.ocx.ErrMsg);
                throw new Exception("getCert失败");
            }
            log.Info("打开设备成功：" + Instance.ocx.strResult + " " + password);
            return Instance.ocx.strResult;
        }

        public static void close()
        {

            if (Instance.ocx.IsDeviceOpened() != 0)
            {
                log.Info("关闭设备");
                Instance.ocx.CloseDevice();
                log.Info("关闭设备成功");
            }
        }

        public static String clientHello()
        {
            log.Info("make clientHello");
            Instance.ocx.ClientHello(0);
            if (Instance.ocx.ErrCode != 0)
            {
                log.Error("clientHello失败：" + Instance.ocx.ErrMsg);
                throw new Exception("clientHello失败");
            }
            log.Info("make clientHello:" + Instance.ocx.strResult);
            return Instance.ocx.strResult;
        }


        public static String ClientAuth(String serverPacket)
        {
            log.Info("make ClientAuth");
            Instance.ocx.ClientAuth(serverPacket);
            if (Instance.ocx.ErrCode != 0)
            {
                log.Error("clientHello失败：" + Instance.ocx.ErrMsg);
                throw new Exception("clientHello失败");
            }
            log.Info("make ClientAuth:" + Instance.ocx.strResult);
            return Instance.ocx.strResult;
        }
    }
}
