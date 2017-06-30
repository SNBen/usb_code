using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SKKey.bean
{
    class RPCResult<T>
    {
        private T _data;

        public T data {
            get { return _data; }
            set 
            {
                if (value is JObject)
                {
                    _data = JsonConvert.DeserializeObject<T>(value.ToString());
                }
                else
                {
                    _data = value;
                }
            }
        }

        public List<Msg> msgs { get; set; }
        public bool success { get; set; }
        public List<string> scripts { get; set; }

        public RPCResult()
        {
            this.msgs = new List<Msg>();
            this.scripts = new List<string>();
            this.success = true;
        }

    }
}
