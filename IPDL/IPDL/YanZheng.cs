using System;
using System.Collections.Generic;
using System.Text;

namespace IPDL
{
    public class YanZheng
    {
        public YanZheng() { }
        public YanZheng(string name, string url)
        {
            this.name = name;
            this.url = url;
        }

        public string name { set; get; }
        public string url { set; get; }
    }
}
