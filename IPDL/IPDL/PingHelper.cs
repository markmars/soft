using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;

namespace IPDL
{
    class PingHelper
    {

        public delegate void DlgPingCompleteHandler(object sender, PingCompletedEventArgs p, params object[] parameters);
        public event DlgPingCompleteHandler PingComplete = null;

        private object[] ps;

        public void PingIP(string ip, params object[] ps)
        {
            Ping p = new Ping();
            this.ps = ps;
            p.PingCompleted += new PingCompletedEventHandler(p_PingCompleted);
            p.SendAsync(ip, 5000, null);
        }

        private void p_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            PingComplete(this, e, ps);
            Ping ping = (Ping)sender;
            ping.Dispose();
        }

    }
}
