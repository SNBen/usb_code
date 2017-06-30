﻿using System;
using System.Collections.Generic;
using System.Text;

using WebSocket4Net;
using SKKey.config;

using System.Threading;
using SuperSocket.ClientEngine;
using Newtonsoft.Json;
using SKKey.task;
namespace SKKey.websocket
{
    class TaskWebsocketClient
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static TaskWebsocketClient instance;

        private WebSocket websocket;

        private Dictionary<long, TaskSocketMessage> reqReturnMap = new Dictionary<long, TaskSocketMessage>();

        private bool connected;

        private TaskWebsocketClient()
        {

        }

        public static TaskWebsocketClient Instance
        {
            get
            {
                if (instance == null || !instance.connected)
                {
                    init();
                }
                return instance;
            }
        }

        public static void init()
        {
            instance = new TaskWebsocketClient();
            instance.connect();
            int i = 0;
            int waitTime = 10;
            while (!instance.connected)
            {
                Thread.Sleep(waitTime);
                i++;
                if (5 * 1000 < i * waitTime)
                {
                    log.Info("taskserver connect 失败");
                    break;
                }
            }
            log.Info("taskserver connected");

        }

        public void connect()
        {
            connected = false;
            String taskServer = ConfigManager.Instance.Config.taskServer;
            if (!"usb".Equals(ConfigManager.Instance.Config.clientType))
            {
                if (ConfigManager.Instance.Config.taskServerIP == null || ConfigManager.Instance.Config.taskServerIP == ""

                   || ConfigManager.Instance.Config.taskServerPort == null || ConfigManager.Instance.Config.taskServerPort == "")
                {
                    return;
                }
                taskServer = "ws://" + ConfigManager.Instance.Config.taskServerIP + ":" + ConfigManager.Instance.Config.taskServerPort + "/websocket";
            }
            log.Info("尝试连接taskServer：" + taskServer);
            websocket = new WebSocket(taskServer);
            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            websocket.Open();
        }

        private void send(TaskSocketMessage taskSocketMessage)
        {
            websocket.Send(JsonConvert.SerializeObject(taskSocketMessage));
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            log.Info("taskserver 已打开");
            instance.connected = true;
        }

        private void websocket_Error(object sender, ErrorEventArgs e)
        {
            log.Error("websocket_Error:");
            log.Error(sender);
            log.Error(e);
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            log.Error("taskserver 断开");
            instance.connected = false;
            connect();
        }

        private void handleMessage(TaskSocketMessage msg)
        {
            log.Info("当前执行线程名称：" + Thread.CurrentThread.GetHashCode());
            if (!msg.request && !msg.async)
            {
                lock (reqReturnMap)
                {
                    reqReturnMap[msg.id] = msg;
                }
                return;
            }

            try
            {
                TaskHandle.handle(msg);
            }
            catch (Exception exp)
            {
                log.Error("处理TaskSocketMessage 失败" + JsonConvert.SerializeObject(msg), exp);
            }
        }

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            log.Info("接收消息：" + e.Message);
            TaskSocketMessage msg = null;
            try
            {
                msg = JsonConvert.DeserializeObject<TaskSocketMessage>(e.Message);
            }
            catch (Exception exp)
            {
                log.Error("处理TaskSocketMessage 失败" + e.Message, exp);
            }
            if (msg == null)
            {
                return;
            }
            Thread thread = new Thread(delegate()
            {
                handleMessage(msg);
            });
            thread.Name = "MessageReceived:" + thread.GetHashCode();
            thread.Start();
        }

        public TaskSocketMessage sendSyncRequest(TaskSocketMessage msg)
        {
            log.Info("request:" + JsonConvert.SerializeObject(msg));
            msg.request = true;
            msg.async = false;
            reqReturnMap[msg.id] = null;
            send(msg);
            //循环次数
            int cycleIndex = 0;
            //休眠毫秒数
            int sleepTimeUnitMil = 5;
            //休眠总时间
            int waitTime = 10 * 1000;
            while (reqReturnMap[msg.id] == null)
            {
                cycleIndex++;
                if (cycleIndex >= (waitTime / sleepTimeUnitMil))
                {
                    break;
                }
                Thread.Sleep(sleepTimeUnitMil);
            }
            TaskSocketMessage returnMsg = reqReturnMap[msg.id];
            lock (reqReturnMap)
            {
                reqReturnMap.Remove(msg.id);
            }
            log.Info("response:" + JsonConvert.SerializeObject(returnMsg));
            return returnMsg;
        }

        public void sendASyncRequest(TaskSocketMessage msg)
        {
            msg.request = true;
            msg.async = true;
            send(msg);
        }

        public void sendASyncResponse(TaskSocketMessage msg)
        {
            msg.request = false;
            msg.async = true;
            send(msg);
            log.Info("sendASyncResponse:" + JsonConvert.SerializeObject(msg));
        }
    }
}