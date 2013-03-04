using System;
using System.Collections.Generic;
using System.Text;

namespace IPDL
{
    public class DaiLi
    {
        public DaiLi() { }
        public DaiLi(string type, string ip, string port, string time, string info)
        {
            this.ip = ip;
            this.type = type;
            this.port = port;
            this.time = time;
            this.info = info;
        }

        public string type { set; get; }
        public string ip { set; get; }
        public string port { set; get; }
        public string time { set; get; }
        public string info { set; get; }
    }
}
