using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceSample.Signalr
{
    public class BaseHub : Hub
    {
        /// <summary>
        /// 紀錄目前已連結的 Client 資料[key = ConnID, value = sID]
        /// </summary>
        public static Dictionary<string, string> CurrClients = new Dictionary<string, string>();

        /// <summary>
        /// 複寫OnConnected
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {            
            return base.OnConnected();
        }

        /// <summary>
        /// 複寫OnReconnected
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {            
            return base.OnReconnected();
        }

        /// <summary>
        /// 斷線
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnected(bool stop)
        {
            Remove(Context.ConnectionId);
            return base.OnDisconnected(stop);
        }


        /// <summary>
        /// 斷線時移除該使用者
        /// </summary>
        /// <param name="connectionId"></param>
        public void Remove(string connectionId)
        {
            lock (CurrClients)
            {
                foreach (var item in CurrClients.ToArray())
                {
                    if (item.Key == connectionId)
                    {
                        CurrClients.Remove(item.Key);
                    }
                }
            }
        }
    }
}
