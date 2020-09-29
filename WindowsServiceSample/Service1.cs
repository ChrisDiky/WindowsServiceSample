using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WindowsServiceSample
{
    public partial class Service1 : ServiceBase
    {
        //從App.config取出檔案路徑及名稱
        private readonly string _FilePath = ConfigurationManager.AppSettings["FilePath"];

        private Timer _Timer;

        public Service1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 服務啟動
        /// </summary>
        protected override void OnStart(string[] args)
        {
            //File.AppendAllText(_FilePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " => 服務開始...\n");

            //_Timer = new Timer();
            //_Timer.Elapsed += new ElapsedEventHandler(TimerEvent);
            //_Timer.Interval = 10 * 1000;
            //_Timer.Start();

            //初始化SignalR
            InitializeSelfHosting();
        }

        /// <summary>
        /// Timer要執行的事件
        /// </summary>
        public void TimerEvent(object sender, ElapsedEventArgs e)
        {
            //寫入現在時間測試服務結果
            File.AppendAllText(_FilePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n");            
        }

        /// <summary>
        /// 服務結束
        /// </summary>
        protected override void OnStop()
        {
            File.AppendAllText(_FilePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " => 服務結束...\n");     
        }

        /// <summary>
        /// 初始化SignalR
        /// </summary>
        public void InitializeSelfHosting()
        {
            const string url = "http://localhost:8080";
            WebApp.Start(url);

            _Timer = new Timer(1000) { AutoReset = true };
            _Timer.Elapsed += Timer_Elapsed;
            _Timer.Start();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var signalR = GlobalHost.ConnectionManager.GetHubContext<Signalr.BroadcastHub>();

            //發送系統時間給client
            signalR.Clients.All.systemTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            foreach (var item in Signalr.BroadcastHub.CurrClients)
            {
                //發送訊息給特定sID的client
                switch (item.Value)
                {
                    case "1001":
                        signalR.Clients.Client(item.Key).broadcast("sID = 1001, Good!..." + new Random().Next(1, 10));
                        break;
                    case "1002":
                        signalR.Clients.Client(item.Key).broadcast("sID = 1002, Good!..." + new Random().Next(11, 20));
                        break;
                    default:
                        signalR.Clients.Client(item.Key).broadcast("sID = null, Error!..." + new Random().Next(-10, 0));
                        break;
                }                                          
            }
        }
    }
}
