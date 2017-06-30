using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using SKKey.utils;

namespace SKKey.websocket
{
    class TaskSocketMessage
    {
        public static String MESSAGE_TYPE_NOTIFY = "notify";

        public static String MESSAGE_TYPE_HEARTBEAT = "heartbeat";

        private static long MESSAGE_ID = DateTimeUtil.getSystemTimestampMilli();


        public long id { get; set; }
        public string type { get; set; }
        public string content { get; set; }
        public Dictionary<string,string> parameters { get; set; }
        public long createTime { get; set; }
        public bool request { get; set; }
        public bool async { get; set; } 

        public TaskSocketMessage()
        {
            this.id = newId();
            this.createTime = DateTimeUtil.getSystemTimestampMilli();
            this.async = false;
            this.parameters = new Dictionary<string, string>();
        }

        public TaskSocketMessage(long id)
        {
            this.id = id;
            this.createTime = DateTimeUtil.getSystemTimestampMilli();
            this.async = false;
            this.parameters = new Dictionary<string, string>();
        }

        private static long newId()
        {
            return Interlocked.Increment(ref MESSAGE_ID);
        }

        public TaskSocketMessage setParameter(string name, string value)
        {
            this.parameters[name] = value;
            return this;
        }

        public String getParameter(String name)
        {
            return this.parameters[name];
        }
    }
}
