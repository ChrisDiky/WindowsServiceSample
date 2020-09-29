using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceSample.Signalr
{
    [HubName("broadcastHub")]
    public class BroadcastHub : BaseHub
    {
        public class Para
        {
            public string sID { set; get; }
        }

        public void Register(Para para)
        {
            Remove(Context.ConnectionId);
            CurrClients.Add(Context.ConnectionId, para.sID);
        }
    }
}
