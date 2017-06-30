using System;
using System.Collections.Generic;
using System.Text;

namespace SKKey.config
{
    class Config
    {
        public string clientType { get; set; }

        public string taskServer { get; set; }

        public string usbClubPort { get; set; }

        public string license{ get; set; }

        public string taskServerIP { get; set; }

        public string taskServerPort { get; set; }

        public string sh{ get; set; }

        public string password{ get; set; }

        public bool errorPassword { get; set; }

        public bool errorLicense { get; set; }

        public string controlVersion { get; set; }
    }
}
