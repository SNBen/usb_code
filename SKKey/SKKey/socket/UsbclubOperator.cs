using SKKey.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace SKKey.socket
{
    internal class UsbclubOperator
    {
        private static readonly log4net.ILog log 
            = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string OPEN_PORT_CMD_PRE = "0000001180";
        private const string CLOSE_PORT_CMD_PRE = "0000001181";
        private const string STR_SQDKHSYQ_JGM_A0 = "A0";
        private const string USBCLUB_EXE_PATH = "C:/TY-USBCLUB/";
        private const int REMOTE_CONL_PORT = 8110;
        private const string STR_QZGBDK_JGM_A1 = "A1";

        /**
         * 根据机柜管理器，发送的端口信息字符串，打开端口
         * 068020002389_127.0.0.1_8
         * dictionary result:0 成功
         *            result:非0 失败  msg:错误信息
         */

        public static Dictionary<string, string> openPort(string portInfo)
        {
            Dictionary<string, string> resultMap = new Dictionary<string, string>();
            if (portInfo != null && !"".Equals(portInfo) && portInfo.Contains("_") && portInfo.Split('_').Length == 3)
            {
                try
                {
                    string[] portxx = portInfo.Split('_');
                    string guid = portxx[0];
                    string ip = portxx[1];
                    string port = portxx[2];
                    string cmdStr = OPEN_PORT_CMD_PRE + HexUtil.getHexStrByString(guid, null, 12);//不传charset
                    cmdStr += HexUtil.int2Hex4BytesStr(int.Parse(port));
                    log.Info("打开端口信息：" + portxx + "-cmdStr:" + cmdStr);
                    Dictionary<string, string> appMap = appRightByUsbPort(ip, REMOTE_CONL_PORT, cmdStr);
                    if (STR_SQDKHSYQ_JGM_A0.Equals(appMap["jgm"]))//成功
                    {
                        //runLocalExe(USBCLUB_EXE_PATH, ip, appMap["sxms"]);
                        System.Threading.Thread.Sleep(2000);
                        resultMap.Add("result", "0");
                        resultMap.Add("msg", "打开端口成功");
                    }
                    else
                    {
                        resultMap.Add("result", appMap["cwm"]);
                        resultMap.Add("msg", appMap["cwxx"]);
                    }
                }
                catch
                {
                    resultMap.Add("result", "-1");
                    resultMap.Add("msg", "打开端口异常");
                }
            }
            else
            {
                resultMap.Add("result", "1");
                resultMap.Add("msg", "端口信息格式不正确");
            }
            string str = dic2str(resultMap);
            log.Info("打开端口对外返回结果：" + str);

            return resultMap;
        }

        /**
        * 根据机柜端口信息，关闭端口
        */
        public static Dictionary<string, string> closePort(string portInfo)
        {
            Dictionary<string, string> resultMap = new Dictionary<string, string>();
            if (portInfo != null && !"".Equals(portInfo) && portInfo.Contains("_") && portInfo.Split('_').Length == 3)
            {
                try
                {
                    string[] portxx = portInfo.Split('_');
                    string guid = portxx[0];
                    string ip = portxx[1];
                    string port = portxx[2];
                    string cmdStr = CLOSE_PORT_CMD_PRE + HexUtil.getHexStrByString(guid, null, 12);//不传charset
                    cmdStr += HexUtil.int2Hex4BytesStr(int.Parse(port));
                    log.Info("关闭端口信息：" + portxx + "-cmdStr:" + cmdStr);
                    Dictionary<string, string> appMap = closeUsbPort(ip, REMOTE_CONL_PORT, cmdStr);
                    if (STR_QZGBDK_JGM_A1.Equals(appMap["jgm"]))//成功
                    {
                        resultMap.Add("result", "0");
                        resultMap.Add("msg", "关闭端口成功");
                    }
                    else
                    {
                        resultMap.Add("result", "1");
                        resultMap.Add("msg", "关闭端口失败");
                    }
                }
                catch
                {
                    resultMap.Add("result", "-1");
                    resultMap.Add("msg", "关闭端口异常");
                }
            }
            else
            {
                resultMap.Add("result", "1");
                resultMap.Add("msg", "端口信息格式不正确");
            }
            string str = dic2str(resultMap);
            log.Info("打开端口对外返回结果：" + str);
            return resultMap;
        }

        /**
         * tcp连接机柜，申请关闭端口
         */
        private static Dictionary<string, string> closeUsbPort(string ip, int port, string cmdStr)
        {
            log.Info("申请关闭端口, ip:" + ip + ", port:" + port + ", cmd:" + cmdStr);
            Dictionary<string, string> resultMap = new Dictionary<string, string>();

            TcpClient tcp = new TcpClient();
            tcp.Connect(ip, port);
            if (tcp.Connected)
            {
                NetworkStream stream = tcp.GetStream();
                lock (stream)
                {
                    byte[] cmdBytes = HexUtil.strToHexByte(cmdStr);
                    stream.Write(cmdBytes, 0, cmdBytes.Length);
                    stream.Flush();
                    byte[] head = new byte[4];
                    stream.Read(head, 0, head.Length);
                    string headHexStr = HexUtil.bytesToHexString(head);
                    log.Info("申请关闭打开，返回头信息" + headHexStr);
                    int dataLength = int.Parse(HexUtil.bytesToHexString(head), System.Globalization.NumberStyles.AllowHexSpecifier);
                    byte[] buf = new byte[dataLength];
                    stream.Read(buf, 0, buf.Length);
                    String hexStr = HexUtil.bytesToHexString(buf);
                    log.Info("申请关闭端口，返回hex数据：" + hexStr);

                    if (STR_QZGBDK_JGM_A1.Equals(hexStr))//成功
                    {
                        resultMap.Add("jgm", hexStr);
                    }
                    else //失败
                    {
                        string cwm = HexUtil.subHexStr(hexStr, 1, 1);
                        resultMap.Add("cwm", HexUtil.hex2Integerstr(cwm));

                        string cwxx = HexUtil.subBytes2Str(buf, 2, buf.Length - 1, null);
                        resultMap.Add("cwxx", cwxx);
                    }

                    string resultStr = dic2str(resultMap);

                    log.Info("申请关闭端口结果解析成集合：" + resultStr);
                    stream.Close();//关闭流
                }
            }

            return resultMap;
        }

        /**
         * tcp连接机柜，申请打开端口
         * 
         */
        private static Dictionary<string, string> appRightByUsbPort(string ip, int port, string cmdStr)
        {
            log.Info("申请打开端口, ip:" + ip + ", port:" + port + ", cmd:" + cmdStr);
            Dictionary<string, string> resultMap = new Dictionary<string, string>();

            TcpClient tcp = new TcpClient();
            tcp.Connect(ip, port);
            if (tcp.Connected)
            {
                NetworkStream stream = tcp.GetStream();
                lock (stream)
                {
                    byte[] cmdBytes = HexUtil.strToHexByte(cmdStr);
                    stream.Write(cmdBytes, 0, cmdBytes.Length);
                    stream.Flush();
                    byte[] head = new byte[4];
                    stream.Read(head, 0, head.Length);
                    string headHexStr = HexUtil.bytesToHexString(head);
                    log.Info("申请端口打开，返回头信息" + headHexStr);
                    //string a = HexUtil.subHexStr(HexUtil.bytesToHexString(head), 3, 2);
                    //int dataLength = Convert.ToInt32(HexUtil.subHexStr(HexUtil.bytesToHexString(head), 3, 2), 16);//16进制字符串转10进制数据长度
                    int dataLength = int.Parse(HexUtil.bytesToHexString(head), System.Globalization.NumberStyles.AllowHexSpecifier);
                    byte[] buf = new byte[dataLength];
                    stream.Read(buf, 0, buf.Length);
                    String hexStr = HexUtil.bytesToHexString(buf);
                    log.Info("申请端口打开，返回hex数据：" + hexStr);

                    if (STR_SQDKHSYQ_JGM_A0.Equals(hexStr))
                    {
                        //添加结果码
                        resultMap.Add("jgm", hexStr);
                    }
                    else
                    {
                        string cwm = HexUtil.subHexStr(hexStr, 1, 1);
                        resultMap.Add("cwm", HexUtil.hex2Integerstr(cwm));

                        string cwxx = HexUtil.subBytes2Str(buf, 2, buf.Length - 1, null);
                        resultMap.Add("cwxx", cwxx);
                    }

                    string resultStr = dic2str(resultMap);

                    log.Info("申请打开端口结果解析成集合：" + resultStr);
                    stream.Close();//关闭流
                }
            }

            return resultMap;
        }

        /**
         * 执行usbclub.exe驱动，挂载机柜usb设备
         * 
         */
        private static void runLocalExe(string exePath, string ip, string sxms)
        {
            string msg = "";
            Process p = new Process();
            p.StartInfo.FileName = exePath + "usbclub.exe";             //设定需要执行的命令
            p.StartInfo.UseShellExecute = false;          //不使用系统shell外壳程序启动
            p.StartInfo.RedirectStandardInput = true;     //重定向输入（一定是true）
            p.StartInfo.RedirectStandardOutput = true;    //重定向输出
            p.StartInfo.RedirectStandardError = true;
            string param = " -a " + ip + " 3240 " + sxms + " " + sxms;
            p.StartInfo.Arguments = param;
            p.StartInfo.CreateNoWindow = true;            //不创建窗口
            try
            {
                if (p.Start())
                {
                    log.Info("本地挂载" + ip + "机柜端口,树形描述：" + sxms + ",参数：" + param);
                    p.Close();
                }
                else
                    msg = "DOS线程创建失败";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            //return msg;
        }

        /**
         * diction数据结构转换为key-value，字符串
         */

        private static string dic2str(Dictionary<string, string> dic)
        {
            string str = "";
            if (dic != null && dic.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    str += kvp.Key + ":" + kvp.Value + ";";
                }
            }
            return str;
        }
    }
}