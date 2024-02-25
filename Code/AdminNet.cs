using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AdminNet
    {
        public static AdminNet admin { get; set; }
        public NetSer ser = null;
        public NetCli cli = null;
        public bool start = false;
        public void Start() {
            admin = this;
        }

        public void StartServer()
        {
            ser = new NetSer();
            cli = new NetCli();
            ser.Start();
            cli.Start("127.0.0.1", true);
            start = true;
        }

        public void StartClient(string ip)
        {
            cli = new NetCli();
            cli.Start(ip);
            start = true;
        }

        public void Update()
        {
            if (ser != null)
            {
                ser.Update();
            }
            if (cli != null)
            {
                cli.Update();
            }
        }
    }
}