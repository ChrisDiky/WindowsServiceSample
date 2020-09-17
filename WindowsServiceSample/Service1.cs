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

        public Service1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 服務啟動
        /// </summary>
        protected override void OnStart(string[] args)
        {
            File.AppendAllText(_FilePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " => 服務開始...");

            var myTimer = new Timer();
            myTimer.Elapsed += new ElapsedEventHandler(TimerEvent);
            myTimer.Interval = 10 * 1000;
            myTimer.Start();

        }

        /// <summary>
        /// Timer要執行的事件
        /// </summary>
        public void TimerEvent(object sender, ElapsedEventArgs e)
        {
            //寫入現在時間測試服務結果
            File.AppendAllText(_FilePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));            
        }

        /// <summary>
        /// 服務結束
        /// </summary>
        protected override void OnStop()
        {
            File.AppendAllText(_FilePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " => 服務結束...");     
        }
    }
}
