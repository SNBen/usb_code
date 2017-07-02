using System;
using System.Collections.Generic;
using System.Text;
using SKKey.websocket;

namespace SKKey.task
{
    class TaskHandle
    {
        private static readonly log4net.ILog log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string GET_TOKEN = "pushTask";
        //public static string GET_TOKEN = "JX_CLIENT_GETSESSIONCLIENTTASK";
        public static string POST_TOKEN = "submitTaskResult";

        public TaskHandle()
        { 
                
        }

        public static void handle(TaskSocketMessage msg)
        {
            TaskSocketMessage returnSocketMessage = null;
            if (GET_TOKEN.Equals(msg.type))
            {
                returnSocketMessage = new TokenTask().handle(msg);
            }
            if (returnSocketMessage != null)
            {
                try {
                    TaskWebsocketClient.Instance.sendASyncResponse(returnSocketMessage);
                }
                catch (Exception e) {
                    log.Error(e.Message, e);
                }
                
            }
        }
    }
}
